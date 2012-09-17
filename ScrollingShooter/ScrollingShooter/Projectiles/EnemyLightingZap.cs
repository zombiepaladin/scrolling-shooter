using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ScrollingShooter
{
    /// <summary>
    /// A limited-range lightning zap fired by the brain boss
    /// </summary>
    public class EnemyLightningZap : Projectile
    {
        /// <summary>
        /// Travel speed of the lightning
        /// </summary>
        private const int SPEED = 200;

        /// <summary>
        /// Distance until the lightning dies
        /// </summary>
        private double distanceLeft = 0;

        /// <summary>
        /// Angle of flight and render angle of the lightning
        /// </summary>
        private float angle = 0;

        /// <summary>
        /// Random number generator
        /// </summary>
        private static Random rand;

        /// <summary>
        /// Creates a new enemy lightning zap
        /// </summary>
        /// <param name="id">The game id to assign to the new object</param>
        /// <param name="content">A ContentManager to load content from</param>
        /// <param name="position">A position on the screen</param>
        public EnemyLightningZap(uint id, ContentManager content, Vector2 position)
            : base(id)
        {
            this.spriteSheet = content.Load<Texture2D>("Spritesheets/newsh(.shp.000000");

            this.spriteBounds = new Rectangle(36, 140, 12, 14);

            this.velocity = new Vector2(0, 0);

            this.position = position;

            if (rand == null)
                rand = new Random();
        }

        /// <summary>
        /// Sets the angle and distance for the lightning
        /// </summary>
        /// <param name="angle">The angle for the lightning to shoot</param>
        /// <param name="distance">The distance the lightning will travel</param>
        public void Initialize(float angle, double distance)
        {
            this.angle = angle;
            this.distanceLeft = distance;

            this.velocity.X = (float)Math.Cos(angle) * SPEED;
            this.velocity.Y = (float)Math.Sin(angle) * SPEED;

            this.angle -= (float) (Math.PI / 4);
        }

        /// <summary>
        /// Updates the lightning
        /// </summary>
        /// <param name="elapsedTime">Time passed since last frame</param>
        public override void Update(float elapsedTime)
        {
            base.Update(elapsedTime);
            
            distanceLeft -= SPEED * elapsedTime;

            if (distanceLeft < 0)
                ScrollingShooterGame.GameObjectManager.DestroyObject(this.ID);
        }

        /// <summary>
        /// Draws the projectile on-screen
        /// </summary>
        /// <param name="elapsedTime">The elapsed time between the previous and current frame</param>
        /// <param name="spriteBatch">An already-initialized SpriteBatch, ready for Draw() commands</param>
        public override void Draw(float elapsedTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(spriteSheet, Bounds, spriteBounds, Color.White, angle, Vector2.Zero, SpriteEffects.None, 1f);
        }
    }
}
