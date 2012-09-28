using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace ScrollingShooter
{
    /// <summary>
    /// A splash screen to display the credits.
    /// </summary>
    public class Credits : SplashScreen
    {
        /// <summary>
        /// Different states for the credits screen.
        /// </summary>
        public enum CreditsState
        {
            Display,
            NonDisplay,
            Finished,
        }

        //Constants
        private const string FILE = "Content/Credits.txt";
        private const string SPRITESHEET = "SpriteFonts/Pescadero";
        private const float STATE_TIME = 1f;

        //Instance variables
        SpriteFont _spriteFont;
        Vector2 _position;
        private bool _loading;
        private CreditsState _state;
        private float _timer;
        private string[] _credits;
        private int _index;
        private string _displayString;

        /// <summary>
        /// Returns the current state of the screen. Once all the credits have been displayed the State will be Finished.
        /// </summary>
        public CreditsState State
        {
            get
            {
                return _state;
            }
        }

        /// <summary>
        /// Creates a new credtis screen.
        /// </summary>
        public Credits()
        {
            _spriteFont = ScrollingShooterGame.Game.Content.Load<SpriteFont>(SPRITESHEET);
            _position = new Vector2(100, 100);
            _state = CreditsState.Display;
            _timer = 0;
            _index = 0;
            _displayString = String.Empty;
            LoadCredits();
        }
    
        /// <summary>
        /// Updates the display of the credits screen.
        /// </summary>
        /// <param name="elapsedTime">Time since the last call.</param>
        public override void Update(float elapsedTime)
        {
            _timer += elapsedTime;
            if (_loading)
                _displayString = "Credits";
            else if (_timer >= STATE_TIME)
            {
                _timer = 0;
                switch (_state)
                {
                    case CreditsState.Display:
                        _state = (_index == _credits.Length) ? CreditsState.Finished : CreditsState.NonDisplay;
                        _displayString = String.Empty;
                        break;
                    case CreditsState.NonDisplay:
                        _state = CreditsState.Display;
                        _displayString = _credits[_index++];
                        break;
                    case CreditsState.Finished:
                        _displayString = "Press Space bar to begin.";
                        break;
                    default:
                        throw new Exception("Unexpected State.");
                }
            }          
        }

        /// <summary>
        /// Draws the credits screen.
        /// </summary>
        /// <param name="elapsedTime">Time since the last call.</param>
        /// <param name="spriteBatch">SpriteBatch to draw too.</param>
        public override void Draw(float elapsedTime, Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(_spriteFont, _displayString, _position, Color.White);
        }

        /// <summary>
        /// Loads in the credtis from FILE. The file must be setup like so:
        /// The first line should define how many lines follow.
        /// Every screen in the credits screen should be separted by a new line.
        /// If the screen needs a new line in itself, place a ;
        /// </summary>
        private void LoadCredits()
        {
            Thread loadingThread = new Thread(() =>
            {
                _loading = true;
                using (StreamReader reader = new StreamReader(File.Open(FILE, FileMode.Open)))
                {
                    int size = Int32.Parse(reader.ReadLine());
                    _credits = new string[size];
                    for (int i = 0; i < size; i++)
                    {
                        _credits[i] = reader.ReadLine().Replace(';', '\n');
                    }
                }
                _loading = false;
            });
            loadingThread.Start();
        }
    }
}
