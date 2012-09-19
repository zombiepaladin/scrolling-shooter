using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace ScrollingShooter
{
    /// <summary>
    /// A cannonball projectile 
    /// </summary>
    public class Cannonball : Projectile
    {
        /// <summary>
        /// Creates a new cannonball
        /// </summary>
        /// <param name="content">A ContentManager to load content from</param>
        /// <param name="position">A position on the screen</param>
        public Cannonball(ContentManager content, Vector2 position)
        {
            this.spriteSheet = content.Load<Texture2D>("Spritesheets/tyrian.shp.01D8A7");

            this.spriteBounds = new Rectangle(47, 69, 11, 21);

            this.velocity = new Vector2(0, -300);

            this.position = position;
        }
    }
}