using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ScrollingShooter
{
    /// <summary>
    /// The different boss types that exist in the game
    /// </summary>
    public enum BossType
    {
        Dart,
    }

    /// <summary>
    /// A base class for enemies in the game
    /// </summary>
    public abstract class Boss : GameObject
    {
        /// <summary>
        /// The enemy's health
        /// </summary>
        public float Health;

        /// <summary>
        /// Constructs a new enemy
        /// </summary>
        /// <param name="id">The unique id of the enemy instance</param>
        public Boss(uint id) : base(id) { }
    }
}
