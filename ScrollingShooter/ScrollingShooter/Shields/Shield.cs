using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ScrollingShooter
{
    /// <summary>
    /// My attempt at a base class for all shields
    /// Author: Josh Zavala
    /// </summary>

    public enum ShieldType
    {
        EightBallShield,
    }
    
    public abstract class Shield : GameObject
    {
        /// <summary>
        /// The shiled's velocity, perhaps for rotation?
        /// </summary>
        protected Vector2 velocity;

        /// <summary>
        /// The shield's position in the game world
        /// </summary>
        protected Vector2 position;

        /// <summary>
        /// The spritesheet containting the shield
        /// </summary>
        protected Texture2D spriteSheet;

        /// <summary>
        /// The position of the projectile's sprite on the 
        /// spritesheet
        /// </summary>
        protected Rectangle spriteBounds;

        /// <summary>
        /// Creates a new instance of a shield
        /// </summary>
        /// <param name="id">the unique id of the shield</param>
        public Shield(uint id) : base(id) { }

        /// <summary>
        /// The location of the sprite in the game world,
        /// although we want it to follow the player ship's movement
        /// </summary>
        public override Rectangle Bounds
        {
            get
            {
                return new Rectangle((int)position.X, (int)position.Y,
                    spriteBounds.Width, spriteBounds.Height);
            }
        }

        /// <summary>
        /// Updates the shield
        /// Perhaps this is where rotation can be used?
        /// </summary>
        /// <param name="elapsedTime">The elapsed time between the current and previous frame</param>
        public override void Update(float elapsedTime)
        {
            position += velocity * elapsedTime;
        }

        /// <summary>
        /// Draws the shield on-screen
        /// </summary>
        /// <param name="elapsedTime">The elpased tiem between the current and previous frame</param>
        /// <param name="spriteBatch">An already-initialized SpriteBatch, ready for Draw() commands</param>
        public override void Draw(float elapsedTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(spriteSheet, Bounds, spriteBounds, Color.White);
        }
    }
}
