using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace ScrollingShooter
{
    /// <summary>
    /// An enemy turret that turns to the player and fires at them
    /// </summary>
    public class TurretTower : Enemy
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
        Rectangle[] spriteBounds = new Rectangle[9];

        /// <summary>
        /// Offset from the center of the sprite to the center of the opening
        /// </summary>
        Vector2 offset;

        /// <summary>
        /// Int to determine which frame we are drawing
        /// </summary>
        int frame = 0; 

        /// <summary>
        /// Rotation of the turret
        /// </summary>
        float alpha;

        /// <summary>
        /// Shot delay of the turret
        /// </summary>
        float shotDelay;

        /// <summary>
        /// Frame delay of the turret
        /// </summary>
        float frameDelay;

        /// <summary>
        /// Bullet velocity
        /// <summary>
        float bulletVel = 200f;

        /// <summary>
        /// The bounding rectangle of the Dart
        /// </summary>
        public override Rectangle Bounds
        {
            get { return new Rectangle((int)position.X, (int)position.Y, spriteBounds[frame].Width, spriteBounds[frame].Height); }
        }

        /// <summary>
        /// Creates a new instance of an enemy turret
        /// </summary>
        /// <param name="content">A ContentManager to load resources with</param>
        /// <param name="position">The position of the turret in the game world</param>
        public TurretTower(uint id, ContentManager content, Vector2 position)
            :base(id)
        {
            this.position = position;

            spritesheet = content.Load<Texture2D>("Spritesheets/newshd.shp.000000");

            spriteBounds[0].X = 24;
            spriteBounds[0].Y = 140;
            spriteBounds[0].Width = 24;
            spriteBounds[0].Height = 28;

            spriteBounds[1].X = 48;
            spriteBounds[1].Y = 140;
            spriteBounds[1].Width = 24;
            spriteBounds[1].Height = 28;

            spriteBounds[2].X = 72;
            spriteBounds[2].Y = 140;
            spriteBounds[2].Width = 24;
            spriteBounds[2].Height = 28;

            spriteBounds[3].X = 96;
            spriteBounds[3].Y = 140;
            spriteBounds[3].Width = 24;
            spriteBounds[3].Height = 28;

            spriteBounds[4].X = 120;
            spriteBounds[4].Y = 140;
            spriteBounds[4].Width = 24;
            spriteBounds[4].Height = 28;

            spriteBounds[5].X = 144;
            spriteBounds[5].Y = 140;
            spriteBounds[5].Width = 24;
            spriteBounds[5].Height = 28;

            spriteBounds[6].X = 168;
            spriteBounds[6].Y = 140;
            spriteBounds[6].Width = 24;
            spriteBounds[6].Height = 28;

            spriteBounds[7].X = 192;
            spriteBounds[7].Y = 140;
            spriteBounds[7].Width = 24;
            spriteBounds[7].Height = 28;

            spriteBounds[8].X = 192;
            spriteBounds[8].Y = 112;
            spriteBounds[8].Width = 24;
            spriteBounds[8].Height = 28;

            alpha = 0;

            shotDelay = 0;

            frameDelay = 0;

            offset = new Vector2(-1, -6);

            Health = 20;
        }

        /// <summary>
        /// Updates the Turret
        /// </summary>
        /// <param name="elapsedTime">The in-game time between the previous and current frame</param>
        public override void  Update(float elapsedTime)
        {
            // Update the frame timer
            frameDelay += elapsedTime;

            if (frameDelay > 0.15f && shotDelay == 0 && frame > 0)
            {
                // Decrement the frame number
                frame--;
            }

            if (frame == 0)
            {
                // Restart the animation
                frame = 0;
                // Update the shot timer
                shotDelay += elapsedTime;
            }

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
                if (shotDelay > 1f)
                {
                    if ((frameDelay > 0.15f && frame < 6) || (frameDelay > 0.24f && frame >= 6))
                    {
                        // Reset the frame delay
                        frameDelay = 0;

                        // Increment the frame number
                        frame++;
                    }

                    if (frame >= 8)
                    {
                        // Reset the shot delay
                        shotDelay = 0;

                        // Spawn the bullet
                        ScrollingShooterGame.GameObjectManager.CreateProjectile(ProjectileType.EnemyTurretTowerBullet, this.position + offset, bulletVel * toPlayer);
                    }
                }
            }
            else if (frameDelay > 0.15f && frame > 0)
            {
                // Decrement the frame number
                frame--;
            }        
        }

        /// <summary>
        /// Draw the turret on-screen
        /// </summary>
        /// <param name="elapsedTime">The in-game time between the previous and current frame</param>
        /// <param name="spriteBatch">An already initialized SpriteBatch, ready for Draw() commands</param>
        public override void Draw(float elapsedTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(spritesheet, Bounds, spriteBounds[frame], Color.White, 0, new Vector2(spriteBounds[frame].Width / 2, spriteBounds[frame].Height / 2), SpriteEffects.None, 0f);
        }

    }
}
