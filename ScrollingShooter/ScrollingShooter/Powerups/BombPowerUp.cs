﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace ScrollingShooter
{
    /// <summary>
    /// A class representing a bomb powerup
    /// </summary>
    public class BombPowerUp : Powerup
    {
        /// <summary>
        /// Creates a new bomb powerup
        /// </summary>
        /// <param name="contentManager">A ContentManager to load resources with</param>
        /// <param name="position">The position the bomb powerup in the world</param>
        public BombPowerUp(uint id,ContentManager contentManager, Vector2 position) : base(id)
        {
            this.type = PowerupType.Bomb;
            this.spriteBounds = new Rectangle(73, 142, 23, 23);
            this.spriteSheet = contentManager.Load<Texture2D>("Spritesheets/tyrian.shp.010008");
            this.position = position;
        }
    }
}
