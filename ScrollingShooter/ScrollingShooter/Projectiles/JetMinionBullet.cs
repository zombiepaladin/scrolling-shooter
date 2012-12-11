//Jet Minion Bullet Class:
//Coders: Nicholas Boen
//Date: 9/9/2012
//Time: 12:08 A.M.
//
//Just a small round that the Jet Minion Fires, extremely simple

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;


namespace ScrollingShooter
{
    /// <summary>
    /// A basic JetMinionBullet 
    /// </summary>
    public class JetMinionBullet : Projectile
    {

        /// <summary>
        /// Creates a new JetMinionBullet
        /// </summary>
        /// <param name="content">A ContentManager to load content from</param>
        /// <param name="position">A position on the screen</param>
        public JetMinionBullet(uint id, ContentManager content, Vector2 position)
            : base(id)
        {   
            this.spriteSheet = content.Load<Texture2D>("Spritesheets/tyrian.shp.000000");

            this.spriteBounds = new Rectangle(171, 58, 6, 12);

            this.velocity = new Vector2(0, 150);

            this.position = position;
        }
    }
}
