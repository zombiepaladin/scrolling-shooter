using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using System;

namespace ScrollingShooter
{
    /// <summary>
    /// Class for the blade projectiles that will circle on top of the Player
    /// </summary>
    class Blades : Projectile
    {
        //Player reference
        private PlayerShip Player;

        private float BladeTimer;

        //Rotation speed
        private float rotationAngle = MathHelper.Pi /2;

        /// <summary>
        /// Creates new blade projectiles
        /// </summary>
        /// <param name="content">A ContentManager to load content from</param>
        /// <param name="rotation">The rotation that it's at around the Player (0-2*PI)</param>
        /// <param name="Player">Reference back to the Player</param>
        public Blades(uint id, ContentManager content) : base (id)
        {
            this.spriteSheet = content.Load<Texture2D>("Spritesheets/newsh#.shp.000000");

            this.spriteBounds = new Rectangle(148, 67, 43, 35);

            this.Player = ScrollingShooterGame.Game.Player;

            this.BladeTimer = 0;
        }

        public override void Update(float elapsedTime)
        {
            this.position.X = Player.Bounds.X;
            this.position.Y = Player.Bounds.Y;

            rotationAngle += elapsedTime + 3.35f;
            float circle = MathHelper.Pi;
            rotationAngle = rotationAngle % circle;

            BladeTimer += elapsedTime;

            if (BladeTimer > 10)
                ScrollingShooterGame.GameObjectManager.DestroyObject(this.ID);

        }

        /// <summary>
        /// Draws the projectile on-screen
        /// </summary>
        /// <param name="elapsedTime">The elapsed time between the previous and current frame</param>
        /// <param name="spriteBatch">An already-initialized SpriteBatch, ready for Draw() commands</param>
        public override void Draw(float elapsedTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(this.spriteSheet, Bounds, spriteBounds, Color.White, rotationAngle, new Vector2(spriteBounds.Width/2, spriteBounds.Height/2), SpriteEffects.None, 1.0f);
        }

    }
}
