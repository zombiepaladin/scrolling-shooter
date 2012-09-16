using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using System;

namespace ScrollingShooter
{
    /// <summary>
    /// A bullet for the bubblebeam powerup 
    /// </summary>
    public class BubbleBullet : Projectile
    {
        /// <summary>
        /// Minimum amount of elapsed time before firing another bullet.
        /// </summary>
        public static float FIRE_INTERVAL_MS = .05f;

        //Constants for the Bubble bullet.
        private const String SPRITESHEET = "Spritesheets/tyrian.shp.01D8A7";
        private static readonly Rectangle SPRITEBOUNDS = new Rectangle(217, 156, 5, 5);
        private static Random rand = new Random();

        /// <summary>
        /// Creates a new bubblebullet
        /// </summary>
        /// <param name="content">A ContentManager to load content from</param>
        /// <param name="position">Starting position on the screen</param>
        public BubbleBullet(uint id, ContentManager content, Vector2 position)
            : base(id)
        {
            this.spriteSheet = content.Load<Texture2D>(SPRITESHEET);
            this.spriteBounds = SPRITEBOUNDS;
            this.velocity = generateRandomVelocity(500);
            this.position = position;
        }

        /// <summary>
        /// Updates the bullet's position and velocity.
        /// </summary>
        /// <param name="elapsedTime">Elapsed time since last update.</param>
        public override void Update(float elapsedTime)
        {
            base.Update(elapsedTime);
            float yValue = Math.Min(velocity.Y + 10, -50);
            this.velocity.Y = yValue;
        }

        /// <summary>
        /// Generates a new random velocity within the given range.
        /// Direction will be any value from -range/2 to range/2
        /// </summary>
        /// <param name = "range">Range of the y value</param>
        /// <returns>Velocity of this bullet</returns>
        private Vector2 generateRandomVelocity(int range)
		{
			int randomNum = rand.Next(range);
			int yComponent = (randomNum / 2) * ((randomNum % 2 == 1) ? -1 : 1);
			return new Vector2(yComponent, -600);
		}
    }
}