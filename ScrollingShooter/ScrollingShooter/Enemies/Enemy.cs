using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
        Rock
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
            // Does nothing
        }
    }
}
