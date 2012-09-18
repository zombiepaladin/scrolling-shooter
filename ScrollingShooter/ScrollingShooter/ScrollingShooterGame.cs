using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using ScrollingShooterWindowsLibrary;

namespace ScrollingShooter
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class ScrollingShooterGame : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        public static ScrollingShooterGame Game;
        public static GameObjectManager GameObjectManager;

        public PlayerShip player;
        public Tilemap tilemap;

        public ScrollingShooterGame()
        {
            Game = this;
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            GameObjectManager = new GameObjectManager(Content);

            // TODO: use this.Content to load your game content here
            player = GameObjectManager.CreatePlayerShip(PlayerShipType.Shrike, new Vector2(300, 300));

            tilemap = Content.Load<Tilemap>("Tilemaps/example");
            tilemap.Scrolling = true;

            GameObjectManager.CreatePowerup(PowerupType.EnergyBlast, new Vector2(400, 250));
            GameObjectManager.CreatePowerup(PowerupType.EnergyBlast, new Vector2(400, 300));
            GameObjectManager.CreatePowerup(PowerupType.EnergyBlast, new Vector2(400, 350));
            GameObjectManager.CreatePowerup(PowerupType.EnergyBlast, new Vector2(400, 400));

            GameObjectManager.CreateEnemy(EnemyType.TurretSingle, new Vector2(300, 300));
            GameObjectManager.CreateEnemy(EnemyType.TurretDouble, new Vector2(150, 100));
            GameObjectManager.CreateEnemy(EnemyType.TurretDouble, new Vector2(300, 100));
            GameObjectManager.CreateEnemy(EnemyType.TurretDouble, new Vector2(450, 100));
            GameObjectManager.CreateEnemy(EnemyType.TurretDouble, new Vector2(600, 100));
            GameObjectManager.CreateEnemy(EnemyType.TurretTower, new Vector2(400, 100));
            GameObjectManager.CreateEnemy(EnemyType.Kamikaze, new Vector2(200, 100));
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            // TODO: Add your update logic here
            float elapsedTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            tilemap.Update(elapsedTime);

            GameObjectManager.Update(elapsedTime);

            // Process collisions
            foreach (CollisionPair pair in GameObjectManager.Collisions)
            {
                GameObject objectA = GameObjectManager.GetObject(pair.A);
                GameObject objectB = GameObjectManager.GetObject(pair.B);

                // Player collisions
                if (objectA.objectType == ObjectType.player || objectB.objectType == ObjectType.player)
                {
                    PlayerShip player = ((objectA.objectType == ObjectType.player) ? objectA : objectB) as PlayerShip;
                    GameObject collider = (objectA.objectType == ObjectType.player) ? objectB : objectA;

                    // Process powerup collisions
                    switch (collider.objectType)
                    {
                        case ObjectType.powerup:
                            Powerup powerup = collider as Powerup;
                            player.ApplyPowerup(powerup.Type);
                            GameObjectManager.DestroyObject(collider.ID);
                            break;
                        case ObjectType.enemy:
                            Enemy enemy = collider as Enemy;
                            //NOTE: Apply to more than the kamikaze enemy?
                            // Process kamakaze collisions
                            if (enemy.GetType() == typeof(Kamikaze))
                            {
                                //Player take damage
                                GameObjectManager.DestroyObject(collider.ID);
                                GameObjectManager.CreateExplosion(collider.ID);
                            }
                            break;
                        case ObjectType.enemyProjectile:
                            Projectile projectile = collider as Projectile;

                            // Damage player
                            player.Health -= projectile.Damage;
                            if (player.Health <= 0)
                            {
                                GameObjectManager.DestroyObject(player.ID);
                                GameObjectManager.CreateExplosion(player.ID);
                            }

                            GameObjectManager.DestroyObject(collider.ID);
                            break;
                    }
                }
                // Player Projectile collisions
                else if (objectA.objectType == ObjectType.playerProjectile || objectB.objectType == ObjectType.playerProjectile)
                {
                    Projectile playerProjectile = ((objectA.objectType == ObjectType.playerProjectile) ? objectA : objectB) as Projectile;
                    GameObject collider = (objectA.objectType == ObjectType.player) ? objectB : objectA;

                    // Process collisions
                    switch (collider.objectType)
                    {
                        case ObjectType.enemy:
                            Enemy enemy = collider as Enemy;
                            //Enemy take damage
                            enemy.Health -= playerProjectile.Damage;

                            // If health <= 0, kill enemy
                            if (enemy.Health <= 0)
                            {
                                GameObjectManager.DestroyObject(collider.ID);
                                GameObjectManager.CreateExplosion(collider.ID);
                            }
                            // Destroy projectile
                            // Note, if there are special things for the bullet, add them here
                            GameObjectManager.DestroyObject(playerProjectile.ID);
                            break;
                        case ObjectType.boss:
                            Boss boss = collider as Boss;
                            // Boss take damage
                            boss.Health -= playerProjectile.Damage;

                            // If health <= 0, kill boss
                            if (boss.Health <= 0)
                            {
                                GameObjectManager.DestroyObject(collider.ID);
                                GameObjectManager.CreateExplosion(collider.ID);
                            }
                            // Destroy projectile
                            // Note, if there are special things for the bullet, add them here
                            GameObjectManager.DestroyObject(playerProjectile.ID);
                            break;
                    }
                }


            }

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            float elapsedGameTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            spriteBatch.Begin();
            tilemap.Draw(elapsedGameTime, spriteBatch);

            GameObjectManager.Draw(elapsedGameTime, spriteBatch);
            
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
