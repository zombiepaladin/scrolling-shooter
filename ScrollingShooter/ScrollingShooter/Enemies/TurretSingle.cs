using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace ScrollingShooter
{
    /// <summary>
    /// An enemy turret that turns to the player and fires at them
    /// </summary>
    public class TurretSingle : Enemy
    {   
        // Turret Variables
        /// <summary>
        /// spritesheet with the turret texture
        /// </summary>
        Texture2D spritesheet;

        /// <summary>
        /// Position of the turret
        /// </summary>
        Vector2 position;

        /// <summary>
        /// Bounds of the turret on the spritesheet
        /// </summary>
        Rectangle spriteBounds = new Rectangle();

        /// <summary>
        /// Offset from the center to the tip of the barrel
        /// </summary>
        Vector2 offset;

        /// <summary>
        /// Rotation of the turret
        /// </summary>
        float alpha;

        /// <summary>
        /// Shot delay of the turret
        /// </summary>
        float shotDelay;

        /// <summary>
        /// Bullet velocity
        /// <summary>
        float bulletVel = 200f;

        /// <summary>
        /// The bounding rectangle of the Dart
        /// </summary>
        public override Rectangle Bounds
        {
            get { return new Rectangle((int)position.X, (int)position.Y, spriteBounds.Width, spriteBounds.Height); }
        }

        /// <summary>
        /// Creates a new instance of an enemy turret
        /// </summary>
        /// <param name="content">A ContentManager to load resources with</param>
        /// <param name="position">The position of the turret in the game world</param>
        public TurretSingle(uint id, ContentManager content, Vector2 position)
            :base(id)
        {
            this.position = position;

            spritesheet = content.Load<Texture2D>("Spritesheets/newsh2.shp.000000");

            spriteBounds.X = 29;
            spriteBounds.Y = 35;
            spriteBounds.Width = 13;
            spriteBounds.Height = 17;

            alpha = 0;

            shotDelay = 0;

            offset = new Vector2(0, (float)this.Bounds.Height / 2);

            Health = 15;
        }

        /// <summary>
        /// Updates the Turret
        /// </summary>
        /// <param name="elapsedTime">The in-game time between the previous and current frame</param>
        public override void  Update(float elapsedTime)
        {
            // Update the shot timer
            shotDelay += elapsedTime;

            // Sense the player's position
            PlayerShip player = ScrollingShooterGame.Game.Player;
            Vector2 playerPosition = new Vector2(player.Bounds.X, player.Bounds.Y);

            // Get a vector from our position to the player's position
            Vector2 toPlayer = playerPosition - this.position;

            if(toPlayer.LengthSquared() < 150000)
            {
                // We sense the player's ship!                  
                // Get a normalized turning vector
                toPlayer.Normalize();

                // Rotate towards them!
                this.alpha = (float)Math.Atan2(toPlayer.Y, toPlayer.X) - MathHelper.PiOver2;

                // If it is time to shoot, fire a bullet towards the player
                if (shotDelay > 1.5f)
                {
                    // Rotation Matrix to get the rotated offset vectors
                    Matrix rotMatrix = Matrix.CreateRotationZ(alpha);

                    // Offset vector that adjusts according to rotation we are using
                    Vector2 offsetTemp = Vector2.Transform(offset, rotMatrix); 

                    // Spawn the bullet
                    ScrollingShooterGame.GameObjectManager.CreateProjectile(ProjectileType.EnemyBullet, this.position + offsetTemp, bulletVel * toPlayer);

                    // Reset the shot delay
                    shotDelay = 0;
                }

            }                        
        }

        /// <summary>
        /// Draw the turret on-screen
        /// </summary>
        /// <param name="elapsedTime">The in-game time between the previous and current frame</param>
        /// <param name="spriteBatch">An already initialized SpriteBatch, ready for Draw() commands</param>
        public override void Draw(float elapsedTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(spritesheet, Bounds, spriteBounds, Color.White, alpha, new Vector2(spriteBounds.Width / 2, spriteBounds.Height / 2), SpriteEffects.None, 0f);
        }

    }
}
