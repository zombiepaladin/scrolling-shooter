using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;

namespace ScrollingShooter
{

    public class GuiManager
    {
        ScrollingShooterGame game;
        SpriteBatch spriteBatch;
        Texture2D HUD;
        SpriteFont font;

        SpriteFont spriteFont;
        SoundEffect progressSound;

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
            spriteFont = game.Content.Load<SpriteFont>("SpriteFonts/Pescadero");

            progressSound = game.Content.Load<SoundEffect>("SFX/anti_tank_gun_single_shot");
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
            if (game.GameState == GameState.Scoring)
            {
                tallyTimer += elapsedTime;

                if (tallyState == TallyingState.Initial)
                {
                    tallyState = TallyingState.Title;
                    progressSound.Play();
                    tallyTimer = 0;
                }
                else if (tallyState == TallyingState.Title && tallyTimer >= 1)
                {
                    tallyState = TallyingState.Kills;
                    progressSound.Play();
                    tallyTimer = 0;
                }
                else if (tallyState == TallyingState.Kills && tallyTimer >= 1)
                {
                    tallyState = TallyingState.Score;
                    progressSound.Play();
                    tallyTimer = 0;
                }
                else if (tallyState == TallyingState.Score && tallyTimer >= 1)
                {
                    tallyState = TallyingState.TotalKills;
                    game.TotalKills += game.Player.Kills;
                    progressSound.Play();
                    tallyTimer = 0;
                }
                else if (tallyState == TallyingState.TotalKills && tallyTimer >= 1)
                {
                    tallyState = TallyingState.TotalScore;
                    game.TotalScore += game.Player.Score;
                    progressSound.Play();
                    tallyTimer = 0;
                }
                else if (tallyState == TallyingState.TotalScore && tallyTimer >= 1)
                {
                    tallyState = TallyingState.PressSpaceToContinue;
                    progressSound.Play();
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
        public void DrawScoringScreen(float elapsedTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            if (tallyState >= TallyingState.Title)
            {
                //Draw title
                spriteBatch.DrawString(spriteFont, "Level Score:", 
                    new Microsoft.Xna.Framework.Vector2(500, 100),
                        Microsoft.Xna.Framework.Color.White, 0, Vector2.Zero, 3, SpriteEffects.None, 0);
            }
            if (tallyState >= TallyingState.Kills)
            {
                //Draw kills
                spriteBatch.DrawString(spriteFont, string.Format("Kills: {0}", game.Player.Kills),
                    new Microsoft.Xna.Framework.Vector2(450, 200),
                        Microsoft.Xna.Framework.Color.White, 0, Vector2.Zero, 3, SpriteEffects.None, 0);
            }
            if (tallyState >= TallyingState.Score)
            {
                //Draw score
                spriteBatch.DrawString(spriteFont, string.Format("Score: {0}", game.Player.Score),
                    new Microsoft.Xna.Framework.Vector2(450, 275),
                        Microsoft.Xna.Framework.Color.White, 0, Vector2.Zero, 3, SpriteEffects.None, 0);
            }
            if (tallyState >= TallyingState.TotalKills)
            {
                //Draw total kills
                spriteBatch.DrawString(spriteFont, string.Format("Total Kills: {0}", game.TotalKills),
                    new Microsoft.Xna.Framework.Vector2(450, 350),
                        Microsoft.Xna.Framework.Color.White, 0, Vector2.Zero, 3, SpriteEffects.None, 0);
            }
            if (tallyState >= TallyingState.TotalScore)
            {
                //Draw total score
                spriteBatch.DrawString(spriteFont, string.Format("Total Score: {0}", game.TotalScore),
                    new Microsoft.Xna.Framework.Vector2(450, 425),
                        Microsoft.Xna.Framework.Color.White, 0, Vector2.Zero, 3, SpriteEffects.None, 0);
            }
            if (tallyState >= TallyingState.PressSpaceToContinue)
            {
                //Draw the last string
                spriteBatch.DrawString(spriteFont, string.Format("Press Space to Continue", game.TotalScore),
                    new Microsoft.Xna.Framework.Vector2(350, 600),
                        Microsoft.Xna.Framework.Color.White,0, Vector2.Zero, 3, SpriteEffects.None, 0);
            }
            spriteBatch.End();
        }
    }
}
