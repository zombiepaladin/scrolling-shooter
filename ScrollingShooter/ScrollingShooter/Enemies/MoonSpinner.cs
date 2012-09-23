using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;


namespace ScrollingShooter.Enemies
{
    class MoonSpinner:Enemy
    {
        enum spinState
        {
            S0 = 0,
            S1,
            S2,
            S3,
            S4,
            S5,
            S6,
            S7
        }
        int sc = 0;
        Texture2D spritesheet;
        Vector2 position;
        Rectangle[] spriteBounds = new Rectangle[8];
        spinState ss = spinState.S0;

        /// <summary>
        /// Creates the moon spinner. 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="content">A Content manager</param>
        /// <param name="position"></param>
        public override Rectangle Bounds
        {
            get { return new Rectangle((int)position.X, (int)position.Y, spriteBounds[(int)ss].Width, spriteBounds[(int)ss].Height); }
        }

        /// <summary>
        /// Updates the moon spinner.
        /// </summary>
        /// <param name="elapsedTime">Time elapsed</param>
        public MoonSpinner(uint id, ContentManager content, Vector2 position)
            : base(id)
        {
            Health = 40;
            this.position = position;
            spritesheet = content.Load<Texture2D>("Spritesheets/newsh2.shp.000000");

            spriteBounds[(int)spinState.S0].X = 0;
            spriteBounds[(int)spinState.S0].Y = 170;
            spriteBounds[(int)spinState.S0].Width = 24;
            spriteBounds[(int)spinState.S0].Height = 20;

            spriteBounds[(int)spinState.S1].X = 24;
            spriteBounds[(int)spinState.S1].Y = 170;
            spriteBounds[(int)spinState.S1].Width = 24;
            spriteBounds[(int)spinState.S1].Height = 20;

            spriteBounds[(int)spinState.S2].X = 48;
            spriteBounds[(int)spinState.S2].Y = 170;
            spriteBounds[(int)spinState.S2].Width = 24;
            spriteBounds[(int)spinState.S2].Height = 20;

            spriteBounds[(int)spinState.S3].X = 72;
            spriteBounds[(int)spinState.S3].Y = 170;
            spriteBounds[(int)spinState.S3].Width = 24;
            spriteBounds[(int)spinState.S3].Height = 20;

            spriteBounds[(int)spinState.S4].X = 96;
            spriteBounds[(int)spinState.S4].Y = 170;
            spriteBounds[(int)spinState.S4].Width = 24;
            spriteBounds[(int)spinState.S4].Height = 20;

            spriteBounds[(int)spinState.S5].X = 120;
            spriteBounds[(int)spinState.S5].Y = 170;
            spriteBounds[(int)spinState.S5].Width = 24;
            spriteBounds[(int)spinState.S5].Height = 20;

            spriteBounds[(int)spinState.S6].X = 144;
            spriteBounds[(int)spinState.S6].Y = 170;
            spriteBounds[(int)spinState.S6].Width = 24;
            spriteBounds[(int)spinState.S6].Height = 20;

            spriteBounds[(int)spinState.S7].X = 168;
            spriteBounds[(int)spinState.S7].Y = 170;
            spriteBounds[(int)spinState.S7].Width = 24;
            spriteBounds[(int)spinState.S7].Height = 20;

            ss = spinState.S0;
        }

        public override void Update(float elapsedTime)
        {
            PlayerShip ps = ScrollingShooterGame.Game.Player;
            Vector2 pp = new Vector2(ps.Bounds.Center.X, ps.Bounds.Center.Y);
            Vector2 dp = pp - this.position;
            if (dp.LengthSquared() < 90000)
            {
                dp.Normalize();
                this.position += dp * elapsedTime * 100;
            }
            if (sc == 1)
            {
                if (ss == spinState.S0) ss = spinState.S1;
                else if (ss == spinState.S1) ss = spinState.S2;
                else if (ss == spinState.S2) ss = spinState.S3;
                else if (ss == spinState.S3) ss = spinState.S4;
                else if (ss == spinState.S6) ss = spinState.S5;
                else if (ss == spinState.S5) ss = spinState.S6;
                else if (ss == spinState.S7) ss = spinState.S0;
                sc = 0;
            }
            else
            {
                sc++;
            }

        }
        /// <summary>
        /// Draws the Moon Spinner.
        /// </summary>
        /// <param name="elapsedTime">Time elapsed</param>
        /// <param name="spriteBatch">Sprite grouping</param>
        public override void Draw(float elapsedTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(spritesheet, Bounds, spriteBounds[(int)ss], Color.White);
        }
    }
}
