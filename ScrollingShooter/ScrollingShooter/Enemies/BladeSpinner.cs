//BladeSpinner.cs by Matthew Hart
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace ScrollingShooter
{
    /// <summary>
    /// enum for the BladeSpinner state
    /// </summary>
    enum spinnerState
    {
        flying,
        attacking,
        dying,
        dead
    }


    /// <summary>
    /// Enum for the particular frame of the BladeSpinner's animation
    /// </summary>
    enum animationFrame
    {
        flying1,
        flying2,
        flying3,
        flying4,
        attacking1,
        attacking2,
        dying1,
        dying2,
        dying3,
        dying4,
        dying5,
    }

    /// <summary>
    /// enum to control flight direction while the BladeSpinner is patroling
    /// </summary>
    enum flightDirection
    {
        negative = -1,
        positive = 1,
    }

    /// <summary>
    /// An enemy ship that with patrol the area around the screen until destroyed
    /// It will attempt to fly into and damage the player.
    /// </summary>
    class BladeSpinner : Enemy
    {
        /// <summary>
        /// Variables for the sprite.
        /// </summary>
        Texture2D spritesheet;
        Vector2 position;
        Vector2 patrolVector;
        Rectangle[] spriteBounds = new Rectangle[11];
        spinnerState state;
        animationFrame frame;
        flightDirection horDir;
        flightDirection vertDir;
        float frameTimer;

        /// <summary>
        /// Bounding Rectangle of the BladeSpinner sprite
        /// The spinner gets bigger while attacking
        /// </summary>
        public override Rectangle Bounds
        {
            get
            {
                if (state == spinnerState.attacking)
                    return new Rectangle((int)position.X, (int)position.Y, (int)(spriteBounds[(int)frame].Width * 1.5), (int)(spriteBounds[(int)frame].Height * 1.5));
                else
                    return new Rectangle((int)position.X, (int)position.Y, spriteBounds[(int)frame].Width, spriteBounds[(int)frame].Height);
            }
        }

        /// <summary>
        /// Creates a new instance of the BladeSpinner Class
        /// </summary>
        /// <param name="id">the ID for the base enemy class</param>
        /// <param name="content">A ContentManager to load resources with</param>
        /// <param name="position">The position of the Dart ship in the game world</param>
        public BladeSpinner(uint id, ContentManager content, Vector2 position)
            : base(id)
        {
            spritesheet = content.Load<Texture2D>("Spritesheets/newsh#.shp.000000");

            //Moves frame initialization to a separate function to make the constructor cleaner
            initializeFrames();

            this.position = position;

            //initial directiion and state settings
            horDir = flightDirection.positive;
            vertDir = flightDirection.positive;
            patrolVector = new Vector2(50, 1);

            state = spinnerState.flying;
            frame = animationFrame.flying1;

            frameTimer = 0;
        }

        /// <summary>
        /// Updates the blade spinner
        /// </summary>
        /// <param name="elapsedTime">The in-game time between the previous and current frame</param>
        public override void Update(float elapsedTime)
        {
            //If the spinner is dead do nothing
            if (state == spinnerState.dead)
                return;

            //"Sense" the player ship
            PlayerShip player = ScrollingShooterGame.Game.Player;

            //-40 is so the Spinner actually covers the ship instead of menacing its back right corner
            Vector2 playerPosition = new Vector2(player.Bounds.Center.X-40, player.Bounds.Center.Y-40);
            // Get a vector from our position to the player's position
            Vector2 toPlayer = playerPosition - this.position;

            //Conditionals to keep the spinner on screen by adjusting the patrol vector
            //A switch statement may be more efficient
            if (horDir == flightDirection.positive && this.position.X > 725)
            {
                horDir = flightDirection.negative;
            }
            else if (horDir == flightDirection.negative && this.position.X < 25)
            {
                horDir = flightDirection.positive;
            }
            if (vertDir == flightDirection.negative && this.position.Y < 25)
            {
                vertDir = flightDirection.positive;
            }
            else if (vertDir == flightDirection.positive && this.position.Y > 400)
            {
                vertDir = flightDirection.negative;
            }
            patrolVector.X = ((int)horDir * 50);
            patrolVector.Y = ((int)vertDir) * 5;
            //End of conditionals to stay on screen


            //Determine if the player is "seen"
            if (toPlayer.LengthSquared() < 30000 && state != spinnerState.dying)
            {
                //if seen attack player
                state = spinnerState.attacking;
            }
            else if (state != spinnerState.dying)
            {
                state = spinnerState.flying;
            }

            switch (state)
            {
                case spinnerState.flying:
                    //normalize the patrol vector
                    patrolVector.Normalize();

                    //move
                    this.position += patrolVector * elapsedTime * 250;
                    break;
                case spinnerState.attacking:
                    // Get a normalized steering vector
                    toPlayer.Normalize();

                    // Steer towards them
                    this.position += toPlayer * elapsedTime * 125;
                    break;
                case spinnerState.dying:
                    //No movement while dying
                    break;
            }

            //Collision detection?
            /*
             * if(state==spinnerState.attacking)
             * {
             *     if(hitPlayer)
             *          damage player
             *     
             *    if(hitProjectile)
             *    {
             *          take a little damage
             *          if(hp==0)
             *              state==spinnerState.dying;
             *     }
             * }
             * else if(state==spinnerState.flying)
             *{
             *      if(hitPorjectile)
             *      {
             *          take more damage
             *          if(hp==0)
             *              state==spinnerState.dying;
             *      }
             *      if(hitPlayer)
             *      {
             *          //Should never happen
             *          damage player a little
             *          state = spinnerState.dying;
             *      }
             *}
             *else if(state==spinnerState.dying)
             *{
             *      if(hit Object)
             *      {
             *          do "med" damage
             *      }
             *}
             */
        }


        /// <summary>
        /// Draw the BladeSpinner on screen
        /// </summary>
        /// <param name="elapsedTime">The in-game time between the previous and current frame</param>
        /// <param name="spriteBatch">An already initialized SpriteBatch, ready for Draw() commands</param>
        public override void Draw(float elapsedTime, SpriteBatch spriteBatch)
        {
            //If the spinner is dead there is nothing to draw
            if (state == spinnerState.dead)
                return;

            //slow down the animation using the frame timer
            frameTimer += elapsedTime;

            //conditions to determine the animation frames to use.
            //There is likely a much more efficient way to do this
            if (frameTimer > .05f)
            {
                switch (state)
                {
                    case spinnerState.flying:
                        frameTimer = 0;
                        switch (frame)
                        {
                            case animationFrame.flying1:
                            case animationFrame.flying2:
                            case animationFrame.flying3:
                                frame++;
                                break;
                            default:
                                frame = 0;
                                break;
                        }
                        break;
                    case spinnerState.attacking:
                        frameTimer = 0;
                        switch (frame)
                        {
                            case animationFrame.attacking1:
                                frame++;
                                break;
                            default:
                                frame = animationFrame.attacking1;
                                break;
                        }
                        break;
                    case spinnerState.dying:
                        //slow down dying annimation
                        frameTimer = -.1f;
                        switch (frame)
                        {
                            case animationFrame.dying1:
                            case animationFrame.dying2:
                            case animationFrame.dying3:
                            case animationFrame.dying4:
                                frame++;
                                break;
                            case animationFrame.dying5:
                                state = spinnerState.dead;
                                break;
                            default:
                                frame = animationFrame.dying1;
                                break;
                        }
                        break;
                }
            }
            spriteBatch.Draw(spritesheet, Bounds, spriteBounds[(int)frame], Color.White);

        }

        /// <summary>
        /// Initializes all the different frames for animation
        /// Clalled from the constructor
        /// </summary>
        private void initializeFrames()
        {
            //flying frames
            spriteBounds[(int)animationFrame.flying1].X = 0;
            spriteBounds[(int)animationFrame.flying1].Y = 3;
            spriteBounds[(int)animationFrame.flying1].Width = 48;
            spriteBounds[(int)animationFrame.flying1].Height = 48;

            spriteBounds[(int)animationFrame.flying2].X = 49;
            spriteBounds[(int)animationFrame.flying2].Y = 3;
            spriteBounds[(int)animationFrame.flying2].Width = 48;
            spriteBounds[(int)animationFrame.flying2].Height = 48;

            spriteBounds[(int)animationFrame.flying3].X = 96;
            spriteBounds[(int)animationFrame.flying3].Y = 3;
            spriteBounds[(int)animationFrame.flying3].Width = 48;
            spriteBounds[(int)animationFrame.flying3].Height = 48;

            spriteBounds[(int)animationFrame.flying4].X = 144;
            spriteBounds[(int)animationFrame.flying4].Y = 3;
            spriteBounds[(int)animationFrame.flying4].Width = 48;
            spriteBounds[(int)animationFrame.flying4].Height = 48;

            //attacking frames
            spriteBounds[(int)animationFrame.attacking1].X = 48;
            spriteBounds[(int)animationFrame.attacking1].Y = 115;
            spriteBounds[(int)animationFrame.attacking1].Width = 48;
            spriteBounds[(int)animationFrame.attacking1].Height = 48;

            spriteBounds[(int)animationFrame.attacking2].X = 96;
            spriteBounds[(int)animationFrame.attacking2].Y = 115;
            spriteBounds[(int)animationFrame.attacking2].Width = 48;
            spriteBounds[(int)animationFrame.attacking2].Height = 48;

            //dying frames
            spriteBounds[(int)animationFrame.dying1].X = 0;
            spriteBounds[(int)animationFrame.dying1].Y = 59;
            spriteBounds[(int)animationFrame.dying1].Width = 48;
            spriteBounds[(int)animationFrame.dying1].Height = 48;

            spriteBounds[(int)animationFrame.dying2].X = 49;
            spriteBounds[(int)animationFrame.dying2].Y = 59;
            spriteBounds[(int)animationFrame.dying2].Width = 48;
            spriteBounds[(int)animationFrame.dying2].Height = 48;

            spriteBounds[(int)animationFrame.dying3].X = 96;
            spriteBounds[(int)animationFrame.dying3].Y = 59;
            spriteBounds[(int)animationFrame.dying3].Width = 48;
            spriteBounds[(int)animationFrame.dying3].Height = 48;

            spriteBounds[(int)animationFrame.dying4].X = 144;
            spriteBounds[(int)animationFrame.dying4].Y = 59;
            spriteBounds[(int)animationFrame.dying4].Width = 48;
            spriteBounds[(int)animationFrame.dying4].Height = 48;

            spriteBounds[(int)animationFrame.dying5].X = 0;
            spriteBounds[(int)animationFrame.dying5].Y = 116;
            spriteBounds[(int)animationFrame.dying5].Width = 48;
            spriteBounds[(int)animationFrame.dying5].Height = 48;
        }
    }
}
