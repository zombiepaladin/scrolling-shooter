using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Media;

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
        private const string SPRITESHEET = "SpriteFonts/Impact";
        private const float STATE_TIME = 2f;

        //Instance variables
        SpriteFont _spriteFont;
        Vector2 _position;
        private CreditsState _state;
        private float _timer;
        private int _index;
        private string _displayString;

        private string[] _credits = new string[]
        {
            "Scrolling Shooter\n\nCIS 580 - Intro to Game Design\n\nKansas State University",
            "Studio Staff-Producers\n\nNathen Bean \nMicheal Marlen",
            "Studio Staff-Programmers/Designers\n\nDevin Kelly-Collins\nJoseph Shaw\nAustin Murphy\nNick Boen\nMatthew McHaney",
            "Studio Staff-Programmers/Designers\n\nAdam Clark\nNick Stanley\nJosh Zavala\nMicheal Fountain\nSam Fike",
            "Studio Staff-Programmers/Designers\n\nMatthew Hart\nAndrew Bellinder\nNicholes Strub\nBrett Barger\nJiri Malina",
            "Studio Staff-Programmers/Designers\n\nAdam Steen\nBen Jazen\nDaniel Rymph",
            "Artwork\n\n Thorbjorn Lindeijer \n\t Tiled Map Editor \nDaniel Cook \n\tRemastered Tyrian Graphics",
            "Music\n\nIwan Gabovitch \n\t Dust Loop - Sneaky Hidden Danger \nK. Alex Rosen \n\t Blitz Kaskade",
            "Developed with Microsoft's \nXNA framwork",
            "Production Babies\n\nNo babies were born during production \n(as far as we know)",
        };

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
            _position = new Vector2(50, 50);
            _state = CreditsState.Display;
            _timer = 0;
            _index = 0;
            _displayString = _credits[_index++];

            Music = ScrollingShooterGame.Game.Content.Load<Song>("Music/12 Superbia");
            NextLevel = (int)LevelManager.Level.Airbase;
        }
    
        /// <summary>
        /// Updates the display of the credits screen.
        /// </summary>
        /// <param name="elapsedTime">Time since the last call.</param>
        public override void Update(float elapsedTime)
        {
            _timer += elapsedTime;
            if (_timer >= STATE_TIME)
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
                        _displayString = "Thank you for playing\n\nPress enter to play again.";
                        Done = true;
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
    }
}
