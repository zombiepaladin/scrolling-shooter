using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace ScrollingShooter
{
    /// <summary>
    /// A smaller ship that shows up when multiship powerup is active
    /// </summary>
    class MultiShip : PlayerShip
    {
        /// <summary>
        /// Create a new smaller multi ship instance
        /// </summary>
        /// <param name="content"></param>
        public MultiShip(ContentManager content)
        {
            this.spriteSheet = content.Load<Texture2D>("Spritesheets/tyrian.shp.007D3C");

            for(int i = 0; i<5; i++)
            {
                this.spriteBounds[i].X = 216;
                this.spriteBounds[i].Y = 197;
                this.spriteBounds[i].Width = 11;
                this.spriteBounds[i].Height = 15;
            }
            this.velocity = new Vector2(100, 100);
        }

        
    }
}
