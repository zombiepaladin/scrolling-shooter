//Samuel Fike



using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Audio;

namespace ScrollingShooter
{

    /// <summary>
    /// The state of the psi emitter
    /// </summary>
    enum PsiEmitterState
    {
        INACTIVE,
        FIRING,
        RECOVERING,
        DESTROYED,
    }

    /// <summary>
    /// The psi emitter enemy that is attached to the brain boss.
    /// </summary>
    class BrainBossPsiEmitter : Enemy
    {
        /// <summary>
        /// Current rotation speed of the projectile direction
        /// </summary>
        private float rotationSpeed = 0;

        /// <summary>
        /// Target rotation speed of the projectile direction
        /// </summary>
        private float targetRotationSpeed = (float)Math.PI;

        /// <summary>
        /// Current delay between shots
        /// </summary>
        private float shotDelay = 0.03f;

        /// <summary>
        /// Target delay between shots
        /// </summary>
        private float targetShotDelay = 0.1f;

        /// <summary>
        /// Current projectile speed
        /// </summary>
        private double currentShotSpeed = 100;

        /// <summary>
        /// Target projectile speed
        /// </summary>
        private int targetShotSpeed = 300;

        /// <summary>
        /// Time until the projectile rotation switches direction
        /// </summary>
        private float timeUntilDirectionSwitch = 0;

        /// <summary>
        /// Time until done recovering
        /// </summary>
        private float recoverTime = 3f;

        /// <summary>
        /// Spritesheet origin of the emblem
        /// </summary>
        private Texture2D emblemSpritesheet;

        /// <summary>
        /// Location of the emblem sprite
        /// </summary>
        private Rectangle emblemSpriteBounds;

        /// <summary>
        /// Spritesheet origin of the psi orb
        /// </summary>
        private Texture2D psiOrbSpritesheet;

        /// <summary>
        /// Location of the psi orb sprite
        /// </summary>
        private Rectangle psiOrbSpriteBounds;

        /// <summary>
        /// Middle of rotation for the psi orb sprite
        /// </summary>
        private Vector2 psiOrbOrigin;

        /// <summary>
        /// Current rotation of the psi orb sprite
        /// </summary>
        private float psiOrbRotation;

        /// <summary>
        /// Current color of projectiles
        /// </summary>
        private Color projectileColor = new Color(0, 80, 160);

        /// <summary>
        /// Current color of the psi emitter
        /// </summary>
        private Color tintColor = Color.White;

        /// <summary>
        /// Random number generator
        /// </summary>
        private Random rand = new Random();

        /// <summary>
        /// State of the psi emitter
        /// </summary>
        private PsiEmitterState state = PsiEmitterState.INACTIVE;

        /// <summary>
        /// Number of projectile arms firing
        /// </summary>
        private int numArms = 1;

        /// <summary>
        /// Rotation of all the projectile arms
        /// </summary>
        private float armRotation = 0;

        /// <summary>
        /// Time until psi emitter can fire again
        /// </summary>
        private float timeUntilNextShot = 0;

        private SoundEffect firingSound;

        private float soundTimer = 0f;

        /// <summary>
        /// Creates a new psi emitter for the brain boss
        /// </summary>
        /// <param name="id">The game id to assign to the new object</param>
        /// <param name="content">A ContentManager to load content from</param>
        /// <param name="position">A position on the screen</param>
        public BrainBossPsiEmitter(uint id, ContentManager content, Vector2 position)
            : base(id)
        {
            this.emblemSpritesheet = content.Load<Texture2D>("Spritesheets/newshg.shp.000000");
            this.psiOrbSpritesheet = content.Load<Texture2D>("Spritesheets/newshg.shp.000000");

            this.psiOrbSpriteBounds = new Rectangle(98, 60, 21, 21);
            this.psiOrbOrigin = new Vector2(psiOrbSpriteBounds.Width / 2, psiOrbSpriteBounds.Height / 2);
            this.psiOrbRotation = 0f;
            this.Score = 80;

            this.emblemSpriteBounds = new Rectangle(122, 28, 21, 28);

            this.position = position;

            timeUntilDirectionSwitch = 15 + rand.Next(15);

            Health = 500;

            firingSound = content.Load<SoundEffect>("SFX/PsiSound");
        }

        /// <summary>
        /// Updates the psi emitter
        /// </summary>
        /// <param name="elapsedTime">Time passed since last frame</param>
        public override void Update(float elapsedTime)
        {

            if (state == PsiEmitterState.INACTIVE || state == PsiEmitterState.DESTROYED)
                return;

            if (state != PsiEmitterState.FIRING) //can't die unless firing
                Health = 99999;

            this.psiOrbRotation += (float)Math.PI * elapsedTime;

            if (timeUntilDirectionSwitch <= 0)
            {
                targetRotationSpeed *= -1;
                timeUntilDirectionSwitch = 15 + rand.Next(15);
            }
            else
            {
                timeUntilDirectionSwitch -= elapsedTime;
            }

            switch (state)
            {
                case PsiEmitterState.INACTIVE:
                    break;

                case PsiEmitterState.FIRING:
                    timeUntilNextShot -= elapsedTime;

                    soundTimer += elapsedTime;
                    if (soundTimer > 0.25f)
                    {
                        firingSound.Play(.5f, 0f, 0f);
                        soundTimer = 0;
                    }

                    if (Health <= 0)
                    {
                        recoverTime = 3f;
                        state = PsiEmitterState.RECOVERING;
                        Health = 500;
                        ScrollingShooterGame.GameObjectManager.CreateExplosion(ID);
                        return;
                    }

                    if (currentShotSpeed < targetShotSpeed)
                        currentShotSpeed += 25 * elapsedTime;
                    else
                        currentShotSpeed = targetShotSpeed;

                    if (shotDelay > targetShotDelay)
                        shotDelay -= 0.015f * elapsedTime;
                    else
                        shotDelay = targetShotDelay;

                    if (rotationSpeed < targetRotationSpeed)
                        rotationSpeed += (float)(Math.PI / 10 * elapsedTime);
                    else
                        rotationSpeed -= (float)(Math.PI/10 * elapsedTime);
                    
                    armRotation += rotationSpeed * elapsedTime;

                    if (timeUntilNextShot > 0)
                        return;
                    else
                        timeUntilNextShot = shotDelay;

                    float armAngle;
                    EnemyPsiBall projectile;

                    for (int i = 0; i < numArms; i++)
                    {
                        armAngle = (float)((Math.PI * 2) * (i / (float) (numArms))) + armRotation;

                        projectile = ScrollingShooterGame.GameObjectManager.CreateProjectile(ProjectileType.EnemyPsyBall, position) as EnemyPsiBall;
                        projectile.Initialize(armAngle, (int) currentShotSpeed);
                        ApplyProjectileColor(projectile);
                    }
                    break;

                case PsiEmitterState.RECOVERING:
                    recoverTime -= elapsedTime;

                    tintColor.R = (byte) rand.Next(255);
                    tintColor.G = (byte) rand.Next(255);
                    tintColor.B = (byte) rand.Next(255);

                    if (recoverTime > 0)
                        return;

                    numArms++;

                    if (numArms > 5)
                    {
                        numArms = 0;
                        Health = 0;
                        state = PsiEmitterState.DESTROYED;
                    }
                    else
                    {

                        rotationSpeed = 0;

                        updateShotValues();

                        tintColor = Color.White;

                        Health = 500;

                        state = PsiEmitterState.FIRING;
                    }
                    break;
            }
        }

        public bool isDestroyed()
        {
            return state == PsiEmitterState.DESTROYED;
        }

        /// <summary>
        /// Sets the projectile to the appropriate color based on the firing stage of the psi emitter
        /// </summary>
        /// <param name="elapsedTime">Time passed since last frame</param>
        private void ApplyProjectileColor(EnemyPsiBall projectile)
        {
            switch (numArms)
            {
                case 1:
                    projectileColor = Color.DarkViolet;
                    break;
                case 2:
                    projectileColor.R = (byte)(projectileColor.R - 6 % 255);
                    projectileColor.G = projectileColor.B = 0;
                    projectile.SetColor(projectileColor);
                    break;
                case 3:
                    projectileColor.R = (byte)((projectileColor.R + 6));
                    projectileColor.G = (byte)((projectileColor.R + 120));
                    projectileColor.B = (byte)((projectileColor.R + 200));
                    projectile.SetColor(projectileColor);
                    break;
                case 4:
                    //start random color
                    projectileColor.R = (byte)(rand.Next(255));
                    projectileColor.G = (byte)(rand.Next(255));
                    projectileColor.B = (byte)(rand.Next(255));
                    projectile.SetColor(projectileColor);
                    break;
                case 5:
                    //strobe
                    projectile.SetRandom(true);
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Sets shot speed, delay, and rotation speed according to the firing state of the psi emitter
        /// </summary>
        private void updateShotValues()
        {
            currentShotSpeed = 75;
            
            rotationSpeed = 0;

            switch (numArms)
            {
                case 1:
                    targetShotDelay = 0.1f;
                    targetShotSpeed = 150;
                    rotationSpeed = targetRotationSpeed = (float)Math.PI / 5f;
                    break;
                case 2:
                    targetShotDelay = 0.08f;
                    targetShotSpeed = 200;
                    targetRotationSpeed = (float)Math.PI / 4.5f;
                    break;
                case 3:
                    targetShotDelay = 0.06f;
                    targetShotSpeed = 250;
                    targetRotationSpeed = (float)Math.PI / 4f;
                    break;
                case 4:
                    targetShotDelay = 0.04f;
                    targetShotSpeed = 300;
                    targetRotationSpeed = (float)Math.PI / 3.75f;
                    break;
                case 5:
                    targetShotDelay = 0.02f;
                    targetShotSpeed = 350;
                    targetRotationSpeed = (float)Math.PI / 3.75f;
                    break;
            }

            shotDelay = targetShotDelay * 2;

            double random = rand.NextDouble();
            if (random < 0.5)
                targetRotationSpeed *= -1;
        }

        /// <summary>
        /// Sets the psi emitter's position
        /// </summary>
        /// <param name="newPosition">The new position of the psi emitter</param>
        public void updatePosition(Vector2 newPosition)
        {
            this.position = newPosition;
        }

        /// <summary>
        /// Awakens the psi emitter so it will start fighting
        /// </summary>
        public void startAttacking()
        {
            this.state = PsiEmitterState.FIRING;
            Health = 500;
            updateShotValues();
        }

        public override Rectangle Bounds
        {
            get { return new Rectangle((int)position.X - psiOrbSpriteBounds.Width / 2, (int)position.Y - psiOrbSpriteBounds.Height / 2, psiOrbSpriteBounds.Width, psiOrbSpriteBounds.Height); }
        }

        /// <summary>
        /// Does nothing because the draw is called from the brain boss in order to have the psi emitter render on top
        /// </summary>
        /// <param name="elapsedTime">The elapsed time between the previous and current frame</param>
        /// <param name="spriteBatch">An already-initialized SpriteBatch, ready for Draw() commands</param>
        public override void Draw(float elapsedTime, SpriteBatch spriteBatch)
        {
            //Do nothing and wait for Draw2
        }

        /// <summary>
        /// Draws the psi emitter on-screen
        /// </summary>
        /// <param name="elapsedTime">The elapsed time between the previous and current frame</param>
        /// <param name="spriteBatch">An already-initialized SpriteBatch, ready for Draw() commands</param>
        public void Draw2(float elapsedTime, SpriteBatch spriteBatch)
        {
            if (state == PsiEmitterState.DESTROYED)
                return;

            spriteBatch.Draw(psiOrbSpritesheet, position, psiOrbSpriteBounds, tintColor, psiOrbRotation, psiOrbOrigin, 1.5f, SpriteEffects.None, 1);
            spriteBatch.Draw(emblemSpritesheet, position, emblemSpriteBounds, tintColor, 0, psiOrbOrigin, 1, SpriteEffects.None, 1);
        }
    }
}
