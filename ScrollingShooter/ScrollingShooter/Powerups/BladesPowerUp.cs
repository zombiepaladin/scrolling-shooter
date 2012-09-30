using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
namespace ScrollingShooter
{
    /// <summary>
    /// A class representing a the blades powerup
    /// </summary>
    public class BladesPowerup : Powerup
    {

        /// <summary>
        /// Creates a new Blades powerup
        /// </summary>
        /// <param name="contentManager">A ContentManager to load resources with</param>
        /// <param name="position">The position of the player which the blades will go on top of</param>
       public BladesPowerup(uint id, ContentManager contentManager, Vector2 position) : base(id)
        {
            this.type = PowerupType.Blades;
            this.spriteSource = new Rectangle(144, 169, 24, 25);
            this.spriteSheet = contentManager.Load<Texture2D>("Spritesheets/tyrian.shp.010008");
            this.spriteBounds = new Rectangle((int)position.X, (int)position.Y, 24, 25);
        }
    }
}

