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
        /// Destination of brain in x dimension
        /// </summary>
        private int targetPositionX;

        /// <summary>
        /// Destination of brain in y dimension
        /// </summary>
        private int targetPositionY;

        /// <summary>
        /// Time until next lightning burst in DeathCharge state
        /// </summary>
        private float lightningRecharge = 0f;

        /// <summary>
        /// State of the brain boss
        /// </summary>
        private BrainBossState state = BrainBossState.Protected;

        /// <summary>
        /// Brain's armor that needs to be destroyed before fighting the brain
        /// </summary>
        private BrainBossProtection protection;

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

            this.Health = 100;

            this.position = position;

            centerOffset = new Vector2(brainSpriteBounds.Width / 2, brainSpriteBounds.Height / 2);

            psiEmitter = ScrollingShooterGame.GameObjectManager.CreateEnemy(EnemyType.BrainBossPsyEmitter, position + centerOffset) as BrainBossPsiEmitter;

            protection = ScrollingShooterGame.GameObjectManager.CreateEnemy(EnemyType.BrainBossProtection, position + centerOffset) as BrainBossProtection;

            targetPositionX = (int)position.X;
            targetPositionY = (int)(position.Y + ScrollingShooterGame.Game.GraphicsDevice.Viewport.Height / 5 - brainSpriteBounds.Height / 2);
        }

        /// <summary>
        /// Updates the brain boss
        /// </summary>
        /// <param name="elapsedTime">Time passed since last frame</param>
        public override void Update(float elapsedTime)
        {
            //TODO: remove damage over time
            this.Health -= 30 * elapsedTime;
            if (this.protection != null)
                protection.Health -= 30 * elapsedTime;
            if (this.psiEmitter != null)
                psiEmitter.Health -= 30 * elapsedTime;

            switch (state)
            {
                case BrainBossState.Protected:
                    //can't die in this state
                    Health = 999999;

                    if (protection.isDestroyed())
                    {
                        ScrollingShooterGame.GameObjectManager.DestroyObject(protection.ID);
                        state = BrainBossState.MovingToCenter;
                        protection = null;
                    }
                    break;
                case BrainBossState.MovingToCenter:
                    //can't die in this state
                    Health = 999999;

                    if (position.X < targetPositionX)
                        position.X += Math.Min(MOVE_SPEED * elapsedTime, Math.Abs(targetPositionX - position.X));
                    else if (position.X > targetPositionY)
                        position.X -= Math.Min(MOVE_SPEED * elapsedTime, Math.Abs(targetPositionX - position.X));

                    if (position.Y < targetPositionY)
                        position.Y += Math.Min(MOVE_SPEED * elapsedTime, Math.Abs(targetPositionY - position.Y));
                    else if (position.Y > targetPositionY)
                        position.Y -= Math.Min(MOVE_SPEED * elapsedTime, Math.Abs(targetPositionY - position.Y));

                    if (Math.Abs(targetPositionX - position.X) < 2 && Math.Abs(targetPositionY - position.Y) < 2)
                    {
                        state = BrainBossState.PsyAttack;
                        psiEmitter.startAttacking();
                    }

                    break;

                case BrainBossState.PsyAttack:
                    //can't die in this state
                    Health = 999999;

                    if (psiEmitter.isDestroyed())
                    {
                        //Health when can be hit
                        Health = 100;
                        state = BrainBossState.DeathCharge;
                    }
                    break;

                case BrainBossState.DeathCharge:

                    if (Health <= 0)
                    {
                        ScrollingShooterGame.GameObjectManager.DestroyObject(this.ID);
                        ScrollingShooterGame.GameObjectManager.CreateExplosion(ID);
                        return;
                    }

                    Vector2 vector = ScrollingShooterGame.Game.Player.GetPosition() - (position + centerOffset);
                    vector.Normalize();
                    vector *= MOVE_SPEED * elapsedTime;

                    position.X += vector.X;
                    position.Y += vector.Y;

                    lightningRecharge -= elapsedTime;
                    if (lightningRecharge <= 0)
                    {
                        lightningRecharge = .66f;

                        for (int i = 0; i < 15; i++)
                            ((EnemyLightningZap)ScrollingShooterGame.GameObjectManager.CreateProjectile(ProjectileType.EnemyLightningZap, this.position + centerOffset)).Initialize((float)(rand.NextDouble() * Math.PI * 2), this.brainSpriteBounds.Width / 2);
                    }
                    break;
            }

            psiEmitter.updatePosition(this.position + centerOffset);
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
