using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace ScrollingShooter
{
    public class Mine : Enemy
    {
        Texture2D spritesheet;
        Vector2 position;
        Rectangle spriteBounds;

        /// <summary>
        /// The bounding rectangle
        /// </summary>
        public override Rectangle Bounds
        {
            get { return new Rectangle((int)position.X, (int)position.Y, spriteBounds.Width, spriteBounds.Height); }
        }

        public Mine(uint id, ContentManager content, Vector2 position)
            : base(id)
        {
            this.position = position;
            spritesheet = content.Load<Texture2D>("Spritesheets/newsha.shp.000000");

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
            spriteBatch.Draw(spritesheet, Bounds, spriteBounds, Color.White, 0f, new Vector2(Bounds.Width / 2, Bounds.Height / 2), SpriteEffects.None, 1f);
        }

        public override void ScrollWithMap(float elapsedTime)
        {
            position.Y += ScrollingSpeed * elapsedTime;
        }
    }
}

