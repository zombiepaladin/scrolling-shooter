using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace ScrollingShooter
{
    /// <summary>
    /// Class for the railgun's sabot round
    /// </summary>
    class RGSabot : Projectile
    {
        protected Rectangle effectSpriteBounds;
        
        /// <summary>
        /// Bounds for the effect sprite. Will not have hit box
        /// </summary>
        public Rectangle EffectBounds
        {
            get { return new Rectangle((int)(position.X-2), (int)(position.Y-2), effectSpriteBounds.Width, effectSpriteBounds.Height); }
        }

        public RGSabot(uint id, ContentManager content, Vector2 position) : base (id)
        {
            this.spriteSheet = content.Load<Texture2D>("Spritesheets/tyrian.shp.01D8A7");

            this.spriteBounds = new Rectangle(38, 57, 8, 11);
            
            //Source of effect sprite
            this.effectSpriteBounds = new Rectangle(84, 14, 11, 27);

            this.velocity = new Vector2(0, -1000);

            this.position = position;
        }

        /// <summary>
        /// Override draw to draw the effect sprite
        /// </summary>
        /// <param name="elapsedTime">The elapsed time between the previous and current frame</param>
        /// <param name="spriteBatch">An already-initialized SpriteBatch, ready for Draw() commands</param>
        public override void Draw(float elapsedTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(spriteSheet, EffectBounds, effectSpriteBounds, Color.White);
            spriteBatch.Draw(spriteSheet, Bounds, spriteBounds, Color.White);
        }
    }
}