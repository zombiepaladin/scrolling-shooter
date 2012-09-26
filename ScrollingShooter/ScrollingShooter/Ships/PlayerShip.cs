using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace ScrollingShooter
{
    /// <summary>
    /// The different types of Player ships available in the game
    /// </summary>
    public enum PlayerShipType
    {
        Shrike,
    }

    /// <summary>
    /// Represents the five possible steering states for our ships
    /// </summary>
    enum SteeringState
    {
        HardLeft = 0,
        Left = 1,
        Straight = 2,
        Right = 3,
        HardRight = 4,
    }

    /// <summary>
    /// A base class for all Player ships
    /// </summary>
    public abstract class PlayerShip : GameObject
    {
        public static short HomingMissileLevel = 0;

        /// <summary>
        /// Player's Health
        /// </summary>
        public float Health = 100;

        // Timers
        /// <summary>
        /// Timer for the default gun
        /// </summary>
        public float defaultGunTimer = 5;
        float bombTimer = 1.5f;
        float shotgunTimer = 0.5f;

        private float homingMissileFireRate = 3;
        private float homingMissileTimer = 0;

        //Timer for how longs the blades have been active.
        float bladesPowerupTimer = 0;

        /// <summary>
        /// Timer for the energy blast gun
        /// </summary>
        public float energyBlastTimer = 0;

        /// <summary>
        /// Timer to adjust refire rate of railgun
        /// </summary>
        float railgunTimer = 0;

        /// <summary>
        /// Rectangle to draw the railgun when the powerup is enabled
        /// </summary>
        protected Rectangle railgunSpriteBounds = new Rectangle(146, 55, 8, 33);

        public Rectangle RailgunBounds
        {
            get { return new Rectangle((int)(position.X + (Bounds.Width / 2) - 4), (int)(position.Y - (Bounds.Height / 2)), railgunSpriteBounds.Width, railgunSpriteBounds.Height); }
        }

        // Powerup Levels
        /// <summary>
        /// Level of the energy blast powerup
        /// </summary>
        public int energyBlastLevel = -1;

        /// <summary>
        /// The velocity of the ship - varies from ship to ship
        /// </summary>
        protected Vector2 velocity;

        /// <summary>
        /// The position of the ship in the game world.  We use a Vector2 for position 
        /// rather than a Rectangle, as floats allow us to move less than a pixel
        /// </summary>
        protected Vector2 position = new Vector2(300, 300);
        public Vector2 Position
        {
            get { return position; }
            set { position = value; }
        }

        /// <summary>
        /// The spritesheet our ship is found upon
        /// </summary>
        protected Texture2D spriteSheet;

        /// <summary>
        /// The spritebounds for our ship, corresponding to the five possible steering states
        /// </summary>
        protected Rectangle[] spriteBounds = new Rectangle[5];

        /// <summary>
        /// The keyboard state from the previous frame, used to see if a key was just pressed,
        /// or held down
        /// </summary>
        KeyboardState oldKeyboardState = Keyboard.GetState();

        // The current steering state of the ship
        SteeringState steeringState = SteeringState.Straight;

        // The PowerupType equipped on this ship
        PowerupType PowerupType = PowerupType.None;

        /// This drunk status of the ship.  If the bool is true, movements are reversed, and damage is doubled.
        /// The drunk counter represents how many more frame updates before the player is sober again.
        bool drunk = false;
        int drunkCounter = 0;

        /// <summary>
        /// The bounding rectangle for the ship.  Generated from the animation frame and the ship's
        /// position.
        /// </summary>
        public override Rectangle Bounds
        {
            get { return new Rectangle((int)Position.X, (int)Position.Y, spriteBounds[0].Width, spriteBounds[0].Height); }
        }


        /// <summary>
        /// Creates a new Player ship instance
        /// </summary>
        /// <param name="id">the unique id of the Player ship</param>
        public PlayerShip(uint id) : base(id) { }


        /// <summary>
        /// Applies the specified powerup to the ship
        /// </summary>
        /// <param name="powerup">the indicated powerup</param>
        public void ApplyPowerup(PowerupType powerup)
        {
            //Meteor triggers on pickup, no need to store it.
            //Alternatively, it could be stored and triggered on a custom key
            //Another alternative - Store it as a once-per-press powerup, and remove it after the first press
            if ((powerup & PowerupType.MeteorPowerup) > 0)
            {
                TriggerMeteor();
                return;
            }

            //This will level us up if we hit another homing missile
            if ((powerup & PowerupType.HomingMissiles) > 0)
            {
                HomingMissileLevel = (short)MathHelper.Min(HomingMissileLevel + 1, 3);
            }

            // Store the new powerup in the PowerupType bitmask
            this.PowerupType |= powerup;

            //Apply special logic for powerups here
            switch (powerup)
            {
                case PowerupType.Blades:
                    ApplyBlades();
                    break;

                case PowerupType.EightBallShield:
                    TriggerEightBallShield();
                    break;

                case PowerupType.TriShield:
                    ApplyTriShield();
                    break;

                case PowerupType.Ale:
                    GetDrunk();
                    break;

                case PowerupType.EnergyBlast:
                    energyBlastLevel++;
                    break;
            }
        }


        /// <summary>
        /// Updates the ship
        /// </summary>
        /// <param name="elapsedTime"></param>
        public override void Update(float elapsedTime)
        {
            KeyboardState currentKeyboardState = Keyboard.GetState();

            // Update timers
            defaultGunTimer += elapsedTime;
            bladesPowerupTimer += elapsedTime;
            energyBlastTimer -= elapsedTime;
            bombTimer += elapsedTime;
            railgunTimer += elapsedTime;
            homingMissileTimer -= elapsedTime;

            if (!drunk)
            {
                // Steer the ship up or down according to user input
                if (currentKeyboardState.IsKeyDown(Keys.Up))
                {
                    position.Y -= elapsedTime * velocity.Y;
                }
                else if (currentKeyboardState.IsKeyDown(Keys.Down))
                {
                    position.Y += elapsedTime * velocity.Y;
                }

                // Steer the ship left or right according to user input
                steeringState = SteeringState.Straight;

                if (currentKeyboardState.IsKeyDown(Keys.Left))
                {
                    if (currentKeyboardState.IsKeyDown(Keys.LeftShift) ||
                        currentKeyboardState.IsKeyDown(Keys.RightShift))
                    {
                        steeringState = SteeringState.HardLeft;
                        position.X -= elapsedTime * 2 * velocity.X;

                    }
                    else
                    {
                        steeringState = SteeringState.Left;
                        position.X -= elapsedTime * velocity.X;
                    }
                }
                else if (currentKeyboardState.IsKeyDown(Keys.Right))
                {
                    if (currentKeyboardState.IsKeyDown(Keys.LeftShift) ||
                        currentKeyboardState.IsKeyDown(Keys.RightShift))
                    {
                        position.X += elapsedTime * 2 * velocity.X;
                        steeringState = SteeringState.HardRight;
                    }
                    else
                    {
                        position.X += elapsedTime * velocity.X;
                        steeringState = SteeringState.Right;
                    }
                }
            }

            //Player is drunk and movements are reversed.
            else
            {
                //Decrease drunkCounter and make the player sober if their drunk time is up.
                drunkCounter--;
                if (drunkCounter == 0)
                {
                    SoberUp();
                }
                // Steer the ship up or down according to user input
                if (currentKeyboardState.IsKeyDown(Keys.Up))
                {
                    position.Y += elapsedTime * velocity.Y;
                }

                else if (currentKeyboardState.IsKeyDown(Keys.Down))
                {
                    position.Y -= elapsedTime * velocity.Y;
                }

                // Steer the ship left or right according to user input
                steeringState = SteeringState.Straight;

                if (currentKeyboardState.IsKeyDown(Keys.Left))
                {
                    if (currentKeyboardState.IsKeyDown(Keys.LeftShift) ||
                        currentKeyboardState.IsKeyDown(Keys.RightShift))
                    {
                        steeringState = SteeringState.HardLeft;
                        position.X += elapsedTime * 2 * velocity.X;

                    }
                    else
                    {
                        steeringState = SteeringState.Left;
                        position.X += elapsedTime * velocity.X;
                    }
                }
                else if (currentKeyboardState.IsKeyDown(Keys.Right))
                {
                    if (currentKeyboardState.IsKeyDown(Keys.LeftShift) ||
                        currentKeyboardState.IsKeyDown(Keys.RightShift))
                    {
                        position.X -= elapsedTime * 2 * velocity.X;
                        steeringState = SteeringState.HardRight;
                    }
                    else
                    {
                        position.X -= elapsedTime * velocity.X;
                        steeringState = SteeringState.Right;
                    }
                }
            }
            // Fire bomb
            if (currentKeyboardState.IsKeyDown(Keys.B))
            {
                //checks if player has the bomb power up
                if ((PowerupType & PowerupType.Bomb) > 0)
                {
                    if (bombTimer > 1.5f)
                    {
                        ScrollingShooterGame.GameObjectManager.CreateProjectile(ProjectileType.Bomb, position);
                        bombTimer = 0f;
                    }
                }
            }
            if (bladesPowerupTimer > 10.0f && (PowerupType & PowerupType.Blades) > 0)
            {
                unApplyBlades();
            }

            // Used to test the energy blast powerup levels
            //if (currentKeyboardState.IsKeyDown(Keys.F) && oldKeyboardState.IsKeyUp(Keys.F))
            //    energyBlastLevel++;
            if ((PowerupType & PowerupType.Blades) == 0)
            {
                // Fire weapons
                if (currentKeyboardState.IsKeyDown(Keys.Space))
                {
                    if ((PowerupType & PowerupType.Freezewave) > 0)
                    {
                        if (defaultGunTimer > .5f)
                        {
                            ScrollingShooterGame.GameObjectManager.CreateProjectile(ProjectileType.FreezewaveProjectile, position);
                        }
                    }
                    // Streaming weapons
                    if ((PowerupType & PowerupType.BubbleBeam) > 0)
                    {
                        if (defaultGunTimer > BubbleBullet.FIRE_INTERVAL_MS)
                        {
                            ScrollingShooterGame.GameObjectManager.CreateProjectile(ProjectileType.BubbleBullet, position);
                            defaultGunTimer = 0f;
                        }
                    }

                    // Fires a shotgun shot if the shotgun powerup is active and half a second has passed since the last shot
                    if ((PowerupType & PowerupType.ShotgunPowerup) > 0 && shotgunTimer > 0.5f)
                    {
                        TriggerShotgun();
                        shotgunTimer = 0;
                    }

                    // Default gun
                    if (defaultGunTimer > 0.25f)
                    {
                        ScrollingShooterGame.GameObjectManager.CreateProjectile(ProjectileType.Bullet, position);
                        defaultGunTimer = 0f;
                    }

                    if((PowerupType & PowerupType.HomingMissiles) > 0)
                    {
                        if (homingMissileTimer <= 0)
                        {
                            homingMissileTimer = homingMissileFireRate;
                            TriggerHomingMissile();
                        }
                    }

                    //Conditionals to fire railgun.
                    if ((PowerupType & PowerupType.Railgun) > 0)
                    {
                        if (railgunTimer > 3.0f)
                        {
                            TriggerRailgun();
                            railgunTimer = 0f;
                        }
                    }

                    // Energy Blast Gun
                    if (((PowerupType & PowerupType.EnergyBlast) > 0) && energyBlastTimer < 0)
                    {
                        TriggerEnergyBlast();
                    }

                    // Fire-once weapons
                    if (oldKeyboardState.IsKeyUp(Keys.Space))
                    {

                        if ((PowerupType & PowerupType.Fireball) > 0)
                            TriggerFireball();

                        if ((PowerupType & PowerupType.DroneWave) > 0)
                        {
                            TriggerDroneWave();
                        }
                    }

                    if ((PowerupType & PowerupType.Frostball) > 0)
                        TriggerFrostball();


                    if ((PowerupType & PowerupType.Birdcrap) > 0)
                    {
                        TriggerBirdcrap();
                    }

                    if ((PowerupType & PowerupType.Bomb) > 0)
                        TriggerBomb();
                }
            }

            // store the current keyboard state for next frame
            oldKeyboardState = currentKeyboardState;
        }


        /// <summary>
        /// Draw the ship on-screen
        /// </summary>
        /// <param name="elaspedTime">The elapsed time</param>
        /// <param name="spriteBatch">An already-initialized spritebatch, ready for Draw() commands</param>
        public override void Draw(float elaspedTime, SpriteBatch spriteBatch)
        {
            if ((PowerupType & PowerupType.Railgun) > 0)
                spriteBatch.Draw(spriteSheet, RailgunBounds, railgunSpriteBounds, Color.White);

            spriteBatch.Draw(spriteSheet, Position, spriteBounds[(int)steeringState], Color.White, 0f, new Vector2(Bounds.Width / 2, Bounds.Height / 2), 1f, SpriteEffects.None, LayerDepth);

            // Draw shadow
            spriteBatch.Draw(spriteSheet, new Vector2(20, 100), spriteBounds[(int)steeringState], Color.Black, 0f, new Vector2(Bounds.Width / 2, Bounds.Height / 2), 1f, SpriteEffects.None, LayerDepth);
        }


        /// <summary>
        /// A helper function that fires a fireball from the ship, 
        /// corresponding to the fireball powerup
        /// </summary>
        void TriggerFireball()
        {
            ScrollingShooterGame.GameObjectManager.CreateProjectile(ProjectileType.Fireball, position);
        }

        /// <summary>
        /// Fires the railgun sabot round from the ship,
        /// corresponding to the railgun powerup
        /// </summary>
        void TriggerRailgun()
        {
            ScrollingShooterGame.GameObjectManager.CreateProjectile(ProjectileType.RGSabot, 
                new Vector2(position.X + (Bounds.Width / 2) - 4, position.Y));
            //Simuated recoil
            position.Y += 10;
        }


        /// <summary>
        /// A helper function that starts a meteor storm,
        /// corresponding to the meteor powerup
        /// </summary>
        void TriggerMeteor()
        {
            //TODO: Constantly do a tiny amount of damage to all enemies during the storm.

            //Reduce object creation by creating variables before loop.
            Vector2 position = new Vector2();
            Random rand = new Random();

            //Add a bunch of decorative meteors
            for (int i = 0; i < 300; i++)
            {
                position.X = rand.Next(800);
                position.Y = -rand.Next(4000) - 200;

                ScrollingShooterGame.GameObjectManager.CreateProjectile(ProjectileType.Meteor, position);
			}
			//Add a few large meteors
            for (int i = 0; i < 10; i++)
            {
                position.X = 50 + rand.Next(800);
                position.Y = -rand.Next(8000) - 1000;

                ScrollingShooterGame.GameObjectManager.CreateProjectile(ProjectileType.BigMeteor, position);
            }
        }

        /// <summary>
        /// A helper function that shoots a spray shot from the ship
        /// when the spray shot powerup is active
        /// </summary>
        void TriggerShotgun()
        {
            ScrollingShooterGame.GameObjectManager.CreateProjectile(ProjectileType.ShotgunBullet, position);
        }

        /// <summary>
        /// Makes the player drunk.  If the player is already drunk the player is just made drunk for longer.  The drunk counter is
        /// increased by a random number.  Time to be drunk is between 5 and 10 seconds.
        /// </summary>
        void GetDrunk()
        {
            drunk = true;
            Random drunkRand = new Random();
            drunkCounter += drunkRand.Next(300, 600);
        }

        /// <summary>
        /// Makes the player sober.  Activated when the drunk time has run out.
        /// </summary>
        void SoberUp()
        {
            drunk = false;
        }

        public Vector2 GetPosition()
        {
            return position;
        }
        void TriggerBomb()
        {
            // TODO: Fire Bomb
        }


        /// <summary>
        /// A helper that fires birdcrap from the ship. Coraspondes to the birdcrap power up.
        /// </summary>
        void TriggerBirdcrap()
        {
            //TODO:Create effect and calculations when colides with enemy ship, the event will determin if the enimy is "blind" or if his ship will crash. Also when audio is implemented it will make a pooping sound and splat sounds.
            ScrollingShooterGame.GameObjectManager.CreateProjectile(ProjectileType.BirdCrap, position);
        }

        /// <summary>
        /// A helper function that fires a frostball from the ship, 
        /// corresponding to the frostball powerup
        /// </summary>
        void TriggerFrostball()
        {
            ScrollingShooterGame.GameObjectManager.CreateProjectile(ProjectileType.Frostball, position);
        }

        /// <summary>
        /// A helper function that initializes the blades powerup.
        /// //Puts a giant spinning blade over player position and doubles the players velocity.
        /// </summary>
        void ApplyBlades()
        {
            ScrollingShooterGame.GameObjectManager.CreateProjectile(ProjectileType.Blades, position);
            this.velocity *= 2;
            bladesPowerupTimer = 0;
            //TO DO: make player invulerable for 10 secs, since not implemented yet.
        }

        /// <summary>
        /// A helper function that will remove the Blade powerup and restore defaults.
        /// </summary>
        void unApplyBlades()
        {
            this.PowerupType = this.PowerupType ^= PowerupType.Blades;
            this.velocity /= 2;
            bladesPowerupTimer = 0;
            //TO DO: make player vulerable again, since not implemented yet.
        }


        /// <summary>
        /// Helper function to create an eightballshield around the ship
        /// </summary>
        void TriggerEightBallShield()
        {
            ScrollingShooterGame.GameObjectManager.CreateShield(ShieldType.EightBallShield, position, this);
        }

        /// <summary>
        /// A helper function that initializes the tri-shield.
        /// Creates three trishield balls.
        /// </summary>
        void ApplyTriShield()
        {
            //Create three balls, each at different rotations around the ship
            ScrollingShooterGame.GameObjectManager.CreateProjectile(ProjectileType.TrishieldBall, new Vector2(Bounds.Center.X, Bounds.Center.Y));
        }

        /// <summary>
        /// A helper function that fires a wide drone wave from the ship, corresponding to the fireball powerup.
        /// </summary>
        void TriggerDroneWave()
        {
            ScrollingShooterGame.GameObjectManager.CreateProjectile(ProjectileType.DroneWave, new Vector2(position.X, position.Y));
        }

        /// <summary>
        /// A helper function that fires an energy blast from the ship, 
        /// corresponding to the energy blast powerup
        /// </summary>
        void TriggerEnergyBlast()
        {
            energyBlastTimer = 0.5f;
            if (energyBlastLevel == 1)
                energyBlastTimer = 0.4f;
            else if (energyBlastLevel == 2)
                energyBlastTimer = 0.3f;
            else if (energyBlastLevel >= 3)
                energyBlastTimer = 0.25f;
            
            ScrollingShooterGame.GameObjectManager.CreateProjectile(ProjectileType.EnergyBlast, position);
        }

        /// <summary>
        /// Handles the firing of a homing missile
        /// </summary>
        void TriggerHomingMissile()
        {
            switch (HomingMissileLevel)
            {
                case 1:
                    ScrollingShooterGame.GameObjectManager.CreateProjectile(ProjectileType.HomingMissile, position);
                    ScrollingShooterGame.GameObjectManager.CreateProjectile(ProjectileType.HomingMissile, position);
                    ScrollingShooterGame.GameObjectManager.CreateProjectile(ProjectileType.HomingMissile, position);
                    break;
                case 2:
                    ScrollingShooterGame.GameObjectManager.CreateProjectile(ProjectileType.HomingMissile, position);
                    ScrollingShooterGame.GameObjectManager.CreateProjectile(ProjectileType.HomingMissile, position);
                    ScrollingShooterGame.GameObjectManager.CreateProjectile(ProjectileType.HomingMissile, position);
                    ScrollingShooterGame.GameObjectManager.CreateProjectile(ProjectileType.HomingMissile, position);
                    ScrollingShooterGame.GameObjectManager.CreateProjectile(ProjectileType.HomingMissile, position);
                    break;
                case 3:
                    ScrollingShooterGame.GameObjectManager.CreateProjectile(ProjectileType.HomingMissile, position);
                    ScrollingShooterGame.GameObjectManager.CreateProjectile(ProjectileType.HomingMissile, position);
                    ScrollingShooterGame.GameObjectManager.CreateProjectile(ProjectileType.HomingMissile, position);
                    ScrollingShooterGame.GameObjectManager.CreateProjectile(ProjectileType.HomingMissile, position);
                    ScrollingShooterGame.GameObjectManager.CreateProjectile(ProjectileType.HomingMissile, position);
                    ScrollingShooterGame.GameObjectManager.CreateProjectile(ProjectileType.HomingMissile, position);
                    ScrollingShooterGame.GameObjectManager.CreateProjectile(ProjectileType.HomingMissile, position);
                    break;
                default:
                    break;
            }
        }

        public void MoveShip(Vector2 direction)
        {
            Vector2 newDir = direction - position;
            newDir.Normalize();
            position += newDir * 2;
        }
    }
}