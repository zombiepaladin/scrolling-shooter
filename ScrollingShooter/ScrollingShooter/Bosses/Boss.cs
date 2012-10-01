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
            // Does nothing
        }
    }
}
