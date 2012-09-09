using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace ScrollingShooter.Enemies
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
            Closed = 0,
            Opening = 1,
            Open = 2
        }

        //Constants
        private const string SPRITESHEET = "Spritesheets/newshd.shp.0000000";
        private static readonly Rectangle[] SPRITEBOUNDS = new Rectangle[] {
            new Rectangle(98, 84, 20, 28),
            new Rectangle(122, 84, 20, 28),
            new Rectangle(147, 84, 20, 28)
        };

        //Instance variables.
        Texture2D _spritesheet;
        Vector2 _position;
        SeedState _state = SeedState.Closed;

        public override Rectangle Bounds
        {
            get { return new Rectangle((int)_position.X, (int)_position.Y, SPRITEBOUNDS[(int)_state].Width, SPRITEBOUNDS[(int)_state].Height); }
        }

        public Seed(ContentManager content, Vector2 position)
        {
            this._position = position;

            this._spritesheet = content.Load<Texture2D>(SPRITESHEET);
        }

        public override void Update(float elapsedTime)
        {
            
        }

        public override void Draw(float elapsedTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_spritesheet, Bounds, SPRITEBOUNDS[(int)_state], Color.White);
        }
    }
}
