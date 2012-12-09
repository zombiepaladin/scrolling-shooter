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
using ScrollingShooter.SplashScreens;

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
        public int CurrentLevel;
        public List<string> Levels;

        public PlayerShip Player;
        Song Music;
        SplashScreen Splash;
        SplashScreenType SplashType;

        public GameState GameState { get; private set; }

        public int TotalKills;
        public int TotalScore;

        KeyboardState oldKS;

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
            CurrentLevel = 0;
            Levels = new List<string> { "Level_1_Tilemap_2", "Airbase", "lavaLevel2", "moon", "crystalland", "AlienBaseSafe", "InsideAlien" };

            oldKS = Keyboard.GetState();

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
            MediaPlayer.IsRepeating = true;
            Player = GameObjectManager.CreatePlayerShip(PlayerShipType.Shrike, new Vector2(300, 300));
            Player.ApplyPowerup(PowerupType.Default);
            LevelManager.LoadContent();
            GuiManager.LoadContent();
            SplashType = SplashScreenType.GameStart;
            GameState = GameState.Splash;
            loadSplashScreen(new GameStart());
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
        /// Reset the GameObjectManager and Load the next level
        /// </summary>
        private void Reset()
        {
            GameState = GameState.Initializing;
            Player.Health = Player.MaxHealth;
            GameObjectManager.Reset(Player);
            LevelManager.UnloadLevel();
            LevelManager.LoadLevel(Levels[CurrentLevel]);
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed
                || Keyboard.GetState().IsKeyDown(Keys.Escape))
                this.Exit();

            //Debug input
#if DEBUG
            if (Keyboard.GetState().IsKeyDown(Keys.OemTilde))
            {
                GameState = GameState.Splash;
                SplashType = SplashScreenType.GameOver;
                loadSplashScreen(new Credits());
            }
#endif
            
            if (Keyboard.GetState().IsKeyDown(Keys.Y) && oldKS.IsKeyUp(Keys.Y))
            {  
                CurrentLevel++;
                if (CurrentLevel < Levels.Count)
                {
                    Reset();
                }
            }

            float elapsedTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            // Update according to current game state
            switch (GameState)
            {
                case GameState.Initializing:
                    if (!LevelManager.Loading)
                        GameState = GameState.Gameplay;
                    break;

                case GameState.Splash:

                    if (SplashType == SplashScreenType.GameStart )
                    {
                        if (Splash.Done && Keyboard.GetState().IsKeyDown(Keys.Enter))
                        {
                            //the game is starting, load the first cutscene and update to the first line of dialog
                            SplashType = SplashScreenType.Beginning;
                            loadSplashScreen(new Beginning());
                        }
                    }
                    else if (SplashType == SplashScreenType.GameOver) //Should be Gameover or credits
                    {
                        if (Splash.Done && Keyboard.GetState().IsKeyDown(Keys.Enter))
                        {
                            //Game over reload first screen.
                            SplashType = SplashScreenType.GameStart;
                            loadSplashScreen(new GameStart());
                        }
                    }
                    else if (Splash.Done || Keyboard.GetState().IsKeyDown(Keys.S) && oldKS.IsKeyUp(Keys.S))
                    {
                        //Load next level
                        CurrentLevel = Splash.NextLevel;
                        Reset();
                    }
                    //Otherwise update.
                    Splash.Update(elapsedTime);
                    break;

                case GameState.Gameplay:
                    LevelManager.Update(elapsedTime);
                    if (!LevelManager.Ending)
                    {
                        GameObjectManager.Update(elapsedTime);
                        ProcessCollisions();
                    }
                    if (LevelManager.LevelDone)
                    {
                        switch (CurrentLevel)
                        {
                            case 1:
                                SplashType = SplashScreenType.EndLevelOne;
                                Splash = new EndLevelOne();
                                break;
                            case 2:
                                SplashType = SplashScreenType.EndLevelTwo;
                                Splash = new EndLevelTwo();
                                break;
                            case 3:
                                SplashType = SplashScreenType.EndLevelThree;
                                Splash = new EndLevelThree();
                                break;
                            case 4:
                                SplashType = SplashScreenType.EndLevelFour;
                                Splash = new EndLevelFour();
                                break;
                            case 5:
                                SplashType = SplashScreenType.EndLevelFive;
                                Splash = new EndLevelFive();
                                break;
                            case 6:
                                SplashType = SplashScreenType.EndLevelSix;
                                Splash = new EndLevelSix();
                                break;
                            case 7:
                                SplashType = SplashScreenType.GameOver;
                                Splash = new Credits();
                                break;
                        }
                        GameState = GameState.Splash;
                        Splash.Update(elapsedTime);
                    }
                    else if (LevelManager.ResetLevel)
                    {
                        //Should be handle by player death method.
                        Reset();
                        LevelManager.ResetLevel = false;
                        Player.Score = 0;
                        Player.Lives = 5;
                        Player.Health = Player.MaxHealth;
                        Player.Dead = false;
                    }
                    break;

                case GameState.Scoring: //Not used
                    GuiManager.Update(elapsedTime);
                    if (GuiManager.tallyState == GuiManager.TallyingState.PressSpaceToContinue
                        && Keyboard.GetState().IsKeyDown(Keys.Space))
                    {
                        GameState = GameState.Splash;
                        Music = Splash.Music;
                        if (Music != null) MediaPlayer.Play(Music);
                    }
                    break;
            }

            oldKS = Keyboard.GetState();
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
                    spriteBatch.Begin(0, null, SamplerState.LinearClamp, null, null, null);
                    Splash.Draw(elapsedGameTime, spriteBatch);
                    spriteBatch.End();
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

                    if (!player.Dead)
                    {

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
                                    player.Health -= enemy.Health;
                                    if(player.Health <= 0 && !(player.InvincibleTimer > 0))
                                        killPlayer(player);
                                    GameObjectManager.DestroyObject(collider.ID);
                                    GameObjectManager.CreateExplosion2(collider.ID, 0.5f);
                                    // Update the player's score
                                    player.Score += enemy.Score;
                                }
                                else
                                {
                                    //Destroy player. Not the enemy
                                    if(!(player.InvincibleTimer > 0))
                                        killPlayer(player);
                                }
                                break;

                            case ObjectType.EnemyProjectile:
                                Projectile projectile = collider as Projectile;

                                // Damage player
                                if (player.InvincibleTimer <= 0)
                                {
                                    player.Health -= projectile.Damage;
                                    if(player.Health <= 0 && !(player.InvincibleTimer > 0))
                                        killPlayer(player);
                                }

                                GameObjectManager.DestroyObject(collider.ID);
                                break;
                            case ObjectType.Boss:
                                //Destroy player. Not the enemy
                                if(!(player.InvincibleTimer > 0))
                                    killPlayer(player);
                                break;
                        }
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
                                LevelManager.Ending = true;
                            }
                            // Destroy projectile
                            // Note, if there are special things for the bullet, add them here
                            GameObjectManager.DestroyObject(playerProjectile.ID);
                            break;
                    }
                }
            }
        }

        public void PlayerDeath()
        {
            loadSplashScreen(new GameOver());
            SplashType = SplashScreenType.GameOver;
            LevelManager.ResetLevel = false;
            Player.Score = 0;
            Player.Lives = 5;
            Player.Health = Player.MaxHealth;
            Player.Dead = false;
            Player.ClearPowerups();
            Player.ApplyPowerup(PowerupType.Default);
        }

        private void loadSplashScreen(SplashScreen ss)
        {
            Splash = ss;
            if (ss.Music != null)
            {
                MediaPlayer.Play(ss.Music);
            }
            else
            {
                MediaPlayer.Stop();
            }
            GameState = GameState.Splash;
            ss.Update(0);
        }

        private void killPlayer(PlayerShip player)
        {
            //GameObjectManager.DestroyObject(player.ID);
            player.Dead = true;
            player.DeathTimer = 2;
            GameObjectManager.CreateExplosion2(player.ID, 1);
            player.Score -= 100;
        }
    }
}
