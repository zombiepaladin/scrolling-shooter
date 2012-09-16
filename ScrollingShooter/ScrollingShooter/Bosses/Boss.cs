using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ScrollingShooter.Bosses
{
    /// <summary>
    /// The different enemy types that exist in the game
    /// </summary>
    public enum EnemyType
    {
        Dart,
        Blimp,
    }

    /// <summary>
    /// A base class for enemies in the game
    /// </summary>
    public abstract class Enemy : GameObject
    {
        /// <summary>
        /// The enemy's health
        /// </summary>
        public float Health;

        /// <summary>
        /// Constructs a new enemy
        /// </summary>
        /// <param name="id">The unique id of the enemy instance</param>
        public Enemy(uint id) : base(id) { }
    }
}
