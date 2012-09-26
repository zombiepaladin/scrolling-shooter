using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace ScrollingShooter
{
    //enum for each part of the ship
    enum BeamShipParts
    {
        Body = 0,
        Weapon = 1,
        WeaponFire = 2
    }

    //enum that tracks the behavior states
    enum BehaviorState
    {
        Flying = 0,
        Charging,
        Firing,
        Fleeing,
        Fragged
    }

    //enum that tracks the damage states
    enum DamageState
    {
        Healthy = 0,
        WeaponBroke = 1,
        Dead = 2
    }

   public class BeamShip : Enemy
    {

        // beam ship state variables
        Texture2D spritesheet;
        Texture2D beamSpriteSheet;
        Vector2 position;
        Rectangle[] spriteBounds = new Rectangle[3];
      
        //DartSteeringState steeringState = DartSteeringState.Straight;

        //rectangle for each part of the ship for drawing purposes
        Rectangle[] drawBounds = new Rectangle[3];

        //integer to determine the offset of the weapon from the body
        static Vector2 weaponOffset = new Vector2(7, 10);
        static Vector2 weaponFireOffset = new Vector2(13, 27);

        //boolean value to see if the weapon or ship are not destroyed
        bool weaponAlive;
        bool shipAlive;

        //boolean to see if the ship is firing
        bool weaponFiring;

        //int that tracks the hit points of the ship
        int shipHealth = 500;

        //int that tracks the hit points of the weapon
        int weaponHealth = 400;

        //enum that manages behavior states
        BehaviorState behaviorState;

        //enum that manages damaged states
        DamageState damageState;

        //timer that is used to handle weapon recharging
        float timer;

        //speed of the ship flying down the screen
        Vector2 velocityY;
        Vector2 velocityX;

        /// <summary>
        /// The bounding rectangle of the beam ship
        /// </summary>
        public override Rectangle Bounds
        {
            get { return new Rectangle((int)position.X, (int)position.Y, spriteBounds[(int)BeamShipParts.Body].Width, spriteBounds[(int)BeamShipParts.Body].Height); }
        }

        /// <summary>
        /// Creates a new instance of an enemy beam ship
        /// </summary>
        /// <param name="content">A ContentManager to load resources with</param>
        /// <param name="position">The position of the beam ship in the game world</param>
        public BeamShip(uint id, ContentManager content, Vector2 position) : base (id)
        {
            this.position = position;
            weaponAlive = true;
            shipAlive = true;
            weaponFiring = false;
            behaviorState = BehaviorState.Flying;
            damageState = DamageState.Healthy;
            velocityY = new Vector2(0, 35);
            velocityX = new Vector2(80, 0);
            timer = -1;
            
            spritesheet = content.Load<Texture2D>("Spritesheets/newsh2.shp.000000");

            //load the body of the craft from the spritesheet
            spriteBounds[(int)BeamShipParts.Body].X = 100;
            spriteBounds[(int)BeamShipParts.Body].Y = 55;
            spriteBounds[(int)BeamShipParts.Body].Width = 37;
            spriteBounds[(int)BeamShipParts.Body].Height = 28;
            drawBounds[(int)BeamShipParts.Body] = new Rectangle((int)position.X, (int)position.Y, spriteBounds[(int)BeamShipParts.Body].Width, spriteBounds[(int)BeamShipParts.Body].Height);

            //load the weapon for the craft from the sprite sheet
            spriteBounds[(int)BeamShipParts.Weapon].X = 107;
            spriteBounds[(int)BeamShipParts.Weapon].Y = 84;
            spriteBounds[(int)BeamShipParts.Weapon].Width = 24;
            spriteBounds[(int)BeamShipParts.Weapon].Height = 28;
            drawBounds[(int)BeamShipParts.Weapon] = new Rectangle((int)(position.X + weaponOffset.X), (int)(position.Y + weaponOffset.Y), spriteBounds[(int)BeamShipParts.Weapon].Width, spriteBounds[(int)BeamShipParts.Weapon].Height);

            //load the weapon's fire animation from the sprite sheet
            beamSpriteSheet = content.Load<Texture2D>("Spritesheets/newsh(.shp.000000");
            spriteBounds[(int)BeamShipParts.WeaponFire].X = 13;
            spriteBounds[(int)BeamShipParts.WeaponFire].Y = 70;
            spriteBounds[(int)BeamShipParts.WeaponFire].Width = 11;
            spriteBounds[(int)BeamShipParts.WeaponFire].Height = 11;
            drawBounds[(int)BeamShipParts.WeaponFire] = new Rectangle((int)(position.X + weaponFireOffset.X), (int)(position.Y + weaponFireOffset.Y), spriteBounds[(int)BeamShipParts.WeaponFire].Width, spriteBounds[(int)BeamShipParts.WeaponFire].Height);
        }

        /// <summary>
        /// Updates the beam ship
        /// </summary>
        /// <param name="elapsedTime">The in-game time between the previous and current frame</param>
        public override void Update(float elapsedTime)
        {
            // Sense the player's position
            PlayerShip player = ScrollingShooterGame.Game.Player;
            Vector2 playerPosition = new Vector2(player.Bounds.Center.X, player.Bounds.Center.Y);
           
            if (timer > 0)
                timer -= elapsedTime;

            switch (behaviorState)
            {
                case BehaviorState.Flying:
                    this.position += velocityY * elapsedTime;

                    if (timer < 0)
                    {
                        if (this.position.X >= playerPosition.X - 23 && this.position.X <= playerPosition.X + 23 && this.position.Y < playerPosition.Y)
                        {
                            timer = 1f;
                            behaviorState = BehaviorState.Charging;
                        }
                    }

                    break;
                case BehaviorState.Charging:
                    weaponFiring = true;
                    if(timer < .9)
                        behaviorState = BehaviorState.Firing;

                    break;
                case BehaviorState.Firing:
                    if (weaponAlive)
                    {
                        ScrollingShooterGame.GameObjectManager.CreateProjectile(ProjectileType.BlueBeam, new Vector2(this.position.X + weaponFireOffset.X + 1, this.position.Y + weaponFireOffset.Y + 8));
                        weaponFiring = false;
                        behaviorState = BehaviorState.Flying;
                    }
                    else
                        behaviorState = BehaviorState.Fleeing;

                    break;
                case BehaviorState.Fleeing:
                    this.position += velocityY * elapsedTime;
                    if (position.X >= ScrollingShooterGame.Game.GraphicsDevice.Viewport.Bounds.Center.X)
                        this.position += velocityX * elapsedTime;
                    else
                        this.position -= velocityX * elapsedTime;
                  
                    break;
                case BehaviorState.Fragged:
                    //the code for the ship dying will go here
                    break;
            }

            switch (damageState)
            {
                case DamageState.Healthy:
                    if (weaponHealth <= 0)
                    {
                        weaponAlive = false;
                        damageState = DamageState.WeaponBroke;
                        behaviorState = BehaviorState.Fleeing;
                    }
                    break;
                case DamageState.WeaponBroke:
                    if (shipHealth <= 0)
                    {
                        damageState = DamageState.Dead;
                        behaviorState = BehaviorState.Fragged;
                    }
                    break;
                case DamageState.Dead:
                    shipAlive = false;
                    //need code for dead ship
                    break;
            }
         
            drawBounds[(int)BeamShipParts.Body] = new Rectangle((int)position.X, (int)position.Y, spriteBounds[(int)BeamShipParts.Body].Width, spriteBounds[(int)BeamShipParts.Body].Height);
            drawBounds[(int)BeamShipParts.Weapon] = new Rectangle((int)(position.X + weaponOffset.X), (int)(position.Y + weaponOffset.Y), spriteBounds[(int)BeamShipParts.Weapon].Width, spriteBounds[(int)BeamShipParts.Weapon].Height);
            drawBounds[(int)BeamShipParts.WeaponFire] = new Rectangle((int)(position.X + weaponFireOffset.X), (int)(position.Y + weaponFireOffset.Y), spriteBounds[(int)BeamShipParts.WeaponFire].Width, spriteBounds[(int)BeamShipParts.WeaponFire].Height);
        }

        /// <summary>
        /// Draw the beam ship on-screen
        /// </summary>
        /// <param name="elapsedTime">The in-game time between the previous and current frame</param>
        /// <param name="spriteBatch">An already initialized SpriteBatch, ready for Draw() commands</param>
        public override void Draw(float elapsedTime, SpriteBatch spriteBatch)
        { 
            if(weaponFiring)
                spriteBatch.Draw(beamSpriteSheet, drawBounds[(int)BeamShipParts.WeaponFire], spriteBounds[(int)BeamShipParts.WeaponFire], Color.White);

            if(shipAlive)
                spriteBatch.Draw(spritesheet, drawBounds[(int)BeamShipParts.Body], spriteBounds[(int)BeamShipParts.Body], Color.White);
            
            if(weaponAlive)
                spriteBatch.Draw(spritesheet, drawBounds[(int)BeamShipParts.Weapon], spriteBounds[(int)BeamShipParts.Weapon], Color.White);
           
            
        }
    }
}