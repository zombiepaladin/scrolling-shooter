using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using System;

namespace ScrollingShooter
{
    /// <summary>
    /// Represents the three charge levels and animation frames for the Laser Drone.
    /// </summary>
    public enum WeaponChargeLevel
    {
        Low = 0,
        Medium = 1,
        Full = 2,
    }

    /// <summary>
    /// Represents the three AI states for the Laser Drone.
    /// </summary>
    enum AIState
    {
        Chasing = 0,
        Firing = 1,
        Recharging = 2,
    }

    /// <summary>
    /// An enemy drone that fires a beam towards the player.
    /// </summary>
    public class LaserDrone : Enemy
    {
        /// <summary>
        /// The time in seconds to recharge the laser.
        /// </summary>
        private const float RECHARGE_TIME = 5f;

        /// <summary>
        /// The time in seconds the laser lasts.
        /// </summary>
        private const float FIRE_TIME = 3.5f;

        /// <summary>
        /// The top speed that the drone moves.
        /// </summary>
        private const float MAX_MOVE_SPEED = 175;

        // LaserDrone state variables
        Texture2D spritesheet;
        Vector2 position;
        Rectangle[] spriteBounds = new Rectangle[3];
        WeaponChargeLevel weaponChargeLevel = WeaponChargeLevel.Low;

        /// <summary>
        /// The bounding rectangle of the LaserDrone
        /// </summary>
        public override Rectangle Bounds
        {
            get { return new Rectangle((int)position.X, (int)position.Y, spriteBounds[(int)weaponChargeLevel].Width * 2, spriteBounds[(int)weaponChargeLevel].Height * 2); }
        }

        /// <summary>
        /// The state of the LaserDrone's AI.
        /// </summary>
        AIState aiState = AIState.Chasing;

        /// <summary>
        /// The time in seconds until the laser can fire again
        /// </summary>
        float rechargeTimeRemaining = 0;

        /// <summary>
        /// The time in seconds until the laser has to recharge
        /// </summary>
        float fireTimeRemaining = 0;

        /// <summary>
        /// The current speed of the drone
        /// </summary>
        private float currentSpeed = MAX_MOVE_SPEED;

        /// <summary>
        /// Store the laser projectile used by the drone
        /// </summary>
        private DroneLaser droneLaser;

        /// <summary>
        /// Creates a new instance of a LaserDrone
        /// </summary>
        /// <param name="content">A ContentManager to load resources with</param>
        /// <param name="position">The position of the LaserDrone in the game world</param>
        public LaserDrone(uint id, ContentManager content, Vector2 position) : base(id)
        {
            this.position = position;

            spritesheet = content.Load<Texture2D>("Spritesheets/newsh8.shp.000000");

            spriteBounds[(int)WeaponChargeLevel.Full].X = 1;
            spriteBounds[(int)WeaponChargeLevel.Full].Y = 202;
            spriteBounds[(int)WeaponChargeLevel.Full].Width = 23;
            spriteBounds[(int)WeaponChargeLevel.Full].Height = 19;

            spriteBounds[(int)WeaponChargeLevel.Medium].X = 24;
            spriteBounds[(int)WeaponChargeLevel.Medium].Y = 202;
            spriteBounds[(int)WeaponChargeLevel.Medium].Width = 23;
            spriteBounds[(int)WeaponChargeLevel.Medium].Height = 19;

            spriteBounds[(int)WeaponChargeLevel.Low].X = 48;
            spriteBounds[(int)WeaponChargeLevel.Low].Y = 202;
            spriteBounds[(int)WeaponChargeLevel.Low].Width = 23;
            spriteBounds[(int)WeaponChargeLevel.Low].Height = 19;

            weaponChargeLevel = WeaponChargeLevel.Full;
        }

        /// <summary>
        /// Updates the LaserDrone
        /// </summary>
        /// <param name="elapsedTime">The in-game time between the previous and current frame</param>
        public override void Update(float elapsedTime)
        {
            // Sense the player's position
            PlayerShip player = ScrollingShooterGame.Game.Player;
            Vector2 playerPosition = new Vector2(player.Bounds.Center.X, player.Bounds.Center.Y);

            switch (aiState)
            {
                case AIState.Chasing:
                    getInPosition(playerPosition, elapsedTime);
                    if (Math.Abs(playerPosition.X - Bounds.Center.X) < 40 && Bounds.Center.Y < playerPosition.Y)
                    {
                        //transition to firing state
                        fireTimeRemaining = FIRE_TIME;
                        aiState = AIState.Firing;
                        currentSpeed = MAX_MOVE_SPEED * 0.66f;
                        if(droneLaser == null)
                            droneLaser = (DroneLaser) ScrollingShooterGame.GameObjectManager.CreateProjectile(ProjectileType.DroneLaser, Vector2.Zero);
                        droneLaser.isOn = true;
                        droneLaser.laserPower = WeaponChargeLevel.Full;
                        droneLaser.updatePosition(position.X + 23, position.Y + 30);
                    }
                    break;

                case AIState.Firing:
                    fireTimeRemaining -= elapsedTime;

                    //change to medium charge sprite when halfway depleted
                    if (weaponChargeLevel == WeaponChargeLevel.Full && fireTimeRemaining / FIRE_TIME < 0.66)
                        weaponChargeLevel = droneLaser.laserPower = WeaponChargeLevel.Medium;
                    else if (weaponChargeLevel == WeaponChargeLevel.Medium && fireTimeRemaining / FIRE_TIME < 0.33)
                        weaponChargeLevel = droneLaser.laserPower = WeaponChargeLevel.Low;

                    if (fireTimeRemaining <= 0)
                    {
                        //transition to recharging state
                        aiState = AIState.Recharging;
                        currentSpeed = MAX_MOVE_SPEED * 0.33f;
                        rechargeTimeRemaining = RECHARGE_TIME;
                        weaponChargeLevel = WeaponChargeLevel.Low;
                        droneLaser.isOn = false;
                    }
                    
                    getInPosition(playerPosition, elapsedTime);
                    droneLaser.updatePosition(position.X + 23, position.Y + 30);

                    break;
                case AIState.Recharging:
                    rechargeTimeRemaining -= elapsedTime;

                    if (weaponChargeLevel == WeaponChargeLevel.Low && rechargeTimeRemaining / RECHARGE_TIME < 0.66)
                        weaponChargeLevel = WeaponChargeLevel.Medium;
                    if (weaponChargeLevel == WeaponChargeLevel.Medium && rechargeTimeRemaining / RECHARGE_TIME < 0.33)
                        weaponChargeLevel = WeaponChargeLevel.Full;

                    if (rechargeTimeRemaining <= 0)
                    {
                        aiState = AIState.Chasing;
                        currentSpeed = MAX_MOVE_SPEED;
                        weaponChargeLevel = WeaponChargeLevel.Full;
                    }

                    moveAwayFrom(playerPosition, elapsedTime);
                    
                    break;
            }
        }

        /// <summary>
        /// Moves the ship into a firing position above the given point.
        /// </summary>
        /// <param name="elapsedTime">The point that the ship should target</param>
        private void getInPosition(Vector2 point, float elapsedTime)
        {
            //Try to stay ~100 away from the player in Y direction
            if (point.Y < position.Y + 150 && position.Y > 10)
                position.Y -= currentSpeed * elapsedTime;
            else if (point.Y > position.Y + 175)
                position.Y += currentSpeed * elapsedTime;

            //Try to get infront of the player
            if (Math.Abs(point.X - Bounds.Center.X) > 10)
            {
                if (point.X < Bounds.Center.X)
                    position.X -= currentSpeed * elapsedTime;
                else
                    position.X += currentSpeed * elapsedTime;
            }
        }

        /// <summary>
        /// Moves the ship away from the given point.
        /// </summary>
        /// <param name="elapsedTime">The point that the ship should get away from</param>
        private void moveAwayFrom(Vector2 point, float elapsedTime)
        {
            //Try to get away from the player in Y direction
            if (point.Y < position.Y + 300 && position.Y > 10)
                position.Y -= currentSpeed * elapsedTime;
            else if (point.Y > position.Y + 450)
                position.Y += currentSpeed * elapsedTime;

            //Run if too close in X direction
            if (Math.Abs(point.X - Bounds.Center.X) < 200)
            {
                if (point.X < Bounds.Center.X)
                    position.X += currentSpeed * elapsedTime;
                else
                    position.X -= currentSpeed * elapsedTime;
            }
        }

        /// <summary>
        /// Draw the LaserDrone ship on-screen
        /// </summary>
        /// <param name="elapsedTime">The in-game time between the previous and current frame</param>
        /// <param name="spriteBatch">An already initialized SpriteBatch, ready for Draw() commands</param>
        public override void Draw(float elapsedTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(spritesheet, position, spriteBounds[(int)weaponChargeLevel], Color.White, 0, Vector2.Zero, 2, SpriteEffects.None, 0);
        }

    }
}
