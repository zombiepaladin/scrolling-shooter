using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace ScrollingShooter
{
    /// <summary>
    /// Class Representing the Railgun Powerup
    /// Written by Matt Hart
    /// Note:  Under current build, you must "collide" with the powerup to activate it
    /// </summary>
    class Railgun : Powerup
    {
        /// <summary>
        /// Creates a new Railgun powerup
        /// </summary>
        /// <param name="contentManager">>A ContentManager to load resources with</param>
        /// <param name="position">The position the railgun powerup in the world</param>
        public Railgun(uint id, ContentManager contentManager, Vector2 position) : base (id)
        {
            this.type = PowerupType.Railgun;
            this.spriteSource = new Rectangle(72, 114, 23, 23);
            this.spriteSheet = contentManager.Load<Texture2D>("Spritesheets/tyrian.shp.010008");
            this.spriteBounds = new Rectangle((int)position.X, (int)position.Y, 23, 23);
        }
    }
}