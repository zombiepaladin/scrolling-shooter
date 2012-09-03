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
using ScrollingShooter.ObjectManagers;
using ScrollingShooter.Gui.GUIObjects;

namespace ScrollingShooter
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class ScrollingShooterGame : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        PlayerShip player;
        public ProjectileManager pManager = new ProjectileManager();
        public GUIManager gManager = new GUIManager();
        public static ScrollingShooterGame Game;

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
#if TEST
            IsMouseVisible = true;
#endif
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            // TODO: use this.Content to load your game content here
            player = new ShrikeShip(Content);
            GUIHealthBar hBar1 = new GUIHealthBar(Content, new Vector2(GraphicsDevice.Viewport.Width-10-150,10), 100, 100);
            GUIHealthBar hBar2 = new GUIHealthBar(Content, new Vector2(10, 10), 100, 100);
            gManager.Add(hBar1);
            gManager.Add(hBar2);
            player.ApplyPowerup(Powerups.Fireball);
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

            player.Update(elapsedTime);
            gManager.UpdateAll(elapsedTime);
            pManager.UpdateAll(elapsedTime);

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
            player.Draw(elapsedGameTime, spriteBatch);
            gManager.DrawAll(elapsedGameTime,spriteBatch);
            pManager.DrawAll(elapsedGameTime, spriteBatch);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
