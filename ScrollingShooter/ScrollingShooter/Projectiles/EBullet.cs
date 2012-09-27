using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace ScrollingShooter
{
    /// <summary>
    /// This is a class that manages the standared baddy bullet.
    /// </summary>
    class EBullet:Projectile
    {
         public EBullet(uint id, ContentManager content, Vector2 position) : base (id)
        {   
            this.spriteSheet = content.Load<Texture2D>("Spritesheets/EBullet");

            this.spriteBounds = new Rectangle(0, 0, 7, 11);

            this.velocity = new Vector2(0, 300);

            this.position = position;
        }
 
    }
}