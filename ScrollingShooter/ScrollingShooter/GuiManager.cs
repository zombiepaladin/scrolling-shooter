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
        /// Various states of the tallying screen
        /// </summary>
        public enum TallyingState
        {
            Initial = 0,
            Title,
            Kills,
            Score,
            TotalKills,
            TotalScore,
            PressSpaceToContinue
        };

        /// <summary>
        /// Stores the current state of the tallying screen
        /// </summary>
        public TallyingState tallyState;

        /// <summary>
        /// Keeps track of the time for the tally screen.
        /// </summary>
        float tallyTimer = 0;

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
            if (game.GameState == GameState.Scoring)
            {
                tallyTimer += elapsedTime;

                if (tallyState == TallyingState.Initial)
                {
                    //Change to title, play sfx
                    tallyTimer = 0;
                }
                else if (tallyState == TallyingState.Title && tallyTimer >= 1000)
                {
                    tallyState = TallyingState.Kills;
                    //play sfx
                    tallyTimer = 0;
                }
                else if (tallyState == TallyingState.Kills && tallyTimer >= 1000)
                {
                    tallyState = TallyingState.Score;
                    //play sfx
                    tallyTimer = 0;
                }
                else if (tallyState == TallyingState.Score && tallyTimer >= 1000)
                {
                    tallyState = TallyingState.TotalKills;
                    //play sfx
                    tallyTimer = 0;
                }
                else if (tallyState == TallyingState.TotalKills && tallyTimer >= 1000)
                {
                    tallyState = TallyingState.TotalScore;
                    //play sfx
                    tallyTimer = 0;
                }
                else if (tallyState == TallyingState.TotalScore && tallyTimer >= 1000)
                {
                    tallyState = TallyingState.PressSpaceToContinue;
                    //play sfx
                    tallyTimer = 0;
                }
            }
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
            if (tallyState >= TallyingState.Title)
            {
                //Draw title
            }
            if (tallyState >= TallyingState.Kills)
            {
                //Draw kills
            }
            if (tallyState >= TallyingState.Score)
            {
                //Draw score
            }
            if (tallyState >= TallyingState.TotalKills)
            {
                //Draw total kills
            }
            if (tallyState >= TallyingState.TotalScore)
            {
                //Draw total score
            }
            // TODO: Draw the scoring screen
        }
    }
}
