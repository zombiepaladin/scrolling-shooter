using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace ScrollingShooter
{
    /// <summary>
    /// A class representing a fireball powerup
    /// </summary>
    public class FireballPowerup : Powerup
    {
        /// <summary>
        /// Creates a new fireball powerup
        /// </summary>
        /// <param name="contentManager">A ContentManager to load resources with</param>
        /// <param name="position">The position the fireball powerup in the world</param>
        public FireballPowerup(uint id, ContentManager contentManager, Vector2 position) : base(id)
        {
            this.spriteSource = new Rectangle(48, 114, 23, 23);
            this.spriteSheet = contentManager.Load<Texture2D>("Spritesheets/tyrian.shp.010008");
            this.spriteBounds = new Rectangle((int)position.X, (int)position.Y, 23, 23);
        }
    }
}
