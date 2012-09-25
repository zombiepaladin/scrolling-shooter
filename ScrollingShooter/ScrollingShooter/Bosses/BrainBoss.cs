using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ScrollingShooter
{
    /// <summary>
    /// The state of the brain boss
    /// </summary>
    enum BrainBossState
    {
        Protected,
        MovingToCenter,
        PsyAttack,
        DeathCharge,
    }

    /// <summary>
    /// The final boss in the game
    /// Starts out protected by organs that you need to destroy (not implemented yet)
    /// Then, it float out to the center of the screen and fires at you
    /// After destroying the emblem on its head multiple times, it will go crazy and fire short range lightning while chasing you
    /// While chasing, it will take damage over time and from the player, and finally die
    /// </summary>
    class BrainBoss : Enemy
    {
        /// <summary>
        /// Move speed of the brain boss
        /// </summary>
        private const int MOVE_SPEED = 50;

        /// <summary>
        /// Position of the brain boss
        /// </summary>
        private Vector2 position;

        /// <summary>
        /// Spritesheet origin of the brain sprite
        /// </summary>
        private Texture2D brainSpritesheet;

        /// <summary>
        /// Spritesheet origin of the psi orb sprite
        /// </summary>
        private Texture2D psiOrbSpritesheet;

        /// <summary>
        /// Location of the brain sprite
        /// </summary>
        private Rectangle brainSpriteBounds;

        /// <summary>
        /// Location of the psi orb sprite
        /// </summary>
        private Rectangle psiOrbBounds;

        /// <summary>
        /// Offset of the psi orb location from the top left of the brain sprite
        /// </summary>
        private Vector2 psiOrbOffset;

        /// <summary>
        /// Offset for the middle of the psi orb for rotation
        /// </summary>
        private Vector2 psiOrbOrigin;

        /// <summary>
        /// Tint color of the brain
        /// </summary>
        private Color tintColor = Color.White;

        /// <summary>
        /// Random number generator
        /// </summary>
        private Random rand = new Random();

        /// <summary>
        /// The psi emitter enemy object attached to the brain
        /// </summary>
        private BrainBossPsiEmitter psiEmitter;

        /// <summary>
        /// Offset of the psi emitter to center it on the brain
        /// </summary>
        private Vector2 centerOffset;

        /// <summary>
        /// Center of the game screen in x dimension
        /// </summary>
        private int screenCenterX;

        /// <summary>
        /// Center of the game screen in y dimension
        /// </summary>
        private int screenCenterY;

        /// <summary>
        /// State of the brain boss
        /// </summary>
        private BrainBossState state = BrainBossState.Protected;

        /// <summary>
        /// Brain's armor that needs to be destroyed before fighting the brain
        /// </summary>
        private Enemy protection;

        /// <summary>
        /// Creates a new brain boss
        /// </summary>
        /// <param name="id">The game id to assign to the new object</param>
        /// <param name="content">A ContentManager to load content from</param>
        /// <param name="position">A position on the screen</param>
        public BrainBoss(uint id, ContentManager content, Vector2 position)
            : base(id)
        {
            this.brainSpritesheet = content.Load<Texture2D>("Spritesheets/newsh5.shp.000000");
            this.psiOrbSpritesheet = content.Load<Texture2D>("Spritesheets/tyrian.shp.000000");

            this.brainSpriteBounds = new Rectangle(60, 143, 73, 80);
            this.psiOrbBounds = new Rectangle(121, 172, 21, 21);
            this.psiOrbOrigin = new Vector2(psiOrbBounds.Width / 2, psiOrbBounds.Height / 2);

            psiOrbOffset.X = brainSpriteBounds.Width / 2;
            psiOrbOffset.Y = brainSpriteBounds.Height / 2;

            this.position = position;

            centerOffset = new Vector2(brainSpriteBounds.Width / 2, brainSpriteBounds.Height / 2);

            psiEmitter = ScrollingShooterGame.GameObjectManager.CreateEnemy(EnemyType.BrainBossPsyEmitter, position + centerOffset) as BrainBossPsiEmitter;

            protection = ScrollingShooterGame.GameObjectManager.CreateEnemy(EnemyType.BrainBossProtection, position + centerOffset);
            //TODO: add protection organs for initial stage

             screenCenterX = ScrollingShooterGame.Game.GraphicsDevice.Viewport.Width / 2 - brainSpriteBounds.Width / 2;
             screenCenterY = ScrollingShooterGame.Game.GraphicsDevice.Viewport.Height / 2 - brainSpriteBounds.Height / 2;
        }

        /// <summary>
        /// Updates the brain boss
        /// </summary>
        /// <param name="elapsedTime">Time passed since last frame</param>
        public override void Update(float elapsedTime)
        {
            switch (state)
            {
                case BrainBossState.Protected:
                    if (protection.Health <= 0)
                        state = BrainBossState.MovingToCenter;
                    break;
                case BrainBossState.MovingToCenter:
                    if (position.X < screenCenterX)
                        position.X += Math.Min(MOVE_SPEED * elapsedTime, Math.Abs(screenCenterX - position.X));
                    else if (position.X > screenCenterY)
                        position.X -= Math.Min(MOVE_SPEED * elapsedTime, Math.Abs(screenCenterX - position.X));

                    if (position.Y < screenCenterY)
                        position.Y += Math.Min(MOVE_SPEED * elapsedTime, Math.Abs(screenCenterY - position.Y));
                    else if (position.Y > screenCenterY)
                        position.Y -= Math.Min(MOVE_SPEED * elapsedTime, Math.Abs(screenCenterY - position.Y));

                    if (Math.Abs(screenCenterX - position.X) < 2 && Math.Abs(screenCenterY - position.Y) < 2)
                    {
                        state = BrainBossState.PsyAttack;
                        psiEmitter.startAttacking();
                    }

                    break;

                case BrainBossState.PsyAttack:
                    if (psiEmitter.isDestroyed())
                        state = BrainBossState.DeathCharge;
                    break;
                case BrainBossState.DeathCharge:
                    checkForCollisions();
                    
                    Vector2 vector = ScrollingShooterGame.Game.Player.GetPosition() - (position + centerOffset);
                    vector.Normalize();
                    vector *= MOVE_SPEED * elapsedTime;

                    position.X += vector.X;
                    position.Y += vector.Y;

                    for(int i = 0; i < 2; i++)
                        ((EnemyLightningZap) ScrollingShooterGame.GameObjectManager.CreateProjectile(ProjectileType.EnemyLightningZap, this.position + centerOffset)).Initialize((float) (rand.NextDouble() * Math.PI * 2), this.brainSpriteBounds.Width);

                    break;
            }

            psiEmitter.updatePosition(this.position + centerOffset);
        }

        /// <summary>
        /// Handles collisions
        /// </summary>
        private void checkForCollisions()
        {
            foreach (CollisionPair pair in ScrollingShooterGame.GameObjectManager.Collisions)
            {
                if (pair.A == this.ID || pair.B == this.ID)
                {
                    uint colliderID = (pair.A == this.ID) ? pair.B : pair.A;
                    GameObject collider = ScrollingShooterGame.GameObjectManager.GetObject(colliderID);

                    Projectile projectile = collider as Projectile;
                    if (projectile != null && projectile as EnemyPsiBall == null && projectile as EnemyLightningZap == null)
                    {
                        //TODO: implement health
                        ScrollingShooterGame.GameObjectManager.DestroyObject(this.ID);
                    }
                }
            }
        }

        /// <summary>
        /// Returns the bounds of the brain sprite
        /// </summary>
        public override Rectangle Bounds
        {
            get { return new Rectangle((int)position.X, (int)position.Y, brainSpriteBounds.Width, brainSpriteBounds.Height); }
        }

        /// <summary>
        /// Draws the brain boss on-screen
        /// </summary>
        /// <param name="elapsedTime">The elapsed time between the previous and current frame</param>
        /// <param name="spriteBatch">An already-initialized SpriteBatch, ready for Draw() commands</param>
        public override void Draw(float elapsedTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(brainSpritesheet, position, brainSpriteBounds, tintColor);
            spriteBatch.Draw(psiOrbSpritesheet, position + psiOrbOffset, psiOrbBounds, tintColor, (float) (rand.NextDouble() * Math.PI * 2), psiOrbOrigin, 1, SpriteEffects.None, 1);

            psiEmitter.Draw2(elapsedTime, spriteBatch);
        }
    }
}
