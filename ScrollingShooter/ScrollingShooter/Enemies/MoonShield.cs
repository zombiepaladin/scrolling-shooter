using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace ScrollingShooter
{
    public class MoonShield : Enemy
    {
        private enum ShieldState
        {
            s1 = 0,
            s2 = 1,
            s3 = 2
        }

        //MoonShield Constants
        private const string SPRITESHEET = "Spritesheets/newshs.shp.000000";
        private static readonly Rectangle[] SPRITEBOUNDS = new Rectangle[] 
        {
            new Rectangle(0, 28, 45, 54),
            new Rectangle(46, 28, 45, 54),
            new Rectangle(92, 28, 45, 54)
        };
        private const float STATE_TIME = .5f;

        //Instance var
        private Texture2D _spriteSheet;
        private Vector2 _position;
        private ShieldState _state;
        private float _timer;

        public MoonShield(uint id, ContentManager content, Vector2 position)
            : base(id)
        {
             _spriteSheet = content.Load<Texture2D>(SPRITESHEET);
            _position = position;
            _state = ShieldState.s1;
            _timer = 0;
            this.Health = 700;
        }

        public override Rectangle Bounds
        {
            get { return new Rectangle((int)_position.X, (int)_position.Y, SPRITEBOUNDS[(int)_state].Width, SPRITEBOUNDS[(int)_state].Height); }
        }

        public override void Update(float elapsedTime)
        {
            _position.Y -= ScrollingShooterGame.LevelManager.CurrentMap.Layers[ScrollingShooterGame.LevelManager.CurrentMap.PlayerLayer].ScrollOffset * elapsedTime;
            _timer += elapsedTime;
            if (_timer >= STATE_TIME)
            {
                _timer = 0;
                switch (_state)
                {
                    case ShieldState.s1:
                        _state = ShieldState.s2;
                        break;
                    case ShieldState.s2:
                        _state = ShieldState.s3;
                        break;
                    case ShieldState.s3:
                        _state = ShieldState.s1;
                        break;
                    default:
                        throw new Exception("Unknown state.");
                }
            }
        }

        public override void Draw(float elapsedTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_spriteSheet, Bounds, SPRITEBOUNDS[(int)_state], Color.White);
        }
    }
}
