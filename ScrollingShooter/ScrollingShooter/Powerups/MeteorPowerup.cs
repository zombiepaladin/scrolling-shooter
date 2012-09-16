using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace ScrollingShooter
{
    /// <summary>
    /// A class representing a meteor powerup
    /// This causes big meteors of various sizes to fly across the screen, damaging enemies they pass through.
    /// Many small meteors are also generated for decoration, but don't do any damage.
    /// Any enemies on screen will take a small amount of damage every second during the storm to simulate being hit by small meteors.
    /// </summary>
	
    public class MeteorPowerup : Powerup
    {
        /// <summary>
        /// Creates a new meteor powerup
        /// </summary>
        public MeteorPowerup(uint id, ContentManager contentManager, Vector2 position) : base (id)
        {
            this.type = PowerupType.MeteorPowerup;
            this.spriteSource = new Rectangle(194, 171, 20, 21);
            this.spriteSheet = contentManager.Load<Texture2D>("Spritesheets/tyrian.shp.010008");
            this.spriteBounds = new Rectangle((int)position.X, (int)position.Y, 20, 21);
        }
    }
}
