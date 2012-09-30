using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace ScrollingShooter
{
    /// <summary>
    /// A generic enemy bullet that is given a velocity, giving the possibility of shooting at the player
    /// </summary>
    public class EnemyTurretTowerBullet : Projectile
    {
        /// <summary>
        /// Creates a new enemy bullet
        /// </summary>
        /// <param name="content">A ContentManager to load content from</param>
        /// <param name="position">A position on the screen</param>
        /// <param name="velocity">A velocity for the bullet</param>
        public EnemyTurretTowerBullet(uint id, ContentManager content, Vector2 position, Vector2 velocity)
            : base(id)
        {
            this.spriteSheet = content.Load<Texture2D>("Spritesheets/tyrian.shp.01D8A7");

            this.spriteBounds = new Rectangle(15, 3, 6, 6);

            this.velocity = velocity;

            this.position = position;

            Damage = 10; 
        }

        /// <summary>
        /// Draws the projectile on-screen
        /// </summary>
        /// <param name="elapsedTime">The elapsed time between the previous and current frame</param>
        /// <param name="spriteBatch">An already-initialized SpriteBatch, ready for Draw() commands</param>
        public override void Draw(float elapsedTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(spriteSheet, Bounds, spriteBounds, Color.OrangeRed, 0f, new Vector2(Bounds.Width / 2, Bounds.Height / 2), SpriteEffects.None, 1f);
        }
    }
}
