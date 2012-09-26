using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace ScrollingShooter
{
    /// <summary>
    /// Represents the three animation frames for the Dart ship
    /// </summary>
    enum BomberSteeringState
    {
        Left = 0,
        Straight = 1,
        Right = 2,
    }

    /// <summary>
    /// An enemy ship that flies toward the player and fires
    /// </summary>
    public class Bomber : Enemy
    {
        // Dart state variables
        Texture2D spritesheet;
        Texture2D spritesheetExplosion;
        Vector2 position;
        Rectangle[] spriteBounds = new Rectangle[3];
        Rectangle[] spriteExplosionBounds = new Rectangle[12];
        int explosionState = 0;

        //workaround for static distance between each bomber
        private static int globalOffset = 0;
        private int offset;

        BomberSteeringState steeringState = BomberSteeringState.Straight;

        // Domb delay timer
        float bombTimer = 1.5f;
        // Turbo delay timer
        float turboTimer = 0f;
        //Explosion delay timer
        float explosionTimer = 0f;
        bool isAlive = true;

        //static float groupFireTimer=0f;
        //static int bomberCount = 0;
        /// <summary>
        /// The bounding rectangle of the Dart
        /// </summary>
        public override Rectangle Bounds
        {
            get { return new Rectangle((int)position.X, (int)position.Y, spriteBounds[(int)steeringState].Width, spriteBounds[(int)steeringState].Height); }
        }
        /// <summary>
        /// The bounding rectangle of the Explosion
        /// </summary>
        public Rectangle ExplosionBounds
        {
            //get { return new Rectangle((int)position.X, (int)position.Y, spriteExplosionBounds[explosionState].Width, spriteExplosionBounds[explosionState].Height); }
            get { return new Rectangle((int)position.X, (int)position.Y, spriteBounds[(int)steeringState].Width, spriteBounds[(int)steeringState].Height); }
        }

        /// <summary>
        /// Creates a new instance of a Dart enemy ship
        /// </summary>
        /// <param name="content">A ContentManager to load resources with</param>
        /// <param name="position">The position of the Dart ship in the game world</param>
        public Bomber(uint id, ContentManager content, Vector2 position)
            : base(id)
        {
            this.position = position;
            //bomberCount++;
            spritesheet = content.Load<Texture2D>("Spritesheets/newsh$.shp.000000");
            spritesheetExplosion = content.Load<Texture2D>("Spritesheets/newsh6.shp.000000");

            spriteBounds[(int)BomberSteeringState.Right].X = 2;
            spriteBounds[(int)BomberSteeringState.Right].Y = 168;
            spriteBounds[(int)BomberSteeringState.Right].Width = 20;
            spriteBounds[(int)BomberSteeringState.Right].Height = 28;

            spriteBounds[(int)BomberSteeringState.Straight].X = 49;
            spriteBounds[(int)BomberSteeringState.Straight].Y = 168;
            spriteBounds[(int)BomberSteeringState.Straight].Width = 22;
            spriteBounds[(int)BomberSteeringState.Straight].Height = 28;

            spriteBounds[(int)BomberSteeringState.Left].X = 98;
            spriteBounds[(int)BomberSteeringState.Left].Y = 168;
            spriteBounds[(int)BomberSteeringState.Left].Width = 20;
            spriteBounds[(int)BomberSteeringState.Left].Height = 28;

            spriteExplosionBounds[0].X = 74;
            spriteExplosionBounds[0].Y = 85;
            spriteExplosionBounds[0].Width = 12;
            spriteExplosionBounds[0].Height = 12;

            spriteExplosionBounds[1].X = 86;
            spriteExplosionBounds[1].Y = 85;
            spriteExplosionBounds[1].Width = 12;
            spriteExplosionBounds[1].Height = 12;

            spriteExplosionBounds[2].X = 96;
            spriteExplosionBounds[2].Y = 85;
            spriteExplosionBounds[2].Width = 12;
            spriteExplosionBounds[2].Height = 12;

            spriteExplosionBounds[3].X = 108;
            spriteExplosionBounds[3].Y = 85;
            spriteExplosionBounds[3].Width = 12;
            spriteExplosionBounds[3].Height = 12;

            spriteExplosionBounds[4].X = 120;
            spriteExplosionBounds[4].Y = 85;
            spriteExplosionBounds[4].Width = 12;

            spriteExplosionBounds[5].X = 132;
            spriteExplosionBounds[5].Y = 85;
            spriteExplosionBounds[5].Width = 12;
            spriteExplosionBounds[5].Height = 12;

            spriteExplosionBounds[6].X = 144;
            spriteExplosionBounds[6].Y = 85;
            spriteExplosionBounds[6].Width = 12;
            spriteExplosionBounds[6].Height = 12;

            spriteExplosionBounds[7].X = 156;
            spriteExplosionBounds[7].Y = 85;
            spriteExplosionBounds[7].Width = 12;
            spriteExplosionBounds[7].Height = 12;

            spriteExplosionBounds[8].X = 168;
            spriteExplosionBounds[8].Y = 85;
            spriteExplosionBounds[8].Width = 12;
            spriteExplosionBounds[8].Height = 12;

            spriteExplosionBounds[9].X = 180;
            spriteExplosionBounds[9].Y = 85;
            spriteExplosionBounds[9].Width = 12;
            spriteExplosionBounds[9].Height = 12;

            spriteExplosionBounds[10].X = 192;
            spriteExplosionBounds[10].Y = 85;
            spriteExplosionBounds[10].Width = 12;
            spriteExplosionBounds[10].Height = 12;

            spriteExplosionBounds[11].X = 204;
            spriteExplosionBounds[11].Y = 85;
            spriteExplosionBounds[11].Width = 12;
            spriteExplosionBounds[11].Height = 12;


            steeringState = BomberSteeringState.Straight;

            offset = globalOffset;
            globalOffset += 35;
            if (globalOffset >= 140) globalOffset = 0;
        }

        /// <summary>
        /// Updates the Dart ship
        /// </summary>
        /// <param name="elapsedTime">The in-game time between the previous and current frame</param>
        public override void Update(float elapsedTime)
        {

            if (isAlive == false)
            {
                explosionTimer += elapsedTime;
                if (explosionTimer > 0.05)
                {
                    explosionState=++explosionState%12 ;
                    //if(explosionState>11){ delete object
                    explosionTimer = 0;
                }

                return;
            }
            
            float velocity = 1;

            // Sense the player's position
            PlayerShip player = ScrollingShooterGame.Game.Player;
            Vector2 playerPosition = new Vector2(player.Bounds.Center.X, player.Bounds.Center.Y);
            //groupFireTimer += elapsedTime;
            bombTimer += elapsedTime;
            turboTimer += elapsedTime;
            //once in a while Bomber can turn on extra engine
            if (turboTimer > 7.5f)
            {
                velocity = 2f;
                if (turboTimer > 12f)
                {
                    turboTimer = 0;
                }
            }


            if (playerPosition.X - Bounds.Center.X < -20)
            {
                steeringState = BomberSteeringState.Left;
                this.position.X -= elapsedTime * 40 * velocity;
            }
            else
            {
                if (playerPosition.X - Bounds.Center.X > 20)
                {
                    steeringState = BomberSteeringState.Right;
                    this.position.X += elapsedTime * 40 * velocity;
                }
                else
                {
                    steeringState = BomberSteeringState.Straight;
                    if (playerPosition.Y > this.Bounds.Center.Y)
                    {
                        if (bombTimer > 1.5f)
                        {
                            ScrollingShooterGame.GameObjectManager.CreateProjectile(ProjectileType.EnemyBomb, position);
                            bombTimer = 0f;
                        }
                    }
                }


            }

            //if (groupFireTimer > (10f * bomberCount) && bombTimer > 1.5f)
            //{
            //    ScrollingShooterGame.GameObjectManager.CreateProjectile(ProjectileType.Bomb, position, false);
            //    bombTimer = 0f;
            //    if (groupFireTimer > (13f * bomberCount))
            //    {
            //        groupFireTimer = 0;
            //    }
            //}
            position.Y += elapsedTime * 30 * velocity;
        }

        /// <summary>
        /// Draw the Dart ship on-screen
        /// </summary>
        /// <param name="elapsedTime">The in-game time between the previous and current frame</param>
        /// <param name="spriteBatch">An already initialized SpriteBatch, ready for Draw() commands</param>
        public override void Draw(float elapsedTime, SpriteBatch spriteBatch)
        {
            if (isAlive)
            {
                spriteBatch.Draw(spritesheet, Bounds, spriteBounds[(int)steeringState], Color.White, MathHelper.Pi, new Vector2(0, 0), SpriteEffects.None, 0f);
            }
            else
            {
                spriteBatch.Draw(spritesheetExplosion, ExplosionBounds, spriteExplosionBounds[explosionState], Color.White, MathHelper.Pi, new Vector2(0, 0), SpriteEffects.None, 0f);
            }
        }

        public void collision()
        {
            isAlive = false;
            //bomberCount--;
        }

    }
}
