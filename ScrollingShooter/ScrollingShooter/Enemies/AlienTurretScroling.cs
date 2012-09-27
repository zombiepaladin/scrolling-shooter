﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using System.Collections.Generic;
using System;

namespace ScrollingShooter
{
    /// <summary>
    /// Represents the three animation frames for the alien turret
    /// </summary>

    /// <summary>
    /// A turret that looks alien
    /// </summary>
    public class AlienTurretScrolling : Enemy
    {
        // Alien Turret drawing variables
        Texture2D spritesheet;
        Vector2 position;
        Rectangle[] spriteBounds = new Rectangle[3];

        const int MAX_HEALTH = 500;
        int frame;
        List<AlienTurretAnimation> animationSequence;

        float angle;

        Vector2 angleVector;

        float timer;

        AlienTurretPhase phase;

        /// <summary>
        /// The bounding rectangle of the alien turret
        /// </summary>
        public override Rectangle Bounds
        {
            get { return new Rectangle((int)position.X, (int)position.Y, spriteBounds[(int)animationSequence[frame]].Width, spriteBounds[(int)animationSequence[frame]].Height); }
        }

        /// <summary>
        /// Creates a new instance of the Alien Turret
        /// </summary>
        /// <param name="content">A ContentManager to load resources with</param>
        /// <param name="position">The position of the Alien Turret in the game world</param>
        public AlienTurretScrolling(uint id, ContentManager content, Vector2 position)
            : base(id)
        {
            this.position = position;
            phase = AlienTurretPhase.Wait;
            Health = MAX_HEALTH;

            spritesheet = content.Load<Texture2D>("Spritesheets/newshh.shp.000000");

            spriteBounds[(int)AlienTurretAnimation.Open].X = 48;
            spriteBounds[(int)AlienTurretAnimation.Open].Y = 0;
            spriteBounds[(int)AlienTurretAnimation.Open].Width = 48;
            spriteBounds[(int)AlienTurretAnimation.Open].Height = 56;

            spriteBounds[(int)AlienTurretAnimation.Mid].X = 96;
            spriteBounds[(int)AlienTurretAnimation.Mid].Y = 0;
            spriteBounds[(int)AlienTurretAnimation.Mid].Width = 48;
            spriteBounds[(int)AlienTurretAnimation.Mid].Height = 56;

            spriteBounds[(int)AlienTurretAnimation.Closed].X = 144;
            spriteBounds[(int)AlienTurretAnimation.Closed].Y = 0;
            spriteBounds[(int)AlienTurretAnimation.Closed].Width = 48;
            spriteBounds[(int)AlienTurretAnimation.Closed].Height = 56;

            frame = 0;
            animationSequence = new List<AlienTurretAnimation>();
            animationSequence.Add(AlienTurretAnimation.Open);
            animationSequence.Add(AlienTurretAnimation.Mid);
            animationSequence.Add(AlienTurretAnimation.Closed);
        }

        /// <summary>
        /// Updates the Pinchers
        /// </summary>
        /// <param name="elapsedTime">The in-game time between the previous and current frame</param>
        public override void Update(float elapsedTime)
        {
            PlayerShip player = ScrollingShooterGame.Game.Player;
            Vector2 playerPosition = new Vector2(player.Bounds.Center.X, player.Bounds.Center.Y);
            Vector2 toPlayer = playerPosition - this.position;

            Vector2 scrollVector =  new Vector2(0, 1);
            scrollVector.Normalize();
            this.position += scrollVector*ScrollingSpeed*elapsedTime;
        
            //If the player is out of range, dont fire!
            if (toPlayer.LengthSquared() > 100000)
                return;

            timer += elapsedTime;
            if (Health <= 0)
                phase = AlienTurretPhase.Dead;

            switch (phase)
            {
                case AlienTurretPhase.Wait:
                    if (timer >= 1)
                    {
                        timer = 0;
                        phase = AlienTurretPhase.Opening;
                    }
                    break;
                
                case AlienTurretPhase.Opening:
                    if (timer >= .2)
                    {
                        timer = 0;
                        if (frame <= 1)
                            frame++;
                        else
                            phase = AlienTurretPhase.Firing;
                    }
                    break;

                case AlienTurretPhase.Firing:
                    if (timer >= .2)
                    {
                        timer = 0;
                       ScrollingShooterGame.GameObjectManager.CreateProjectile(ProjectileType.AlienTurretOrbScrolling, position);
                        phase = AlienTurretPhase.Closing;
                    }
                    break;

                case AlienTurretPhase.Closing:
                    if (timer >= .2)
                    {
                        timer = 0;
                        if (frame >= 1)
                            frame--;
                        else
                        {                          
                            phase = AlienTurretPhase.Wait;
                        }
                    }
                    break;
            }

        }

        /// <summary>
        /// Draw the Alien Turret on-screen
        /// </summary>
        /// <param name="elapsedTime">The in-game time between the previous and current frame</param>
        /// <param name="spriteBatch">An already initialized SpriteBatch, ready for Draw() commands</param>
        public override void Draw(float elapsedTime, SpriteBatch spriteBatch)
        {
            if(phase != AlienTurretPhase.Dead)
                spriteBatch.Draw(spritesheet, Bounds, spriteBounds[(int)animationSequence[frame]], Color.White, angle, new Vector2(Bounds.Width / 2, Bounds.Height / 2), SpriteEffects.None, 1f);
        }

        public void ReviveTurret()
        {
            Health = MAX_HEALTH;
            phase = AlienTurretPhase.Wait;
        }

        public bool isDead()
        {
            if (Health <= 0)
                return true;
            else
                return false;
        }

    }
}