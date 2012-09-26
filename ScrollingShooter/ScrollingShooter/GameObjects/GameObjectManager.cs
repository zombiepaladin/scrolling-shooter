using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace ScrollingShooter
{
    /// <summary>
    /// A class for managing game objects
    /// </summary>
    public class GameObjectManager
    {
        ContentManager content;

        uint objectCount = 0;

        Dictionary<uint, GameObject> gameObjects;

        Queue<GameObject> createdGameObjects;
        Queue<GameObject> destroyedGameObjects;

        Dictionary<uint, BoundingBox> boundingBoxes;

        List<Bound> horizontalAxis;
        List<Bound> verticalAxis;

        HashSet<CollisionPair> horizontalOverlaps;
        HashSet<CollisionPair> verticalOverlaps;
        HashSet<CollisionPair> collisions;

        //control the lavabug boss
        bool lavaFlip;

        /// <summary>
        /// Constructs a new GameObjectManager instance
        /// </summary>
        /// <param name="content">A ContentManager to use in loading assets</param>
        public GameObjectManager(ContentManager content)
        {
            this.content = content;

            gameObjects = new Dictionary<uint, GameObject>();

            createdGameObjects = new Queue<GameObject>();
            destroyedGameObjects = new Queue<GameObject>();

            boundingBoxes = new Dictionary<uint, BoundingBox>();
            horizontalAxis = new List<Bound>();
            verticalAxis = new List<Bound>();

            horizontalOverlaps = new HashSet<CollisionPair>();
            verticalOverlaps = new HashSet<CollisionPair>();
            collisions = new HashSet<CollisionPair>();

            //lavabug
            lavaFlip = true;
        }


        /// <summary>
        /// Updates the GameObjectManager, and all objects in the game
        /// </summary>
        /// <param name="elapsedTime">The time between this and the previous frame</param>
        public void Update(float elapsedTime)
        {
            // Add newly created game objects
            while (createdGameObjects.Count > 0)
            {
                GameObject go = createdGameObjects.Dequeue();
                AddGameObject(go);
            }

            // Remove destroyed game objects
            while (destroyedGameObjects.Count > 0)
            {
                GameObject go = destroyedGameObjects.Dequeue();
                RemoveGameObject(go);
            } 
            
            // Update our game objects
            //foreach (GameObject go in gameObjects.Values)
            //{
            //    // Call the GameObject's own update method
            //    go.Update(elapsedTime);

            //    // Update the game object's bounds to reflect 
            //    // changes this frame
            //    UpdateGameObject(go.ID);
            //}

            // Update our axis lists
            UpdateAxisLists();
        }


        /// <summary>
        /// Draws all game Objects
        /// </summary>
        /// <param name="elapsedTime"></param>
        /// <param name="spriteBatch"></param>
        public void Draw(float elapsedTime, SpriteBatch spriteBatch)
        {
            foreach (GameObject go in gameObjects.Values)
            {
                go.Draw(elapsedTime, spriteBatch);
            }
        }


        #region Query Methods

        /// <summary>
        /// A HashSet containing all unique collisions for
        /// the current frame
        /// </summary>
        public HashSet<CollisionPair> Collisions
        {
            get { return collisions; }
        }

        /// <summary>
        /// Returns the GameObject instance corresponding to the specified 
        /// game object id
        /// </summary>
        /// <param name="id">The game object's ID</param>
        /// <returns>The specified GameObject</returns>
        public GameObject GetObject(uint id)
        {
            return gameObjects[id];
        }


        /// <summary>
        /// Removes the indicated game object from the game
        /// </summary>
        /// <param name="id">The game object's ID</param>
        /// <returns>The specified GameObject</returns>
        public GameObject DestroyObject(uint id)
        {
            GameObject go = gameObjects[id];
            destroyedGameObjects.Enqueue(go);
            return go;
        }


        /// <summary>
        /// Queries a rectangular region and retuns game object ids for
        /// all game objects within that region
        /// </summary>
        /// <param name="bounds">A bounding rectangle for the region of interest</param>
        /// <returns>An array of game object ids</returns>
        public uint[] QueryRegion(Rectangle bounds)
        {
            HashSet<uint> matches = new HashSet<uint>();

            // Find the minimal index in the horizontal axis list using binary search
            Bound left = new Bound(null, bounds.Left, BoundType.Min);

            int minHorizontalIndex = horizontalAxis.BinarySearch(left);
            if (minHorizontalIndex < 0)
            {
                // If the index returned by binary search is negative,
                // then our current bound value does not exist within the
                // axis (most common case).  The bitwise compliement (~) of
                // that index value indicates the index of the first element 
                // in the axis list *larger* than our bound, and therefore
                // the first potentailly intersecting item
                minHorizontalIndex = ~minHorizontalIndex;
            }

            Bound right = new Bound(null, bounds.Left, BoundType.Max);
            int maxHorizontalIndex = horizontalAxis.BinarySearch(right);
            if (maxHorizontalIndex < 0) maxHorizontalIndex = ~maxHorizontalIndex;

            for (int i = minHorizontalIndex; i < maxHorizontalIndex; i++)
            {
                matches.Add(horizontalAxis[i].Box.GameObjectID);
            }

            Bound top = new Bound(null, bounds.Left, BoundType.Min);
            int minVerticalIndex = verticalAxis.BinarySearch(top);
            if (minVerticalIndex < 0) minVerticalIndex = ~minVerticalIndex;

            Bound bottom = new Bound(null, bounds.Bottom, BoundType.Max);
            int maxVerticalIndex = verticalAxis.BinarySearch(bottom);
            if (maxVerticalIndex < 0) maxVerticalIndex = ~maxVerticalIndex;

            for (int i = minVerticalIndex; i < maxVerticalIndex; i++)
            {
                matches.Add(verticalAxis[i].Box.GameObjectID);
            }

            return matches.ToArray();
        }

        #endregion

        #region Factory Methods

        /// <summary>
        /// Helper method that provides the next unique game object ID
        /// </summary>
        /// <returns>The next unique game object id</returns>
        private uint NextID()
        {
            uint id = objectCount;
            objectCount++;
            return id;
        }


        /// <summary>
        /// Helper method for enqueueing our game object for creation at the 
        /// start of the next update cycle.  
        /// </summary>
        /// <param name="go">The ready-to-spawn game object</param>
        private void QueueGameObjectForCreation(GameObject go)
        {
            createdGameObjects.Enqueue(go);
        }


        public Boss CreateBoss(BossType enemyType, Vector2 position)
        {
            Boss boss;
            uint id = NextID();
            switch (enemyType)
            {
                case BossType.Blimp:
                    boss = new Blimp(id, position, content);
                    QueueGameObjectForCreation(new LeftGun(NextID(), content, boss as Blimp));
                    QueueGameObjectForCreation(new RightGun(NextID(), content, boss as Blimp));
                    break;

                case BossType.TwinJetManager:
                    boss = new TwinJetManager(id, content, position);
                    break;
                
                default:
                    throw new NotImplementedException("The boss type " + Enum.GetName(typeof(BossType), enemyType) + " is not supported");
            }
            boss.ObjectType = ObjectType.Boss;
            QueueGameObjectForCreation(boss);
            return boss;
        }


        /// <summary>
        /// Factory method for creating an explosion
        /// </summary>
        /// <param name="colliderID">The source of the explosion</param>
        /// <returns>The newly-spawned explosion</returns>

        public Explosion CreateExplosion(uint colliderID)
        {
            Explosion ex;
            uint id = NextID();
            Vector2 pos = new Vector2(GetObject(colliderID).Bounds.X, GetObject(colliderID).Bounds.Y);
            ex = new Explosion(id, pos, content);
            QueueGameObjectForCreation(ex);
            return ex;
        }


        /// <summary>
        /// Factory method to create a Player ship
        /// </summary>
        /// <param name="PlayerShipType">The type of Player ship to create</param>
        /// <param name="position">The position of the Player ship in the game world</param>
        /// <returns>The game object id of the Player ship</returns>
        public PlayerShip CreatePlayerShip(PlayerShipType PlayerShipType, Vector2 position)
        {
            PlayerShip PlayerShip;
            uint id = NextID();

            switch (PlayerShipType)
            {
                case PlayerShipType.Shrike:
                    PlayerShip = new ShrikeShip(id, content, position);
                    break;

                default:
                    throw new NotImplementedException("The Player ship type " + Enum.GetName(typeof(PlayerShipType), PlayerShipType) + " is not supported");
            }

            PlayerShip.ObjectType = ObjectType.Player;
            QueueGameObjectForCreation(PlayerShip);
            return PlayerShip;
        }


        /// <summary>
        /// Factory method for spawning a projectile
        /// </summary>
        /// <param name="projectileType">The type of projectile to create</param>
        /// <param name="position">The position of the projectile in the game world</param>
        /// <returns>The game object id of the projectile</returns>
        public Powerup CreatePowerup(PowerupType powerupType, Vector2 position)
        {
            Powerup powerup;
            uint id = NextID();

            switch (powerupType)
            {
                case PowerupType.Fireball:
                    powerup = new FireballPowerup(id, content, position);
                    break;

                case PowerupType.Frostball:
                    powerup = new FrostballPowerup(id, content, position);
                    break;

                case PowerupType.BubbleBeam:
                    powerup = new BubbleBeamPowerup(id, content, position);
                    break;

                case PowerupType.Freezewave:
                    powerup = new FreezewavePowerup(id, content, position);
                    break;
                
                case PowerupType.Blades:
                    powerup = new BladesPowerup(id, content, position);
                    break;
                
                case PowerupType.EightBallShield: //added EightBallShield
                    powerup = new EightBallShieldPowerup(id, content, position);
                    break;
                
                case PowerupType.TriShield:
                    powerup = new TriShieldPowerup(id, content, position);
                    break;

                case PowerupType.DroneWave:
                    powerup = new DroneWavePowerup(id, content, position);
                    break;

                case PowerupType.Birdcrap:
                    powerup = new BirdcrapPowerup(id, content, position);
                    break;

                case PowerupType.EnergyBlast:
                    powerup = new EnergyBlastPowerup(id, content, position);
                    break;

                case PowerupType.Bomb:
                    powerup = new BombPowerUp(id, content, position);
                    break;

                case PowerupType.Ale:
                    powerup = new AlePowerup(id, content, position);
                    break;

                case PowerupType.ShotgunPowerup:
                    powerup = new ShotgunPowerup(id, content, position);
                    break;

                case PowerupType.MeteorPowerup:
                    powerup = new MeteorPowerup(id, content, position);
                    break;

                case PowerupType.Railgun:
                    powerup = new Railgun(id, content, position);
                    break;

                case PowerupType.HomingMissiles:
                    powerup = new HomingMissilesPowerup(id, content, position);
                    break;

                default:
                    throw new NotImplementedException("The powerup type " + Enum.GetName(typeof(ProjectileType), powerupType) + " is not supported");
            }

            powerup.ObjectType = ObjectType.Powerup;
            QueueGameObjectForCreation(powerup);
            return powerup;
        }


        /// <summary>
        /// Factory method for spawning a projectile
        /// </summary>
        /// <param name="projectileType">The type of projectile to create</param>
        /// <param name="position">The position of the projectile in the game world</param>
        /// <returns>The game object id of the projectile</returns>
        public Projectile CreateProjectile(ProjectileType projectileType, Vector2 position)
        {
            return CreateProjectile(projectileType, position, Vector2.Zero);
        }


        /// <summary>
        /// Factory method for spawning a projectile
        /// </summary>
        /// <param name="projectileType">The type of projectile to create</param>
        /// <param name="position">The position of the projectile in the game world</param>
        /// <returns>The game object id of the projectile</returns>
        public Projectile CreateProjectile(ProjectileType projectileType, Vector2 position, Vector2 velocity)
        {
            Projectile projectile = null;
            uint id = NextID();

            switch (projectileType)
            {
                case ProjectileType.Bullet:
                    projectile = new Bullet(id, content, position);
                    break;

                case ProjectileType.Fireball:
                    projectile = new Fireball(id, content, position);
                    break;
                
                case ProjectileType.BubbleBullet:
                    projectile = new BubbleBullet(id, content, position);
                    break;
                
                case ProjectileType.Bomb:
                    projectile = new Bomb(id, content, position, true);
                    break;

                case ProjectileType.Blades:
                    projectile = new Blades(id, content);
                    break;

                case ProjectileType.DroneLaser:
                    projectile = new DroneLaser(id, content, position);
                    break;

                case ProjectileType.ToPlayerBullet:
                    projectile = new ToPlayerBullet(id, content, position);
                    break;

                case ProjectileType.ArrowProjectile:
                    projectile = new ArrowProjectile(id, content, position);
                    break;

                case ProjectileType.BirdCrap:
                    projectile = new BirdCrap(id, content, position);
                    break;
                case ProjectileType.EBullet:
                    projectile = new EBullet(id, content, position);
                    break;

                case ProjectileType.Frostball:
                    projectile = new Frostball(id, content, position);
                    break;

                case ProjectileType.BlueBeam:
                    projectile = new blueBeam(id, content, position);
                    break;

                //This method doesn't fit the trishield very well, so this code is a bit poor in quality.
                case ProjectileType.TrishieldBall:
                    for (int i = 0; i < 2; i++)
                    {
                        projectile = new TriShieldBall(id, content, 2 * MathHelper.Pi / 3 * i);
                        QueueGameObjectForCreation(projectile);
                        id = NextID();
                    }
                    projectile = new TriShieldBall(id, content, 4 * MathHelper.Pi / 3);
                    break;

                case ProjectileType.GenericEnemyBullet:
                    projectile = new GenericEnemyBullet(id, content, position);
                    break;

                case ProjectileType.DroneWave:                    
                    // waveIndex helps draw the wave to the left and right of the ship, while waveSpacing holds the vector difference of space between each drone.
                    // Drone count is managed by 2*i.
                    Vector2 waveIndex = new Vector2(-1, 1);
                    Vector2 waveSpacing = new Vector2(40,30);
                    for (int i = 0; i < 5; i++)
                    {
                        projectile = new DroneWave(id, content, position + waveSpacing * waveIndex * i);
                        QueueGameObjectForCreation(projectile);
                        id = NextID();
                        projectile = new DroneWave(id, content, position + waveSpacing * i);
                        QueueGameObjectForCreation(projectile);
                        id = NextID();
                    }
                    break;

                case ProjectileType.TurretFireball:
                    projectile = new TurretFireball(id, content, position);
                    break;

                case ProjectileType.JetMinionBullet:
                    projectile = new JetMinionBullet(id, content, position);
                    break;

                case ProjectileType.EnergyBlast:
                    projectile = new EnergyBlast(id, content, position, ScrollingShooterGame.Game.Player.energyBlastLevel);
                    break;

                case ProjectileType.EnemyBullet:

                    // Bullet velocity
                    float bulletVel = 200f;

                    //ScrollingShooterGame.Game.projectiles.Add(new EnemyBullet(ScrollingShooterGame.Game.Content, this.position + offset, bulletVel * toPlayer));

                    Vector2 toPlayer = (new Vector2(ScrollingShooterGame.Game.Player.Bounds.Center.X,
                        ScrollingShooterGame.Game.Player.Bounds.Center.Y) - position);

                    toPlayer.Normalize();

                    projectile = new EnemyBullet(id, content, position, bulletVel * toPlayer);
                    break;
                case ProjectileType.EnemyBomb:
                    projectile = new Bomb(id, content, position, false);
                    break;

                case ProjectileType.ShotgunBullet:
                    projectile = new ShotgunBullet(id, content, position, BulletDirection.Straight);
                    QueueGameObjectForCreation(new ShotgunBullet(NextID(), content, position, BulletDirection.Left));
                    QueueGameObjectForCreation(new ShotgunBullet(NextID(), content, position, BulletDirection.Right));
                    QueueGameObjectForCreation(new ShotgunBullet(NextID(), content, position, BulletDirection.HardLeft));
                    QueueGameObjectForCreation(new ShotgunBullet(NextID(), content, position, BulletDirection.HardRight));
                    break;

                case ProjectileType.BlimpShotgun:
                    projectile = new BlimpShotgun(id, content, position, BulletDirection.Straight);
                    QueueGameObjectForCreation(new BlimpShotgun(NextID(), content, position, BulletDirection.Left));
                    QueueGameObjectForCreation(new BlimpShotgun(NextID(), content, position, BulletDirection.Right));
                    QueueGameObjectForCreation(new BlimpShotgun(NextID(), content, position, BulletDirection.HardLeft));
                    QueueGameObjectForCreation(new BlimpShotgun(NextID(), content, position, BulletDirection.HardRight));
                    break;

                case ProjectileType.Meteor:
                    projectile = new Meteor(id, content, position);
                    break;

                case ProjectileType.BigMeteor:
                    projectile = new BigMeteor(id, content, position);
                    break;

                case ProjectileType.EnemyFlameball:
                    projectile = new EnemyFlameball(id, content, position);
                    break;

                case ProjectileType.RGSabot:
                    projectile = new RGSabot(id, content, position);
                    break;

                case ProjectileType.BlimpBullet:
                    projectile = new BlimpBullet(id, content, position);
                    break;

                case ProjectileType.BirdWrath:
                    projectile = new BirdWrath(id, content, position);
                    break;

                case ProjectileType.FreezewaveProjectile:
                    projectile = new FreezewaveProjectile(id, content, position);
                    break;
                    
                case ProjectileType.Photon:
                    projectile = new Photon(id, content, position);
                    break;

                case ProjectileType.Pincher:
                    projectile = new Pincher(id, content, position);
                    break;

                case ProjectileType.GreenOrb:
                    projectile = new GreenOrb(id, content, position);
                    break;

                case ProjectileType.AlienTurretOrb:
                    projectile = new AlienTurretOrb(id, content, position);
                    break;

                case ProjectileType.TwinJetMissile:
                    projectile = new Boss_TwinJetMissile(id, content, position);
                    break;

                case ProjectileType.TwinJetBullet:
                    projectile = new Boss_TwinJetBullet(id, content, position);
                    break;

                case ProjectileType.HomingMissile:
                    projectile = new HomingMissileProjectile(content, position, 1, id);
                    break;

                case ProjectileType.EnemyPsyBall:
                    projectile = new EnemyPsiBall(id, content, position);
                    break;

                case ProjectileType.EnemyLightningZap:
                    projectile = new EnemyLightningZap(id, content, position);
                    break;

                default:
                    throw new NotImplementedException("The projectile type " + Enum.GetName(typeof(ProjectileType), projectileType) + " is not supported");
            }

            if ((int)projectileType < 100)
                projectile.ObjectType = ObjectType.PlayerProjectile;
            else
                projectile.ObjectType = ObjectType.EnemyProjectile;
            QueueGameObjectForCreation(projectile);
            return projectile;
        }

        /// <summary>
        /// Factory method for spawning a shield
        /// </summary>
        /// <param name="shieldType">The type of shield to create</param>
        /// <param name="position">Position of the shield in the game world</param>
        /// <param name="PlayerShip">The Player</param>
        /// <returns>The game object id of the projectile</returns>
        public Shield CreateShield(ShieldType shieldType, Vector2 position,
            PlayerShip PlayerShip)
        {
            Shield shield;
            uint id = NextID();

            switch (shieldType)
            {
                case ShieldType.EightBallShield:
                    shield = new EightBallShield(id, content, position, PlayerShip);
                    break;
                default:
                    throw new NotImplementedException("EightBallShield failed.");
            }

            shield.ObjectType = ObjectType.Shield;
            QueueGameObjectForCreation(shield);
            return shield;
        }


        /// <summary>
        /// Factory method for spawning enemies.
        /// </summary>
        /// <param name="enemyType">The type of enemy to spawn</param>
        /// <param name="position">The location to spawn the enemy</param>
        /// <returns></returns>
        public Enemy CreateEnemy(EnemyType enemyType, Vector2 position)
        {
            Enemy enemy;
            uint id = NextID();

            switch (enemyType)
            {
                case EnemyType.Dart:
                    enemy = new Dart(id, content, position);
                    break;

                case EnemyType.GreenGoblin:
                    enemy = new GreenGoblin(id, content, position);
                    break;

                case EnemyType.LaserDrone:
                    enemy = new LaserDrone(id, content, position);
                    break;

                case EnemyType.Cobalt:
                    enemy = new Cobalt(id, content, position);
                    break;

                case EnemyType.JetMinion:
                    enemy = new JetMinion(id, content, position);
                    break;

                case EnemyType.Seed:
                    enemy = new Seed(id, content, position);
                    break;

                case EnemyType.Bomber:
                    enemy = new Bomber(id, content, position);
                    break;

                case EnemyType.Arrow:
                    enemy = new Arrow(id, content, position);
                    break;

                case EnemyType.TurretSingle:
                    enemy = new TurretSingle(id, content, position);
                    break;

                case EnemyType.TurretDouble:
                    enemy = new TurretDouble(id, content, position);
                    break;

                case EnemyType.TurretTower:
                    enemy = new TurretTower(id, content, position);
                    break;
                
                case EnemyType.StdBaddy:
                    enemy = new StdBaddy(id, content, position);
                    break;

                case EnemyType.BeamShip:
                    enemy = new BeamShip(id, content, position);
                    break;

                case EnemyType.Kamikaze:
                    enemy = new Kamikaze(id, content, position);
                    break;

                case EnemyType.Panzer:
                    enemy = new Panzer(id, content, position);
                    break;

                case EnemyType.Panzer2:
                    enemy = new Panzer2(id, content, position);
                    break;

                case EnemyType.Lavabug:
                    enemy = new Lavabug(id, content, position);
                    break;

                case EnemyType.Lavabug2:
                    enemy = new Lavabug2(id, content, position);
                    break;

                case EnemyType.Mandible:
                    enemy = new Mandible(id, content, position, lavaFlip);
                    lavaFlip = !lavaFlip;
                    break;

                case EnemyType.BladeSpinner:
                    enemy = new BladeSpinner(id, content, position);
                    break;

                case EnemyType.DeerTickDown:
                    enemy = new DeerTick(id, content, position, DeerTickDirection.Straight);
                    break;

                case EnemyType.DeerTickLeft:
                    enemy = new DeerTick(id, content, position, DeerTickDirection.Left);
                    break;

                case EnemyType.DeerTickRight:
                    enemy = new DeerTick(id, content, position, DeerTickDirection.Right);
                    break;

                case EnemyType.Turret:
                    enemy = new Turret(id, content, position);
                    break;

                case EnemyType.JTurret:
                    enemy = new JTurret(id, content, position);
                    break;

                case EnemyType.DrillLeft:
                    enemy = new Drill(id, content, true);
                    break;

                case EnemyType.DrillRight:
                    enemy = new Drill(id, content, false);
                    break;

                case EnemyType.SuicideBomber:
                    enemy = new SuicideBomber(id, content, position);
                    break;
                
                case EnemyType.LavaFighter:
                    enemy = new LavaFighter(id, content, position);
                    break;

                case EnemyType.TwinJet:
                    enemy = new TwinJet(id, content, position);
                    break;

                case EnemyType.AlienTurret:
                    enemy = new AlienTurret(id, content, position);
                    break;

                case EnemyType.RightClaw:
                    enemy = new RightClaw(id, content, position);
                    break;

                case EnemyType.LeftClaw:
                    enemy = new LeftClaw(id, content, position);
                    break;

                case EnemyType.Bird:
                    enemy = new Bird(id, content, position);
                    break;

                case EnemyType.BrainBoss:
                    enemy = new BrainBoss(id, content, position);
                    break;

                case EnemyType.BrainBossPsyEmitter:
                    enemy = new BrainBossPsiEmitter(id, content, position);
                    break;

                case EnemyType.BrainBossProtection:
                    enemy = new BrainBossProtection(id, content, position);
                    break;

                default:
                    throw new NotImplementedException("The enemy type " + Enum.GetName(typeof(EnemyType), enemyType) + " is not supported");
            }

            enemy.ObjectType = ObjectType.Enemy;
            QueueGameObjectForCreation(enemy);
            return enemy;
        }

        #endregion

        #region Axis List Helper Methods

        /// <summary>
        /// Sorts the axis using insertion sort - provided the list is
        /// already nearly sorted, this should happen very quickly 
        /// </summary>
        /// <param name="axis">The axis to sort</param>
        private void UpdateAxisLists()
        {
            HashSet<CollisionPair> overlaps = new HashSet<CollisionPair>();
            int i, j;

            // Sort the horizontal axis
            for (i = 1; i < horizontalAxis.Count; i++)
            {
                Bound bound = horizontalAxis[i];
                j = i - 1;

                // if our bound needs to be moved left... lower in the list
                while ((j >= 0) && horizontalAxis[j].CompareTo(bound) > 0)
                {
                    // What are we passing, and what are we passing it with?
                    if (horizontalAxis[j].Type == BoundType.Min && bound.Type == BoundType.Max)
                    {
                        // when a Max bound passes a min, we remove it from 
                        // the collision set
                        collisions.Remove(new CollisionPair(horizontalAxis[j].Box.GameObjectID, bound.Box.GameObjectID));
                        horizontalOverlaps.Remove(new CollisionPair(horizontalAxis[j].Box.GameObjectID, bound.Box.GameObjectID));
                    }
                    else if (horizontalAxis[j].Type == BoundType.Max && bound.Type == BoundType.Min)
                    {
                        // when a Min bound passes a Max, we add it to the 
                        // potential collision set
                        horizontalOverlaps.Add(new CollisionPair(horizontalAxis[j].Box.GameObjectID, bound.Box.GameObjectID));
                    }

                    // Shift the elment at j up the list by one index
                    horizontalAxis[j + 1] = horizontalAxis[j];
                    j--;
                }
                horizontalAxis[j + 1] = bound;
            }

            // Sort the vertical axis
            for (i = 1; i < verticalAxis.Count; i++)
            {
                Bound bound = verticalAxis[i];
                j = i - 1;

                // if our bound needs to be moved left... lower in the list
                while ((j >= 0) && verticalAxis[j].CompareTo(bound) > 0)
                {
                    // What are we passing, and what are we passing it with?
                    if (verticalAxis[j].Type == BoundType.Min && bound.Type == BoundType.Max)
                    {
                        // when a Max bound passes a min, we remove it from 
                        // the collision set
                        collisions.Remove(new CollisionPair(verticalAxis[j].Box.GameObjectID, bound.Box.GameObjectID));
                        verticalOverlaps.Remove(new CollisionPair(verticalAxis[j].Box.GameObjectID, bound.Box.GameObjectID));
                    }
                    else if (verticalAxis[j].Type == BoundType.Max && bound.Type == BoundType.Min)
                    {
                        // when a Min bound passes a Max, we add it to the 
                        // potential collision set
                        verticalOverlaps.Add(new CollisionPair(verticalAxis[j].Box.GameObjectID, bound.Box.GameObjectID));
                    }

                    // Shift the elment at j up the list by one index
                    verticalAxis[j + 1] = verticalAxis[j];
                    j--;
                }
                verticalAxis[j + 1] = bound;
            }

            // Check the potential overlaps for intersection
            foreach (CollisionPair pair in verticalOverlaps)
            {
                GameObject A = GetObject(pair.A);
                GameObject B = GetObject(pair.B);
                if (A.Bounds.Intersects(B.Bounds))
                {
                    collisions.Add(pair);
                }
            }
        }


        /// <summary>
        /// Inserts a new bound into an axis list.  The list is assumed to be
        /// sorted, so the method uses binary insertion for speed.
        /// </summary>
        /// <param name="axis">The axis list to insert the Bound into</param>
        /// <param name="bound">The Bound to insert</param>
        /// <returns>The index where the bound was inserted</returns>
        private int InsertBoundIntoAxis(List<Bound> axis, Bound bound)
        {
            // Use binary search for fast O(log(n)) indentification
            // of appropriate index for our bound
            int index = axis.BinarySearch(bound);
            if (index < 0)
            {
                // If the index returned by binary search is negative,
                // then our current bound value does not exist within the
                // axis (most common case).  The bitwise compliement (~) of
                // that index value indicates the index of the first element 
                // in the axis list *larger* than our bound, and therefore
                // the appropriate place for our item
                index = ~index;
                axis.Insert(index, bound);
            }
            else
            {
                // If the index returned by binary search is positive, then
                // we have another bound with the *exact same value*.  We'll
                // go ahead and insert at that position.
                axis.Insert(index, bound);
            }

            return index;
        }

        
        /// <summary>
        /// Helper method that adds a GameObject to the GameObjectManager
        /// </summary>
        /// <param name="gameObject">The Game Object to add</param>
        private void AddGameObject(GameObject gameObject)
        {
            uint id = gameObject.ID;

            // Store the game object in our list of all game objects
            gameObjects.Add(id, gameObject);

            // Create the game object's bounding box
            BoundingBox box = new BoundingBox(id, gameObject.Bounds);
            boundingBoxes.Add(id, box);

            // Store the game object's bounds in our horizontal axis list
            int leftIndex = InsertBoundIntoAxis(horizontalAxis, box.Left);
            int rightIndex = InsertBoundIntoAxis(horizontalAxis, box.Right);

            // Grab any game object ids for game objects whose bounds fall
            // between the min and max bounds of our new game object
            for (int i = leftIndex + 1; i < rightIndex; i++)
            {
                horizontalOverlaps.Add(new CollisionPair(id, horizontalAxis[i].Box.GameObjectID));
            }

            // Store the new game object's bounds in our vertical axis list
            int topIndex = InsertBoundIntoAxis(verticalAxis, box.Top);
            int bottomIndex = InsertBoundIntoAxis(verticalAxis, box.Bottom);

            // Grab any game object ids for game objects whose bounds fall
            // between the min and max of our new game object
            for (int i = topIndex + 1; i < bottomIndex; i++)
            {
                verticalOverlaps.Add(new CollisionPair(id, verticalAxis[i].Box.GameObjectID));
            }
        }


        /// <summary>
        /// Updates the position of a GameObject within the axis
        /// lists
        /// </summary>
        /// <param name="gameObjectID">The ID of the game object to update</param>
        public void UpdateGameObject(uint gameObjectID)
        {
            // Grab our bounding box
            BoundingBox box = boundingBoxes[gameObjectID];

            // Grab our game object
            GameObject go = gameObjects[gameObjectID];

            // Apply the changes to bounding box
            box.Left.Value = go.Bounds.Left;
            box.Right.Value = go.Bounds.Right;
            box.Top.Value = go.Bounds.Top;
            box.Bottom.Value = go.Bounds.Bottom;
        }

        /// <summary>
        /// Removes a Game object from the axis
        /// </summary>
        /// <param name="gameObject">The game object to remove</param>
        private void RemoveGameObject(GameObject gameObject)
        {
            uint id = gameObject.ID;

            // Remove the game object from our list of all game objects
            gameObjects.Remove(id);

            // Remove the game object from our collection of collisions
            int i = collisions.Count - 1;
            while (i >= 0)
            {
                CollisionPair pair = collisions.ElementAt(i);
                if (pair.A == id || pair.B == id)
                    collisions.Remove(pair);
                i--;
            }

            // Remove the game object from our collection of overlaps
            i = horizontalOverlaps.Count - 1;
            while (i >= 0)
            {
                CollisionPair pair = horizontalOverlaps.ElementAt(i);
                if (pair.A == id || pair.B == id)
                    horizontalOverlaps.Remove(pair);
                i--;
            }
            i = verticalOverlaps.Count - 1;
            while (i >= 0)
            {
                CollisionPair pair = verticalOverlaps.ElementAt(i);
                if (pair.A == id || pair.B == id)
                    verticalOverlaps.Remove(pair);
                i--;
            }

            // Grab the game object's bounding box
            BoundingBox box = boundingBoxes[id];

            // Remove the game objects' bounds from our horizontal axis lists
            horizontalAxis.Remove(box.Left);
            horizontalAxis.Remove(box.Right);
            verticalAxis.Remove(box.Top);
            verticalAxis.Remove(box.Bottom);

            // Remove the game object's bounding box
            boundingBoxes.Remove(id);
        }

        #endregion
    }
}