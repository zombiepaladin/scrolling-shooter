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
    /// Phase 2 of the vulcano boss. Moves specified distance apart, fires at
    /// the player.
    /// </summary>
    class Lavabug2 : Enemy
    {
        //the vars
        Texture2D spritesheet;
        Vector2 position;
        Rectangle[] spriteBounds = new Rectangle[1];
        float defaultGunTimer = 0; //maybe more
        bool direction;

        #region Sound Effects
        SoundEffect photonFired;
        #endregion

        /// <summary>
        /// The bounding rectangle of the LavaBug2
        /// </summary>
        public override Rectangle Bounds
        {
            get { return new Rectangle((int)position.X, (int)position.Y, 
                spriteBounds[0].Width, spriteBounds[0].Height); }
        }

        /// <summary>
        /// Creates a new instance of the Lavabug2
        /// </summary>
        /// <param name="id">the id tag</param>
        /// <param name="content">>A ContentManager to load resources with</param>
        /// <param name="position">The position of the Lavabug2 in the game world</param>
        public Lavabug2(uint id, ContentManager content, Vector2 position)
            : base(id)
        {
            this.position = position;
            this.Health = 100; //TODO: change this to be higher?
            this.Score = 100;
            //spritesheet
            spritesheet = content.Load<Texture2D>("Spritesheets/accessories");

            spriteBounds[0].X = 93;
            spriteBounds[0].Y = 38;
            spriteBounds[0].Width = 62;
            spriteBounds[0].Height = 88;
            photonFired = content.Load<SoundEffect>("SFX/gamalaser");


            //spritebounds
        }

        /// <summary>
        /// Updates the Lavabug2
        /// </summary>
        /// <param name="elapsedTime">The in-game time between the previous and current frame</param>
        public override void Update(float elapsedTime)
        {
            // Sense the player's position
            PlayerShip player = ScrollingShooterGame.Game.Player;
            Vector2 playerPosition = new Vector2(player.Bounds.Center.X, player.Bounds.Center.Y);

            if (this.Health <= 0)
                ScrollingShooterGame.GameObjectManager.DestroyObject(this.ID);

            // Move and fire when in front of player
            if (direction)
            {
                this.position.X -= elapsedTime * 200;

            }
            else this.position.X += elapsedTime * 200;
            if (this.position.X > 400) direction = !direction;
            if (this.position.X < 0) direction = !direction;

            //fire at player
            Fire(elapsedTime);
        }

        /// <summary>
        /// Draw the Lavabug2 on-screen
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
                photonFired.Play();
            }
        }
    }
}
