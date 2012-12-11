using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace ScrollingShooter
{
    /// <summary>
    /// A generic enemy bullet that is given a velocity, giving the possibility of shooting at the Player
    /// </summary>
    public class EnemyBullet : Projectile
    {
        /// <summary>
        /// Creates a new enemy bullet
        /// </summary>
        /// <param name="content">A ContentManager to load content from</param>
        /// <param name="position">A position on the screen</param>
        /// <param name="velocity">A velocity for the bullet</param>
        public EnemyBullet(uint id, ContentManager content, Vector2 position, Vector2 velocity) : base (id)
        {   
            this.spriteSheet = content.Load<Texture2D>("Spritesheets/tyrian.shp.000000");

            this.spriteBounds = new Rectangle(195, 158, 5, 5);

            this.velocity = velocity;

            this.position = position;

            Damage = 5;
        }
    }
}
