using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace ScrollingShooter
{
    /// <summary>
    /// Fireball that is fired from the Turret class.
    /// </summary>
    public class TurretFireball : Projectile
    {
        /// <summary>
        /// Creates a tracking fireball.
        /// </summary>
        /// <param name="content">Content Manager</param>
        /// <param name="position">Our turrets position</param>
        public TurretFireball(uint id, ContentManager content, Vector2 position)
            : base(id)
        {
            this.spriteSheet = content.Load<Texture2D>("Spritesheets/newshp.shp.000000");

            this.spriteBounds = new Rectangle(193, 88, 21, 21);

            PlayerShip Player = ScrollingShooterGame.Game.Player;
            Vector2 PlayerPosition = new Vector2(Player.Bounds.Center.X, Player.Bounds.Center.Y);

            this.velocity = new Vector2(0, 0.5f);

            this.position = position;                  
        }

        /// <summary>
        /// The update method that tracks the Players ship to hunt it
        /// down. Parts taken from the PlayerShip class.
        /// </summary>
        /// <param name="elapsedTime"></param>
        public override void Update(float elapsedTime)
        {
            // Sense the Player's position
            PlayerShip Player = ScrollingShooterGame.Game.Player;
            Vector2 PlayerPosition = new Vector2(Player.Bounds.Center.X, Player.Bounds.Center.Y);

            // Get a vector from our position to the Player's position
            Vector2 toPlayer = PlayerPosition - this.position;

            if (toPlayer.LengthSquared() < 95000)
            {
                toPlayer.Normalize();

                // Seems like 60 is the slowest I can go
                // without jitters. Would like a slower projectile, but I
                // think I will make it just update less.
                this.position += toPlayer * elapsedTime * 60;

            }
        }

        //Needs a deconstructor as these are fireballs. In my opinion.
    }
}
