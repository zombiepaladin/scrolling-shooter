using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace ScrollingShooter
{
    /// <summary>
    /// A class representing a drone wave powerup
    /// </summary>
    class DroneWavePowerup : Powerup
    {
        /// <summary>
        /// Creates a new drone wave powerup
        /// </summary>
        /// <param name="contentManager">A ContentManager to load resources with</param>
        /// <param name="position">The position of the drone wave powerup in the world</param>
        public DroneWavePowerup(uint id, ContentManager contentManager, Vector2 position)
            : base(id)
        {
            this.type = PowerupType.DroneWave;

            this.spriteSource = new Rectangle(170, 198, 21, 22);

            this.spriteSheet = contentManager.Load<Texture2D>("Spritesheets/tyrian.shp.010008");

            this.spriteBounds = new Rectangle((int)position.X, (int)position.Y, 21, 22);


        }
    }
}