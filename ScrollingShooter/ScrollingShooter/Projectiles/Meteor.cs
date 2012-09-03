using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using System;

namespace ScrollingShooter
{

    /// <summary>
    /// A decorative meteor sprite, slight damage will be done to all enemies on screen every second while meteor storm is active,
    /// instead of having each small meteor have collision detection.
    /// </summary>
    public class Meteor : Projectile
    {
        /// <summary>
        /// Stores the scale of the meteor.
        /// </summary>
        protected float scale;

        /// <summary>
        /// Creates a new small decorative meteor.
        /// </summary>
        /// <param name="content">A ContentManager to load content from</param>
        /// <param name="position">A position on the screen</param>
        public Meteor(ContentManager content, Vector2 position)
        {   
            this.spriteSheet = content.Load<Texture2D>("Spritesheets/tyrian.shp.000000");

            //cut off some bottom pixels of the sprite on purpose to make it look like a meteor
            this.spriteBounds = new Rectangle(49, 127, 9, 8);

            //Time-dependent seed would generate the same number for every object when they are created at the same time.
            //Use position to generate a seed instead.
            Random rand = new Random((int) (position.X * position.Y)); 
            int randomModifier = rand.Next(3);
            
            this.velocity = new Vector2(0, 500 + (randomModifier * 30));

            this.scale = 0.75f + (randomModifier * 0.5f);

            this.position = position;
        }

        /// <summary>
        /// Draws the projectile on-screen with scaling.
        /// </summary>
        /// <param name="elapsedTime">The elapsed time between the previous and current frame</param>
        /// <param name="spriteBatch">An already-initialized SpriteBatch, ready for Draw() commands</param>
        public override void Draw(float elapsedTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(spriteSheet, position, spriteBounds, Color.White, 0, Vector2.Zero, scale, SpriteEffects.None, 0);
        }

    }
}
