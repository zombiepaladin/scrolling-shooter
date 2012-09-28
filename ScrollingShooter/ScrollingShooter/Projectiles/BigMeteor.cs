using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using System;

namespace ScrollingShooter
{
    /// <summary>
    /// A large meteor that damages all enemies it passes through.
    /// Unlike small meteors, these require collision detection.
    /// They will only do damage if they are on the screen, otherwise they would spawn-kill enemies before you even see them.
    /// </summary>
    public class BigMeteor : Meteor
    {

        private Vector2 startPos;

        private int shotsLeft = 10;

        /// <summary>
        /// Creates a new big meteor
        /// </summary>
        /// <param name="content">A ContentManager to load content from</param>
        /// <param name="position">A position on the screen</param>
        public BigMeteor(uint id, ContentManager content, Vector2 position) : base(id, content, position)
        {
            //Different sprite - Have to use a right-facing meteor and rotate it 90 degrees, the down-facing meteor has a weird border that shows up
            this.spriteBounds = new Rectangle(108, 183, 12, 12);

            this.startPos = position;
            //Large meteors travel twice as fast as normal.
            velocity *= 2;

            Randomize();
        }

        private void Randomize()
        {
            //Random enlargement
            Random rand = new Random((int)(position.X * position.Y)); //Time-dependent seed would generate the same number for every object when they are created right after eachother.
            scale = 4 + rand.Next(7);

            position.X = rand.Next((int)startPos.X*2);
            position.Y = startPos.Y - rand.Next(200);

            //TODO: Make damage scale with size.
        }

        public override Rectangle Bounds
        {
            get
            {
                return new Rectangle((int) position.X, (int) position.Y, (int) (spriteBounds.Width * scale), (int) (spriteBounds.Height * scale));
            }
        }

        public override void Update(float elapsedTime)
        {
            base.Update(elapsedTime);

            if (position.Y > startPos.Y + 500)
            {
                shotsLeft--;
                if (shotsLeft <= 0)
                    ScrollingShooterGame.GameObjectManager.DestroyObject(this.ID);
                else
                    Randomize();
            }
        }

        /// <summary>
        /// Draws the projectile on-screen with scaling and rotated to fix the sprite.
        /// </summary>
        /// <param name="elapsedTime">The elapsed time between the previous and current frame</param>
        /// <param name="spriteBatch">An already-initialized SpriteBatch, ready for Draw() commands</param>
        public override void Draw(float elapsedTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(spriteSheet, position, spriteBounds, Color.White, (float) (Math.PI / 2), Vector2.Zero, scale, SpriteEffects.None, 0);
        }
        
        //TODO: collision detection
    }

}
