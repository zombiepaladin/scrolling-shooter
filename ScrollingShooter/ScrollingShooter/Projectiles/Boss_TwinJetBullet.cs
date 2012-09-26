//Twin Jet Bullet Class:
//Coders: Nicholas Boen
//Date: 9/17/2012
//Time: 3:51 P.M.
//
//The bullet that the Twin Jets fire

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;


namespace ScrollingShooter
{
    class Boss_TwinJetBullet : Projectile
    {
        /// <summary>
        /// Constructs a brand new bullet, still steaming
        /// </summary>
        /// <param name="id">The factory id of this bullet</param>
        /// <param name="content">The Content Manager to draw this bullet</param>
        /// <param name="position">The position to draw this bullet to</param>
        public Boss_TwinJetBullet(uint id, ContentManager content, Vector2 position)
            : base(id)
        {
            //Set the sprite sheet
            this.spriteSheet = content.Load<Texture2D>("Spritesheets/tyrian.shp.000000");

            //Set the sprite bounds
            this.spriteBounds = new Rectangle(4, 59, 4, 11);

            //Set the velocity
            this.velocity = new Vector2(0, 300);

            //Set the position
            this.position = position;
        }
    }
}
