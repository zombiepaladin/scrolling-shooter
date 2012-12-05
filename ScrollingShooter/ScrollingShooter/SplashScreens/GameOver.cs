using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace ScrollingShooter.SplashScreens
{
    public class GameOver
    {
        SpriteFont spriteFont;

        public GameOver()
        {
            spriteFont = ScrollingShooterGame.Game.Content.Load<SpriteFont>("SpriteFonts/Pescadero");
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
