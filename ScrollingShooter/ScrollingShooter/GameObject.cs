using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ScrollingShooter
{
    /// <summary>
    /// The base class for all game object
    /// </summary>
    public abstract class GameObject
    {
        /// <summary>
        /// The bounds of this game object in the game world
        /// </summary>
        public abstract Rectangle Bounds { get; }

        /// <summary>
        /// Updates the game object - called every frame
        /// </summary>
        /// <param name="elaspsedTime"></param>
        public abstract void Update(float elapsedTime);

        /// <summary>
        /// Renders the game object - called every frame
        /// </summary>
        /// <param name="elaspedTime"></param>
        /// <param name="spriteBatch"></param>
        public abstract void Draw(float elapsedTime, SpriteBatch spriteBatch);
    }
}
