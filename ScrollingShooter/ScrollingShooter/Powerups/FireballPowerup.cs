﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace ScrollingShooter
{
    /// <summary>
    /// A class representing a fireball powerup
    /// </summary>
    public class FireballPowerup : Powerup
    {
        /// <summary>
        /// Creates a new fireball powerup
        /// </summary>
        /// <param name="contentManager">A ContentManager to load resources with</param>
        /// <param name="position">The position the fireball powerup in the world</param>
        public FireballPowerup(uint id, ContentManager contentManager, Vector2 position) : base(id)
        {
            this.type = PowerupType.Fireball;
            this.spriteBounds = new Rectangle(50, 115, 20, 21);
            this.spriteSheet = contentManager.Load<Texture2D>("Spritesheets/tyrian.shp.010008");
            this.position = position;
        }
    }
}
