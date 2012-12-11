using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using System;
using System.Collections.Generic;

//Authors: Adam Clark
//         Josh Zavala
//         Nick Stanley
namespace ScrollingShooter
{
    /// <summary>
    /// Start of 2-phase boss fight for the vulcano level.
    /// </summary>
    class Lavabug : Boss
    {
        //the vars
        Texture2D spritesheet;
        Vector2 position;
        Rectangle[] spriteBounds = new Rectangle[1];
        float defaultGunTimer = 0; //maybe more
        private Mandible _mandible1;
        private Mandible _mandible2;

        #region Sound Effects
        SoundEffect laserFired;
        #endregion

        /// <summary>
        /// The bounding rectangle of the Lavabug
        /// </summary>
        public override Rectangle Bounds
        {
            get
            {
                return new Rectangle((int)position.X, (int)position.Y,
                    spriteBounds[0].Width, spriteBounds[0].Height);
            }
        }

        /// <summary>
        /// Creates a new instance of the Lavabug
        /// </summary>
        /// <param name="id">the id tag</param>
        /// <param name="content">>A ContentManager to load resources with</param>
        /// <param name="position">The position of the Lavabug in the game world</param>
        public Lavabug(uint id, ContentManager content, Vector2 position)
            : base(id)
        {
            this.position = position;
            //spritesheet
            spritesheet = content.Load<Texture2D>("Spritesheets/accessories");
            this.Health = 100;
            spriteBounds[0].X = 4;
            spriteBounds[0].Y = 20;
            spriteBounds[0].Width = 70;
            spriteBounds[0].Height = 132;
            laserFired = content.Load<SoundEffect>("SFX/gamalaser");
            this.Score = 150;

            _mandible1 = (Mandible)ScrollingShooterGame.GameObjectManager.CreateEnemy(EnemyType.Mandible,
                new Vector2(this.position.X - 11, this.position.Y));
            _mandible2 = (Mandible)ScrollingShooterGame.GameObjectManager.CreateEnemy(EnemyType.Mandible,
                new Vector2(this.position.X + 11, this.position.Y));
        }

        /// <summary>
        /// Updates the Lavabug
        /// </summary>
        /// <param name="elapsedTime">The in-game time between the previous and current frame</param>
        public override void Update(float elapsedTime)
        {
            // Pause the scrolling
            if (-ScrollingShooterGame.LevelManager.scrollDistance / 2 <= position.Y - 50) ScrollingShooterGame.LevelManager.Scrolling = false;

            // Sense the player's position
            PlayerShip player = ScrollingShooterGame.Game.Player;
            Vector2 playerPosition = new Vector2(player.Bounds.Center.X, player.Bounds.Center.Y);

            if (this.Health <= 50)
            {
                _mandible1.isFired = true;
                _mandible2.isFired = true;
            }
            if (this.Health <= 0)
            {
                ScrollingShooterGame.GameObjectManager.CreateEnemy(EnemyType.Lavabug2,
                    new Vector2(this.position.X, this.position.Y));
                ScrollingShooterGame.GameObjectManager.CreateEnemy(EnemyType.Lavabug2,
                    new Vector2(this.position.X + 10, this.position.Y + 60));
                ScrollingShooterGame.GameObjectManager.DestroyObject(this.ID);
            }

            // Move in front of player
            //this.position.Y = playerPosition.Y;
            this.position.X = playerPosition.X;

            //fire at player
            Fire(elapsedTime);
        }

        /// <summary>
        /// Draw the Lavabug on-screen
        /// </summary>
        /// <param name="elapsedTime">The in-game time between the previous and current frame</param>
        /// <param name="spriteBatch">An already initialized SpriteBatch, ready for Draw() commands</param>
        public override void Draw(float elapsedTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(spritesheet, Bounds, spriteBounds[0], Color.White, 0f, new Vector2(Bounds.Width / 2, Bounds.Height / 2), SpriteEffects.None, 1f);
        }

        public void Fire(float elapsedTime)
        {
            defaultGunTimer += elapsedTime;
            if (defaultGunTimer > .5f)
            {
                //Make use of the ToPlayerBullet class
                ScrollingShooterGame.GameObjectManager.CreateProjectile(ProjectileType.Photon, position);
                defaultGunTimer = 0;
                laserFired.Play();
            }
        }

        public override void ScrollWithMap(float elapsedTime)
        {
            position.Y += ScrollingSpeed * elapsedTime;
        }


    }
}
