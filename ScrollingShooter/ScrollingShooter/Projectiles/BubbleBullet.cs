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
        private static int PL = 0;
        public static int POWER_LEVEL
        {
            get { return PL; }
            set
            {
                if (value <= 4)
                    PL = value;
            }
        }

        //Constants for the Bubble bullet.
        private const String SPRITESHEET = "Spritesheets/tyrian.shp.01D8A7";
        private static readonly Rectangle[] SPRITEBOUNDS = new Rectangle[] 
        {
            new Rectangle(14, 2, 6, 6),
            new Rectangle(24, 1, 8, 8),
            new Rectangle(35, 0, 11, 11),
            new Rectangle(48, 0, 11, 11)
        };
        private const int BASE_DAMAGE = 2;
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
            this.spriteBounds = SPRITEBOUNDS[POWER_LEVEL];
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