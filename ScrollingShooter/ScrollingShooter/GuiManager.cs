using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace ScrollingShooter
{

    public class GuiManager
    {
        ScrollingShooterGame game;
        SpriteBatch spriteBatch;
        Texture2D HUD;
        SpriteFont font;

        /// <summary>
        /// Indicates if the Scoring Screen is actively tallying points
        /// </summary>
        public bool Tallying = false;

        /// <summary>
        /// Creates a new GuiManager
        /// </summary>
        /// <param name="game">The game this GuiManager belongs to</param>
        public GuiManager(ScrollingShooterGame game)
        {
            this.game = game;
        }

        /// <summary>
        /// Loads the Gui content
        /// </summary>
        public void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(game.GraphicsDevice);
            HUD = game.Content.Load<Texture2D>("Spritesheets/gui");
            font = game.Content.Load<SpriteFont>("SpriteFonts/Pescadero");
        }

        /// <summary>
        /// Updates the GuiManager
        /// </summary>
        /// <param name="elapsedTime">the time between this and the previous frame</param>
        public void Update(float elapsedTime)
        {
            // TODO: Update GUI
        }

        /// <summary>
        /// Draws the in-game GUI (limited to the side of the screen)
        /// </summary>
        /// <param name="elapsedTime">The time between this and the previous frame</param>
        public void DrawHUD(float elapsedTime)
        {
            // TODO: Draw GUI
            int lives = game.Player.Lives;
            float health = game.Player.Health;
            float maxHealth = game.Player.MaxHealth;
            int score = game.Player.Score;
            String healthDisplay = health.ToString() + "/" + maxHealth.ToString();

            spriteBatch.Begin();
            spriteBatch.Draw(HUD, new Vector2(0, 0), Color.White);

            // Draw the Lives
            Vector2 origin = font.MeasureString(lives.ToString()) / new Vector2(2.0f, 2.0f);
            spriteBatch.DrawString(font, lives.ToString(), new Vector2(256,156), Color.White, 0, origin, 2, SpriteEffects.None, 0);

            // Draw the Health
            origin = font.MeasureString(healthDisplay) / new Vector2(2.0f, 2.0f);
            spriteBatch.DrawString(font, healthDisplay, new Vector2(256, 396), Color.White, 0, origin, 2, SpriteEffects.None, 0);

            // Draw the Score
            origin = font.MeasureString(score.ToString()) / new Vector2(2.0f, 2.0f);
            spriteBatch.DrawString(font, score.ToString(), new Vector2(256, 636), Color.White, 0, origin, 2, SpriteEffects.None, 0);
            
            spriteBatch.End();
        }

        /// <summary>
        /// Draws the end-of-level scoring screen
        /// </summary>
        /// <param name="elapsedTime"></param>
        public void DrawScoringScreen(float elapsedTime)
        {
            // TODO: Draw the scoring screen
        }
    }
}
