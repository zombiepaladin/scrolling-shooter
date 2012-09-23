using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace ScrollingShooter
{
    /// <summary>
    /// Represents the state of the explosion,
    /// higher numbers are later parts of the explosion
    /// </summary>
    enum ExplosionState
    {
        Stage1 = 0,
        Stage2 = 1,
        Stage3 = 2,
        Stage4 = 3,
        Stage5 = 4,
        Stage6 = 5,
        Stage7 = 6,
        Stage8 = 7,
        Stage9 = 8,
        Stage10 = 9,
        Stage11 = 10,
        Finished
    }

    /// <summary>
    /// A generic explosion
    /// </summary>
    public class Explosion : GameObject
    {
        //Explosion state variables
        Vector2 position;
        Texture2D spriteSheet;
        Rectangle[,] spriteBounds = new Rectangle[4, 11];
        ExplosionState explosionState;
        float explosionTimer;

        /// <summary>
        /// Gets the bounds for the entire assembled explosion
        /// </summary>
        public override Rectangle Bounds
        {
            get { return new Rectangle((int)position.X - 12, (int)position.Y - 14, 24, 28); }
        }

        /// <summary>
        /// The bounding rectangle of the specified section of the explosion
        /// </summary>
        /// <param name="section">The section of the explosion to get the bounds for</param>
        /// <returns></returns>
        public Rectangle ExplosionBounds(int section)
        {
            if (section == 0) return new Rectangle((int)position.X - 12, (int)position.Y - 14, spriteBounds[section, (int)explosionState].Width, spriteBounds[section, (int)explosionState].Height);
            else if (section == 1) return new Rectangle((int)position.X, (int)position.Y - 14, spriteBounds[section, (int)explosionState].Width, spriteBounds[section, (int)explosionState].Height);
            else if (section == 2) return new Rectangle((int)position.X - 12, (int)position.Y, spriteBounds[section, (int)explosionState].Width, spriteBounds[section, (int)explosionState].Height);
            else
            {
                System.Diagnostics.Debug.Assert(section == 3);
                return new Rectangle((int)position.X, (int)position.Y, spriteBounds[section, (int)explosionState].Width, spriteBounds[section, (int)explosionState].Height);
            }
        }

        /// <summary>
        /// Creates a new explosion
        /// </summary>
        /// <param name="position">The position of the explosion within the game world</param>
        /// <param name="content">A ContentManager to load resources with</param>
        public Explosion(uint id, Vector2 position, ContentManager content)
            : base(id)
        {
            this.position = position;

            this.spriteSheet = content.Load<Texture2D>("Spritesheets/tyrian.shp.01D8A7");

            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 11; j++)
                {
                    spriteBounds[i, j] = new Rectangle((120 - j * 12), (126 + i * 14), 12, 14);
                }
            }

            this.explosionState = ExplosionState.Stage1;
            this.explosionTimer = 0;
        }

        /// <summary>
        /// Updates the stage of the explosion
        /// </summary>
        /// <param name="elapsedTime">The in-game time between the previous and current frame</param>
        public override void Update(float elapsedTime)
        {
            explosionTimer += elapsedTime;
            if (explosionState == ExplosionState.Finished)
            {
                ScrollingShooterGame.GameObjectManager.DestroyObject(this.ID);
            }
            else if (explosionState == ExplosionState.Stage11)
            {
                if (explosionTimer > 0.1f)
                {
                    explosionState++;
                    explosionTimer = 0;
                }
            }
            else if (explosionTimer >= 0.05f)
            {
                explosionState++;
                explosionTimer = 0;
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
            if (explosionState != ExplosionState.Finished)
            {
                spriteBatch.Draw(spriteSheet, ExplosionBounds(0), spriteBounds[0, (int)explosionState], Color.White);
                spriteBatch.Draw(spriteSheet, ExplosionBounds(1), spriteBounds[1, (int)explosionState], Color.White);
                spriteBatch.Draw(spriteSheet, ExplosionBounds(2), spriteBounds[2, (int)explosionState], Color.White);
                spriteBatch.Draw(spriteSheet, ExplosionBounds(3), spriteBounds[3, (int)explosionState], Color.White);
            }
        }
    }
}
