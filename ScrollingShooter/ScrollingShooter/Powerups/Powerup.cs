using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ScrollingShooter
{
    /// <summary>
    /// Represents all the possible powerups our ship might pick up; uses
    /// a bitmask so multiple powerups can be represented with a single variable
    /// </summary>
    public enum PowerupType
    {
        None = 0,
        Fireball = 0x1,
        EightBallShield = 0x2,
    }

    /// <summary>
    /// An abstract base class for powerups
    /// </summary>
    public abstract class Powerup : GameObject
    {
        /// <summary>
        /// The spritesheet the powerup is found on
        /// </summary>
        protected Texture2D spriteSheet;

        /// <summary>
        /// The portion of the spritesheet containing the powerup
        /// </summary>
        protected Rectangle spriteSource;

        /// <summary>
        /// The bounding rectangle of the powerup in the game
        /// </summary>
        protected Rectangle spriteBounds;
        
        /// <summary>
        /// The location of the powerup in the game world
        /// </summary>
        public override Rectangle Bounds
        {
            get { return spriteBounds; }
        }

        /// <summary>
        /// The type of powerup this powerup is
        /// </summary>
        protected PowerupType type;
        public PowerupType Type
        {
            get { return type; }
        }

        /// <summary>
        /// Creates a new instance of a powerup
        /// </summary>
        /// <param name="id">The powerup's unique id</param>
        public Powerup(uint id)
            : base(id)
        { }

        /// <summary>
        /// Updates the powerup.
        /// </summary>
        /// <param name="elaspsedTime">The elapsed game time</param>
        public override void Update(float elaspsedTime)
        {
            // The powerup takes no actions
        }

        /// <summary>
        /// Draws the powerup
        /// </summary>
        /// <param name="elaspedTime">The elapsed game time</param>
        /// <param name="spriteBatch">
        /// An already-initialized SpriteBatch, ready for
        /// Draw() calls.
        /// </param>
        public override void Draw(float elaspedTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(spriteSheet, Bounds, spriteSource, Color.White, 0f, new Vector2(Bounds.Width / 2, Bounds.Height / 2), SpriteEffects.None, 1f);
        }
    }
}
