//Samuel Fike

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ScrollingShooter
{
    

    class BrainBossProtection : Enemy
    {
        private int partsLeft = 3;

        private Texture2D eyeSpritesheet;
        private Texture2D spikesSpritesheet;
        private Texture2D helmetSpritesheet;

        private Rectangle eyeSpriteBounds;
        private Rectangle spikesSpriteBounds;
        private Rectangle helmetSpriteBounds;

        private Vector2 eyeCenterOffset;
        private Vector2 spikesCenterOffset;
        private Vector2 helmetCenterOffset;

        private float eyeRotation = 0;
        private Vector2 eyeRotationOrigin;

        private Random rand;

        private float shotTimer = 0.1f;

        /// <summary>
        /// The armor and eye for the BrainBoss
        /// </summary>
        /// <param name="id">The game id to assign to the new object</param>
        /// <param name="content">A ContentManager to load content from</param>
        /// <param name="position">A position on the screen</param>
        public BrainBossProtection(uint id, ContentManager content, Vector2 position)
            : base(id)
        {
            this.position = position;
            this.Health = 100;
            this.Score = 80;

            rand = new Random();

            eyeSpritesheet = content.Load<Texture2D>("Spritesheets/newsh9.shp.000000");
            spikesSpritesheet = content.Load<Texture2D>("Spritesheets/newsh9.shp.000000");
            helmetSpritesheet = content.Load<Texture2D>("Spritesheets/newsh9.shp.000000");

            eyeSpriteBounds = new Rectangle(0, 112, 72, 69);
            eyeCenterOffset = new Vector2(0, 15); //already centered with rotation origin
            eyeRotationOrigin = new Vector2(eyeSpriteBounds.Width / 2, eyeSpriteBounds.Height / 2);

            spikesSpriteBounds = new Rectangle(73, 116, 95, 52);
            spikesCenterOffset = new Vector2(spikesSpriteBounds.Width / 2, spikesSpriteBounds.Height / 2 - 25);

            helmetSpriteBounds = new Rectangle(96, 8, 71, 75);
            helmetCenterOffset = new Vector2(helmetSpriteBounds.Width / 2, helmetSpriteBounds.Height / 2 + 10);
        }

        /// <summary>
        /// Returns true if the protection is in the destroyed state.
        /// </summary>
        /// <returns></returns>
        public bool isDestroyed()
        {
            return partsLeft <= 0;
        }

        /// <summary>
        /// Updates the protection every frame
        /// </summary>
        /// <param name="elapsedTime">Time passed since last frame</param>
        public override void Update(float elapsedTime)
        {
            if (partsLeft <= 0)
            {
                Health = 0;
                return;
            }

            if (Health <= 0)
            {
                partsLeft--;
                Health = 100;
                ScrollingShooterGame.GameObjectManager.CreateExplosion(ID);
            }

            shotTimer -= elapsedTime;

            if (partsLeft == 3) // eye is functional
            {
                //fire toward player
                Vector2 playerCenter = ScrollingShooterGame.Game.Player.Position;
                Vector2 eyeCenterPosition = position - eyeCenterOffset;

                eyeRotation = (float)(Math.Atan2(playerCenter.Y - eyeCenterPosition.Y, playerCenter.X - eyeCenterPosition.X) - Math.PI / 2f);

                if (shotTimer <= 0)
                {
                    shotTimer = 0.4f;
                    EnemyLightningZap zap = (EnemyLightningZap)ScrollingShooterGame.GameObjectManager.CreateProjectile(ProjectileType.EnemyLightningZap, position - eyeCenterOffset);
                    zap.Initialize(eyeRotation + (float)(Math.PI / 2f), 500);
                }
            }
            else // random fire
            {
                if (shotTimer <= 0)
                {
                    shotTimer = 0.4f;

                    eyeRotation = (float) (rand.NextDouble() * Math.PI * 2);

                    EnemyLightningZap zap = (EnemyLightningZap)ScrollingShooterGame.GameObjectManager.CreateProjectile(ProjectileType.EnemyLightningZap, position - eyeCenterOffset);
                    zap.Initialize(eyeRotation + (float)(Math.PI / 2f), 500);

                    zap = (EnemyLightningZap)ScrollingShooterGame.GameObjectManager.CreateProjectile(ProjectileType.EnemyLightningZap, position - eyeCenterOffset);
                    zap.Initialize(eyeRotation + (float)(Math.PI / 2f - Math.PI / 8f), 500);

                    zap = (EnemyLightningZap)ScrollingShooterGame.GameObjectManager.CreateProjectile(ProjectileType.EnemyLightningZap, position - eyeCenterOffset);
                    zap.Initialize(eyeRotation + (float)(Math.PI / 2f + Math.PI / 8f), 500);
                }
            }
        }

        /// <summary>
        /// Returns the bounds of the armor
        /// </summary>
        public override Rectangle Bounds
        {
            get { return new Rectangle((int)position.X, (int)position.Y, eyeSpriteBounds.Width, eyeSpriteBounds.Height); }
        }

        /// <summary>
        /// Draws the parts of the protection that are alive
        /// </summary>
        /// <param name="elapsedTime">The elapsed time between the previous and current frame</param>
        /// <param name="spriteBatch">An already-initialized SpriteBatch, ready for Draw() commands</param>
        public override void Draw(float elapsedTime, Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch)
        {
            if(partsLeft > 1)
                spriteBatch.Draw(spikesSpritesheet, position - spikesCenterOffset, spikesSpriteBounds, Color.White);

            spriteBatch.Draw(helmetSpritesheet, position - helmetCenterOffset, helmetSpriteBounds, Color.White, 0, Vector2.Zero, 1,  SpriteEffects.FlipVertically, 0);

            if(partsLeft > 2)
                spriteBatch.Draw(eyeSpritesheet, position - eyeCenterOffset, eyeSpriteBounds, Color.White, eyeRotation, eyeRotationOrigin, 1, SpriteEffects.None, 0);
        }
    }
}
