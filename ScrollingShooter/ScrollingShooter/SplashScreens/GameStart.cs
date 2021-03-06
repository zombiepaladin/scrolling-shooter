﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;

namespace ScrollingShooter
{
    /// <summary>
    /// The first splash screen displayed when the game is loaded.  
    /// </summary>
    public class GameStart : SplashScreen
    {
        private const float TIME_BLINK = .5f;

        bool drawFont;
        SpriteFont spriteFont;
        Texture2D spriteSheet;

        float timer;

        public GameStart()
        {
            spriteFont = ScrollingShooterGame.Game.Content.Load<SpriteFont>("SpriteFonts/Pescadero");
            spriteSheet = ScrollingShooterGame.Game.Content.Load<Texture2D>("Spritesheets/TitleScreen");
            timer = 0f;

            Music = ScrollingShooterGame.Game.Content.Load<Song>("Music/02 World Nap");
            NextLevel = (int)LevelManager.Level.Airbase;
            drawFont = true;
            Done = false;
        }

        public override void Update(float elapsedTime)
        {
            timer += elapsedTime;
            if (timer >= TIME_BLINK)
            {
                Done = true;
                drawFont = !drawFont;
                timer = 0;
            }
        }

        public override void Draw(float elapsedTime, Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(spriteSheet, new Vector2(0, 0), Color.White);
            if(drawFont)
                spriteBatch.DrawString(spriteFont, "Press enter to Begin", new Vector2(500, 400), Color.Black);
        }
    }
}
