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
    /// Indicates the state of the game
    /// </summary>
    public enum GameState
    {
        Initializing,
        Splash,
        Gameplay,
        Scoring,
        Credits,
    }

    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class ScrollingShooterGame : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        Viewport gameViewport;
        Viewport worldViewport;
        Viewport guiViewport;

        public static ScrollingShooterGame Game;
        public static GameObjectManager GameObjectManager;
        public static LevelManager LevelManager;
        public static GuiManager GuiManager;

        public PlayerShip Player;
        Song Music;
        SplashScreen Splash;

        public GameState GameState { get; private set; }

        public int TotalKills;
        public int TotalScore;


        public ScrollingShooterGame()
        {
            Game = this;
            graphics = new GraphicsDeviceManager(this);
            // Use HD TV resolution
            graphics.PreferredBackBufferWidth = 1280;
            graphics.PreferredBackBufferHeight = 720;
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
            LevelManager = new LevelManager(this);
            GuiManager = new GuiManager(this);

            TotalKills = 0;
            TotalScore = 0;

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create our viewports
            gameViewport = GraphicsDevice.Viewport;
            worldViewport = new Viewport(0, 0, 768, 720); // Twice as wide as 16 tiles
            guiViewport = new Viewport(768, 0, 512, 720); // Remaining space

            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            GameObjectManager = new GameObjectManager(Content);

            // TODO: use this.Content to load your game content here
            Player = GameObjectManager.CreatePlayerShip(PlayerShipType.Shrike, new Vector2(300, 300));

            LevelManager.LoadContent();
            GuiManager.LoadContent();
            GameState = GameState.Initializing;

            //These lines are only to display Assignment 1 through 3 for Devin Kelly-Collins
            Player.ApplyPowerup(PowerupType.BubbleBeam);
            LevelManager.LoadLevel("moon");
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

            float elapsedTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            // Update according to current game state
            switch (GameState)
            {
                case GameState.Initializing:
                    if (!LevelManager.Loading)
                        GameState = GameState.Gameplay;
                    break;

                case GameState.Splash:

                    if (!LevelManager.Loading && Keyboard.GetState().IsKeyDown(Keys.Space))
                    {
                        GameState = GameState.Gameplay;
                        LevelManager.NextLevel();
                        Music = LevelManager.CurrentSong;
                        if (Music != null) MediaPlayer.Play(Music);
                    }
                    break;

                case GameState.Gameplay:
                    LevelManager.Update(elapsedTime);
                    GameObjectManager.Update(elapsedTime);
                    ProcessCollisions();
                    break;

                case GameState.Scoring:
                    GuiManager.Update(elapsedTime);
                    if (GuiManager.tallyState == GuiManager.TallyingState.PressSpaceToContinue
                        && Keyboard.GetState().IsKeyDown(Keys.Space))
                    {
                        GameState = GameState.Splash;
                        Music = Splash.Music;
                        if (Music != null) MediaPlayer.Play(Music);
                    }
                    break;

                case GameState.Credits:
                    // TODO: Launch new game when player hits space
                    break;
            }

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            // Set the viewport to the entire screen
            GraphicsDevice.Viewport = gameViewport;
            GraphicsDevice.Clear(Color.Black);

            float elapsedGameTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            // Render according to current game state
            switch (GameState)
            {
                case GameState.Splash:
                    // TODO: Render splash screen
                    break;

                case GameState.Gameplay:

                    // Render the game world
                    GraphicsDevice.Viewport = worldViewport;
                    LevelManager.Draw(elapsedGameTime);

                    // Render the gui
                    GraphicsDevice.Viewport = guiViewport;
                    GuiManager.DrawHUD(elapsedGameTime);

                    break;

                case GameState.Scoring:
                    // TODO: Render the end-of-level scoring screen
                    GuiManager.DrawScoringScreen(elapsedGameTime, spriteBatch);
                    break;

                case GameState.Credits:
                    // TODO: Render the credits screen
                    break;
            }

            base.Draw(gameTime);
        }


        /// <summary>
        /// Helper method for processing gameobject collisions
        /// </summary>
        void ProcessCollisions()
        {
            // Process collisions
            foreach (CollisionPair pair in GameObjectManager.Collisions)
            {
                GameObject objectA = GameObjectManager.GetObject(pair.A);
                GameObject objectB = GameObjectManager.GetObject(pair.B);

                // Player collisions
                if (objectA.ObjectType == ObjectType.Player || objectB.ObjectType == ObjectType.Player)
                {
                    PlayerShip player = ((objectA.ObjectType == ObjectType.Player) ? objectA : objectB) as PlayerShip;
                    GameObject collider = (objectA.ObjectType == ObjectType.Player) ? objectB : objectA;

                    // Process powerup collisions
                    switch (collider.ObjectType)
                    {
                        case ObjectType.Powerup:
                            Powerup powerup = collider as Powerup;
                            player.ApplyPowerup(powerup.Type);
                            GameObjectManager.DestroyObject(collider.ID);
                            break;

                        case ObjectType.Enemy:
                            Enemy enemy = collider as Enemy;
                            if (enemy.GetType() == typeof(Kamikaze) || enemy.GetType() == typeof(Mandible) ||
                                enemy.GetType() == typeof(SuicideBomber) || enemy.GetType() == typeof(Mine) ||
                                enemy.GetType() == typeof(Rock))
                            {
                                //Player take damage
                                GameObjectManager.DestroyObject(collider.ID);
                                GameObjectManager.CreateExplosion2(collider.ID, 0.5f);
                                // Update the player's score
                                player.Score += enemy.Score;
                            }
                            break;

                        case ObjectType.EnemyProjectile:
                            Projectile projectile = collider as Projectile;

                            // Damage player
                            player.Health -= projectile.Damage;
                            if (player.Health <= 0)
                            {
                                GameObjectManager.DestroyObject(player.ID);
                                GameObjectManager.CreateExplosion2(player.ID, 1);
                                player.Score -= 100;
                            }

                            GameObjectManager.DestroyObject(collider.ID);
                            break;
                    }
                }

                // Player Projectile collisions
                else if (objectA.ObjectType == ObjectType.PlayerProjectile || objectB.ObjectType == ObjectType.PlayerProjectile)
                {
                    Projectile playerProjectile = ((objectA.ObjectType == ObjectType.PlayerProjectile) ? objectA : objectB) as Projectile;
                    GameObject collider = (objectA.ObjectType == ObjectType.PlayerProjectile) ? objectB : objectA;

                    // Process collisions
                    switch (collider.ObjectType)
                    {
                        case ObjectType.Enemy:
                            Enemy enemy = collider as Enemy;
                            //Enemy take damage
                            enemy.Health -= playerProjectile.Damage;

                            // If health <= 0, kill enemy
                            if (enemy.Health <= 0)
                            {
                                GameObjectManager.DestroyObject(collider.ID);
                                GameObjectManager.CreateExplosion(collider.ID);
                                GameObjectManager.CreateExplosion2(collider.ID, 0.5f);
                                Player.Kills++;
                                Player.Score += enemy.Score;
                            }
                            // Destroy projectile
                            // Note, if there are special things for the bullet, add them here
                            GameObjectManager.DestroyObject(playerProjectile.ID);
                            break;

                        case ObjectType.Boss:
                            Boss boss = collider as Boss;
                            // Boss take damage
                            boss.Health -= playerProjectile.Damage;

                            // If health <= 0, kill boss
                            if (boss.Health <= 0)
                            {
                                if (boss is Blimp) boss.Update(0);
                                GameObjectManager.DestroyObject(collider.ID);
                                GameObjectManager.CreateExplosion(collider.ID);
                                GameObjectManager.CreateExplosion2(collider.ID, 1.5f);
                                Player.Kills++;
                                Player.Score += boss.Score;
                            }
                            // Destroy projectile
                            // Note, if there are special things for the bullet, add them here
                            GameObjectManager.DestroyObject(playerProjectile.ID);
                            break;
                    }
                }
            }
        }
    }
}
