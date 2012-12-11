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
        public EightBallShieldPowerup(uint id, ContentManager contentManager, Vector2 position)
            : base(id)
        {
            this.type = PowerupType.EightBallShield;
            this.spriteBounds = new Rectangle(167, 85, 26, 26);
            this.spriteSheet = contentManager.Load<Texture2D>("Spritesheets/tyrian.shp.010008");
            this.position = position;
        }
    }
}
