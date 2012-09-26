using System;
using System.Threading;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using ScrollingShooterWindowsLibrary;

namespace ScrollingShooter
{
    /// <summary>
    /// Indicates if we are currently in-level or displaying the
    /// pre-level Splash screen.
    /// </summary>
    enum LevelState
    {
        Splash,
        Gameplay,
    }

    /// <summary>
    /// A class for managing the loading, updating, and rendering of
    /// levels.
    /// </summary>
    public class LevelManager
    {
        Game game;
        SpriteBatch spriteBatch;
        BasicEffect basicEffect;

        float scrollDistance;
        Rectangle scrollBounds;

        bool loading;
        LevelState levelState;

        public SplashScreen CurrentSplash;
        public Tilemap CurrentMap;

        public bool Scrolling = false;

        /// <summary>
        /// Creates a new LevelManager
        /// </summary>
        /// <param name="game"></param>
        public LevelManager(Game game)
        {
            this.game = game;
        }


        /// <summary>
        /// Loads the LevelManager's content and initializes any
        /// functionality that needs a created graphics device
        /// </summary>
        public void LoadContent()
        {
            // Create our scroll bounds (the part of the world that is visible)
            // Note 
            scrollBounds = new Rectangle(0, 0, 384, 360);

            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(game.GraphicsDevice);

            // Create a basic effect, used with the spritebatch 
            basicEffect = new BasicEffect(game.GraphicsDevice)
            {
                TextureEnabled = true,
                VertexColorEnabled = true,
            };

            // Create our first splash screen
            CurrentSplash = new GameStart();
        }


        /// <summary>
        /// Loads a new level asynchronously
        /// </summary>
        /// <param name="level">The name of the level to load</param>
        public void LoadLevel(string level)
        {
            // Set the level state and loading flag
            levelState = LevelState.Splash;
            loading = true;

            ThreadStart threadStarter = delegate
            {
                CurrentMap = game.Content.Load<Tilemap>("Tilemaps/" + level);
                CurrentMap.LoadContent(game.Content);

                // Set the *default* starting scroll position to 16 tiles
                // above the bottom of the map
                scrollDistance = (-2 * CurrentMap.Height * CurrentMap.TileHeight) + (2 * 16 * CurrentMap.TileHeight);

                // Load the game objects
                for (int i = 0; i < CurrentMap.GameObjectGroupCount; i++)
                {
                    for (int j = 0; j < CurrentMap.GameObjectGroups[i].GameObjectData.Count(); j++)
                    {
                        GameObjectData goData = CurrentMap.GameObjectGroups[i].GameObjectData[j];
                        Vector2 position = new Vector2(goData.Position.Center.X, goData.Position.Center.Y);
                        GameObject go;

                        switch (goData.Category)
                        {
                            case "PlayerStart":
                                ScrollingShooterGame.Game.Player.Position = position;
                                scrollDistance = -2 * position.Y + 300;
                                break;

                            case "LevelEnd":
                                break;

                            case "Powerup":
                                go = ScrollingShooterGame.GameObjectManager.CreatePowerup((PowerupType)Enum.Parse(typeof(PowerupType), goData.Type), position);
                                CurrentMap.GameObjectGroups[i].GameObjectData[j].ID = go.ID;
                                go.LayerDepth = CurrentMap.GameObjectGroups[i].LayerDepth;
                                go.ScrollingSpeed = CurrentMap.GameObjectGroups[i].ScrollingSpeed;
                                break;

                            case "Enemy":
                                go = ScrollingShooterGame.GameObjectManager.CreateEnemy((EnemyType)Enum.Parse(typeof(EnemyType), goData.Type), position);
                                CurrentMap.GameObjectGroups[i].GameObjectData[j].ID = go.ID;
                                go.LayerDepth = CurrentMap.GameObjectGroups[i].LayerDepth;
                                go.ScrollingSpeed = CurrentMap.GameObjectGroups[i].ScrollingSpeed;
                                break;
                            case "Boss":
                                go = ScrollingShooterGame.GameObjectManager.CreateBoss((BossType)Enum.Parse(typeof(BossType), goData.Type), position);
                                CurrentMap.GameObjectGroups[i].GameObjectData[j].ID = go.ID;
                                go.LayerDepth = CurrentMap.GameObjectGroups[i].LayerDepth;
                                go.ScrollingSpeed = CurrentMap.GameObjectGroups[i].ScrollingSpeed;
                                break;
                        }
                    }
                }

                // Mark level as loaded
                loading = false;
            };

            Thread loadingThread = new Thread(threadStarter);
            loadingThread.Start();
        }

        /// <summary>
        /// Updates the Level
        /// </summary>
        /// <param name="elapsedTime">the time elapsed between this and the previous frame</param>
        public void Update(float elapsedTime)
        {
            if (levelState == LevelState.Splash) // Update the loading screen
            {
                if (!loading && Keyboard.GetState().IsKeyDown(Keys.Space))
                {
                    levelState = LevelState.Gameplay;
                }
            }
            else // Update the level
            {
                // Update the scrolling distance - the distance
                // the screen has scrolled past the Player
                float scrollDelta = elapsedTime * (CurrentMap.Layers[CurrentMap.PlayerLayer].ScrollingSpeed);
                scrollDistance += scrollDelta;
                ScrollingShooterGame.Game.Player.Position -= new Vector2(0, scrollDelta / 2);

                // Scroll all the tile layers
                for (int i = 0; i < CurrentMap.LayerCount; i++)
                {
                    CurrentMap.Layers[i].ScrollOffset += elapsedTime * CurrentMap.Layers[i].ScrollingSpeed;
                }

                // Update only the game objects that appear near our scrolling region
                Rectangle bounds = new Rectangle(0,
                    (int)(-scrollDistance / 2) + 300,
                    CurrentMap.Width * CurrentMap.TileWidth,
                    16 * CurrentMap.TileHeight);
                foreach (uint goID in ScrollingShooterGame.GameObjectManager.QueryRegion(bounds))
                {
                    GameObject go = ScrollingShooterGame.GameObjectManager.GetObject(goID);
                    go.Update(elapsedTime);
                    ScrollingShooterGame.GameObjectManager.UpdateGameObject(goID);
                }
            }
        }


        /// <summary>
        /// Draw the level
        /// </summary>
        /// <param name="elapsedTime">The time between this and the last frame</param>
        public void Draw(float elapsedTime)
        {
            if (levelState == LevelState.Splash) // Draw loading screen
            {
                spriteBatch.Begin();
                CurrentSplash.Draw(elapsedTime, spriteBatch);
                spriteBatch.End();
            }
            else // Draw level
            {
                Viewport viewport = game.GraphicsDevice.Viewport;
                basicEffect.World = Matrix.CreateScale(2, 2, 1);
                basicEffect.View = Matrix.CreateTranslation(new Vector3(0, scrollDistance, 0));
                basicEffect.Projection = Matrix.CreateOrthographicOffCenter(0, viewport.Width, viewport.Height, 0, 0, -1);

                spriteBatch.Begin(0, null, SamplerState.LinearClamp, null, null, basicEffect);

                for (int i = 0; i < CurrentMap.LayerCount; i++)
                {
                    // To minimize drawn tiles, we limit ourselves to those onscreen
                    int miny = (int)((-scrollDistance - 2 * CurrentMap.Layers[i].ScrollOffset) /
                        (CurrentMap.TileHeight * 2));
                    int maxy = miny + 15;

                    // And those that exist
                    if (miny < 0) miny = 0;
                    if (maxy > CurrentMap.Height) maxy = CurrentMap.Height;

                    for (int y = miny; y < maxy; y++)
                    {
                        // Since our maps are only as wide as our rendering area, 
                        // no need for optimizaiton here
                        for (int x = 0; x < CurrentMap.Width; x++)
                        {
                            int index = x + y * CurrentMap.Width;
                            TileData tileData = CurrentMap.Layers[i].TileData[index];
                            if (tileData.TileID != 0)
                            {
                                Tile tile = CurrentMap.Tiles[tileData.TileID - 1];
                                Rectangle onScreen = new Rectangle(
                                    x * CurrentMap.TileWidth,
                                    (int)(y * CurrentMap.TileHeight + CurrentMap.Layers[i].ScrollOffset),
                                    CurrentMap.TileWidth,
                                    CurrentMap.TileHeight);
                                spriteBatch.Draw(CurrentMap.Textures[tile.TextureID], onScreen, tile.Source, Color.White, 0f, new Vector2(CurrentMap.TileWidth / 2, CurrentMap.TileHeight / 2), tileData.SpriteEffects, CurrentMap.Layers[i].LayerDepth);
                            }
                        }
                    }
                }

                // Draw only the game objects that appear within our scrolling region
                Rectangle bounds = new Rectangle(0,
                    (int)(-scrollDistance / 2),
                    CurrentMap.Width * CurrentMap.TileWidth,
                    16 * CurrentMap.TileHeight);

                foreach (uint goID in ScrollingShooterGame.GameObjectManager.QueryRegion(bounds))
                {
                    GameObject go = ScrollingShooterGame.GameObjectManager.GetObject(goID);
                    go.Draw(elapsedTime, spriteBatch);
                }

                spriteBatch.End();
            }
        }
    }
}
