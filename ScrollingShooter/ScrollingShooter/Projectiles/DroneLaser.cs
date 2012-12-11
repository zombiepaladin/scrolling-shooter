using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace ScrollingShooter
{

    /// <summary>
    /// A laser fired from a LaserDrone
    /// </summary>
    public class DroneLaser : Projectile
    {
        /// <summary>
        /// Stores the 3 sprites of the laser
        /// </summary>
        new Rectangle[] spriteBounds = new Rectangle[3];

        /// <summary>
        /// Stores the power level of the laser
        /// </summary>
        public WeaponChargeLevel laserPower = WeaponChargeLevel.Full;

        /// <summary>
        /// Scale vector used to stretch the laser sprite
        /// </summary>
        private Vector2 scale;

        /// <summary>
        /// Indicates whether the laser is turned on
        /// </summary>
        public bool isOn = true;

        /// <summary>
        /// Creates a new DroneLaser
        /// </summary>
        /// <param name="content">A ContentManager to load content from</param>
        /// <param name="position">A position on the screen</param>
        public DroneLaser(uint id, ContentManager content, Vector2 position)
            : base(id)
        {
            this.spriteSheet = content.Load<Texture2D>("Spritesheets/tyrian.shp.000000");

            spriteBounds[(int)WeaponChargeLevel.Full].X = 49;
            spriteBounds[(int)WeaponChargeLevel.Full].Y = 210;
            spriteBounds[(int)WeaponChargeLevel.Full].Width = 11;
            spriteBounds[(int)WeaponChargeLevel.Full].Height = 14;

            spriteBounds[(int)WeaponChargeLevel.Medium].X = 2;
            spriteBounds[(int)WeaponChargeLevel.Medium].Y = 210;
            spriteBounds[(int)WeaponChargeLevel.Medium].Width = 9;
            spriteBounds[(int)WeaponChargeLevel.Medium].Height = 14;

            spriteBounds[(int)WeaponChargeLevel.Low].X = 39;
            spriteBounds[(int)WeaponChargeLevel.Low].Y = 210;
            spriteBounds[(int)WeaponChargeLevel.Low].Width = 7;
            spriteBounds[(int)WeaponChargeLevel.Low].Height = 14;

            this.scale = new Vector2(1, 40);

            this.velocity = Vector2.Zero;

            this.position = position;
        }

        /// <summary>
        /// Moves the top-center of the laser to the given coordinates
        /// </summary>
        /// <param name="newX">The X value to move the top-center of the laser to</param>
        /// <param name="newY">The Y value to move the top-center of the laser to</param>
        public void updatePosition(float newX, float newY)
        {
            position.X = newX - spriteBounds[(int)laserPower].Width / 2;
            position.Y = newY;
        }

        /// <summary>
        /// Draw the Laser on the screen if it is enabled
        /// </summary>
        /// <param name="elapsedTime">The in-game time between the previous and current frame</param>
        /// <param name="spriteBatch">An already initialized SpriteBatch, ready for Draw() commands</param>
        public override void Draw(float elapsedTime, SpriteBatch spriteBatch)
        {
            if (isOn)
                spriteBatch.Draw(spriteSheet, position, spriteBounds[(int)laserPower], Color.White, 0, Vector2.Zero, scale, SpriteEffects.None, 0);
        }
    }
}
