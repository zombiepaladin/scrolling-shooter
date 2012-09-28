﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace ScrollingShooter
{
    public class Asteriod : Enemy
    {
        //Asteriod constants
        private const string SPRITESHEET = "Spritesheets/newshd.shp.000000";
        private static readonly Rectangle[] SPRITEBOUNDS = new Rectangle[] {
            new Rectangle(215, 84, 12, 12),
            new Rectangle(49, 0, 23, 20),
            new Rectangle(1, 2, 43, 47),
            new Rectangle(122, 2, 66, 76)
        };
        private static readonly Vector2[] VELOCITIES = new Vector2[] {
            new Vector2(0, -200),
            new Vector2(0, -100),
            new Vector2(0, -50),
            new Vector2(0, -25)
        };
        
        //Instance vars
        private int _size;
        private Vector2 _position;
        private Texture2D _spritesheet;

        /// <summary>
        /// Creates a new asteriod object.
        /// </summary>
        /// <param name="id">ID of the gameobject.</param>
        /// <param name="content">Content manager to load resources with.</param>
        /// <param name="position">Starting position of the asteriod.</param>
        /// <param name="size">Size of the asteriod (1 - 4)</param>
        public Asteriod(uint id, ContentManager content, Vector2 position, int size)
            : base(id)
        {
            this._position = position;
            this._spritesheet = content.Load<Texture2D>(SPRITESHEET);
            this._size = size;
            this.Health = size * 100;
        }

        /// <summary>
        /// Bounds of the asteriod.
        /// </summary>
        public override Rectangle Bounds
        {
            get { return SPRITEBOUNDS[_size - 1]; }
        }

        /// <summary>
        /// Updates the asteriods position and will spawn more asteriod of the asteriod is destoryed.
        /// </summary>
        /// <param name="elapsedTime">Time that has passed since the last call.</param>
        public override void Update(float elapsedTime)
        {
            _position += VELOCITIES[_size - 1];
            
            //If the asteriod is destroyed then spawn more in it's place.
            if (Health <= 0 && _size > 1)
            {
                for (int i = 0; i < _size; i++)
                {
                    ScrollingShooterGame.GameObjectManager.CreateEnemy(EnemyType.Asteriod, _position, new object[] { _size - 1 });
                }
            }
        }

        /// <summary>
        /// Draws the asteriod.
        /// </summary>
        /// <param name="elapsedTime">Time that has passed since the last call.</param>
        /// <param name="spriteBatch">SpriteBatch to draw to.</param>
        public override void Draw(float elapsedTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_spritesheet, Bounds, SPRITEBOUNDS[_size - 1], Color.White);
        }
    }
}
