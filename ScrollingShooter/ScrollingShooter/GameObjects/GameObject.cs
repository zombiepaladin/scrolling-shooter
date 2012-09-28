using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace ScrollingShooter
{
    /// <summary>
    /// Object's type for use in collisions
    /// </summary>
    public enum ObjectType
    {
        Player = 0,
        PlayerProjectile = 1,
        Enemy = 2,
        EnemyProjectile = 3,
        Boss = 4,
        Powerup = 5,
        Shield = 6,
        Other = 7, // used as the default for random things like explosions
    }

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

        /// Object's type for use in collisions
        /// </summary>
        public ObjectType ObjectType = ObjectType.Other;

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
        /// The speed at which this game object scrolls (if any)
        /// </summary>
        public float ScrollingSpeed { get; set; }

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
