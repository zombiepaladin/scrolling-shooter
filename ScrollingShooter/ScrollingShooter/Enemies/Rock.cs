using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace ScrollingShooter
{
    public class Rock : Enemy
    {
        /// <summary>
        /// The bounding rectangle
        /// </summary>
        public override Rectangle Bounds
        {
            get { return new Rectangle((int)position.X, (int)position.Y, spriteBounds.Width, spriteBounds.Height); }
        }

        public Rock(uint id, ContentManager content, Vector2 position)
            : base(id)
        {
            this.position = position;
            this.Score = 25;

            spriteBounds.X = 191;
            spriteBounds.Y = 142;
            spriteBounds.Width = 25;
            spriteBounds.Height = 26;
        }

        public override void Update(float elapsedTime)
        {

        }

        public override void Draw(float elapsedTime, SpriteBatch spriteBatch)
        {
           //DO nothing
        }

        public override void ScrollWithMap(float elapsedTime)
        {
            position.Y += ScrollingSpeed * elapsedTime;
        }
    }
}

