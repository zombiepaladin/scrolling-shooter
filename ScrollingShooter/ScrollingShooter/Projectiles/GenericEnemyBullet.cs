using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace ScrollingShooter
{
    /// <summary>
    /// A generic bullet for enemies. Has 3 unique frames of animation.
    /// </summary>
    public class GenericEnemyBullet : Projectile
    {
        Rectangle[] animatedSpriteBounds = new Rectangle[3];
        float animationTimer;
        int currentSpriteBound;
        int spriteBoundIncrement = 1;

        const float GEB_VELOCITY = 120f;    //speed of GenericEnemyBullet
        const float ANIMATION_SPEED = 0.2f; //time for a single frame
        
        /// <summary>
        /// Creates a new GenericEnemyBullet
        /// </summary>
        /// <param name="content">A ContentManager to load content from</param>
        /// <param name="position">A position on the screen</param>
        public GenericEnemyBullet(uint id, ContentManager content, Vector2 position)
            : base(id)
        {
            this.spriteSheet = content.Load<Texture2D>("Spritesheets/tyrian.shp.000000");

            this.animatedSpriteBounds[0] = new Rectangle(195, 74, 7, 7);
            this.animatedSpriteBounds[1] = new Rectangle(207, 74, 7, 7);
            this.animatedSpriteBounds[2] = new Rectangle(219, 74, 7, 7);
            spriteBounds = this.animatedSpriteBounds[0];

            this.position = position;

            // This bullet aims once at the player and then continues in that direction
            Vector2 target = new Vector2(ScrollingShooterGame.Game.Player.Bounds.Center.X, ScrollingShooterGame.Game.Player.Bounds.Center.Y);
            Vector2 toTarget = target - this.position;
            toTarget.Normalize();
            this.velocity = toTarget * GEB_VELOCITY;

            //Reset the timer and current sprite
            this.animationTimer = ANIMATION_SPEED;
            this.currentSpriteBound = 0;
        }

        /// <summary>
        /// Updates the projectile
        /// </summary>
        /// <param name="elapsedTime">The time elapsed between the previous and current frame</param>
        public override void Update(float elapsedTime)
        {
            animationTimer -= elapsedTime;

            if (animationTimer <= 0) //if more than the ANIMATION_SPEED amount of time has elapsed, advance the animation
            {
                //This will make the following pattern of frames: 0,1,2,1,0,1,2,1,0,1, etc.
                currentSpriteBound += spriteBoundIncrement;
                if (currentSpriteBound == 2 || currentSpriteBound == 0) spriteBoundIncrement *= -1;

                spriteBounds = animatedSpriteBounds[currentSpriteBound];

                //Reset the timer
                animationTimer = ANIMATION_SPEED;
            }

            base.Update(elapsedTime);
        }
    }
}
