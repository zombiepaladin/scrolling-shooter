using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace ScrollingShooter
{
    /// <summary>
    /// Represents the five turns for the std bad ship.
    /// </summary>
    enum StdBaddyStearingState
    {
        Left = 0,
        Streight = 1,
        Right = 2,
    }
    /// <summary>
    /// This creates the Standerd Baddy class.
    /// </summary>
    class StdBaddy:Enemy
    {
        float dgt = 0;
        Texture2D spritesheet;
        Vector2 position;
        Rectangle[] spriteBounds = new Rectangle[3];
        StdBaddyStearingState steeringState = StdBaddyStearingState.Streight;


        public override Rectangle Bounds
        {
            get { return new Rectangle((int)position.X, (int)position.Y, spriteBounds[(int)steeringState].Width, spriteBounds[(int)steeringState].Height); }
        }
        /// <summary>
        /// Creates a Standared baddy. 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="content">A Content manager</param>
        /// <param name="position"></param>
        public StdBaddy(uint id, ContentManager content, Vector2 position)
            : base(id)
        {
            this.position = position;

            spritesheet = content.Load<Texture2D>("Spritesheets/newsh2.shp.000000");

            spriteBounds[(int)StdBaddyStearingState.Left].X = 25;
            spriteBounds[(int)StdBaddyStearingState.Left].Y = 140;
            spriteBounds[(int)StdBaddyStearingState.Left].Width = 20;
            spriteBounds[(int)StdBaddyStearingState.Left].Height = 28;

            spriteBounds[(int)StdBaddyStearingState.Streight].X = 2;
            spriteBounds[(int)StdBaddyStearingState.Streight].Y = 140;
            spriteBounds[(int)StdBaddyStearingState.Streight].Width = 20;
            spriteBounds[(int)StdBaddyStearingState.Streight].Height = 28;

            spriteBounds[(int)StdBaddyStearingState.Right].X = 145;
            spriteBounds[(int)StdBaddyStearingState.Right].Y = 140;
            spriteBounds[(int)StdBaddyStearingState.Right].Width = 20;
            spriteBounds[(int)StdBaddyStearingState.Right].Height = 28;

            steeringState = StdBaddyStearingState.Streight;
        }
        /// <summary>
        /// Updates the Standard Baddy.
        /// </summary>
        /// <param name="elapsedTime">Time elapsed</param>
        public override void Update(float elapsedTime)
        {
            dgt += elapsedTime;
            PlayerShip ps = ScrollingShooterGame.Game.Player;
            Vector2 pp = new Vector2(ps.Bounds.Center.X, ps.Bounds.Center.Y);
            Vector2 dp = pp - this.position;
            if (dp.LengthSquared() > 30000)
            {
                dp.Normalize();
                this.position += dp * elapsedTime * 100;
                if (dp.X < -0.5f) steeringState = StdBaddyStearingState.Left;
                else if (dp.X > 0.5f) steeringState = StdBaddyStearingState.Right;
                else steeringState = StdBaddyStearingState.Streight;
            }
            if (dgt > .75f)
            {
                ScrollingShooterGame.GameObjectManager.CreateProjectile(ProjectileType.EBullet, position);
                dgt = 0;
            }
        }
        /// <summary>
        /// Draws the standared baddy.
        /// </summary>
        /// <param name="elapsedTime">Time elapsed</param>
        /// <param name="spriteBatch">Sprite grouping</param>
        public override void Draw(float elapsedTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(spritesheet, Bounds, spriteBounds[(int)steeringState], Color.White);
        }
    }
}