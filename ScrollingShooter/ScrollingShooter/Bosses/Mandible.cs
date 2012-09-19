using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

//Authors: Adam Clark
//         Josh Zavala
//         Nick Stanley
namespace ScrollingShooter
{
    /// <summary>
    /// This Enemy takes damage from the player and then fires at them when
    /// health is 0 - it is a part of the boss.
    /// </summary>
    class Mandible : Enemy
    {
        //the vars
        Texture2D spritesheet;
        Vector2 position;
        Rectangle[] spriteBounds = new Rectangle[1];
        bool isLeft;
        bool isFired;

        /// <summary>
        /// The bounding rectangle of the Mandible
        /// </summary>
        public override Rectangle Bounds
        {
            get
            {
                return new Rectangle((int)position.X, (int)position.Y,
                    spriteBounds[0].Width, spriteBounds[0].Height);
            }
        }

        /// <summary>
        /// Creates a new instance of the Mandible
        /// </summary>
        /// <param name="id">the id tag</param>
        /// <param name="content">>A ContentManager to load resources with</param>
        /// <param name="position">The position of the Mandible in the game world</param>
        public Mandible(uint id, ContentManager content, Vector2 position, bool side)
            : base(id)
        {
            this.position = position;
            this.isLeft = side;
            Health = 8;
            this.isFired = false;

            //spritesheet
            spritesheet = content.Load<Texture2D>("Spritesheets/vulcano");

            spriteBounds[0].X = 96;
            spriteBounds[0].Y = 140;
            spriteBounds[0].Width = 24;
            spriteBounds[0].Height = 57;


            //spritebounds
        }

        /// <summary>
        /// Updates the Mandible
        /// </summary>
        /// <param name="elapsedTime">The in-game time between the previous and current frame</param>
        public override void Update(float elapsedTime)
        {
            // Sense the player's position
            PlayerShip player = ScrollingShooterGame.Game.player;
            
            Vector2 playerPosition = new Vector2(player.Bounds.Center.X, player.Bounds.Center.Y);
            Vector2 toPlayer = playerPosition - this.position;

            Health -= elapsedTime; //TODO: remove this from testing

            // Move in front of player
            if (Health <= 0)
            {
                if (!this.isFired)
                {
                    isFired = true;
                    toPlayer.Normalize();
                    this.position += toPlayer * elapsedTime * 500;
                }
                else
                {
                    this.position.Y += elapsedTime * 500;
                }
            }
            if (!isFired)
            {
                if (isLeft) this.position.X = playerPosition.X - 30; //change?
                else this.position.X = playerPosition.X + 30;
            }
        }

        /// <summary>
        /// Draw the Mandible on-screen
        /// </summary>
        /// <param name="elapsedTime">The in-game time between the previous and current frame</param>
        /// <param name="spriteBatch">An already initialized SpriteBatch, ready for Draw() commands</param>
        public override void Draw(float elapsedTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(spritesheet, Bounds, spriteBounds[0], Color.White, 0f, new Vector2(Bounds.Width / 2, Bounds.Height / 2), SpriteEffects.None, 1f);
        }
    }
}
