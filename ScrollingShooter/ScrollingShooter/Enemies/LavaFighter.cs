using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace ScrollingShooter
{
    /// <summary>
    /// Represents the three animation frames for the LavaFighter ship
    /// </summary>
    enum LavaFighterSteeringState
    {
        Left = 0,
        Straight = 1,
        Right = 2,
    }
    public class LavaFighter : Enemy
    {
        Texture2D spritesheet;
        Vector2 position;
        Rectangle[] spriteBounds = new Rectangle[3];
        LavaFighterSteeringState steeringState = LavaFighterSteeringState.Straight;
        float gunTimer = 0;

        /// <summary>
        /// The bounding rectangle of the Dart
        /// </summary>
        public override Rectangle Bounds
        {
            get { return new Rectangle((int)position.X, (int)position.Y, spriteBounds[(int)steeringState].Width, spriteBounds[(int)steeringState].Height); }
        }

        /// <summary>
        /// Creates a new instance of a LavaFighter enemy ship
        /// </summary>
        /// <param name="content">A ContentManager to load resources with</param>
        /// <param name="position">The position of the Dart ship in the game world</param>
        public LavaFighter(uint id, ContentManager content, Vector2 position)
            : base(id)
        {
            this.position = position;

            spritesheet = content.Load<Texture2D>("Spritesheets/newshe.shp.000000");

            spriteBounds[(int)LavaFighterSteeringState.Left].X = 76;
            spriteBounds[(int)LavaFighterSteeringState.Left].Y = 197;
            spriteBounds[(int)LavaFighterSteeringState.Left].Width = 20;
            spriteBounds[(int)LavaFighterSteeringState.Left].Height = 28;

            spriteBounds[(int)LavaFighterSteeringState.Straight].X = 51;
            spriteBounds[(int)LavaFighterSteeringState.Straight].Y = 197;
            spriteBounds[(int)LavaFighterSteeringState.Straight].Width = 20;
            spriteBounds[(int)LavaFighterSteeringState.Straight].Height = 28;

            spriteBounds[(int)LavaFighterSteeringState.Right].X = 28;
            spriteBounds[(int)LavaFighterSteeringState.Right].Y = 197;
            spriteBounds[(int)LavaFighterSteeringState.Right].Width = 20;
            spriteBounds[(int)LavaFighterSteeringState.Right].Height = 28;

            steeringState = LavaFighterSteeringState.Straight;

        }

        /// <summary>
        /// Updates the LavaFighter ship
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
            this.position.Y += elapsedTime * 100;

            if (toPlayer.LengthSquared() < 60000)
            {
                // We sense the player's ship!                  
                // Get a normalized steering vector
                toPlayer.Normalize();

                // Change the steering state to reflect our direction
                if (toPlayer.X < -0.5f)
                {
                    steeringState = LavaFighterSteeringState.Left;
                    //Fire!
                    if (gunTimer > .7f)
                    {
                        ScrollingShooterGame.GameObjectManager.CreateProjectile(ProjectileType.ToPlayerBullet, position);
                        gunTimer = 0f;
                    }
                }
                else if (toPlayer.X > 0.5f)
                {
                    steeringState = LavaFighterSteeringState.Right;
                    //Fire!
                    if (gunTimer > .7f)
                    {
                        ScrollingShooterGame.GameObjectManager.CreateProjectile(ProjectileType.ToPlayerBullet, position);
                        gunTimer = 0f;
                    }
                }
                else
                    steeringState = LavaFighterSteeringState.Straight;
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
