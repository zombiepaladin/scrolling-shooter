using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;

namespace ScrollingShooter
{
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
    /// Represents all the possible powerups our ship might pick up; uses
    /// a bitmask so multiple powerups can be represented with a single variable
    /// </summary>
    public enum Powerups
    {
        None = 0,
        Fireball = 0x1,
        // Enum value for Railgun,  may need to change to accomodate number overlapping with other powerups
        Railgun = 0x8
    }

    /// <summary>
    /// A base class for all player ships
    /// </summary>
    public abstract class PlayerShip : GameObject
    {
        float defaultGunTimer = 0;
        
        /// <summary>
        /// Timer to adjust refire rate of railgun
        /// </summary>
        float railgunTimer = 0;

        /// <summary>
        /// The velocity of the ship - varies from ship to ship
        /// </summary>
        protected Vector2 velocity;

        /// <summary>
        /// The position of the ship in the game world.  We use a Vector2 for position 
        /// rather than a Rectangle, as floats allow us to move less than a pixel
        /// </summary>
        protected Vector2 position;

        /// <summary>
        /// The spritesheet our ship is found upon
        /// </summary>
        protected Texture2D spriteSheet;

        /// <summary>
        /// The spritebounds for our ship, corresponding to the five possible steering states
        /// </summary>
        protected Rectangle[] spriteBounds = new Rectangle[5];

        /// <summary>
        /// Rectangle to draw the railgun when the powerup is enabled
        /// </summary>
        protected Rectangle railgunSpriteBounds = new Rectangle(146, 55, 8, 33);

        /// <summary>
        /// The keyboard state from the previous frame, used to see if a key was just pressed,
        /// or held down
        /// </summary>
        KeyboardState oldKeyboardState = Keyboard.GetState();

        // The current steering state of the ship
        SteeringState steeringState = SteeringState.Straight;

        // The powerups equipped on this ship
        Powerups powerups = Powerups.None;


        /// <summary>
        /// The bounding rectangle for the ship.  Generated from the animation frame and the ship's
        /// position.
        /// </summary>
        public override Rectangle Bounds
        {
            get { return new Rectangle((int)position.X, (int)position.Y, spriteBounds[0].Width, spriteBounds[0].Height); }
        }

        public Rectangle RailgunBounds
        {
            get { return new Rectangle((int)(position.X+(Bounds.Width/2)-4), (int)(position.Y-(Bounds.Height/2)), railgunSpriteBounds.Width, railgunSpriteBounds.Height); }
        }


        /// <summary>
        /// Applies the specified powerup to the ship
        /// </summary>
        /// <param name="powerup">the indicated powerup</param>
        public void ApplyPowerup(Powerups powerup)
        {
            // Store the new powerup in the powerups bitmask
            this.powerups |= powerup;
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

            railgunTimer += elapsedTime;
            

            // Steer the ship up or down according to user input
            if(currentKeyboardState.IsKeyDown(Keys.Up))
            {
                position.Y -= elapsedTime * velocity.Y;
            } 
            else if(currentKeyboardState.IsKeyDown(Keys.Down))
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

            // Fire weapons
            if (currentKeyboardState.IsKeyDown(Keys.Space))
            {
                // Streaming weapons

                // Default gun
                if (defaultGunTimer > 0.25f)
                {
                    ScrollingShooterGame.Game.projectiles.Add(new Bullet(ScrollingShooterGame.Game.Content, position));
                    defaultGunTimer = 0f;
                }

                //Conditionals to fire railgun.
                if ((powerups & Powerups.Railgun) > 0)
                {
                    if (railgunTimer > 3.0f)
                    {
                        TriggerRailgun();
                        railgunTimer = 0f;
                    }
                }

                // Fire-once weapons
                if (oldKeyboardState.IsKeyUp(Keys.Space))
                {
                    if ((powerups & Powerups.Fireball) > 0)
                        TriggerFireball();
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
            if ((powerups & Powerups.Railgun) > 0)
                spriteBatch.Draw(spriteSheet, RailgunBounds, railgunSpriteBounds, Color.White);

            spriteBatch.Draw(spriteSheet, Bounds, spriteBounds[(int)steeringState], Color.White);
        }


        /// <summary>
        /// A helper function that fires a fireball from the ship, 
        /// corresponding to the fireball powerup
        /// </summary>
        void TriggerFireball()
        {
            // TODO: Fire fireball
        }
        
        /// <summary>
        /// Fires the railgun sabot round from the ship,
        /// corresponding to the railgun powerup
        /// </summary>
        void TriggerRailgun()
        {
            ScrollingShooterGame.Game.projectiles.Add(new RGSabot(ScrollingShooterGame.Game.Content, 
                new Vector2(position.X+(Bounds.Width/2)-4,position.Y)));
            //Simuated recoil
            position.Y += 10;
        }
    }
}
