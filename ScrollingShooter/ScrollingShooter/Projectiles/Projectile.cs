using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ScrollingShooter
{
    /// <summary>
    /// The different types of projectiles available in the game
    /// </summary>
    public enum ProjectileType
    {
        // Player projectiles
        Bullet,
        Fireball,
        ShotgunBullet,
        BubbleBullet,
        Bomb,
        Frostball,
        Blades,
        TrishieldBall,
        BirdCrap,
        DroneWave,
        EnergyBlast,
        Meteor,
        BigMeteor,
        HomingMissile,
        FreezewaveProjectile,

        // Enemy projectiles start with an index of 100;
        // this allows us to differentiate between projectiles
        // without needing a second base class
        BlueBeam = 100,
        EBullet = 101,
        TurretFireball = 102,
        EnemyBullet = 103,
        ArrowProjectile = 104,
        EnemyBomb = 105,
        ToPlayerBullet = 106,
        JetMinionBullet = 107,
        EnemyFlameball = 108,
        GenericEnemyBullet = 109,
        DroneLaser = 110,
        RGSabot = 111,
        Photon = 112,
        BlimpShotgun = 113,
        BlimpBullet = 114,
        Pincher = 115, 
        GreenOrb = 116,
        AlienTurretOrb = 117,
        TwinJetBullet = 118,
        TwinJetMissile = 119,
        Laser = 120,
        BirdWrath = 121,
        EnemyPsyBall = 163,
        EnemyLightningZap = 164,
        EnemyTurretTowerBullet = 165,
    }

    /// <summary>
    /// A base class for all projectiles
    /// </summary>
    public abstract class Projectile : GameObject
    {
        public static int POWER_LEVEL = 1;

        public float Damage = 1;

        /// <summary>
        /// The projectile's velocity
        /// </summary>
        protected Vector2 velocity;

        /// <summary>
        /// The projectile's position in the game world
        /// </summary>
        protected Vector2 position;

        /// <summary>
        /// The spritesheet containing the projectile
        /// </summary>
        protected Texture2D spriteSheet;

        /// <summary>
        /// The position of the projectile's sprite on the 
        /// spritesheet
        /// </summary>
        protected Rectangle spriteBounds;

        /// <summary>
        /// The location of the sprite in the game world
        /// </summary>
        public override Rectangle Bounds
        {
            get { return new Rectangle((int)position.X, (int)position.Y, spriteBounds.Width, spriteBounds.Height); }
        }

        /// <summary>
        /// Creates a new instance of a projectile
        /// </summary>
        /// <param name="id">The unique id of the projectile</param>
        public Projectile(uint id) : base(id) { }

        /// <summary>
        /// Updates the projectile
        /// </summary>
        /// <param name="elapsedTime">The time elapsed between the previous and current frame</param>
        public override void Update(float elapsedTime)
        {
            position += velocity * elapsedTime;
        }


        /// <summary>
        /// Draws the projectile on-screen
        /// </summary>
        /// <param name="elapsedTime">The elapsed time between the previous and current frame</param>
        /// <param name="spriteBatch">An already-initialized SpriteBatch, ready for Draw() commands</param>
        public override void Draw(float elapsedTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(spriteSheet, Bounds, spriteBounds, Color.White, 0f, new Vector2(Bounds.Width / 2, Bounds.Height / 2), SpriteEffects.None, 1f);
        }
    }
}
