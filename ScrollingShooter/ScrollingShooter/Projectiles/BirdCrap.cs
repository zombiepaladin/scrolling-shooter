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
    /// Represents birdcrap.
    /// </summary>
    class BirdCrap:Projectile
    {
        /// <summary>
        /// This creates a new birdcrap
        /// </summary>
        /// <param name="content">A content manager to load content from</param>
        /// <param name="position">A positon on the screen</param>
        public BirdCrap(ContentManager content, Vector2 position)
        {
            this.spriteSheet = content.Load<Texture2D>("Spritesheets/birdcrapsprite");
            this.spriteBounds = this.spriteBounds = new Rectangle(0, 0, 9, 13);
            this.velocity = new Vector2(0, -300);
            this.position = position;
        }
    }
}
