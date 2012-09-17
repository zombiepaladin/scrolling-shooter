using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace ScrollingShooter
{
    /// <summary>
    /// Represents the three animation frames for the Deer Tick ship
    /// </summary>
    public enum DeerTickDirection {
        Straight = 0,
        Right = 1,
        Left = 2
    }

    /// <summary>
    /// An enemy ship that flies either down, left or right and fires. Not fast, fairly simple.
    /// </summary>
    public class DeerTick : Enemy
    {   
        // Deer Tick state variables
        Texture2D spritesheet;
        Vector2 position;
        Rectangle[] spriteBounds = new Rectangle[3];
        DeerTickDirection direction = DeerTickDirection.Straight;
        float shotTimer; //used to keep track of when to shoot the next bullet

        const float DT_SHOT_DELAY = 1f; // delay between bullets
        const float DT_SPEED = 100f; //speed of the deer tick ship

        /// <summary>
        /// The bounding rectangle of the Deer Tick
        /// </summary>
        public override Rectangle Bounds
        {
            get { return new Rectangle((int)position.X, (int)position.Y, spriteBounds[(int)direction].Width, spriteBounds[(int)direction].Height); }
        }

        /// <summary>
        /// Creates a new instance of a Deer Tick enemy ship
        /// </summary>
        /// <param name="content">A ContentManager to load resources with</param>
        /// <param name="position">The position of the Deer Tick ship in the game world</param>
        public DeerTick(uint id, ContentManager content, Vector2 position, DeerTickDirection dir)
            : base(id)
        {
            this.position = position;

            spritesheet = content.Load<Texture2D>("Spritesheets/newsh2.shp.000000");

            spriteBounds[(int)DeerTickDirection.Straight].X = 0;
            spriteBounds[(int)DeerTickDirection.Straight].Y = 196;
            spriteBounds[(int)DeerTickDirection.Straight].Width = 24;
            spriteBounds[(int)DeerTickDirection.Straight].Height = 28;

            spriteBounds[(int)DeerTickDirection.Right].X = 24;
            spriteBounds[(int)DeerTickDirection.Right].Y = 196;
            spriteBounds[(int)DeerTickDirection.Right].Width = 24;
            spriteBounds[(int)DeerTickDirection.Right].Height = 28;

            spriteBounds[(int)DeerTickDirection.Left].X = 48;
            spriteBounds[(int)DeerTickDirection.Left].Y = 196;
            spriteBounds[(int)DeerTickDirection.Left].Width = 24;
            spriteBounds[(int)DeerTickDirection.Left].Height = 28;

            direction = dir;
            shotTimer = DT_SHOT_DELAY; //reset the bullet timer
        }

        /// <summary>
        /// Updates the Deer Tick ship
        /// </summary>
        /// <param name="elapsedTime">The in-game time between the previous and current frame</param>
        public override void  Update(float elapsedTime)
        {
            //Advance either down, left or right depending on which direction it's facing
            switch(this.direction)
            {
                case DeerTickDirection.Straight:
                    this.position.Y += DT_SPEED * elapsedTime;
                    break;
                case DeerTickDirection.Right:
                    this.position.X += DT_SPEED * elapsedTime;
                    break;
                case DeerTickDirection.Left:
                    this.position.X -= DT_SPEED * elapsedTime;
                    break;
            }

            shotTimer -= elapsedTime; 

            if (shotTimer <= 0) //If a DT_SHOT_DELAY amount of time has passed, shoot a bullet
            {
                ShootGenericEnemyBullet();
                shotTimer = DT_SHOT_DELAY; //reset the timer
            }
        }

        /// <summary>
        /// Shoot a single GenericEnemyBullet that will aim for the player.
        /// </summary>
        private void ShootGenericEnemyBullet()
        {
            ScrollingShooterGame.GameObjectManager.CreateProjectile(ProjectileType.GenericEnemyBullet, new Vector2(Bounds.Center.X, Bounds.Center.Y));
        }

        /// <summary>
        /// Draw the Deer Tick ship on-screen
        /// </summary>
        /// <param name="elapsedTime">The in-game time between the previous and current frame</param>
        /// <param name="spriteBatch">An already initialized SpriteBatch, ready for Draw() commands</param>
        public override void Draw(float elapsedTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(spritesheet, Bounds, spriteBounds[(int)direction], Color.White, 0f, new Vector2(Bounds.Width / 2, Bounds.Height / 2), SpriteEffects.None, 1f);
        }

    }
}
