using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace ScrollingShooter
{
    /// <summary>
    /// A fireball projectile 
    /// </summary>
    public class Fireball : Projectile
    {
        /// <summary>
        /// Creates a new fireball
        /// </summary>
        /// <param name="content">A ContentManager to load content from</param>
        /// <param name="position">A position on the screen</param>
        public Fireball(uint id, ContentManager content, Vector2 position) : base (id)
        {   
            this.spriteSheet = content.Load<Texture2D>("Spritesheets/tyrian.shp.01D8A7");

            this.spriteBounds = new Rectangle(49, 56, 9, 13);

            this.velocity = new Vector2(0, -300);

            this.position = position;
        }
    }
}
