using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace ScrollingShooter
{
    /// <summary>
    /// A enemy fired flameball 
    /// </summary>
    public class EnemyFlameball : Projectile
    {
        /// <summary>
        /// Creates a new enemy flameball
        /// </summary>
        /// <param name="content">A ContentManager to load content from</param>
        /// <param name="position">A position on the screen</param>
        public EnemyFlameball(uint id, ContentManager content, Vector2 position, ProjectileType type)
            : base(id, type)
        {
            this.spriteSheet = content.Load<Texture2D>("Spritesheets/tyrian.shp.000000");

            this.spriteBounds = new Rectangle(203, 56, 13, 14);

            this.velocity = new Vector2(0, 250);

            this.position = position;
        }
    }
}
