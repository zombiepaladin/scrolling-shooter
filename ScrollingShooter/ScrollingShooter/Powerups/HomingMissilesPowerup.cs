//Homing Missile Powerup Class:
//Coders: Nicholas Boen
//Date: 9/1/2012
//Time: 11:25 P.M.

/*Class Diagram***************************************************
 *                   Homing Missile Powerup                      *
 *---------------------------------------------------------------*
 *---------------------------------------------------------------*
 * +HomingMissileProjectile(ContentManager, Vector2)             *
 *****************************************************************/

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace ScrollingShooter
{
    /// <summary>
    /// A class representing a homing missile powerup
    /// </summary>
    public class HomingMissilesPowerup : Powerup
    {
        #region Constructor

        /// <summary>
        /// Creates a new homing missile powerup
        /// </summary>
        /// <param name="contentManager">A ContentManager to load resources with</param>
        /// <param name="position">The position the homing missile powerup in the world</param>
        public HomingMissilesPowerup(uint id, ContentManager contentManager, Vector2 position):base(id)
        {
            this.type = PowerupType.HomingMissiles;
            this.spriteSource = new Rectangle(74, 171, 20, 21);
            this.spriteSheet = contentManager.Load<Texture2D>("Spritesheets/tyrian.shp.010008");
            this.spriteBounds = new Rectangle((int)position.X, (int)position.Y, 20, 21);
        }

        #endregion
    }
}
