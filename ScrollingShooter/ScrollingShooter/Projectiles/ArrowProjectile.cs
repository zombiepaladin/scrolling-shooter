using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace ScrollingShooter
{
    public class ArrowProjectile : Projectile
    {
        /// <summary>
        /// Creates a new projecilte for the Arrow ship
        /// </summary>
        /// <param name="content">A ContentManager to load content from</param>
        /// <param name="position">A position on the screen</param>
        public ArrowProjectile(uint id, ContentManager content, Vector2 position, ProjectileType type)
            : base(id, type)
        {
            this.spriteSheet = content.Load<Texture2D>("Spritesheets/newsh(.shp.000000");

            this.spriteBounds = new Rectangle(96, 56, 12, 13);

            this.velocity = new Vector2(0, 370);

            this.position = position;
        }
    }
}