using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace ScrollingShooter
{
    /// <summary>
    /// A frostball projectile 
    /// </summary>
    public class Frostball : Projectile
    {
        /// <summary>
        /// Creates a new frostball
        /// </summary>
        /// <param name="content">A ContentManager to load content from</param>
        /// <param name="position">A position on the screen</param>
        public Frostball(uint id, ContentManager content, Vector2 position) : base(id)
        {
            this.spriteSheet = content.Load<Texture2D>("Spritesheets/tyrian.shp.000000");

            this.spriteBounds = new Rectangle(71, 182, 12, 15);

            this.velocity = new Vector2(0, -200);

            this.position = position;
        }
    }
}