using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using System;
using System.Collections.Generic;

//Author: Josh Zavala
namespace ScrollingShooter
{
    /// <summary>
    /// Represents the 8 positions the tank can face
    /// </summary>
    enum Panzer2AimState
    {
        North = 0,
        NorthEast,
        East,
        SouthEast,
        South,
        SouthWest,
        West,
        NorthWest,
    }

    /// <summary>
    /// A ground tank that shoot at the player if they are in range,
    /// moves slowly towards them.
    /// </summary>
    public class Panzer2 : Enemy
    {
        //the vars
        Rectangle[] spriteBounds = new Rectangle[8];
        Panzer2AimState aimState;
        float defaultGunTimer = 0;
        #region Sound Effects
        SoundEffect photonFired;
        #endregion

        /// <summary>
        /// The bounding rectangle of the Panzer
        /// </summary>
        public override Rectangle Bounds
        {
            get
            {
                return new Rectangle((int)position.X, (int)position.Y,
                    spriteBounds[(int)aimState].Width, spriteBounds[(int)aimState].Height);
            }
        }

        /// <summary>
        /// Create a new instance of a Panzer enemy tank.
        /// </summary>
        /// <param name="id">Obj id</param>
        /// <param name="content">A ContentManager to load resources with</param>
        /// <param name="position">The position of the Dart ship in the game world</param>
        public Panzer2(uint id, ContentManager content, Vector2 position)
            : base(id)
        {
            this.position = position;
            this.Score = 16;
            spritesheet = content.Load<Texture2D>("Spritesheets/vulcano");
            photonFired = content.Load<SoundEffect>("SFX/fireworksmortar3");

            spriteBounds[(int)Panzer2AimState.North].X = 7;
            spriteBounds[(int)Panzer2AimState.North].Y = 30;
            spriteBounds[(int)Panzer2AimState.North].Width = 15;
            spriteBounds[(int)Panzer2AimState.North].Height = 20;

            spriteBounds[(int)Panzer2AimState.NorthEast].X = 101;
            spriteBounds[(int)Panzer2AimState.NorthEast].Y = 20;
            spriteBounds[(int)Panzer2AimState.NorthEast].Width = 17;
            spriteBounds[(int)Panzer2AimState.NorthEast].Height = 18;

            spriteBounds[(int)Panzer2AimState.East].X = 121;
            spriteBounds[(int)Panzer2AimState.East].Y = 36;
            spriteBounds[(int)Panzer2AimState.East].Width = 20;
            spriteBounds[(int)Panzer2AimState.East].Height = 14;

            spriteBounds[(int)Panzer2AimState.SouthEast].X = 101;
            spriteBounds[(int)Panzer2AimState.SouthEast].Y = 51;
            spriteBounds[(int)Panzer2AimState.SouthEast].Width = 18;
            spriteBounds[(int)Panzer2AimState.SouthEast].Height = 16;

            spriteBounds[(int)Panzer2AimState.South].X = 57;
            spriteBounds[(int)Panzer2AimState.South].Y = 32;
            spriteBounds[(int)Panzer2AimState.South].Width = 15;
            spriteBounds[(int)Panzer2AimState.South].Height = 20;

            spriteBounds[(int)Panzer2AimState.SouthWest].X = 76;
            spriteBounds[(int)Panzer2AimState.SouthWest].Y = 51;
            spriteBounds[(int)Panzer2AimState.SouthWest].Width = 18;
            spriteBounds[(int)Panzer2AimState.SouthWest].Height = 16;

            spriteBounds[(int)Panzer2AimState.West].X = 30;
            spriteBounds[(int)Panzer2AimState.West].Y = 35;
            spriteBounds[(int)Panzer2AimState.West].Width = 20;
            spriteBounds[(int)Panzer2AimState.West].Height = 14;

            spriteBounds[(int)Panzer2AimState.NorthWest].X = 76;
            spriteBounds[(int)Panzer2AimState.NorthWest].Y = 20;
            spriteBounds[(int)Panzer2AimState.NorthWest].Width = 18;
            spriteBounds[(int)Panzer2AimState.NorthWest].Height = 18;

            aimState = Panzer2AimState.South;

        }

        /// <summary>
        /// Updates the Panzer2 tank
        /// </summary>
        /// <param name="elapsedTime">In-game time between previous and current frame</param>
        public override void Update(float elapsedTime)
        {
            //Sense the player's position
            PlayerShip player = ScrollingShooterGame.Game.Player;
            Vector2 playerPosition = new Vector2(player.Bounds.Center.X,
                player.Bounds.Center.Y);

            //Get the vector from Panzer's position to the player's position
            Vector2 toPlayer = playerPosition - this.position;

            if (toPlayer.LengthSquared() < 200000)
            {
                //normalize the vector
                toPlayer.Normalize();
                //update the steering vector pointing to the player
                SteeringState(toPlayer);

                //fire cannon
                FireCannon(elapsedTime);
            }
        }

        /// <summary>
        /// Draws the Panzer on-screen
        /// </summary>
        /// <param name="elapsedTime">In-game time between previous and current frame</param>
        /// <param name="spriteBatch">SpriteBatch using Draw()</param>
        public override void Draw(float elapsedTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(spritesheet, Bounds, spriteBounds[(int)aimState],
                Color.White, 0f, new Vector2(Bounds.Width / 2, Bounds.Height / 2),
                SpriteEffects.None, 1f);
        }

        /// <summary>
        /// Changes direction the Panzer faces relative to the Player
        /// </summary>
        /// <param name="toPlayer">Unit Vector in the direction of the Player</param>
        public void SteeringState(Vector2 toPlayer)
        {
            toPlayer.Normalize();
            if ((toPlayer.X < -0.5f) && (toPlayer.Y > 0.5f)) aimState = Panzer2AimState.SouthWest;
            else if ((toPlayer.X < -0.5f) && (toPlayer.Y < -0.5f)) aimState = Panzer2AimState.NorthWest;
            else if ((toPlayer.X > 0.5f) && (toPlayer.Y < -0.5f)) aimState = Panzer2AimState.NorthEast;
            else if ((toPlayer.X > 0.5f) && (toPlayer.Y > 0.5f)) aimState = Panzer2AimState.SouthEast;
            else if (toPlayer.X < -0.5f) aimState = Panzer2AimState.West;
            else if (toPlayer.X > 0.5f) aimState = Panzer2AimState.East;
            else if (toPlayer.Y < -0.5f) aimState = Panzer2AimState.North;
            else aimState = Panzer2AimState.South;
        }

        /// <summary>
        /// Fires a bullet from the Panzer towards the player
        /// </summary>
        /// <param name="elapsedTime">In-game time between previous and current frame</param>
        public void FireCannon(float elapsedTime)
        {
            defaultGunTimer += elapsedTime;
            if (defaultGunTimer > 2f)
            {
                //Make use of the ToPlayerBullet class
                ScrollingShooterGame.GameObjectManager.CreateProjectile(ProjectileType.Photon, position);
                defaultGunTimer = 0;
                photonFired.Play();

                //ScrollingShooterGame.GameObjectManager.CreateExplosion(this.ID);
            }
        }

        public override void ScrollWithMap(float elapsedTime)
        {
            position.Y += ScrollingSpeed * elapsedTime;
        }
    }
}
