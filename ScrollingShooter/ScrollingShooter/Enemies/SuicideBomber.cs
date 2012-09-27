using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using System;

namespace ScrollingShooter
{
    /// <summary>
    /// Represents the state of the SuicideBomber Enemy
    /// </summary>
    enum SuicideBomberState
    {
        Left = 0,
        Straight = 1,
        Right = 2,
    }

    /// <summary>
    /// An enemy that flies straight at the player and explodes on impact
    /// </summary>f
    public class SuicideBomber : Enemy
    {
        // SuicideBomber state variables
        Texture2D spritesheet;
        Vector2 position;
        Rectangle[] spriteBounds = new Rectangle[3];
        SuicideBomberState steeringState = SuicideBomberState.Straight;

        /// <summary>
        /// The bounding rectangle of the SuicideBomber
        /// </summary>
        public override Rectangle Bounds
        {
            get { return new Rectangle((int)position.X, (int)position.Y, spriteBounds[(int)steeringState].Width, spriteBounds[(int)steeringState].Height); }
        }

        /// <summary>
        /// Creates a new instance of a SuicideBomber enemy ship
        /// </summary>
        /// <param name="content">A ContentManager to load resources with</param>
        /// <param name="position">The position of the Dart ship in the game world</param>
        public SuicideBomber(uint id, ContentManager content, Vector2 position)
            : base(id)
        {
            this.position = position;

            spritesheet = content.Load<Texture2D>("Spritesheets/newshf.shp.000000");

            spriteBounds[(int)DartSteeringState.Left].X = 97;
            spriteBounds[(int)DartSteeringState.Left].Y = 0;
            spriteBounds[(int)DartSteeringState.Left].Width = 24;
            spriteBounds[(int)DartSteeringState.Left].Height = 27;

            spriteBounds[(int)DartSteeringState.Straight].X = 121;
            spriteBounds[(int)DartSteeringState.Straight].Y = 0;
            spriteBounds[(int)DartSteeringState.Straight].Width = 24;
            spriteBounds[(int)DartSteeringState.Straight].Height = 27;

            spriteBounds[(int)DartSteeringState.Right].X = 146;
            spriteBounds[(int)DartSteeringState.Right].Y = 0;
            spriteBounds[(int)DartSteeringState.Right].Width = 24;
            spriteBounds[(int)DartSteeringState.Right].Height = 27;

            steeringState = SuicideBomberState.Straight;
        }

        /// <summary>
        /// Updates the SuicideBomber ship
        /// </summary>
        /// <param name="elapsedTime">The in-game time between the previous and current frame</param>
        public override void Update(float elapsedTime)
        {
            // Sense the player's position
            PlayerShip player = ScrollingShooterGame.Game.Player;
            Vector2 playerPosition = new Vector2(player.Bounds.Center.X, player.Bounds.Center.Y);

            // Get a vector from our position to the player's position
            Vector2 toPlayer = playerPosition - this.position;

            if (toPlayer.LengthSquared() < 90000)
            {
                // We sense the player's ship and get a normalized movement vector
                toPlayer.Normalize();

                // We steer towards the player's ship
                this.position += toPlayer * elapsedTime * 100;

                // Change the steering state to reflect our direction
                if (toPlayer.X < -0.5f) steeringState = SuicideBomberState.Left;
                else if (toPlayer.X > 0.5f) steeringState = SuicideBomberState.Right;
                else steeringState = SuicideBomberState.Straight;
            }
        }

        /// <summary>
        /// Draw the SuicideBomber ship on-screen
        /// </summary>
        /// <param name="elapsedTime">The in-game time between the previous and current frame</param>
        /// <param name="spriteBatch">An already initialized SpriteBatch, ready for Draw() commands</param>
        public override void Draw(float elapsedTime, SpriteBatch spriteBatch)
        {
            PlayerShip player = ScrollingShooterGame.Game.Player;
            Vector2 playerPosition = new Vector2(player.Bounds.Center.X, player.Bounds.Center.Y);
            Vector2 toPlayer = playerPosition - this.position;
            double angle = (2 * Math.PI) - Math.Atan2(toPlayer.X, toPlayer.Y);
            if (toPlayer.LengthSquared() < 90000)
            {
                spriteBatch.Draw(spritesheet, Bounds, spriteBounds[(int)steeringState], Color.White, (float)angle, new Vector2(Bounds.Width / 2, Bounds.Height / 2), SpriteEffects.None, 1f);
            }
            else
                spriteBatch.Draw(spritesheet, Bounds, spriteBounds[(int)steeringState], Color.White);
        }
    }
}
