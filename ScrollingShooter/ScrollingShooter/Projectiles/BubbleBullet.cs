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
        private static int powerLevel = 0;
        public static int POWER_LEVEL
        {
            get { return powerLevel; }
            set
            {
                if (value <= 4)
                    powerLevel = value;
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
        private const int BASE_DAMAGE = 10;
        private static Random rand = new Random();
        
        //Instance vars
        private int _powerLevel;

        public int Damage
        {
            get
            {
                return _powerLevel * BASE_DAMAGE;
            }
        }

        /// <summary>
        /// Creates a new bubblebullet with the current power level.
        /// </summary>
        /// <param name="id">Id of the object.</param>
        /// <param name="content">A ContentManager to load content from</param>
        /// <param name="position">Starting position on the screen</param>
        public BubbleBullet(uint id, ContentManager content, Vector2 position)
            : base(id)
        {
            this.spriteSheet = content.Load<Texture2D>(SPRITESHEET);
            this.spriteBounds = SPRITEBOUNDS[POWER_LEVEL - 1];
            this.velocity = generateRandomVelocity(500);
            this.position = position;
            this._powerLevel = POWER_LEVEL;
        }

        /// <summary>
        /// Creates a bubble bullet with the given power level.
        /// </summary>
        /// <param name="id">Id of the object</param>
        /// <param name="content">A ContentManager to load content from</param>
        /// <param name="position">Starting position on the screen</param>
        /// <param name="powerLevel">Power level to use.</param>
        public BubbleBullet(uint id, ContentManager content, Vector2 position, int powerLevel)
            : base(id)
        {
            this.spriteSheet = content.Load<Texture2D>(SPRITESHEET);
            this.spriteBounds = SPRITEBOUNDS[powerLevel];
            this.velocity = generateRandomVelocity(500);
            this.position = position;
            this._powerLevel = powerLevel;
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