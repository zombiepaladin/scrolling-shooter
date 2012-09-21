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
        BasicEffect basicEffect;

        Viewport gameViewport;
        Viewport worldViewport;
        Viewport guiViewport;

        public static ScrollingShooterGame Game;
        public static GameObjectManager GameObjectManager;
        public static LevelManager LevelManager;
        
        public PlayerShip Player;

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

            // Create a basic effect, used with the spritebatch 
            basicEffect = new BasicEffect(GraphicsDevice)
            {
                TextureEnabled = true,
                VertexColorEnabled = true,
            };

            GameObjectManager = new GameObjectManager(Content);

            // TODO: use this.Content to load your game content here
            Player = GameObjectManager.CreatePlayerShip(PlayerShipType.Shrike, new Vector2(300, 300));
            GameObjectManager.CreatePowerup(PowerupType.Fireball, new Vector2(100, 200));
            //Player.ApplyPowerup(PowerupType.Fireball);

            LevelManager.LoadContent();
            LevelManager.LoadLevel("example2");
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
            
            LevelManager.Update(elapsedTime);
            
            GameObjectManager.Update(elapsedTime);

            // Process collisions
            foreach (CollisionPair pair in GameObjectManager.Collisions)
            {
                // Player collisions
                if (pair.A == Player.ID || pair.B == Player.ID)
                {
                    uint colliderID = (pair.A == Player.ID) ? pair.B : pair.A;
                    GameObject collider = GameObjectManager.GetObject(colliderID);
                    
                    // Process powerup collisions
                    Powerup powerup = collider as Powerup;
                    if (powerup != null)
                    {
                        Player.ApplyPowerup(powerup.Type);
                        GameObjectManager.DestroyObject(colliderID);
                    }

                    //NOTE: Apply to more than the kamikaze enemy?
                    // Process kamakaze collisions
                    Enemy enemy = collider as Enemy;
                    if (enemy != null && enemy.GetType() == typeof(Kamikaze))
                    {
                        //Player take damage
                        GameObjectManager.DestroyObject(colliderID);
                        GameObjectManager.CreateExplosion(colliderID);
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
            // Set the viewport to the entire screen
            GraphicsDevice.Viewport = gameViewport;
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            float elapsedGameTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            
            // Render the game world
            GraphicsDevice.Viewport = worldViewport;
            LevelManager.Draw(elapsedGameTime);

            spriteBatch.Begin();

//            tilemap.Draw(elapsedGameTime, spriteBatch);

            //GameObjectManager.Draw(elapsedGameTime, spriteBatch);


            spriteBatch.End();

            // Render the gui
            GraphicsDevice.Viewport = guiViewport;

            base.Draw(gameTime);
        }
    }
}
