﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace ScrollingShooter
{
    /// <summary>
    /// A class representing a Frostball powerup
    /// </summary>
    public class FrostballPowerup : Powerup
    {
        /// <summary>
        /// Creates a new frostball powerup
        /// </summary>
        /// <param name="contentManager">A ContentManager to load resources with</param>
        /// <param name="position">The position the frostball powerup in the world</param>
        public FrostballPowerup(ContentManager contentManager, Vector2 position)
        {
            this.spriteSource = new Rectangle(0, 170, 23, 23);
            this.spriteSheet = contentManager.Load<Texture2D>("Spritesheets/tyrian.shp.010008");
            this.spriteBounds = new Rectangle((int)position.X, (int)position.Y, 23, 23);
        }
    }
}
