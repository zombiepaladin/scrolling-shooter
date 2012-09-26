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
        /// Constructs a new enemy
        /// </summary>
        /// <param name="id">The unique id of the enemy instance</param>
        public Enemy(uint id) : base(id) { }
    }
}
