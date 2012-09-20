﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace ScrollingShooter
{
    /// <summary>
    /// Represents the three animation frames for the Dart ship
    /// </summary>
    enum DartSteeringState {
        Left = 0,
        Straight = 1,
        Right = 2,
    }

    /// <summary>
    /// An enemy ship that flies toward the Player and fires
    /// </summary>
    public class Dart : Enemy
    {   
        // Dart state variables
        Texture2D spritesheet;
        Vector2 position;
        Rectangle[] spriteBounds = new Rectangle[3];
        DartSteeringState steeringState = DartSteeringState.Straight;

        /// <summary>
        /// The bounding rectangle of the Dart
        /// </summary>
        public override Rectangle Bounds
        {
            get { return new Rectangle((int)position.X, (int)position.Y, spriteBounds[(int)steeringState].Width, spriteBounds[(int)steeringState].Height); }
        }

        /// <summary>
        /// Creates a new instance of a Dart enemy ship
        /// </summary>
        /// <param name="content">A ContentManager to load resources with</param>
        /// <param name="position">The position of the Dart ship in the game world</param>
        public Dart(uint id, ContentManager content, Vector2 position) : base (id)
        {
            this.position = position;

            spritesheet = content.Load<Texture2D>("Spritesheets/newshf.shp.000000");

            spriteBounds[(int)DartSteeringState.Left].X = 98;
            spriteBounds[(int)DartSteeringState.Left].Y = 84;
            spriteBounds[(int)DartSteeringState.Left].Width = 20;
            spriteBounds[(int)DartSteeringState.Left].Height = 28;

            spriteBounds[(int)DartSteeringState.Straight].X = 122;
            spriteBounds[(int)DartSteeringState.Straight].Y = 84;
            spriteBounds[(int)DartSteeringState.Straight].Width = 20;
            spriteBounds[(int)DartSteeringState.Straight].Height = 28;

            spriteBounds[(int)DartSteeringState.Right].X = 147;
            spriteBounds[(int)DartSteeringState.Right].Y = 84;
            spriteBounds[(int)DartSteeringState.Right].Width = 20;
            spriteBounds[(int)DartSteeringState.Right].Height = 28;

            steeringState = DartSteeringState.Straight;
         
        }

        /// <summary>
        /// Updates the Dart ship
        /// </summary>
        /// <param name="elapsedTime">The in-game time between the previous and current frame</param>
        public override void  Update(float elapsedTime)
        {
            // Sense the Player's position
            PlayerShip Player = ScrollingShooterGame.Game.Player;
            Vector2 PlayerPosition = new Vector2(Player.Bounds.Center.X, Player.Bounds.Center.Y);

            // Get a vector from our position to the Player's position
            Vector2 toPlayer = PlayerPosition - this.position;

            if(toPlayer.LengthSquared() < 40000)
            {
                // We sense the Player's ship!                  
                // Get a normalized steering vector
                toPlayer.Normalize();

                // Steer towards them!
                //this.position += toPlayer * elapsedTime * 100;

                // Change the steering state to reflect our direction
                if (toPlayer.X < -0.5f) steeringState = DartSteeringState.Left;
                else if (toPlayer.X > 0.5f) steeringState = DartSteeringState.Right;
                else steeringState = DartSteeringState.Straight;
            }                        
        }

        /// <summary>
        /// Draw the Dart ship on-screen
        /// </summary>
        /// <param name="elapsedTime">The in-game time between the previous and current frame</param>
        /// <param name="spriteBatch">An already initialized SpriteBatch, ready for Draw() commands</param>
        public override void Draw(float elapsedTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(spritesheet, Bounds, spriteBounds[(int)steeringState], Color.White, 0f, new Vector2(Bounds.Width / 2, Bounds.Height / 2), SpriteEffects.None, 1f);
        }

    }
}