using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

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
