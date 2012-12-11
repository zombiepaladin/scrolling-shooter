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
            spriteFont = ScrollingShooterGame.Game.Content.Load<SpriteFont>("SpriteFonts/Impact");

            Music = ScrollingShooterGame.Game.Content.Load<Song>("Music/Hard as Nails 11-13-28");
            NextLevel = (int)LevelManager.Level.Airbase;
            Done = true;
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
