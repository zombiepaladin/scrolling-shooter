using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Audio;
namespace ScrollingShooter
{
    /// <summary>
    /// A generic explosion
    /// </summary>
    public class Explosion2 : GameObject
    {
        //Explosion state variables
        Vector2 position;
        Texture2D spriteSheet;
        Rectangle[] spriteBounds = new Rectangle[12];
        SoundEffect explosionSound;
        int explosionState;
        float explosionTimer;
        float Scale;

        /// <summary>
        /// Gets the bounds for the entire assembled explosion
        /// </summary>
        public override Rectangle Bounds
        {
            get { return new Rectangle((int)position.X, (int)position.Y, spriteBounds[explosionState].Width, spriteBounds[explosionState].Height); }
        }

        /// <summary>
        /// Creates a new explosion
        /// </summary>
        /// <param name="position">The position of the explosion within the game world</param>
        /// <param name="content">A ContentManager to load resources with</param>
        public Explosion2(uint id, Vector2 position, ContentManager content, float scale)
            : base(id)
        {
            this.position = position;

            Scale = scale;

            this.spriteSheet = content.Load<Texture2D>("Spritesheets/explosion");
            explosionSound = content.Load<SoundEffect>("SFX/Blast");
            explosionSound.Play();

            for (int i = 0; i < 12; i++)
            {
                spriteBounds[0] = new Rectangle(134 * i, 0, 134, 134);
            }
            spriteBounds[0] = new Rectangle(0, 0, 134, 134);
            spriteBounds[1] = new Rectangle(134, 0, 134, 134);
            spriteBounds[2] = new Rectangle(268, 0, 134, 134);
            spriteBounds[3] = new Rectangle(402, 0, 134, 134);
            spriteBounds[4] = new Rectangle(536, 0, 134, 134);
            spriteBounds[5] = new Rectangle(670, 0, 134, 134);
            spriteBounds[6] = new Rectangle(804, 0, 134, 134);
            spriteBounds[7] = new Rectangle(938, 0, 134, 134);
            spriteBounds[8] = new Rectangle(1072, 0, 134, 134);
            spriteBounds[9] = new Rectangle(1206, 0, 134, 134);
            spriteBounds[10] = new Rectangle(1340, 0, 134, 134);
            spriteBounds[11] = new Rectangle(1474, 0, 134, 134);

            this.explosionState = 0;
            this.explosionTimer = 0;
        }

        /// <summary>
        /// Updates the stage of the explosion
        /// </summary>
        /// <param name="elapsedTime">The in-game time between the previous and current frame</param>
        public override void Update(float elapsedTime)
        {
            explosionTimer += elapsedTime;
            if (explosionTimer >= 0.05f)
            {
                explosionState++;
                explosionTimer = 0;
                if (explosionState > 11)
                {
                    explosionState--;                    
                    ScrollingShooterGame.GameObjectManager.DestroyObject(this.ID);
                }
            }
        }

        /// <summary>
        /// Draws the explosion on-screen
        /// Note: The explosion is assembled from four different textures
        /// </summary>
        /// <param name="elapsedTime">The in-game time between the previous and current frame</param>
        /// <param name="spriteBatch">An already initialized SpriteBatch, ready for Draw() commands</param>
        public override void Draw(float elapsedTime, SpriteBatch spriteBatch)
        {
            if (explosionState < 12)
            {
                spriteBatch.Draw(spriteSheet, position, spriteBounds[explosionState], Color.White, 0f, new Vector2(Bounds.Width / 2, Bounds.Height / 2), Scale, SpriteEffects.None, 1f);
            }
        }

        /// <summary>
        /// Scrolls the object with the map
        /// </summary>
        /// <param name="elapsedTime">The in-game time between the previous and current frame</param>
        public override void ScrollWithMap(float elapsedTime)
        {
            // Does nothing
        }
    }
}
