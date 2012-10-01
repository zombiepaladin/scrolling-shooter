using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;


namespace ScrollingShooter
{
    class BigBad:Enemy
    {
        float dgt = 0;
        Texture2D spritesheet;
        Vector2 position;
        Rectangle spriteBounds = new Rectangle();


        public override Rectangle Bounds
        {
            get { return new Rectangle((int)position.X, (int)position.Y, spriteBounds.Width, spriteBounds.Height); }
        }

        public BigBad(uint id, ContentManager content, Vector2 position)
            : base(id)
        {
            Health = 60;
            this.position = position;

            spritesheet = content.Load<Texture2D>("Spritesheets/newsh2.shp.000000");

            spriteBounds.X = 96;
            spriteBounds.Y = 55;
            spriteBounds.Width = 45;
            spriteBounds.Height = 28;
        }

        /// <summary>
        /// Updates the Standard Baddy.
        /// </summary>
        /// <param name="elapsedTime">Time elapsed</param>
        public override void Update(float elapsedTime)
        {
            dgt += elapsedTime;
            PlayerShip ps = ScrollingShooterGame.Game.Player;
            Vector2 pp = new Vector2(ps.Bounds.Center.X, ps.Bounds.Center.Y);
            Vector2 dp = pp - this.position;
            if (dp.LengthSquared() > 30000)
            {
                dp.Normalize();
                this.position += dp * elapsedTime * 100;
            }
            if (dgt > .75f)
            {
                ScrollingShooterGame.GameObjectManager.CreateProjectile(ProjectileType.EBullet, position);
                dgt = 0;
            }
        }

        /// <summary>
        /// Draws the standared baddy.
        /// </summary>
        /// <param name="elapsedTime">Time elapsed</param>
        /// <param name="spriteBatch">Sprite grouping</param>
        public override void Draw(float elapsedTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(spritesheet, Bounds, spriteBounds, Color.White);
        }
    }
}
