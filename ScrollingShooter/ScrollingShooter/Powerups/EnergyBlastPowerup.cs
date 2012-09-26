using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace ScrollingShooter
{
    /// <summary>
    /// A class representing an energy blast powerup
    /// </summary>
    public class EnergyBlastPowerup : Powerup
    {
        /// <summary>
        /// Creates a new energy blast powerup
        /// </summary>
        /// <param name="contentManager">A ContentManager to load resources with</param>
        /// <param name="position">The position the energy blast powerup spawns at in the world</param>
        public EnergyBlastPowerup(uint id, ContentManager contentManager, Vector2 position)
            :base(id)
        {
            this.type = PowerupType.EnergyBlast;
            this.spriteSource = new Rectangle(98, 115, 20, 21);
            this.spriteSheet = contentManager.Load<Texture2D>("Spritesheets/tyrian.shp.010008");
            this.spriteBounds = new Rectangle((int)position.X, (int)position.Y, 20, 21);
        }
    }
}
