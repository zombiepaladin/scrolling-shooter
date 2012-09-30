using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace ScrollingShooter
{
    /// <summary>
    /// An eight ball shield
    /// </summary>
    public class EightBallShield : Shield
    {
        Vector2 playerPosition;
        PlayerShip playerShip;

        /// <summary>
        /// Creates a new eight ball shield
        /// </summary>
        /// <param name="contentManager">A ContentManager to load content from</param>
        /// <param name="playerPosition">A position on the screen</param>
        /// <param name="playerShip">A pointer to the current player ship</param>
        public EightBallShield(uint id, ContentManager contentManager, Vector2 playerPosition,
            PlayerShip playerShip) : base (id)
        {
            this.spriteSheet = contentManager.Load<Texture2D>("Spritesheets/eightballshield");
            this.playerPosition = playerPosition;
            this.playerShip = playerShip;
            this.spriteBounds = new Rectangle(24, 23, 53, 58);

            //set to 0 for x & y to test
            //don't actually want the shield to deviate from
            // the player ship's position
            this.velocity = new Vector2(0, 0);

            //test an offset position (ghetto)
            this.position.X = playerPosition.X - 25;
            this.position.Y = playerPosition.Y - 26;
        }

        public override void Update(float elapsedTime)
        {
            this.position.X = playerShip.Bounds.X - 25;
            this.position.Y = playerShip.Bounds.Y - 26;
        }
     }
}
