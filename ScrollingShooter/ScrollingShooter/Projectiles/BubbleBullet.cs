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
        public static float FIRE_INTERVAL_MS = .1f;

        private const String SPRITESHEET = "Spritesheets/tyrian.shp.01D8A7";
        private static readonly Rectangle SPRITEBOUNDS = new Rectangle(38, 57, 7, 11);
        private static Random rand = new Random();

        /// <summary>
        /// Creates a new bubblebullet
        /// </summary>
        /// <param name="content">A ContentManager to load content from</param>
        /// <param name="position">A position on the screen</param>
        public BubbleBullet(ContentManager content, Vector2 position)
        {
            this.spriteSheet = content.Load<Texture2D>(SPRITESHEET);
            this.spriteBounds = SPRITEBOUNDS;
            this.velocity = generateRandomVelocity(500);
            this.position = position;
        }

        /// <summary>
        /// Generates a new random velocity within the given range.
        /// Direction will be any value from -range/2 to range/2
        /// </summary>
        /// <param name = "range"> </param>
        /// <returns></returns>
        private Vector2 generateRandomVelocity(int range)
		{
			int randomNum = rand.Next(range);
			int yComponent = (randomNum / 2) * ((randomNum % 2 == 1) ? -1 : 1);
			return new Vector2(yComponent, -600);
		}
    }
}