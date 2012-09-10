using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace ScrollingShooter
{
    /// <summary>
    /// A class representing a Trishield powerup
    /// </summary>
    public class TriShieldPowerup : Powerup
    {
        //To do:
        //1. Handle exceptional circumstances (aka have a trishield, pick up another)
        //2. Locate a better sprite for the projectile (just a bigger one will do)

        /// <summary>
        /// Creates a new Trishield powerup
        /// </summary>
        /// <param name="contentManager">A ContentManager to load resources with</param>
        /// <param name="position">The position for the trishield powerup in the world</param>
        public TriShieldPowerup(uint id, ContentManager contentManager, Vector2 position) : base(id)
        {
            this.type = PowerupType.TriShield;
            this.spriteSource = new Rectangle(72, 198, 24, 28);
            this.spriteSheet = contentManager.Load<Texture2D>("Spritesheets/tyrian.shp.010008");
            this.spriteBounds = new Rectangle((int)position.X, (int)position.Y, 24, 28);
        }
    }
}
