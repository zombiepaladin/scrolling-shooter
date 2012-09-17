using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace ScrollingShooter
{
    /// <summary>
    /// creates the laser for the moon boss
    /// </summary>
    class Laser:Projectile
    {
        new Rectangle spriteBounds = new Rectangle();
        /// <summary>
        /// creates a new laser
        /// </summary>
        /// <param name="id"></param>
        /// <param name="content"></param>
        /// <param name="position"></param>
        public Laser(uint id, ContentManager content, Vector2 position)
            : base(id)
        {
            this.spriteSheet = content.Load<Texture2D>("Spritesheets/tyrian.shp.000000");

            spriteBounds.X = 49;
            spriteBounds.Y = 210;
            spriteBounds.Width = 11;
            spriteBounds.Height = 14;

            this.velocity = Vector2.Zero;

            this.position = position;
        }
        /// <summary>
        /// draws a new laser
        /// </summary>
        /// <param name="elapsedTime"></param>
        /// <param name="spriteBatch"></param>
        public override void Draw(float elapsedTime, SpriteBatch spriteBatch)
        {
                spriteBatch.Draw(spriteSheet, position, spriteBounds, Color.White);
        }
    }
}
