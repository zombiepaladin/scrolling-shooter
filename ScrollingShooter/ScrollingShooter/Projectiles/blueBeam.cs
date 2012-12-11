using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using System.Collections.Generic;

namespace ScrollingShooter
{
   

    /// <summary>
    /// A blueBeam projectile 
    /// </summary>
    public class blueBeam : Projectile
    {
        /// <summary>
        /// Creates a new blueBeam
        /// </summary>
        /// <param name="content">A ContentManager to load content from</param>
        /// <param name="position">A position on the screen</param>
        public blueBeam(uint id, ContentManager content, Vector2 position)
            : base(id)
        {
            this.spriteSheet = content.Load<Texture2D>("Spritesheets/newsh(.shp.000000");

            spriteBounds.X = 14;
            spriteBounds.Y = 83;
            spriteBounds.Width = 9;
            spriteBounds.Height = 30;
            
            this.velocity = new Vector2(0, 200);

            this.position = position;

           
        }

        public override void Draw(float elapsedTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(spriteSheet, Bounds, spriteBounds, Color.White);
            spriteBatch.Draw(spriteSheet, new Rectangle(Bounds.X, Bounds.Y - 15, Bounds.Width, Bounds.Height), spriteBounds, Color.White, 0f, Vector2.Zero, SpriteEffects.FlipVertically, 0);
        }
    }
}