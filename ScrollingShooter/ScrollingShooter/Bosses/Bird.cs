using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace ScrollingShooter
{
    public enum birdEntranceState
    {
        flyOver = 0,
        enter = 1,
        normal = 2,
        exit = 3,
    }

    public enum LaserState
    {
        off = 0,
        turningOn = 1,
        on = 2,
        turningOff = 3,
    }

    class Bird : Enemy
    {
        // Dart state variables
        Texture2D spritesheet;
        Vector2 position;

        Vector2 beakPosition;
        Vector2 LeftLaserPosition;
        Vector2 RightLaserPosition;

        Rectangle[] spriteBounds = new Rectangle[1];
        Rectangle[] spriteBoundsAnim = new Rectangle[4];
        Rectangle[] spriteBoundsLaser = new Rectangle[3];

        DroneLaser LeftLaser;
        DroneLaser RightLaser;


        int headAnimState = 0;
        bool openingBeak = true;
        static int NUMBER_OF_ANIM_STATES = 4;

        float headAnimTimer = 0f;
        float fireTimer = 3.0f;

        int laserAnimState = 0;
        static int NUMBER_OF_ANIM_STATES_LASER = 3;

        float laserAnimTimer = 0;
        float laserTimer = 0f;
        bool laserStatus = false;
        LaserState laserState = 0;

        birdEntranceState BES;


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
            get 
            {
                if (BES == birdEntranceState.flyOver)
                {
                    return new Rectangle((int)position.X + (spriteBounds[0].Width / 2) - (spriteBoundsAnim[0].Width / 2), (int)position.Y - spriteBounds[0].Height + 58, spriteBoundsAnim[0].Width, spriteBoundsAnim[0].Height);
                }
                return new Rectangle((int)position.X + (spriteBounds[0].Width / 2) - (spriteBoundsAnim[0].Width / 2), (int)position.Y + spriteBounds[0].Height, spriteBoundsAnim[0].Width, spriteBoundsAnim[0].Height);
            }
        }

        public Rectangle LaserLeftBounds
        {
            get
            {
                if (BES == birdEntranceState.flyOver)
                {
                    return new Rectangle((int)position.X + (spriteBounds[0].Width / 2 - 49) - (spriteBoundsLaser[0].Width / 2), (int)position.Y + spriteBounds[0].Height - 22 - 58, spriteBoundsLaser[0].Width, spriteBoundsLaser[0].Height);
                }
                return new Rectangle((int)position.X + (spriteBounds[0].Width / 2 - 49) - (spriteBoundsLaser[0].Width / 2), (int)position.Y + spriteBounds[0].Height - 22, spriteBoundsLaser[0].Width, spriteBoundsLaser[0].Height);
            }
        }
        public Rectangle LaserRightBounds
        {
            get
            {
                if (BES == birdEntranceState.flyOver)
                {
                    return new Rectangle((int)position.X + (spriteBounds[0].Width / 2 + 49) - (spriteBoundsLaser[0].Width / 2), (int)position.Y + spriteBounds[0].Height - 22 - 58, spriteBoundsLaser[0].Width, spriteBoundsLaser[0].Height);
                }
                return new Rectangle((int)position.X + (spriteBounds[0].Width / 2 + 49) - (spriteBoundsLaser[0].Width / 2), (int)position.Y + spriteBounds[0].Height - 22, spriteBoundsLaser[0].Width, spriteBoundsLaser[0].Height);
            }
        }
        /// <summary>
        /// Creates a new instance of a Bird boss 
        /// </summary>
        /// <param name="content">A ContentManager to load resources with</param>
        /// <param name="position">The position of the Bird boss in the game world</param>
        public Bird(uint id, ContentManager content, Vector2 position)
            : base(id)
        {
            this.position = new Vector2(300, 800);
            //ScrollingShooterGame.LevelManager.Scrolling = false;
                
            this.position = position;
            spritesheet = content.Load<Texture2D>("Spritesheets/newsh0.shp.000001");

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

            spriteBoundsLaser[0].X = 123;
            spriteBoundsLaser[0].Y = 84;
            spriteBoundsLaser[0].Width = 19;
            spriteBoundsLaser[0].Height = 23;

            spriteBoundsLaser[1].X = 147;
            spriteBoundsLaser[1].Y = 84;
            spriteBoundsLaser[1].Width = 19;
            spriteBoundsLaser[1].Height = 23;

            spriteBoundsLaser[2].X = 195;
            spriteBoundsLaser[2].Y = 84;
            spriteBoundsLaser[2].Width = 19;
            spriteBoundsLaser[2].Height = 23;

            beakPosition.X = (spriteBounds[0].Width / 2) + 8;
            beakPosition.Y = spriteBounds[0].Height + spriteBoundsAnim[0].Height;

            LeftLaserPosition.X = (spriteBounds[0].Width / 2) + 49;
            LeftLaserPosition.Y = spriteBounds[0].Height + spriteBoundsLaser[0].Height - 30;

            RightLaserPosition.X = (spriteBounds[0].Width / 2) - 49;
            RightLaserPosition.Y = spriteBounds[0].Height + spriteBoundsLaser[0].Height - 30;

            LeftLaser = (DroneLaser)ScrollingShooterGame.GameObjectManager.CreateProjectile(ProjectileType.DroneLaser, LeftLaserPosition);
            LeftLaser.isOn = laserStatus;
            LeftLaser.laserPower = WeaponChargeLevel.Low;
            RightLaser = (DroneLaser)ScrollingShooterGame.GameObjectManager.CreateProjectile(ProjectileType.DroneLaser, RightLaserPosition);
            RightLaser.isOn = laserStatus;
            RightLaser.laserPower = WeaponChargeLevel.Low;
            BES = birdEntranceState.flyOver;

        }

        /// <summary>
        /// Updates the Dart ship
        /// </summary>
        /// <param name="elapsedTime">The in-game time between the previous and current frame</param>
        public override void Update(float elapsedTime)
        {
            switch (BES)
            {
                case birdEntranceState.flyOver:
                    this.position.Y -= elapsedTime * 500;

                    if (position.Y <= 3892) // -200
                    {
                        BES = birdEntranceState.enter;
                    }
                    break;
                case birdEntranceState.enter:
                    this.position.Y += elapsedTime * 100;
                    if (position.Y >= 0)
                    {
                        BES = birdEntranceState.normal;
                    }
                    ScrollingShooterGame.LevelManager.Scrolling = false;
                    break;
                case birdEntranceState.normal:
                    fireTimer += elapsedTime;
                    laserTimer += elapsedTime;

                    float velocity = 1;
                    // Sense the player's position
                    PlayerShip player = ScrollingShooterGame.Game.Player;
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

                    //Firein Mah Lazars!!
                    switch (laserState)
                    {
                        case LaserState.off:
                            if (laserTimer > 3f)
                            {
                                laserState = LaserState.turningOn;
                                laserAnimState = 0;
                            }
                            break;
                        case LaserState.turningOn:
                            laserAnimState++;
                            if (laserAnimState == NUMBER_OF_ANIM_STATES_LASER - 1)
                            {
                                laserState = LaserState.on;
                                laserTimer = 0f;
                            }
                            break;
                        case LaserState.on:
                            LeftLaser.isOn = true;
                            RightLaser.isOn = true;
                            LeftLaser.updatePosition(position.X + LeftLaserPosition.X, position.Y + LeftLaserPosition.Y);
                            RightLaser.updatePosition(position.X + RightLaserPosition.X, position.Y + RightLaserPosition.Y);
                            if (laserTimer > 3f)
                            {
                                LeftLaser.isOn = false;
                                RightLaser.isOn = false;
                                laserState = LaserState.turningOff;
                            }
                            break;
                        case LaserState.turningOff:
                            laserAnimState--;
                            if (laserAnimState == 0)
                            {
                                laserState = LaserState.off;
                                laserTimer = 0f;
                            }
                            break;

                    }
                    break;
                case birdEntranceState.exit:
                    this.position.Y += elapsedTime * 500;
                    ScrollingShooterGame.LevelManager.Scrolling = true;
                    break;
            }
        }

        /// <summary>
        /// Draw the Dart ship on-screen
        /// </summary>
        /// <param name="elapsedTime">The in-game time between the previous and current frame</param>
        /// <param name="spriteBatch">An already initialized SpriteBatch, ready for Draw() commands</param>
        public override void Draw(float elapsedTime, SpriteBatch spriteBatch)
        {
            headAnimTimer += elapsedTime;
            laserAnimTimer += elapsedTime;
            if (BES == birdEntranceState.flyOver)
            {
                spriteBatch.Draw(spritesheet, Bounds, spriteBounds[0], Color.White, 0, new Vector2(), SpriteEffects.FlipVertically, 0);
                spriteBatch.Draw(spritesheet, HeadBounds, spriteBoundsAnim[headAnimState], Color.White, 0, new Vector2(), SpriteEffects.FlipVertically, 0);
                spriteBatch.Draw(spritesheet, LaserLeftBounds, spriteBoundsLaser[0], Color.White, 0, new Vector2(), SpriteEffects.FlipVertically, 0);
                spriteBatch.Draw(spritesheet, LaserRightBounds, spriteBoundsLaser[0], Color.White, 0, new Vector2(), SpriteEffects.FlipVertically, 0);
            }

            else
            {
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
                    if (headAnimState == NUMBER_OF_ANIM_STATES - 1)
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
                spriteBatch.Draw(spritesheet, LaserLeftBounds, spriteBoundsLaser[laserAnimState], Color.White);
                spriteBatch.Draw(spritesheet, LaserRightBounds, spriteBoundsLaser[laserAnimState], Color.White);
            }
        }

        public void collision()
        {
            ScrollingShooterGame.LevelManager.Scrolling = true;
        }
    }

  
}
