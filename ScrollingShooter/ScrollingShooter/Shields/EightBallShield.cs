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
        Vector2 PlayerPosition;
        PlayerShip PlayerShip;

        /// <summary>
        /// Creates a new eight ball shield
        /// </summary>
        /// <param name="contentManager">A ContentManager to load content from</param>
        /// <param name="PlayerPosition">A position on the screen</param>
        /// <param name="PlayerShip">A pointer to the current Player ship</param>
        public EightBallShield(uint id, ContentManager contentManager, Vector2 PlayerPosition,
            PlayerShip PlayerShip) : base (id)
        {
            this.spriteSheet = contentManager.Load<Texture2D>("Spritesheets/eightballshield");
            this.PlayerPosition = PlayerPosition;
            this.PlayerShip = PlayerShip;
            this.spriteBounds = new Rectangle(24, 23, 53, 58);

            //set to 0 for x & y to test
            //don't actually want the shield to deviate from
            // the Player ship's position
            this.velocity = new Vector2(0, 0);

            //test an offset position (ghetto)
            this.position.X = PlayerPosition.X - 25;
            this.position.Y = PlayerPosition.Y - 26;
        }

        public override void Update(float elapsedTime)
        {
            this.position.X = PlayerShip.Bounds.X - 25;
            this.position.Y = PlayerShip.Bounds.Y - 26;
        }
     }
}
