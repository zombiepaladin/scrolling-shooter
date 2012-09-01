using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace ScrollingShooter
{
    /// <summary>
    /// A class representing a bomb powerup
    /// </summary>
    public class BombPowerUp : Powerup
    {
        /// <summary>
        /// Creates a new bomb powerup
        /// </summary>
        /// <param name="contentManager">A ContentManager to load resources with</param>
        /// <param name="position">The position the bomb powerup in the world</param>
        public BombPowerUp(ContentManager contentManager, Vector2 position)
        {
            this.spriteSource = new Rectangle(73, 142, 23, 23);
            this.spriteSheet = contentManager.Load<Texture2D>("Spritesheets/tyrian.shp.010008");
            this.spriteBounds = new Rectangle((int)position.X, (int)position.Y, 23, 23);
        }
    }
}
