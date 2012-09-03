using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace ScrollingShooter
{
    /// <summary>
    /// A class representing a powerup that gives multiple firing ships
    /// </summary>
    class MultipleShipPowerup : Powerup
    {
        /// <summary>
        /// Creates a new mutliple ship powerup
        /// </summary>
        /// <param name="contentManager">A ContentManager to load resources with</param>
        /// <param name="position">The position the mutliple ship powerup in the world</param>
        public MultipleShipPowerup(ContentManager contentManager, Vector2 position)
        {
            this.spriteSource = new Rectangle(197, 88, 15, 16);
            this.spriteSheet = contentManager.Load<Texture2D>("Spritesheets/tyrian.shp.010008");
            this.spriteBounds = new Rectangle((int)position.X, (int)position.Y, 15, 16);
        }
    }
}
