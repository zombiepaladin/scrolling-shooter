using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace ScrollingShooter
{
    public class FreezewaveProjectile : Projectile
    {
        float animateTimer;
        Rectangle[] spriteBoundsAnimate;
        public FreezewaveProjectile(uint id, ContentManager contentManager, Vector2 position) : base(id)
        {
            this.spriteSheet = contentManager.Load<Texture2D>("Spritesheets/newsh(.shp.000000");
            this.spriteBounds = new Rectangle(0, 155, 13, 50);
            this.velocity = new Vector2(0, -200);
            this.position = position;
            spriteBoundsAnimate = new Rectangle[3];
            spriteBoundsAnimate[0] = new Rectangle(0, 155, 12, 50);
            spriteBoundsAnimate[1] = new Rectangle(12, 155, 12, 50);
            spriteBoundsAnimate[2] = new Rectangle(24, 155, 12, 50);
            animateTimer = 0f;
        }

        public override void Draw(float elapsedTime, SpriteBatch spriteBatch)
        {
            animateTimer += elapsedTime;
            if (animateTimer < .25f)
                spriteBatch.Draw(spriteSheet, Bounds, spriteBoundsAnimate[0], Color.White);
            else if (animateTimer < .5f)
                spriteBatch.Draw(spriteSheet, Bounds, spriteBoundsAnimate[1], Color.White);
            else if (animateTimer < .75f)
                spriteBatch.Draw(spriteSheet, Bounds, spriteBoundsAnimate[0], Color.White);
            else if (animateTimer < 1f)
                spriteBatch.Draw(spriteSheet, Bounds, spriteBoundsAnimate[2], Color.White);
            else if (animateTimer < 1.25f)
                animateTimer = 0f;
        }
    }
}
