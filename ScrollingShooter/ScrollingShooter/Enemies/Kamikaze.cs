using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace ScrollingShooter
{
    /// <summary>
    /// Represents the three animation frames for the Kamikaze ship
    /// </summary>
    enum KamikazeState {
        Left = 0,
        Straight = 1,
        Right = 2,
    }

    /// <summary>
    /// An enemy ship that flies toward the Player and fires
    /// </summary>
    public class Kamikaze : Enemy
    {   
        // Dart state variables
        Texture2D spritesheet;
        Vector2 position;
        Rectangle[] spriteBounds = new Rectangle[3];
        DartSteeringState steeringState = DartSteeringState.Straight;

        /// <summary>
        /// The bounding rectangle of the Kamikaze
        /// </summary>
        public override Rectangle Bounds
        {
            get { return new Rectangle((int)position.X, (int)position.Y, spriteBounds[(int)steeringState].Width, spriteBounds[(int)steeringState].Height); }
        }

        /// <summary>
        /// Creates a new instance of a Kamikaze enemy ship
        /// </summary>
        /// <param name="content">A ContentManager to load resources with</param>
        /// <param name="position">The position of the Kamikaze ship in the game world</param>
        public Kamikaze(uint id, ContentManager content, Vector2 position)
            : base(id)
        {
            this.position = position;

            spritesheet = content.Load<Texture2D>("Spritesheets/newsh2.shp.000000");

            spriteBounds[(int)DartSteeringState.Left].X =148;
            spriteBounds[(int)DartSteeringState.Left].Y = 140;
            spriteBounds[(int)DartSteeringState.Left].Width = 15;
            spriteBounds[(int)DartSteeringState.Left].Height =19;

            spriteBounds[(int)DartSteeringState.Straight].X = 3;
            spriteBounds[(int)DartSteeringState.Straight].Y = 141;
            spriteBounds[(int)DartSteeringState.Straight].Width = 17;
            spriteBounds[(int)DartSteeringState.Straight].Height = 19;

            spriteBounds[(int)DartSteeringState.Right].X = 53;
            spriteBounds[(int)DartSteeringState.Right].Y = 141;
            spriteBounds[(int)DartSteeringState.Right].Width = 15;
            spriteBounds[(int)DartSteeringState.Right].Height = 19;

            steeringState = DartSteeringState.Straight;
         
        }

        /// <summary>
        /// Updates the Kamakaze ship
        /// </summary>
        /// <param name="elapsedTime">The in-game time between the previous and current frame</param>
        public override void  Update(float elapsedTime)
        {
            //Scroll with the screen
            position.Y += ScrollingSpeed * elapsedTime;

            // Sense the Player's position
            PlayerShip Player = ScrollingShooterGame.Game.Player;
            Vector2 PlayerPosition = new Vector2(Player.Bounds.Center.X, Player.Bounds.Center.Y);

            // Get a vector from our position to the Player's position
            Vector2 toPlayer = PlayerPosition - this.position;

            if (toPlayer.LengthSquared() < 90000 && (this.Bounds.Y < Player.Bounds.Y))
            {
                // We sense the Player's ship!                  
                // Get a normalized steering vector
                toPlayer.Normalize();

                // Steer towards them
                this.position += toPlayer * elapsedTime * 150;

                // Change the steering state to reflect our direction
                if (toPlayer.X < -0.5f) steeringState = DartSteeringState.Left;
                else if (toPlayer.X > 0.5f) steeringState = DartSteeringState.Right;
                else steeringState = DartSteeringState.Straight;
            }
            else
            {
                this.position.Y += elapsedTime * 150;
                steeringState = DartSteeringState.Straight;
            }
        }

        /// <summary>
        /// Draw the Kamikaze ship on-screen
        /// </summary>
        /// <param name="elapsedTime">The in-game time between the previous and current frame</param>
        /// <param name="spriteBatch">An already initialized SpriteBatch, ready for Draw() commands</param>
        public override void Draw(float elapsedTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(spritesheet, Bounds, spriteBounds[(int)steeringState], Color.White, 0f, new Vector2(Bounds.Width / 2, Bounds.Height / 2), SpriteEffects.None, 1f);
        }

    }
}

