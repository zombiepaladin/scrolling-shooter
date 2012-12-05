using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ScrollingShooter
{
    /// <summary>
    /// The different enemy types that exist in the game
    /// </summary>
    public enum EnemyType
    {
        Dart,
        GreenGoblin,
        LaserDrone,
		Cobalt,
        JetMinion,
        Seed,
        Bomber,
        Arrow,
        LavaFighter,
        StdBaddy,
        BeamShip,
        Kamikaze,
        Panzer,
        Panzer2,
        Lavabug,
        Lavabug2,
        Mandible,
        BladeSpinner,
        DeerTickDown,
        DeerTickRight,
        DeerTickLeft,
        Turret,
        JTurret,
        DrillLeft,
        DrillRight,
        SuicideBomber,
        AlienHead,
        Asteriod1,
        Asteriod2,
        Asteriod3,
        Asteriod4,
        ShieldGenerator,
        AlienTurret,
        RightClaw,
        LeftClaw,
        TwinJet,
        Bird,
        BrainBoss,
        BrainBossPsyEmitter,
        BrainBossProtection,
        TurretSingle,
        TurretDouble,
        TurretTower,
        Mine,
        Rock,
        MoonSpiner,
        MoonShield,
    }

    /// <summary>
    /// A base class for enemies in the game
    /// </summary>
    public abstract class Enemy : GameObject
    {
        /// <summary>
        /// The enemy's health
        /// </summary>
        public float Health = 1;

        /// <summary>
        /// The enemy's point value
        /// </summary>
        public int Score = 10;

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
        /// Constructs a new enemy
        /// </summary>
        /// <param name="id">The unique id of the enemy instance</param>
        public Enemy(uint id) : base(id) { }

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
