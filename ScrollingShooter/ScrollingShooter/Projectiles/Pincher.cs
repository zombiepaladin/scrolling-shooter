using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using System.Collections.Generic;
using System;

namespace ScrollingShooter
{
    /// <summary>
    /// Represents the three animation frames for the pincher
    /// </summary>
    enum PincherAnimationFrame
    {
        Open = 0,
        Mid = 1,
        Closed = 2,
    }

    /// <summary>
    /// A pincher that moves down the screen
    /// </summary>
    public class Pincher : Projectile
    {
        // Pincher state variables
        Texture2D spritesheet;
        Vector2 position;
        Rectangle[] spriteBounds = new Rectangle[3];

        int frame;
        List<PincherAnimationFrame> animationSequence;

        float angle;

        Vector2 angleVector;

        float timer;
        
        /// <summary>
        /// The bounding rectangle of the Pincher
        /// </summary>
        public override Rectangle Bounds
        {
            get { return new Rectangle((int)position.X, (int)position.Y, spriteBounds[(int)animationSequence[frame]].Width, spriteBounds[(int)animationSequence[frame]].Height); }
        }

        /// <summary>
        /// Creates a new instance of the Pincher 
        /// </summary>
        /// <param name="content">A ContentManager to load resources with</param>
        /// <param name="position">The position of the Pincher in the game world</param>
        public Pincher(uint id, ContentManager content, Vector2 position)
            : base(id)
        {
            this.position = position;

            spritesheet = content.Load<Texture2D>("Spritesheets/newshg.shp.000000");

            spriteBounds[(int)PincherAnimationFrame.Open].X = 73;
            spriteBounds[(int)PincherAnimationFrame.Open].Y = 84;
            spriteBounds[(int)PincherAnimationFrame.Open].Width = 23;
            spriteBounds[(int)PincherAnimationFrame.Open].Height = 27;

            spriteBounds[(int)PincherAnimationFrame.Mid].X = 98;
            spriteBounds[(int)PincherAnimationFrame.Mid].Y = 84;
            spriteBounds[(int)PincherAnimationFrame.Mid].Width = 21;
            spriteBounds[(int)PincherAnimationFrame.Mid].Height = 27;

            spriteBounds[(int)PincherAnimationFrame.Closed].X = 124;
            spriteBounds[(int)PincherAnimationFrame.Closed].Y = 84;
            spriteBounds[(int)PincherAnimationFrame.Closed].Width = 17;
            spriteBounds[(int)PincherAnimationFrame.Closed].Height = 27;

            frame = 0;
            animationSequence = new List<PincherAnimationFrame>();
            animationSequence.Add(PincherAnimationFrame.Open);
            animationSequence.Add(PincherAnimationFrame.Mid);
            animationSequence.Add(PincherAnimationFrame.Closed);
            animationSequence.Add(PincherAnimationFrame.Mid);

            //determine the angle

            // Sense the player's position
            PlayerShip player = ScrollingShooterGame.Game.Player;
            Vector2 playerPosition = new Vector2(player.Bounds.Center.X, player.Bounds.Center.Y);

            // Get a vector from our position to the player's position and normalize it
            angleVector = playerPosition - this.position;

            //normalize the angleVector
            angleVector.Normalize();
            playerPosition.Normalize();

            angle = (float)Math.Acos(Vector2.Dot(playerPosition, angleVector)) - .75f;
        }

        /// <summary>
        /// Updates the Pinchers
        /// </summary>
        /// <param name="elapsedTime">The in-game time between the previous and current frame</param>
        public override void Update(float elapsedTime)
        {
            timer += elapsedTime;
            if (timer >= .2)
            {
                timer = 0;
                if(++frame >= 4)
                    frame = 0;
            }

            position += angleVector * 3;
            
        }

        /// <summary>
        /// Draw the Pincher on-screen
        /// </summary>
        /// <param name="elapsedTime">The in-game time between the previous and current frame</param>
        /// <param name="spriteBatch">An already initialized SpriteBatch, ready for Draw() commands</param>
        public override void Draw(float elapsedTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(spritesheet, Bounds, spriteBounds[(int)animationSequence[frame]], Color.White, angle, new Vector2(Bounds.Width / 2, Bounds.Height / 2), SpriteEffects.None, 1f);
        }

    }
}