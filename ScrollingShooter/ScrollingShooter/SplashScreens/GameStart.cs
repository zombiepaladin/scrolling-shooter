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
        SpriteFont spriteFont;

        public GameStart()
        {
            spriteFont = ScrollingShooterGame.Game.Content.Load<SpriteFont>("SpriteFonts/Pescadero");
            Music = ScrollingShooterGame.Game.Content.Load<Song>("Music/SFDemo 6812 Clark Aboud.mp3");
        }

        public override void Update(float elapsedTime)
        {
            // Do nothing
        }

        public override void Draw(float elapsedTime, Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(spriteFont, "Press Space to Begin", new Vector2(100, 100), Color.White);
        }
    }
}
