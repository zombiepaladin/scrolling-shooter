using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace ScrollingShooter
{
    /// <summary>
    /// A small fast ship that is invincible until it fires.
    /// </summary>
    public class Seed : Enemy
    {
        /// <summary>
        /// Represents states of the seed ship.
        /// </summary>
        private enum SeedState {
            Closed = 2,
            Opening = 1,
            Open = 0
        }

        //Constants
        private const string SPRITESHEET = "Spritesheets/newshd.shp.000000";
        private static readonly Rectangle[] SPRITEBOUNDS = new Rectangle[] {
            new Rectangle(47, 112, 23, 24),
            new Rectangle(73, 112, 18, 24),
            new Rectangle(99, 112, 15, 24)
        };
        //Time it takes to change states.
        private const float STATE_TIME = .5f;
        private const int NUM_OF_BULLETS_PER_FIRE = 5;
        private const float FIRE_TIME = .1f;

        //Instance variables.
        private Texture2D _spritesheet;
        private Vector2 _position;
        private SeedState _state = SeedState.Closed;
        private float _timer = 0f;
        private bool _opening = false;
        private int firedBullets = 0;

        /// <summary>
        /// Current bounds of the ship
        /// </summary>
        public override Rectangle Bounds
        {
            get { return new Rectangle((int)_position.X, (int)_position.Y, SPRITEBOUNDS[(int)_state].Width, SPRITEBOUNDS[(int)_state].Height); }
        }

        /// <summary>
        /// Creates a Seed ship.
        /// </summary>
        /// <param name="content">ContentManager to load resources with.</param>
        /// <param name="position">Position to create the ship at.</param>
        /// <param name="id">Unique ID of the ship.</param>
        public Seed(uint id, ContentManager content, Vector2 position)
            : base(id)
        {
            this._position = position;

            this._spritesheet = content.Load<Texture2D>(SPRITESHEET);
        }

        /// <summary>
        /// Updates the seed ship. Ship will move towards the player and fire bullets when it is close.
        /// </summary>
        /// <param name="elapsedTime">Time since last update.</param>
        public override void Update(float elapsedTime)
        {
            _timer += elapsedTime;

            //Move towards the player.
            PlayerShip player = ScrollingShooterGame.Game.player;
            Vector2 playerPosition = new Vector2(player.Bounds.Center.X, player.Bounds.Center.Y);
            Vector2 toPlayer = playerPosition - this._position;
            this._position += toPlayer * (float)((double)elapsedTime * .5);
            
            switch (_state)
            {
                case SeedState.Closed:
                    if (toPlayer.LengthSquared() < 9000)
                    {
                        //Close enough, start opening.
                        _timer = 0f;
                        _opening = true;
                        _state = SeedState.Opening;
                    }
                    break;
                case SeedState.Opening:
                    if (_timer >= STATE_TIME)
                    {
                        //Ready to open or close.
                        _state = (_opening) ? SeedState.Open : SeedState.Closed;
                        _timer = 0f;
                    }
                    break;
                case SeedState.Open:
                    //Fire bullets until we have fired 5 bullets.
                    if (firedBullets < NUM_OF_BULLETS_PER_FIRE && _timer >= FIRE_TIME)
                    {
                        ScrollingShooterGame.GameObjectManager.CreateProjectile(ProjectileType.ToPlayerBullet, _position);
                        firedBullets++;
                    }
                    else if (firedBullets == NUM_OF_BULLETS_PER_FIRE)
                    {
                        //Reset bullet count and start closing.
                        firedBullets = 0;
                        _timer = 0f;
                        _opening = false;
                        _state = SeedState.Opening;
                    }
                    break;
                default:
                    throw new Exception("Unknown state!");
            }
        }

        /// <summary>
        /// Draws sprites for the seed ship.
        /// </summary>
        /// <param name="elapsedTime">Time since the last time this method was called.</param>
        /// <param name="spriteBatch">SpriteBatch to draw too.</param>
        public override void Draw(float elapsedTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_spritesheet, Bounds, SPRITEBOUNDS[(int)_state], Color.White);
        }
    }
}
