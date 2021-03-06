﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace ScrollingShooter
{
    /// <summary>
    /// A class representing a shotgun powerup
    /// </summary>
    public class ShotgunPowerup : Powerup
    {
        /// <summary>
        /// Creates a new shotgun powerup
        /// </summary>
        /// <param name="contentManager">A ContentManager to load resources with</param>
        /// <param name="position">The position of the shotgun powerup within the world</param>
        public ShotgunPowerup(uint id, ContentManager contentManager, Vector2 position)
            : base(id)
        {
            this.type = PowerupType.ShotgunPowerup;
            this.spriteBounds = new Rectangle(97, 142, 22, 23);
            this.spriteSheet = contentManager.Load<Texture2D>("Spritesheets/tyrian.shp.010008");
            this.position = position;
        }
    }
}
