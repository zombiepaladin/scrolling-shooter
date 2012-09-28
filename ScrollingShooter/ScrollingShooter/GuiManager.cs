using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace ScrollingShooter
{

    public class GuiManager
    {
        ScrollingShooterGame game;
        SpriteBatch spriteBatch;

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
