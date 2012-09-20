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
        /// The game object's ID, which should be unique and never change
        /// We use the readonly keyword so it can only be set at object creation
        /// </summary>
        public readonly uint ID;

        /// <summary>
        /// The depth at which this GameObject's sprite is drawn
        /// </summary>
        public float LayerDepth;

        /// <summary>
        /// The bounds of this game object in the game world
        /// </summary>
        public abstract Rectangle Bounds { get; }

        /// <summary>
        /// Creates a new GameObject with the specified ID
        /// </summary>
        /// <param name="id">The game object's id</param>
        public GameObject(uint id)
        {
            this.ID = id;
        }

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
