using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace ScrollingShooter
{
    /// <summary>
    /// A drone projectile
    /// </summary>
    public class DroneWave : Projectile
    {
        /// <summary>
        /// Creates a new drone 
        /// </summary>
        /// <param name="content">A ContentManager to load content from</param>
        /// <param name="position">A position on the screen</param>
        public DroneWave(uint id, ContentManager content, Vector2 position)
            : base(id)
        {
            this.spriteSheet = content.Load<Texture2D>("Spritesheets/tyrian.shp.010008");

            this.spriteBounds = new Rectangle(146, 63, 21, 14);

            this.velocity = new Vector2(0, -210);

            this.position = position;
        }
    }
}