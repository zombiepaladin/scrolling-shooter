using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace ScrollingShooter
{

    /// <summary>
    /// An enemy ship that flies toward the Player and fires
    /// </summary>
    public class Cobalt : Enemy
    {
        // Cobalt state variables
        Texture2D spritesheet;
        Vector2 position;
        Rectangle spriteBounds;

        /// <summary>
        /// The bounding rectangle of the Dart
        /// </summary>
        public override Rectangle Bounds
        {
            get { return new Rectangle((int)position.X, (int)position.Y, spriteBounds.Width, spriteBounds.Height); }
        }

        /// <summary>
        /// Creates a new instance of a Cobalt enemy ship
        /// </summary>
        /// <param name="content">A ContentManager to load resources with</param>
        /// <param name="position">The position of the Cobalt ship in the game world</param>
        public Cobalt(uint id, ContentManager content, Vector2 position)
            : base(id)
        {
            this.position = position;

            spritesheet = content.Load<Texture2D>("Spritesheets/newsh2.shp.000000");

            spriteBounds.X = 100;
            spriteBounds.Y = 55;
            spriteBounds.Width = 37;
            spriteBounds.Height = 28;
        }

        /// <summary>
        /// Updates the Cobalt ship
        /// </summary>
        /// <param name="elapsedTime">The in-game time between the previous and current frame</param>
        public override void Update(float elapsedTime)
        {
            // Sense the Player's position
            PlayerShip Player = ScrollingShooterGame.Game.Player;
            Vector2 PlayerPosition = new Vector2(Player.Bounds.Center.X, Player.Bounds.Center.Y);

            // Get a vector from our position to the Player's position
            Vector2 toPlayer = PlayerPosition - this.position;

            //Sense the Player from longer away but move slower.
            if (toPlayer.LengthSquared() < 80000)
            {
                // We sense the Player's ship!                  
                // Get a normalized steering vector
                toPlayer.Normalize();

                // Steer towards them!
                this.position += toPlayer * elapsedTime * 50;
            }
            if (toPlayer.LengthSquared() < 10000)
            {
                //Player is close fire weapons
                //Thinking of some type of pulse wave from the ship

            }
        }

        /// <summary>
        /// Draw the Cobalt ship on-screen
        /// </summary>
        /// <param name="elapsedTime">The in-game time between the previous and current frame</param>
        /// <param name="spriteBatch">An already initialized SpriteBatch, ready for Draw() commands</param>
        public override void Draw(float elapsedTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(spritesheet, Bounds, spriteBounds, Color.White);
        }

    }
}