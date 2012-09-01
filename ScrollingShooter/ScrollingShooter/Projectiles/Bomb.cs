using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace ScrollingShooter
{
    /// <summary>
    /// A bomb 
    /// </summary>
    public class Bomb : Projectile
    {

        /// <summary>
        /// Creates a new bomb
        /// </summary>
        /// <param name="content">A ContentManager to load content from</param>
        /// <param name="position">A position on the screen</param>
        public Bomb(ContentManager content, Vector2 position)
        {
            this.spriteSheet = content.Load<Texture2D>("Spritesheets/tyrian.shp.000000");

            this.spriteBounds = new Rectangle(47, 154, 13, 13);

            this.velocity = new Vector2(0, -100);

            this.position = position;
        }

    }
}
