using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace ScrollingShooter
{
    /// <summary>
    /// A class representing a AlePowerup powerup
    /// The ale powerup will reverse the ship's movement directions and make the ship deal double damage.  It will last for about
    /// 5-10 seconds.
    /// </summary>
    /// TODO LIST
    /// 1.  Double damage dealt (when damage system has been added).
    /// 
   
    public class AlePowerup : Powerup
    {
        /// <summary>
        /// Creates a new AlePowerup powerup
        /// </summary>
        /// <param name="contentManager">
        /// A ContentManager to load resources with
        /// </param>
        /// <param name="position">
        /// A Vector2 indicating the powerup's position 
        /// within the game world
        /// </param>
        public AlePowerup(uint id, ContentManager contentManager, Vector2 position) : base (id)
        {
            this.type = PowerupType.Ale;
            this.spriteSource = new Rectangle(148, 173, 19, 18);
            this.spriteSheet = contentManager.Load<Texture2D>("Spritesheets/tyrian.shp.01673F");
            this.spriteBounds = new Rectangle((int)position.X, (int)position.Y, 19, 18);
        }
    }
}