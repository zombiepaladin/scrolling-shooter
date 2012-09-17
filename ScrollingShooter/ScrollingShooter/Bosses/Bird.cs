using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace ScrollingShooter
{
    class Bird : Enemy

    {
             // Dart state variables
        Texture2D spritesheet;
        Vector2 position;

        Vector2 beakPosition;

        Rectangle[] spriteBounds = new Rectangle[1];
        Rectangle[] spriteBoundsAnim = new Rectangle[4];


        int headAnimState = 0;
        bool openingBeak = true;
        static int NUMBER_OF_ANIM_STATES = 4;

        float headAnimTimer = 0f;
        float fireTimer = 3.0f;



        /// <summary>
        /// The bounding rectangle of the Bird
        /// </summary>
        public override Rectangle Bounds
        {
            get { return new Rectangle((int)position.X, (int)position.Y, spriteBounds[0].Width, spriteBounds[0].Height); }
        }
        /// <summary>
        /// The bounding rectangle of the Birds head
        /// </summary>
        public Rectangle HeadBounds
        {
            get { return new Rectangle((int)position.X + (spriteBounds[0].Width / 2) - (spriteBoundsAnim[0].Width/2), (int)position.Y + spriteBounds[0].Height, spriteBoundsAnim[0].Width, spriteBoundsAnim[0].Height); }
        }
       

        /// <summary>
        /// Creates a new instance of a Bird boss 
        /// </summary>
        /// <param name="content">A ContentManager to load resources with</param>
        /// <param name="position">The position of the Bird boss in the game world</param>
        public Bird(uint id, ContentManager content, Vector2 position)
            : base(id)
        {
            this.position = position;
            spritesheet = content.Load<Texture2D>("Spritesheets/newsh0.shp.000000");

            spriteBounds[0].X = 3;
            spriteBounds[0].Y = 0;
            spriteBounds[0].Width = 187;
            spriteBounds[0].Height = 84;

            spriteBoundsAnim[2].X = 48;
            spriteBoundsAnim[2].Y = 84;
            spriteBoundsAnim[2].Width = 23;
            spriteBoundsAnim[2].Height = 25;

            spriteBoundsAnim[1].X = 72;
            spriteBoundsAnim[1].Y = 84;
            spriteBoundsAnim[1].Width = 23;
            spriteBoundsAnim[1].Height = 25;

            spriteBoundsAnim[0].X = 96;
            spriteBoundsAnim[0].Y = 84;
            spriteBoundsAnim[0].Width = 23;
            spriteBoundsAnim[0].Height = 25;

            spriteBoundsAnim[3].X = 168;
            spriteBoundsAnim[3].Y = 84;
            spriteBoundsAnim[3].Width = 23;
            spriteBoundsAnim[3].Height = 25;

            beakPosition.X = (spriteBounds[0].Width / 2)+8;
            beakPosition.Y = spriteBounds[0].Height + spriteBoundsAnim[0].Height;

           
        }

        /// <summary>
        /// Updates the Dart ship
        /// </summary>
        /// <param name="elapsedTime">The in-game time between the previous and current frame</param>
        public override void Update(float elapsedTime)
        {

            fireTimer += elapsedTime;
            
            float velocity = 1;
            // Sense the player's position
            PlayerShip player = ScrollingShooterGame.Game.player;
            Vector2 playerPosition = new Vector2(player.Bounds.Center.X, player.Bounds.Center.Y);
         

            if (playerPosition.X - Bounds.Center.X < -20)
            {
               
                this.position.X -= elapsedTime * 40 * velocity;
            }
            else
            {
                if (playerPosition.X - Bounds.Center.X > 20)
                {
                    this.position.X += elapsedTime * 40 * velocity;
                }
                else
                {
                    if (fireTimer > 0.6f)
                    {
                        ScrollingShooterGame.GameObjectManager.CreateProjectile(ProjectileType.BirdWrath, position + beakPosition);
                        fireTimer = 0f;
                    }
                }


            }

            if (fireTimer > 1.5f)
            {
                ScrollingShooterGame.GameObjectManager.CreateProjectile(ProjectileType.BirdWrath, position + beakPosition);
                fireTimer = 0f;
            }
        }

        /// <summary>
        /// Draw the Dart ship on-screen
        /// </summary>
        /// <param name="elapsedTime">The in-game time between the previous and current frame</param>
        /// <param name="spriteBatch">An already initialized SpriteBatch, ready for Draw() commands</param>
        public override void Draw(float elapsedTime, SpriteBatch spriteBatch)
        {
            headAnimTimer+=elapsedTime;
                
           
                spriteBatch.Draw(spritesheet, Bounds, spriteBounds[0], Color.White);
                if (headAnimTimer > 0.1f)
                {
                    
                    if (openingBeak)
                    {
                        headAnimState++;
                    }
                    else
                    {
                        headAnimState--;
                    }
                    if (headAnimState == NUMBER_OF_ANIM_STATES-1)
                    {
                        openingBeak = false;
                    }
                    if (headAnimState == 0)
                    {
                        openingBeak = true;
                    }
                    headAnimTimer = 0;
                }
                spriteBatch.Draw(spritesheet, HeadBounds, spriteBoundsAnim[headAnimState], Color.White);
           
        }

       

    }
}
