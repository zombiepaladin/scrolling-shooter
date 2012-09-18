//Twin Jet Manager Class:
//Coders: Nicholas Boen
//Date: 9/16/2012
//Time: 6:32 P.M.
//
//Manages the two Twin Jets and acts as a boss

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using System;

namespace ScrollingShooter
{
    /// <summary>
    /// Manages the Phase state of the Jet manager
    /// </summary>
    enum JetManagerPhaseState
    {
        PAUSED = 0,
        INTRO1,
        INTRO2,
        FIGHT
    }

    /// <summary>
    /// Manages the current flight pattern of the jet manager
    /// </summary>
    enum JetManagerFlightPattern
    {
        PATTERN1 = 0,
        PATTERN2,
        PATTERN3
    }

    public class TwinJetManager : Boss
    {
        /// <summary>
        /// The total time until a new pattern is selected for the jets
        /// </summary>
        private const float NewPatternTime = 15f;

        /// <summary>
        /// The flight patterns available to the manager
        /// </summary>
        private BossFlightPattern[] _flightPatterns = new BossFlightPattern[3];

        /// <summary>
        /// The total health between the two jets
        /// </summary>
        private int _totalHealth;

        /// <summary>
        /// The references to the two jets (or more)
        /// </summary>
        private TwinJet[] _myJets = new TwinJet[2];

        /// <summary>
        /// The time left before a new pattern is selected
        /// </summary>
        private float _timerToNewPattern;

        /// <summary>
        /// Whether or not this boss is still alive
        /// </summary>
        private bool _isAlive;

        /// <summary>
        /// The phase of the jet manager
        /// </summary>
        private JetManagerPhaseState _myPhase;

        /// <summary>
        /// The flight pattern state of the manager
        /// </summary>
        private JetManagerFlightPattern _myPattern;

        /// <summary>
        /// The size of the screen, this will change later
        /// </summary>
        private Vector2 screenSize;
        /// <summary>
        /// The dimensions of the jet sprites themselves
        /// </summary>
        private static Vector2 jetSize = new Vector2(93, 80);

        /// <summary>
        /// Returns the Bounds of this manager, a non existent entity
        /// </summary>
        public override Rectangle Bounds { get { return Rectangle.Empty; } }

        /// <summary>
        /// Makes a shiny new Jet Manager
        /// </summary>
        /// <param name="id">The factory id on the label of the Jet Manager</param>
        /// <param name="content">The content manager to use</param>
        /// <param name="position">The "position" to "spawn" at</param>
        public TwinJetManager(uint id, ContentManager content, Vector2 position)
            : base(id)
        {
            //Getting and setting the size of the screen
            screenSize = new Vector2(ScrollingShooterGame.Game.GraphicsDevice.Viewport.Width, ScrollingShooterGame.Game.GraphicsDevice.Viewport.Height);

            //Set the time to get a new pattern
            _timerToNewPattern = 0;

            //Setting up the phases
            _myPhase = JetManagerPhaseState.PAUSED;
            _myPattern = JetManagerFlightPattern.PATTERN1;

            //Creating the patterns
            _flightPatterns[(int)JetManagerFlightPattern.PATTERN1] = new BossFlightPattern(new Vector2(screenSize.X/2 - jetSize.X/2, 300));
            _flightPatterns[(int)JetManagerFlightPattern.PATTERN1].Add(new Vector2(jetSize.X, screenSize.Y/2 - jetSize.Y/2));
            _flightPatterns[(int)JetManagerFlightPattern.PATTERN1].Add(new Vector2(jetSize.X, jetSize.Y));
            _flightPatterns[(int)JetManagerFlightPattern.PATTERN1].Add(new Vector2(screenSize.X - jetSize.X, screenSize.Y/2  - jetSize.Y/2));
            _flightPatterns[(int)JetManagerFlightPattern.PATTERN1].Add(new Vector2(screenSize.X - jetSize.X, jetSize.Y));

            _flightPatterns[(int)JetManagerFlightPattern.PATTERN2] = new BossFlightPattern(new Vector2(jetSize.X, jetSize.Y));
            _flightPatterns[(int)JetManagerFlightPattern.PATTERN2].Add(new Vector2(screenSize.X/2 - jetSize.X/2, screenSize.Y/2 - jetSize.Y/2));
            _flightPatterns[(int)JetManagerFlightPattern.PATTERN2].Add(new Vector2(screenSize.X - jetSize.X, jetSize.Y));
            _flightPatterns[(int)JetManagerFlightPattern.PATTERN2].Add(new Vector2(screenSize.X/2 - jetSize.X/2, screenSize.Y/2 - jetSize.Y/2 - 100));

            _flightPatterns[(int)JetManagerFlightPattern.PATTERN3] = new BossFlightPattern(new Vector2(jetSize.X, screenSize.Y / 2 - jetSize.Y / 2));
            _flightPatterns[(int)JetManagerFlightPattern.PATTERN3].Add(new Vector2(screenSize.X/2 - jetSize.X/2, screenSize.Y - jetSize.Y));
            _flightPatterns[(int)JetManagerFlightPattern.PATTERN3].Add(new Vector2(screenSize.X - jetSize.X, screenSize.Y/2 - jetSize.Y/2));
            _flightPatterns[(int)JetManagerFlightPattern.PATTERN3].Add(new Vector2(screenSize.X/2 - jetSize.X/2, screenSize.Y - jetSize.Y));

            //Creating the Jets
            _myJets[0] = (TwinJet)ScrollingShooterGame.GameObjectManager.CreateEnemy(EnemyType.TwinJet, new Vector2(-30, 510));
            _myJets[1] = (TwinJet)ScrollingShooterGame.GameObjectManager.CreateEnemy(EnemyType.TwinJet, new Vector2(830, 510));

            //Start the intro
            StartIntro();

        }

        /// <summary>
        /// The update method that gets called each update cycle
        /// </summary>
        /// <param name="elapsedTime">The time since the last call</param>
        public override void Update(float elapsedTime)
        {
            //Handles the introduction of the jets via these states
            switch (_myPhase)
            {
                case JetManagerPhaseState.INTRO1:
                    IntroPart1();
                    break;

                case JetManagerPhaseState.INTRO2:
                    IntroPart2();
                    break;

                case JetManagerPhaseState.FIGHT:
                    _timerToNewPattern -= elapsedTime;

                    if (_timerToNewPattern <= 0)
                    {
                        AssignNewPattern();
                        _timerToNewPattern = NewPatternTime;
                    }

                    break;
            }

            _myJets[0].Update(elapsedTime);
            _myJets[1].Update(elapsedTime);
        }

        /// <summary>
        /// The draw method that gets called each draw cycle
        /// </summary>
        /// <param name="elapsedTime">The time elapsed since the last call</param>
        /// <param name="spriteBatch">The sprite batch to use</param>
        public override void Draw(float elapsedTime, SpriteBatch spriteBatch)
        {
            //Drawin the Jets

            _myJets[0].Draw(elapsedTime, spriteBatch);
            _myJets[1].Draw(elapsedTime, spriteBatch);
        }

        /// <summary>
        /// Handles the first part of the introduction
        /// </summary>
        private void IntroPart1()
        {
            //Each jet starts at a bottom corner and flys up to the opposite upper
            //corner before going to the second part of the intro

            _myJets[0].Velocity = new Vector2(screenSize.X, 0) - _myJets[0].Position;
            _myJets[1].Velocity = new Vector2(0, 0) - _myJets[1].Position;

            if (_myJets[0].Position.X > 800 && _myJets[0].Position.Y < 0)
            {
                _myJets[0].Velocity = Vector2.Zero;
                _myJets[0].Position = new Vector2(600, -50);
            }
            if (_myJets[1].Position.X < 0 && _myJets[1].Position.Y < 0)
            {
                _myJets[1].Velocity = new Vector2(0,0);
                _myJets[1].Position = new Vector2(400, -50);
            }

            if (_myJets[0].Position.Y == -50 && _myJets[1].Position.Y == -50)
            {
                _myPhase = JetManagerPhaseState.INTRO2;
            }
        }

        /// <summary>
        /// Handles the second part of the introduction
        /// </summary>
        private void IntroPart2()
        {
            //Handles moving the jets above the screen and having
            //them enter the battlefield from the top

            _myJets[0].Velocity = new Vector2(0, 1);
            _myJets[1].Velocity = new Vector2(0, 1);

            if (_myJets[0].Position.Y > 150 || _myJets[1].Position.Y > 150)
            {
                _myJets[0].Velocity = Vector2.Zero;
                _myJets[1].Velocity = Vector2.Zero;

                _myPhase = JetManagerPhaseState.FIGHT;
                AssignNewPattern();
                _myJets[0].StopIntro();
                _myJets[1].StopIntro();
            }
        }

        /// <summary>
        /// Handles Assigning a new pattern to the jets
        /// </summary>
        private void AssignNewPattern()
        {
            Random r1 = new Random(DateTime.Now.Millisecond + 20);
            Random r2 = new Random(DateTime.Now.Millisecond + 10);

            _myJets[0].SetPattern(_flightPatterns[r1.Next(3)]);
            _myJets[1].SetPattern(_flightPatterns[r2.Next(3)]);
        }

        /// <summary>
        /// Trigger to start playing the intro sequence
        /// </summary>
        public void StartIntro()
        {
            switch (_myPhase)
            {
                case JetManagerPhaseState.PAUSED:
                    _myPhase = JetManagerPhaseState.INTRO1;
                    break;
            }
        }
    }
}

