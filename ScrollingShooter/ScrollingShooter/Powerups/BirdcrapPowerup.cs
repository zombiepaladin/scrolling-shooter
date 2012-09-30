using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace ScrollingShooter
{
    /// <summary>
    /// This represents a bidcrap powerup.
    /// </summary>
    class BirdcrapPowerup:Powerup
    {
        /// <summary>
        /// creates the birdcrap powerup pickup.
        /// </summary>
        /// <param name="contentManager">A content manager for content</param>
        /// <param name="position">a postion on the screen</param>
        public BirdcrapPowerup(uint id, ContentManager contentManager, Vector2 position):base(id)
        {
            this.type = PowerupType.Birdcrap;
            this.spriteSource = new Rectangle(48, 120, 23, 23);
            this.spriteSheet = contentManager.Load<Texture2D>("Spritesheets/tyrian.shp.010008");
            this.spriteBounds = new Rectangle((int)position.X, (int)position.Y, 23, 23);
        }
    }
}