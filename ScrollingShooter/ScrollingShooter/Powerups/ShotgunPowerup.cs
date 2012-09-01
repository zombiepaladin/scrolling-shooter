using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace ScrollingShooter
{
    /// <summary>
    /// A class representing a spray shot powerup
    /// </summary>
    public class ShotgunPowerup : Powerup
    {
        /// <summary>
        /// Creates a new spray shot powerup
        /// </summary>
        /// <param name="contentManager">A ContentManager to load resources with</param>
        /// <param name="position">The position of the spray shot powerup within the world</param>
        public ShotgunPowerup(ContentManager contentManager, Vector2 position)
        {
            this.spriteSource = new Rectangle(97, 142, 22, 23);
            this.spriteSheet = contentManager.Load<Texture2D>("Spritesheets/tyrian.shp.010008");
            this.spriteBounds = new Rectangle((int)position.X, (int)position.Y, 22, 23);
        }
    }
}
