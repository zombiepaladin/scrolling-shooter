using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace ScrollingShooter.Bosses
{
    /// <summary>
    /// Represents the state of the Blimp Boss
    /// </summary>
    enum BlimpState
    {
        Normal = 0,
        LeftGunDead,
        RightGunDead,
        BothGunsDead,
        Below25Percent,
    }

    /// <summary>
    /// A blimp boss
    /// </summary>
    public class Blimp : Enemy
    {
        // Blimp state variables
        Texture2D spritesheet;
        Vector2 position;
        Rectangle[] spriteBounds = new Rectangle[5];
        BlimpState state;
        int maxHealth = 1000;

        /// <summary>
        /// The bounding rectangle of the SuicideBomber
        /// </summary>
        public override Rectangle Bounds
        {
            get { return new Rectangle((int)position.X, (int)position.Y, spriteBounds[(int)state].Width, spriteBounds[(int)state].Height); }
        }

        /// <summary>
        /// Creates a new Blimp
        /// </summary>
        /// <param name="content">A ContentManager to load resources with</param>
        /// <param name="position">The position of the Blimp in the game world</param>
        public Blimp(uint id, Vector2 position, ContentManager content) : base(id)
        {
            this.position = position;

            spritesheet = content.Load<Texture2D>("Spritesheets/newshi.shp.000000");

            this.Health = (float)maxHealth;

            spriteBounds[(int)BlimpState.Normal].X = 0;
            spriteBounds[(int)BlimpState.Normal].Y = 10;
            spriteBounds[(int)BlimpState.Normal].Width = 96;
            spriteBounds[(int)BlimpState.Normal].Height = 130;

            
            spriteBounds[(int)BlimpState.LeftGunDead].X = 12;
            spriteBounds[(int)BlimpState.LeftGunDead].Y = 10;
            spriteBounds[(int)BlimpState.LeftGunDead].Width = 84;
            spriteBounds[(int)BlimpState.LeftGunDead].Height = 130;

            spriteBounds[(int)BlimpState.RightGunDead].X = 0;
            spriteBounds[(int)BlimpState.RightGunDead].Y = 10;
            spriteBounds[(int)BlimpState.RightGunDead].Width = 83;
            spriteBounds[(int)BlimpState.RightGunDead].Height = 130;

            spriteBounds[(int)BlimpState.BothGunsDead].X = 12;
            spriteBounds[(int)BlimpState.BothGunsDead].Y = 10;
            spriteBounds[(int)BlimpState.BothGunsDead].Width = 71;
            spriteBounds[(int)BlimpState.BothGunsDead].Height = 130;

            spriteBounds[(int)BlimpState.Below25Percent].X = 108;
            spriteBounds[(int)BlimpState.Below25Percent].Y = 10;
            spriteBounds[(int)BlimpState.Below25Percent].Width = 71;
            spriteBounds[(int)BlimpState.Below25Percent].Height = 130;

            this.state = BlimpState.Normal;
        }

        public override void Update(float elapsedTime)
        {
            if ((Health / maxHealth) < 0.25f) this.state = BlimpState.Below25Percent;
        }

        public override void Draw(float elapsedTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(spritesheet, Bounds, spriteBounds[(int)state], Color.White);
        }
    }
}
