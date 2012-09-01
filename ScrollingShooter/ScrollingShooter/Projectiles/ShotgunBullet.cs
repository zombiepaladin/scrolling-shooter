using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace ScrollingShooter
{
    /// <summary>
    /// Represents the ____ possible directions for the bullet to travel in
    /// </summary>
    public enum BulletDirection
    {
        HardLeft = 0,
        Left = 1,
        Straight = 2,
        Right = 3,
        HardRight = 4,
    }

    public class ShotgunBullet : Projectile
    {
        BulletDirection direction;

        /// <summary>
        /// Creates a new shotgun bullet
        /// </summary>
        /// <param name="content">A ContentManager to load content from</param>
        /// <param name="position">The starting position of the bullet</param>
        /// <param name="bulletDirection"></param>
        public ShotgunBullet(ContentManager content, Vector2 position, BulletDirection bulletDirection)
        {
            this.spriteSheet = content.Load<Texture2D>("Spritesheets/tyrian.shp.01D8A7");

            direction = bulletDirection;

            this.spriteBounds = new Rectangle(38, 57, 7, 11);

            // Sets the velocity based on the direction the bullet should be headed in
            if (bulletDirection == BulletDirection.Right)
                this.velocity = new Vector2(50, -250);

            else if (bulletDirection == BulletDirection.Left)
                this.velocity = new Vector2(-50, -250);

            else if (bulletDirection == BulletDirection.HardLeft)
                this.velocity = new Vector2(-100, -200);

            else if (bulletDirection == BulletDirection.HardRight)
                this.velocity = new Vector2(100, -200);

            else
                this.velocity = new Vector2(0, -300);

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
