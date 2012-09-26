using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace ScrollingShooter
{
    /// <summary>
    /// Represents the three animation frames for the Arrow ship
    /// </summary>
    enum ArrowSteeringState
    {
        Left = 0,
        Straight = 1,
        Right = 2,
    }
    /// <summary>
    /// The Arrow is a fast moving enemy ship.
    /// Fires when the player is to the left/right every .7 seconds
    /// </summary>
    public class Arrow : Enemy
    {
        // Arrow state variables
        Texture2D spritesheet;
        Vector2 position;
        Rectangle[] spriteBounds = new Rectangle[3];
        ArrowSteeringState steeringState = ArrowSteeringState.Straight;
        float gunTimer = 0;

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
        public Arrow(uint id, ContentManager content, Vector2 position)
            : base(id)
        {
            this.position = position;

            spritesheet = content.Load<Texture2D>("Spritesheets/newsh$.shp.000000");

            spriteBounds[(int)ArrowSteeringState.Left].X = 5;
            spriteBounds[(int)ArrowSteeringState.Left].Y = 197;
            spriteBounds[(int)ArrowSteeringState.Left].Width = 20;
            spriteBounds[(int)ArrowSteeringState.Left].Height = 28;

            spriteBounds[(int)ArrowSteeringState.Straight].X = 52;
            spriteBounds[(int)ArrowSteeringState.Straight].Y = 197;
            spriteBounds[(int)ArrowSteeringState.Straight].Width = 20;
            spriteBounds[(int)ArrowSteeringState.Straight].Height = 28;

            spriteBounds[(int)ArrowSteeringState.Right].X = 101;
            spriteBounds[(int)ArrowSteeringState.Right].Y = 197;
            spriteBounds[(int)ArrowSteeringState.Right].Width = 20;
            spriteBounds[(int)ArrowSteeringState.Right].Height = 28;

            steeringState = ArrowSteeringState.Straight;

        }

        /// <summary>
        /// Updates the Arrow ship
        /// </summary>
        /// <param name="elapsedTime">The in-game time between the previous and current frame</param>
        public override void Update(float elapsedTime)
        {
            gunTimer += elapsedTime;

            // Sense the player's position
            PlayerShip player = ScrollingShooterGame.Game.Player;
            Vector2 playerPosition = new Vector2(player.Bounds.Center.X, player.Bounds.Center.Y);

            // Get a vector from our position to the player's position
            Vector2 toPlayer = playerPosition - this.position;

            if (toPlayer.LengthSquared() < 20000)
            {
                // We sense the player's ship!        
                // Get a normalized steering vector
                toPlayer.Normalize();

                // Steer towards them!
                this.position += toPlayer * elapsedTime * 120;        

                // Change the steering state to reflect our direction
                if (toPlayer.X < -0.5f)
                {
                    steeringState = ArrowSteeringState.Left;
                    //Fire!
                    if (gunTimer > .7f)
                    {
                        ScrollingShooterGame.GameObjectManager.CreateProjectile(ProjectileType.ArrowProjectile, position);
                        gunTimer = 0f;
                    }
                }
                else if (toPlayer.X > 0.5f)
                {
                    steeringState = ArrowSteeringState.Right;
                    //Fire!
                    if (gunTimer > .7f)
                    {
                        ScrollingShooterGame.GameObjectManager.CreateProjectile(ProjectileType.ArrowProjectile, position);
                        gunTimer = 0f;
                    }
                }
                else
                    steeringState = ArrowSteeringState.Straight;
            }
        }

        /// <summary>
        /// Draw the Arrow ship on-screen
        /// </summary>
        /// <param name="elapsedTime">The in-game time between the previous and current frame</param>
        /// <param name="spriteBatch">An already initialized SpriteBatch, ready for Draw() commands</param>
        public override void Draw(float elapsedTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(spritesheet, Bounds, spriteBounds[(int)steeringState], Color.White, 0f, new Vector2(Bounds.Width / 2, Bounds.Height / 2), SpriteEffects.FlipVertically, 1f);
        }

    }
}
