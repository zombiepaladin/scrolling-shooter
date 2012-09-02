using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using System;

namespace ScrollingShooter
{
    /// <summary>
    /// Class for the blade projectiles that will circle on top of the player
    /// </summary>
    class Blades: Projectile
    {
        //player reference
        private PlayerShip player;
        private Vector2 origin;

        //Rotation speed
        //To do: This will slow down as the powerup wears off
        private float rotationAngle = MathHelper.Pi / 32;

         /// <summary>
        /// Creates new blade projectiles
        /// </summary>
        /// <param name="content">A ContentManager to load content from</param>
        /// <param name="rotation">The rotation that it's at around the player (0-2*PI)</param>
        /// <param name="player">Reference back to the player</param>
        public Blades(ContentManager content, PlayerShip player)
        {   
            this.spriteSheet = content.Load<Texture2D>("Spritesheets/newsh#.shp.000000");

            this.spriteBounds = new Rectangle(148, 67, 43, 35);

            this.player = player;
        }

        public override void Update(float elapsedTime)
        {
            this.position.X = player.Bounds.X;
            this.position.Y = player.Bounds.Y;

            origin.X = player.Bounds.Center.X;
            origin.Y = player.Bounds.Center.Y;

            //TO DO: implement sprite rotation
            //rotationAngle += elapsedTime + 3;
            //float circle = MathHelper.Pi * 2;
            //rotationAngle = rotationAngle % circle;

        }

        //TO DO: Implement sprite rotation
        /// <summary>
        /// Draws the projectile on-screen
        /// </summary>
        /// <param name="elapsedTime">The elapsed time between the previous and current frame</param>
        /// <param name="spriteBatch">An already-initialized SpriteBatch, ready for Draw() commands</param>
        //public override void Draw(float elapsedTime, SpriteBatch spriteBatch)
        //{
        //    spriteBatch.Draw(this.spriteSheet, Bounds, spriteBounds, Color.White, rotationAngle, origin, SpriteEffects.None, 0.0f);
        //}

    }
}
