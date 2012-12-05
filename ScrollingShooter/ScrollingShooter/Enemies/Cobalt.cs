using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using System;

namespace ScrollingShooter
{

    /// <summary>
    /// An enemy ship that flies toward the player and fires
    /// </summary>
    public class Cobalt : Enemy
    {
        // Cobalt state variables
        Rectangle spriteBounds;
        float delay;
        
        /// <summary>
        /// The bounding rectangle of the Cobalt
        /// </summary>
        public override Rectangle Bounds
        {
            get { return new Rectangle((int)position.X, (int)position.Y, spriteBounds.Width, spriteBounds.Height); }
        }

        /// <summary>
        /// Creates a new instance of a Cobalt enemy ship
        /// </summary>
        /// <param name="content">A ContentManager to load resources with</param>
        /// <param name="position">The position of the Cobalt ship in the game world</param>
        public Cobalt(uint id, ContentManager content, Vector2 position)
            : base(id)
        {
            this.position = position;

            spritesheet = content.Load<Texture2D>("Spritesheets/newsh2.shp.000000");

            spriteBounds.X = 100;
            spriteBounds.Y = 55;
            spriteBounds.Width = 37;
            spriteBounds.Height = 28;

            Health = 5;
        }

        /// <summary>
        /// Updates the Cobalt ship
        /// </summary>
        /// <param name="elapsedTime">The in-game time between the previous and current frame</param>
        public override void Update(float elapsedTime)
        {
            // Sense the player's position
            PlayerShip player = ScrollingShooterGame.Game.Player;
            Vector2 playerPosition = new Vector2(player.Bounds.Center.X, player.Bounds.Center.Y);
            delay += elapsedTime;

            // Get a vector from our position to the player's position
            Vector2 toPlayer = playerPosition - this.position;

            //Sense the player from longer away but move slower.
            if (toPlayer.LengthSquared() < 160000)
            {
                if (toPlayer.LengthSquared() < 10000)
                {
                    if (delay > .5f)
                    {
                        //Player is close fire weapons
                        Vector2 travel = position;
                        travel.X += Bounds.Width / 2;
                        travel.Y += Bounds.Height / 2;
                        ScrollingShooterGame.GameObjectManager.CreateProjectile(ProjectileType.CobaltBomb, travel);
                        delay = 0;
                    }
                }
                // We sense the player's ship!                  
                // Get a normalized steering vector
                toPlayer.Normalize();

                // Steer towards them!
                this.position += toPlayer * elapsedTime * 50;

            }

        }

        public override void ScrollWithMap(float elapsedTime)
        {
            this.position.Y += ScrollingSpeed * elapsedTime;
        }

        /// <summary>
        /// Draw the Cobalt ship on-screen
        /// </summary>
        /// <param name="elapsedTime">The in-game time between the previous and current frame</param>
        /// <param name="spriteBatch">An already initialized SpriteBatch, ready for Draw() commands</param>
        public override void Draw(float elapsedTime, SpriteBatch spriteBatch)
        {
            PlayerShip player = ScrollingShooterGame.Game.Player;
            Vector2 playerPosition = new Vector2(player.Bounds.Center.X, player.Bounds.Center.Y);
            Vector2 toPlayer = playerPosition - this.position;
            double angle = (2 * Math.PI) - Math.Atan2(toPlayer.X, toPlayer.Y);
            if (toPlayer.LengthSquared() < 90000)
            {
                spriteBatch.Draw(spritesheet, Bounds, spriteBounds, Color.White, (float)angle, new Vector2(Bounds.Width / 2, Bounds.Height / 2), SpriteEffects.None, 1f);
            }
            else
                spriteBatch.Draw(spritesheet, Bounds, spriteBounds, Color.White);
        }

    }
    public class CobaltBomb : Projectile
    {
        /// <summary>
        /// Creates a new blimp bullet
        /// </summary>
        /// <param name="content">A ContentManager to load content from</param>
        /// <param name="position">A position on the screen</param>
        public CobaltBomb(uint id, ContentManager content, Vector2 position)
            : base(id)
        {
            this.spriteSheet = content.Load<Texture2D>("Spritesheets/newsh(.shp.000000");

            this.spriteBounds = new Rectangle(48, 197, 13, 11);

            this.position = position;

            // Sense the player's position
            PlayerShip player = ScrollingShooterGame.Game.Player;
            Vector2 playerPosition = new Vector2(player.Bounds.Center.X, player.Bounds.Center.Y);

            // Get a vector from our position to the player's position
            Vector2 toPlayer = playerPosition - this.position;

            velocity = toPlayer;

        }
    }
}