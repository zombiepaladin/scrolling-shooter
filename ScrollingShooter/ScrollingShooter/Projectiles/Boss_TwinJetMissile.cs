//Twin Jet Missile Class:
//Coders: Nicholas Boen
//Date: 9/17/2012
//Time: 3:51 P.M.
//
//The homing missile that the Twin Jet boss
//Fires

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using System;

namespace ScrollingShooter
{
    class Boss_TwinJetMissile : Projectile
    {
        #region Constants

        /// <summary>
        /// The Maximum speed any missile can travel
        /// </summary>
        private const int MaxSpeed = 250;

        /// <summary>
        /// The amount of time (in seconds) that the missile
        /// will live for before exploding (let's chalk it up
        /// to 'out of gas' for now)
        /// </summary>
        private const int MissileLife = 2;

        /// <summary>
        /// The scalar value that alters how quickly the missiles
        /// 'snap' to their target vectors, the higher the value
        /// the tighter they will turn
        /// </summary>
        private const int HomingVectorScalar = 15;

        #endregion

        #region Personal Variables

        /// <summary>
        /// A flag marking whether this missile is alive or not
        /// </summary>
        private bool isAlive;

        /// <summary>
        /// The time left before the missile explodes
        /// (runs out of fuel)
        /// </summary>
        private float timerMissileLife;

        /// <summary>
        /// The angle (in radians) at which to rotate
        /// the missile
        /// </summary>
        private float rotation;

        /// <summary>
        /// A reference to the player, just used to access
        /// the players position for tracking
        /// </summary>
        private PlayerShip player;

        #endregion

        /// <summary>
        /// Constructs a brand new, fully feuled missile ready
        /// to blow some smug pilot to pieces
        /// </summary>
        /// <param name="id">The factory id of this missile</param>
        /// <param name="content">The Content Manager used to get Textures</param>
        /// <param name="position">The position to spawn this missile at</param>
        public Boss_TwinJetMissile(uint id, ContentManager content, Vector2 position)
            : base(id)
        {
            //Set a slight time modifier so that the missiles don't explode all in groups
            //makes it a bit more realistic
            Random timeModifier = new Random((int)(DateTime.Now.Millisecond * id));

            //The sprite sheet containing our missile texture
            this.spriteSheet = content.Load<Texture2D>("Spritesheets/tyrian.shp.000000");

            //The box denoting the sprite texture on the sprite sheet
            this.spriteBounds = new Rectangle(156, 42, 12, 14);

            //Setting the initial velocity of our missile
            this.velocity = new Vector2(0, 1 * MaxSpeed);

            //Setting the initial position of our missile
            this.position = position;

            //Setting the reference to our brave, hopeless hero
            this.player = ScrollingShooterGame.Game.player;

            //Makes sure that, upon creation, this missile is 'Still Alive'
            this.isAlive = true;

            //Sets the initial rotation to 0
            rotation = 0;

            //Sets the Life timer to the max and applies the modifier
            timerMissileLife = MissileLife + (float)(timeModifier.NextDouble() + timeModifier.Next(-1,1));
        }

        /// <summary>
        /// Sets the missiles velocity to angle toward our smug player
        /// </summary>
        private void TrackPlayer()
        {
            //This will keep track of the vector pointing toward the player
            Vector2 directionToPlayer;

            //Setting the direction by subtracting the vectors (end - start)
            directionToPlayer = this.player.GetPosition() - this.position;

            //Normalizing the direction, we only need a unit vector
            directionToPlayer.Normalize();

            //Alters the velocity and moves it toward the player, using the scalar
            this.velocity += directionToPlayer * HomingVectorScalar;

            //Normalizes the vector so we can change it's magnitude
            this.velocity.Normalize();

            //Now we can set the magnitude to our max speed
            this.velocity = this.velocity * MaxSpeed;

        }

        /// <summary>
        /// This Adjusts the rotation of our missile, so its more realistic
        /// </summary>
        private void AdjustRotation()
        {
            //The angle between the components of the velocity vector (tan(theta) = opp / adjacent)
            float angleBetween = (float)Math.Atan2((double)velocity.Y, (double)velocity.X);

            //setting the rotation to the angle, plus our offset since the sprite is initially upright
            rotation = angleBetween + MathHelper.PiOver2;
        }

        /// <summary>
        /// The update method that occurs every update cycle
        /// </summary>
        /// <param name="elapsedTime">The elapsed time since the last call</param>
        public override void Update(float elapsedTime)
        {
            //Don't need to do anything if the missile is dead
            if (!isAlive)
                return;

            //Check if the missile has failed his comrades
            if (this.timerMissileLife <= 0)
            {
                //this missile has failed, refill his timer to remind him
                this.timerMissileLife = MissileLife;

                //Make sure he knows he's dead
                this.isAlive = false;

                //And make him explode, there shall be no quarter for failure
                ScrollingShooterGame.GameObjectManager.CreateExplosion(this.ID);
            }
            else
            {
                //Remind this missile of his mission and duty to the mothership
                this.timerMissileLife -= elapsedTime;

                //Make sure he can track his target
                TrackPlayer();

                //Make sure he's facing the right direction
                AdjustRotation();

                //and finally send him on his way to glory
                this.position += this.velocity * elapsedTime;
            }
        }

        /// <summary>
        /// The draw method that occurs every draw cycle
        /// </summary>
        /// <param name="elapsedTime">The elapsed time since the last call</param>
        /// <param name="spriteBatch">The sprite batch to include this draw into</param>
        public override void Draw(float elapsedTime, SpriteBatch spriteBatch)
        {
            //No need for special treatment if this missile has failed
            if (isAlive)
            {
                //Draw this missile in all of the engineering glory of the mothership
                spriteBatch.Draw(this.spriteSheet, this.position, this.spriteBounds, Color.White, rotation, new Vector2(spriteBounds.Width / 2, spriteBounds.Y / 2), 1f, SpriteEffects.None, 0);
            }
        }

    }
}
