using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ScrollingShooter
{
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
            spriteBatch.Draw(spriteSheet, spriteBounds, spriteSource, Color.White);
        }
    }
}
