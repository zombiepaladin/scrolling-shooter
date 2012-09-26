//Homing Missile Projectile Class:
//Coders: Nicholas Boen
//Date: 9/1/2012
//Time: 11:25 P.M.

/*Class Diagram***************************************************
 *                   Homing Missile Projectile                   *
 *---------------------------------------------------------------*
 * -isAlive             : bool                                   *
 * -speed               : int                                    *
 * -lockedTarget        : Vector2                                *
 * -targetVectorScalar  : Vector2                                *
 *---------------------------------------------------------------*
 * +HomingMissileProjectile(ContentManager, Vector2, short)      *
 * +HomingMissileProjectile(ContentManager, Vector2, short, int) *
 * +Update(float) : void                                         *
 * +Draw(float, Spritebatch) : void                              *
 * +RecycleClass(ContentManager, Vector2, short) : void          *
 * +RecycleClass(ContentManager, Vector2, short, int) : void     *
 * +Kill() : void                                                *
 * -AdjustRotation() : void                                      *
 * -Initialize(ContentManager, Vector2, short, int) : void       *
 * -TrackTarget(Vector2) : void                                  *
 *****************************************************************/

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using System;
using Microsoft.Xna.Framework.Input;

namespace ScrollingShooter
{
    //TODO:
    // 1. Add Explosion - may need explosion class
    // 2. Add smoke trail - may need smoke class
    //     NOTE: explosion and smoke would be animation classes, so that could be a base
    // 3. Add enemy target lock - still need to get in an enemy ship
    // 4. Rotate missiles along their trajectory

    /// <summary>
    /// The Class representing a single Homing Missile
    /// </summary>
    public class HomingMissileProjectile : Projectile
    {
        #region Private Fields

        /// <summary>
        /// Marks whether or not this missile has been destroyed or not
        /// </summary>
        private bool isAlive;

        /// <summary>
        /// This holds the speed or our missile
        /// otherwise known as the magnitude of our velocity
        /// This is necessary if I'm going to implement an arced
        /// trajectory as I'll need a way to clamp the velocity values
        /// </summary>
        private int speed;

        /// <summary>
        /// This will be the position of our target
        /// Ideally this will be an enemy ship rather than a vector
        /// since the enemy will likely be moving, but this is for testing
        /// </summary>
        private Vector2 lockedTarget;

        /// <summary>
        /// This is the scalar that changes how severely missiles react to their targeting.
        /// The higher the number, the faster the response
        /// </summary>
        private int targetVectorScalar;

        private float rotation;

        #endregion

        #region Constructors

        /// <summary>
        /// This constructor is good when making a single call, if making multiple constructor calls
        /// (i.e. in a loop) then use the other constructor. Creates a new missile.
        /// </summary>
        /// <param name="content">A ContentManager to load content from</param>
        /// <param name="newPosition">A position on the screen</param>
        /// <param name="missileLevel">The level of the missile powerup when this missile was shot</param>
        public HomingMissileProjectile(ContentManager content, Vector2 newPosition, short missileLevel, uint id):base(id)
        {
            Initialize(content, newPosition, missileLevel, 1);
        }

        /// <summary>
        /// An overridden constructor that ceates a new missile and requires a seed value, use this
        /// when making quick successive calls to the constructor (the interative variable makes a 
        /// good seed modifier)
        /// </summary>
        /// <param name="content">A ContentManager to load content from</param>
        /// <param name="newPosition">A position on the screen</param>
        /// <param name="missileLevel">The level of the missile powerup when this missile was shot</param>
        /// <param name="randomSeedModifier">Indicates the modifier to the random seed for calculating position, 1 is default</param>
        public HomingMissileProjectile(ContentManager content, Vector2 newPosition, short missileLevel, int randomSeedModifier, uint id):base(id)
        {
            Initialize(content, newPosition, missileLevel, randomSeedModifier);
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Empties out lists and such within the class and sets it up to be reused as opposed to being destroyed
        /// and another instance created, this is the simple one without the seed modifier
        /// </summary>
        /// <param name="content">A ContentManager to load content from</param>
        /// <param name="newPosition">A position on the screen</param>
        /// <param name="missileLevel">The level of the missile powerup when this missile was shot</param>
        public void RecycleClass(ContentManager content, Vector2 newPosition, short missileLevel)
        {
            Initialize(content, newPosition, missileLevel, 1);
        }

        /// <summary>
        /// Empties out lists and such within the class and sets it up to be reused as opposed to being destroyed
        /// and another instance created, requires a seed modifier for multiple consecutive calls, otherwise use the other
        /// RecycleClass() post-constructor
        /// </summary>
        /// <param name="content">A ContentManager to load content from</param>
        /// <param name="newPosition">A position on the screen</param>
        /// <param name="missileLevel">The level of the missile powerup when this missile was shot</param>
        /// <param name="randomSeedModifier">Indicates the modifier to the random seed for calculating position, 1 is default</param>
        public void RecycleClass(ContentManager content, Vector2 newPosition, short missileLevel, int randomSeedModifier)
        {
            Initialize(content, newPosition, missileLevel, randomSeedModifier);
        }

        public void Kill()
        {
            this.isAlive = false;

            this.position = Vector2.Zero;
            this.velocity = Vector2.Zero;
            this.speed = 0;

            //TODO: 
            //1. Manage anything else that needs to be adjusted
            //2. Populate/Initialize the Explosion animation class

        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Does the work you would normally find in the Constructor. This keeps the constructors small and flexible and easy
        /// to override
        /// </summary>
        /// <param name="content">A ContentManager to load content from</param>
        /// <param name="newPosition">A position on the screen</param>
        /// <param name="missileLevel">The level of the missile powerup when this missile was shot</param>
        /// <param name="randomSeedModifier">Indicates the modifier to the random seed for calculating position, 1 is default</param>
        private void Initialize(ContentManager content, Vector2 newPosition, short missileLevel, int randomSeedModifier)
        {
            Random r = new Random(System.DateTime.Now.Millisecond * randomSeedModifier);

            int spawnBoxWidth = 40;
            int spawnBoxHeight = 20;

            //There will be a 'box' of 40x40 whose bottom is centered on the given position
            //a point will be randomly selected inside this box and will represent the spawn position of the missile
            Vector2 positionModifier = new Vector2(r.Next(0, spawnBoxWidth), r.Next(0, spawnBoxHeight));

            //The left side of our pseudo-box
            float spawnBoxLeft = newPosition.X - spawnBoxWidth / 2;

            //The bottom of our pseudo-box
            float spawnBoxBottom = newPosition.Y;

            //and now we will set the position of our missile
            this.position = new Vector2(spawnBoxLeft + positionModifier.X, spawnBoxBottom - positionModifier.Y);


            //Setting the Sprite sheet of our missile (all sprites are on one sheet)
            this.spriteSheet = content.Load<Texture2D>("Spritesheets/tyrian.shp.000000");

            //Determining which sprite to use and the speed of this missile and the targetVectorScalar based on level
            if (missileLevel == 2)
            {
                this.spriteBounds = new Rectangle(134, 42, 9, 14);
                this.speed = 300;
                this.targetVectorScalar = 25;
            }
            else if (missileLevel == 3)
            {
                this.spriteBounds = new Rectangle(218, 56, 8, 14);
                this.speed = 420;
                this.targetVectorScalar = 50;
            }
            else
            {
                this.spriteBounds = new Rectangle(159, 29, 7, 13);
                this.speed = 150;
                this.targetVectorScalar = 10;
            }

            //Setting the initial velocity - This will change when we implement
            //enemy ships so I can find a target and begin that way
            this.velocity = new Vector2(0, -1 * speed);

            //Make sure to mark this instance as alive before we push it into the world
            this.isAlive = true;

            //We'll initialize the rotation to 0.0, don't worry, it'll get changed later
            rotation = 0f;

        }

        /// <summary>
        /// This function alters the flight path of the missile to track its target in real time
        /// </summary>
        /// <param name="targetPosition">The current position of the target</param>
        private void TrackTarget(Vector2 targetPosition)
        {
            Vector2 vectorToTarget;

            //First we need to find the vector that points to our target
            vectorToTarget = targetPosition - this.position;

            //Next we need to convert that to a unit vector, which points in the direction of our target
            vectorToTarget.Normalize();

            //We can use this unit vector to adjust our current velocity
            this.velocity += (vectorToTarget * this.targetVectorScalar);

            //However, we need to be careful that velocity does not exceed our max (this.speed)
            //To do this we'll first convert our velocity into a unit vector
            this.velocity.Normalize();

            //And then scale our velocity to our speed
            this.velocity = this.speed * this.velocity;

            //now we have an adjusted velocity
        }

        /// <summary>
        /// Rotates the sprite image to match the missiles trajectory for a nice realistic effect
        /// </summary>
        private void AdjustRotation()
        {
            //Pretty simple, tan(theta) = opposite / adjacent right? same thing here
            float angleBetween = (float)Math.Atan2((double)velocity.Y, (double)velocity.X);

            //Since our graphic is originally facing up at vector (0,-1), we need to offset it a little
            rotation = angleBetween + MathHelper.PiOver2;
        }

        #endregion

        #region Update and Draw

        /// <summary>
        /// Updates the properties of the missile (i.e. velocity, position, target position, etc.)
        /// </summary>
        /// <param name="elapsedTime">The time passed since the last update</param>
        public override void Update(float elapsedTime)
        {
            //This is what the actual target assignment may look like, but will be done in the constructor
            //and a reference to the target stored in the class
            //lockedTarget = new Vector2(ScrollingShooterGame.Game.powerups[0].Bounds.Left, ScrollingShooterGame.Game.powerups[0].Bounds.Top);

            MouseState current_mouse = Mouse.GetState();
            lockedTarget = new Vector2(current_mouse.X, current_mouse.Y);

            if (this.isAlive)
            {
                this.TrackTarget(lockedTarget);
                //Will need to call AdjustRotation() here
                this.AdjustRotation();
            }
            //Will need to call the Update function for the explosion and smoke stream classes here (explosion only if this.isAlive == false)

            base.Update(elapsedTime);
        }

        /// <summary>
        /// Draws the missile to the screen
        /// </summary>
        /// <param name="elapsedTime">The time passed since the last draw</param>
        /// <param name="spriteBatch">The spritebatch to include this call in</param>
        public override void Draw(float elapsedTime, SpriteBatch spriteBatch)
        {
            //Will need to call the Draw function for the explosion and smoke stream classes here (explosion only if this.isAlive == false)
            if (isAlive)
            {
                //need to do our own draw function if we are to include rotation
                spriteBatch.Draw(spriteSheet, position, spriteBounds, Color.White, rotation, new Vector2(spriteBounds.Width / 2, spriteBounds.Y / 2), 1f, SpriteEffects.None, 0);
            }
        }

        #endregion

    }
}
