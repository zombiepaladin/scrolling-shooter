using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Media;

namespace ScrollingShooter.SplashScreens
{
    public class GameOver : SplashScreen
    {
        SpriteFont spriteFont;

        public GameOver()
        {
            spriteFont = ScrollingShooterGame.Game.Content.Load<SpriteFont>("SpriteFonts/Pescadero");

            //Music = ScrollingShooterGame.Game.Content.Load<Song>("Music/GameOver");
            NextLevel = (int)LevelManager.Level.Airbase;
            IsFree = true;
        }

        public override void Update(float elapsedTime)
        {
            // Do nothing
        }

        public override void Draw(float elapsedTime, Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(spriteFont, "GAME OVER", new Vector2(100, 100), Color.White);
        }
    }
}
