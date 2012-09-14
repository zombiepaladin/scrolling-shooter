using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace ScrollingShooter
{
    public class FreezewavePowerup : Powerup
    {
        /// <summary>
        /// Creates a Freezewave Powerup
        /// </summary>
        /// <param name="contentManager">Content Manager to load content</param>
        /// <param name="position">Where the powerup is drawn on screen</param>
        public FreezewavePowerup(uint id, ContentManager contentManager, Vector2 position) : base(id)
        {
            this.spriteSource = new Rectangle(168, 114, 23, 23);
            this.spriteSheet = contentManager.Load<Texture2D>("Spritesheets/tyrian.shp.010008");
            this.spriteBounds = new Rectangle((int)position.X, (int)position.Y, 23, 23);
        }
    }
}
