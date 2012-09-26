using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ScrollingShooter
{
    /// <summary>
    /// The splash screen displayed when level one is passed.  
    /// </summary>
    public class EndLevelOne : SplashScreen
    {
        SpriteFont spriteFont;

        /// <summary>
        /// Create a new EndLevelOne splash screen
        /// </summary>
        public EndLevelOne()
        {
            NextLevel = "example2";
            spriteFont = ScrollingShooterGame.Game.Content.Load<SpriteFont>("SpriteFonts/Pescadero");
        }

        /// <summary>
        /// Update the splash screen
        /// </summary>
        /// <param name="elapsedTime">The time that has passed between this and the previous frame</param>
        public override void Update(float elapsedTime)
        {
            // Do nothing
        }

        /// <summary>
        /// Render the splash screen
        /// </summary>
        /// <param name="elapsedTime">The time passed between this and the previous frame</param>
        /// <param name="spriteBatch">An already-initialized spritebatch, ready for Draw() commands</param>
        public override void Draw(float elapsedTime, Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(spriteFont, "Press Space to Begin", new Vector2(100, 100), Color.White);
        }
    }
}
