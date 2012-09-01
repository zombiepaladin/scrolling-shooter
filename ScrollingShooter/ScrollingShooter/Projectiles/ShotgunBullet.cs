using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace ScrollingShooter
{
    /// <summary>
    /// Represents the 5 possible directions for the bullet to travel in
    /// </summary>
    public enum BulletDirection
    {
        HardLeft = 0,
        Left = 1,
        Straight = 2,
        Right = 3,
        HardRight = 4,
    }

    /// <summary>
    /// Represents a bullet that is shot when the shotgun powerup is active
    /// </summary>
    public class ShotgunBullet : Projectile
    {
        // The direction the bullet is traveling
        BulletDirection direction;

        /// <summary>
        /// Creates a new shotgun bullet
        /// </summary>
        /// <param name="content">A ContentManager to load content from</param>
        /// <param name="position">The starting position of the bullet</param>
        /// <param name="bulletDirection"></param>
        public ShotgunBullet(ContentManager content, Vector2 position, BulletDirection bulletDirection)
        {
            this.spriteSheet = content.Load<Texture2D>("Spritesheets/newsh(.shp.000000");

            direction = bulletDirection;

            this.spriteBounds = new Rectangle(146, 99, 7, 13);

            // Sets the velocity based on the direction the bullet should be headed in
            if (bulletDirection == BulletDirection.Right)
                this.velocity = new Vector2(100, -500);

            else if (bulletDirection == BulletDirection.Left)
                this.velocity = new Vector2(-100, -500);

            else if (bulletDirection == BulletDirection.HardLeft)
                this.velocity = new Vector2(-200, -400);

            else if (bulletDirection == BulletDirection.HardRight)
                this.velocity = new Vector2(200, -400);

            else
                this.velocity = new Vector2(0, -600);

            this.position = position;
        }

        /// <summary>
        /// Draws a shotgun bullet and rotates it in the direction it is traveling
        /// </summary>
        /// <param name="elapsedTime">The elapsed time</param>
        /// <param name="spriteBatch">An already-initialized spritebatch, ready for Draw() commands</param>
        public override void Draw(float elapsedTime, SpriteBatch spriteBatch)
        {
            if (direction == BulletDirection.Right)
                spriteBatch.Draw(spriteSheet, Bounds, spriteBounds, Color.White, 0.393f, new Vector2(0, 0), new SpriteEffects(), 0);

            else if (direction == BulletDirection.Left)
                spriteBatch.Draw(spriteSheet, Bounds, spriteBounds, Color.White, -0.393f, new Vector2(0, 0), new SpriteEffects(), 0);

            else if (direction == BulletDirection.HardLeft)
                spriteBatch.Draw(spriteSheet, Bounds, spriteBounds, Color.White, -0.785f, new Vector2(0, 0), new SpriteEffects(), 0);

            else if (direction == BulletDirection.HardRight)
                spriteBatch.Draw(spriteSheet, Bounds, spriteBounds, Color.White, 0.785f, new Vector2(0, 0), new SpriteEffects(), 0);

            else
                spriteBatch.Draw(spriteSheet, Bounds, spriteBounds, Color.White);
        }
    }
}
