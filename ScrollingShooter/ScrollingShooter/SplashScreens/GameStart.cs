using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;

namespace ScrollingShooter
{
    /// <summary>
    /// The first splash screen displayed when the game is loaded.  
    /// </summary>
    public class GameStart : SplashScreen
    {
        private const float TIME_BLINK = .1f;

        bool _drawFont;
        SpriteFont spriteFont;
        Texture2D spriteSheet;

        float _timer;

        public GameStart()
        {
            spriteFont = ScrollingShooterGame.Game.Content.Load<SpriteFont>("SpriteFonts/Pescadero");
            spriteSheet = ScrollingShooterGame.Game.Content.Load<Texture2D>("Spritesheets/TitleScreen");
            _timer = 0f;

            //Music = ScrollingShooterGame.Game.Content.Load<Song>("Music/StartMusic");
            NextLevel = (int)LevelManager.Level.Airbase;
            Done = false;
        }

        public override void Update(float elapsedTime)
        {
            _timer = elapsedTime;
            if (_timer >= TIME_BLINK)
            {
                _drawFont = !_drawFont;
                _timer = 0;
            }
        }

        public override void Draw(float elapsedTime, Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(spriteSheet, new Vector2(0, 0), Color.White);
            if(_drawFont)
                spriteBatch.DrawString(spriteFont, "Press Space to Begin", new Vector2(100, 100), Color.White);
        }
    }
}
