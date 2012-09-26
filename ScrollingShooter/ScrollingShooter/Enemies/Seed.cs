using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace ScrollingShooter
{
    /// <summary>
    /// Handles drawing the explosion after the seed ship is destoryed.
    /// </summary>
    class SeedExplosion : GameObject
    {
        private const string SPRITESHEET = "Spritesheets/newsh6.shp.000000";
        private const float STATE_TIME = .1f;
        //Bounds for the rightside of the explosion.
        private static readonly Rectangle[] RIGHT_BOUNDS = new Rectangle[] 
        {
            new Rectangle(0, 112, 11, 26),
            new Rectangle(11, 112, 11, 26),
            new Rectangle(22, 112, 11, 26),
            new Rectangle(33, 112, 11, 26),
            new Rectangle(44, 112, 11, 26),
            new Rectangle(55, 112, 11, 26),
            new Rectangle(66, 112, 11, 26),
            new Rectangle(77, 112, 11, 26),
            new Rectangle(88, 112, 11, 26),
            new Rectangle(99, 112, 11, 26),
            new Rectangle(110, 112, 11, 26),
            new Rectangle(121, 112, 11, 26),
            new Rectangle(132, 112, 11, 26),
        };

        //Bounds for the leftside of the explosion.
        private static readonly Rectangle[] LEFT_BOUNDS = new Rectangle[]
        {
            new Rectangle(0, 141, 11, 26),
            new Rectangle(11, 141, 11, 26),
            new Rectangle(22, 141, 11, 26),
            new Rectangle(33, 141, 11, 26),
            new Rectangle(44, 141, 11, 26),
            new Rectangle(55, 141, 11, 26),
            new Rectangle(66, 141, 11, 26),
            new Rectangle(77, 141, 11, 26),
            new Rectangle(88, 141, 11, 26),
            new Rectangle(99, 141, 11, 26),
            new Rectangle(110, 141, 11, 26),
            new Rectangle(121, 141, 11, 26),
            new Rectangle(132, 141, 11, 26),
        };

        private Vector2 _position;
        private Texture2D _spritesheet;
        private float _timer = 0f;
        private int index = 0;

        /// <summary>
        /// Creates a new explosion.
        /// </summary>
        /// <param name="content">ContentManager to load resources with.</param>
        /// <param name="position">Position to create the ship at.</param>
        /// <param name="id">Unique ID of the ship.</param>
        public SeedExplosion(uint id, ContentManager content, Vector2 position)
            : base(id)
        {
            _spritesheet = content.Load<Texture2D>(SPRITESHEET);
            this._position = position;
        }

        /// <summary>
        /// Draws the explosion on the spritebatch
        /// </summary>
        /// <param name="elapsedtime">Time since last call</param>
        /// <param name="spritebatch">spritebatch to draw on.</param>
        public override void Draw(float elapsedtime, SpriteBatch spritebatch)
        {
            _timer += elapsedtime;
            if (_timer >= STATE_TIME)
            {
                index++;
                _timer = 0f;
            }
            if (index < RIGHT_BOUNDS.Length)
            {
                spritebatch.Draw(_spritesheet, new Rectangle((int)_position.X, (int)_position.Y, RIGHT_BOUNDS[index].Width, RIGHT_BOUNDS[index].Height), RIGHT_BOUNDS[index], Color.White);
                spritebatch.Draw(_spritesheet, new Rectangle((int)_position.X + RIGHT_BOUNDS[index].Width, (int)_position.Y, LEFT_BOUNDS[index].Width, LEFT_BOUNDS[index].Height), RIGHT_BOUNDS[index], Color.White);
            }
        }

        /// <summary>
        /// Bounds of the explosion.
        /// </summary>
        public override Rectangle Bounds
        {
            get { return new Rectangle((int)_position.X, (int)_position.Y, RIGHT_BOUNDS[index].Width + LEFT_BOUNDS[index].Width, RIGHT_BOUNDS[index].Height); }
        }

        /// <summary>
        /// Does nothing
        /// </summary>
        /// <param name="elapsedTime">Time since last call.</param>
        public override void Update(float elapsedTime)
        {
            //Do nothing.
        }
    }

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
            Open = 0,
            Destroyed = 4,
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
        private SeedExplosion explosion;

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

            explosion = new SeedExplosion(id, content, position);
        }

        /// <summary>
        /// Updates the seed ship. Ship will move towards the player and fire bullets when it is close.
        /// </summary>
        /// <param name="elapsedTime">Time since last update.</param>
        public override void Update(float elapsedTime)
        {
            _timer += elapsedTime;

            //Move towards the player.
            PlayerShip player = ScrollingShooterGame.Game.Player;
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
                case SeedState.Destroyed:
                    //Do nothing for now.
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
            if (_state != SeedState.Destroyed)
                spriteBatch.Draw(_spritesheet, Bounds, SPRITEBOUNDS[(int)_state], Color.White);
            else
                explosion.Draw(elapsedTime, spriteBatch);
        }
    }
}