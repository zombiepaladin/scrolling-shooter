using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ScrollingShooter
{
    /// <summary>
    /// The different boss types that exist in the game
    /// </summary>
    public enum BossType
    {
        GiantAlien,
        Blimp,
        TwinJetManager,
        MoonBoss,
    }

    /// <summary>
    /// A base class for bosses in the game
    /// </summary>
    public abstract class Boss : GameObject
    {
        /// <summary>
        /// The boss's health
        /// </summary>
        public float Health;
        /// <summary>
        /// The boss's point value
        /// </summary>
        public int Score = 100;

        /// <summary>
        /// The spritesheet the powerup is found on
        /// </summary>
        public Texture2D spritesheet;

        /// <summary>
        /// The bounding rectangle of the powerup in the game
        /// </summary>
        protected Rectangle spriteBounds;

        /// <summary>
        /// The position of the Powerup
        /// </summary>
        protected Vector2 position;

        /// <summary>
        /// The location of the powerup in the game world
        /// </summary>
        public override Rectangle Bounds
        {
            get { return new Rectangle((int)position.X, (int)position.Y, spriteBounds.Width, spriteBounds.Height); }
        }

        /// <summary>
        /// Constructs a new boss
        /// </summary>
        /// <param name="id">The unique id of the enemy instance</param>
        public Boss(uint id) : base(id) { }

        /// <summary>
        /// Scrolls the object with the map
        /// </summary>
        /// <param name="elapsedTime">The in-game time between the previous and current frame</param>
        public override void ScrollWithMap(float elapsedTime)
        {
            position.Y += ScrollingSpeed * elapsedTime;
        }
    }
}
