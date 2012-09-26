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
    /// A psi ball fired by the brain boss
    /// </summary>
    public class EnemyPsiBall : Projectile
    {
        /// <summary>
        /// Current speed of the psi ball
        /// </summary>
        private int speed = 300;

        /// <summary>
        /// Current color to tint the psi ball
        /// </summary>
        private Color color;

        /// <summary>
        /// Flight angle of the psi ball
        /// </summary>
        private float angle = 0;

        /// <summary>
        /// If true, turns a random color every time it renders.
        /// </summary>
        private bool randomize = false;

        /// <summary>
        /// Random number generator
        /// </summary>
        private static Random rand;

        /// <summary>
        /// Width of the game screen
        /// </summary>
        private static int gameWidth = ScrollingShooterGame.Game.GraphicsDevice.Viewport.Width;
        
        /// <summary>
        /// Height of the game screen
        /// </summary>
        private static int gameHeight = ScrollingShooterGame.Game.GraphicsDevice.Viewport.Height;

        /// <summary>
        /// Render origin for draw method
        /// </summary>
        private static Vector2 origin;

        /// <summary>
        /// Creates a new enemy psi ball
        /// </summary>
        /// <param name="id">The game id to assign to the new object</param>
        /// <param name="content">A ContentManager to load content from</param>
        /// <param name="position">A position on the screen</param>
        public EnemyPsiBall(uint id, ContentManager content, Vector2 position)
            : base(id)
        {
            this.spriteSheet = content.Load<Texture2D>("Spritesheets/newsh^.shp.000000");

            this.spriteBounds = new Rectangle(123, 117, 18, 18);

            this.velocity = new Vector2(0, 0);

            this.position = position;

            color = Color.White;
            
            if(rand == null)
                rand = new Random();

            if (origin == null || origin.Equals(Vector2.Zero))
                origin = new Vector2(9, 9);
        }

        /// <summary>
        /// Set the fire angle and speed of the psi ball
        /// </summary>
        /// <param name="angle">The angle that the ball will move</param>
        /// <param name="speed">The speed that the ball will move</param>
        public void Initialize(float angle, int speed)
        {
            this.angle = angle;
            this.speed = speed;

            this.velocity.X = (float) Math.Cos(angle) * speed;
            this.velocity.Y = (float) Math.Sin(angle) * speed;
        }

        /// <summary>
        /// Set the color of the psi ball
        /// </summary>
        /// <param name="color">The color of the psi ball</param>
        public void SetColor(Color color)
        {
            this.color = color;
        }

        /// <summary>
        /// Turn on/off color randomization (strobing)
        /// </summary>
        /// <param name="randomize">Indicates whether or not to randomize the psi ball's color every step</param>
        public void SetRandom(bool randomize)
        {
            this.randomize = randomize;
        }

        /// <summary>
        /// Updates the psi ball
        /// </summary>
        /// <param name="elapsedTime">Time passed since last frame</param>
        public override void Update(float elapsedTime)
        {
            base.Update(elapsedTime);

            if (position.X < 0 || position.X > gameWidth || position.Y < 0 || position.Y > gameHeight)
            {
                ScrollingShooterGame.GameObjectManager.DestroyObject(this.ID);
            }
        }

        /// <summary>
        /// Draws the projectile on-screen
        /// </summary>
        /// <param name="elapsedTime">The elapsed time between the previous and current frame</param>
        /// <param name="spriteBatch">An already-initialized SpriteBatch, ready for Draw() commands</param>
        public override void Draw(float elapsedTime, SpriteBatch spriteBatch)
        {
            if (randomize)
            {
                color.R = (byte)rand.Next(255);
                color.G = (byte)rand.Next(255);
                color.B = (byte)rand.Next(255);
            }
            spriteBatch.Draw(spriteSheet, Bounds, spriteBounds, color, 0f, origin, SpriteEffects.None, 1f);
        }
    }
}
