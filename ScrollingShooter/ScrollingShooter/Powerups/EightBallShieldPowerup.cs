using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace ScrollingShooter
{
    /// <summary>
    /// This class controls the eight ball shield.
    /// Author: Josh Zavala
    /// </summary>
    public class EightBallShieldPowerup : Powerup
    {
        /// <summary>
        /// Creates a new eight ball shield powerup
        /// </summary>
        /// <param name="contentManager">A ContentManager to load resources with</param>
        /// <param name="position">The position the eight ball shield powerup in the world</param>
        public EightBallShieldPowerup(ContentManager contentManager, Vector2 position)
        {
            this.spriteSource = new Rectangle(167, 85, 26, 26);
            this.spriteSheet = contentManager.Load<Texture2D>("Spritesheets/tyrian.shp.010008");
            this.spriteBounds = new Rectangle((int)position.X, (int)position.Y, 23, 23);
        }
    }
}
