using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace ScrollingShooter
{
    /// <summary>
    /// A shield generator that protects the final boss
    /// </summary>
    public class ShieldGenerator : Enemy
    {
        Texture2D spritesheet;
        Vector2 position;
        Rectangle spriteBounds;
        
        /// <summary>
        /// The bounding rectangle of the generator
        /// </summary>
        public override Rectangle Bounds
        {
            get { return new Rectangle((int)position.X, (int)position.Y, spriteBounds.Width, spriteBounds.Height); }
        }

        /// <summary>
        /// Creates a new instance of a shield generator
        /// </summary>
        /// <param name="content">A ContentManager to load resources with</param>
        /// <param name="position">The position of the Dart ship in the game world</param>
        public ShieldGenerator(uint id, ContentManager content, Vector2 position)
            : base(id)
        {
            Health = 500;

            this.position = position;

            spritesheet = content.Load<Texture2D>("Spritesheets/newshr.shp.000000");

            spriteBounds.X = 48;
            spriteBounds.Y = 2;
            spriteBounds.Width = 72;
            spriteBounds.Height = 82;

        }

        /// <summary>
        /// Updates the ShieldGenerator
        /// </summary>
        /// <param name="elapsedTime">The in-game time between the previous and current frame</param>
        public override void Update(float elapsedTime)
        {
            this.Health--;
        }

        /// <summary>
        /// Draw the Dart ship on-screen
        /// </summary>
        /// <param name="elapsedTime">The in-game time between the previous and current frame</param>
        /// <param name="spriteBatch">An already initialized SpriteBatch, ready for Draw() commands</param>
        public override void Draw(float elapsedTime, SpriteBatch spriteBatch)
        {
            if(Health > 0)
                spriteBatch.Draw(spritesheet, Bounds, spriteBounds, Color.White, 0f, new Vector2(Bounds.Width / 2, Bounds.Height / 2), SpriteEffects.None, 1f);
        }

    }
}