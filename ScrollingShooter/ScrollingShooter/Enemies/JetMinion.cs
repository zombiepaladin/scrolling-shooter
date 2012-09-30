//Jet Minion Enemy Class:
//Coders: Nicholas Boen
//Date: 9/8/2012
//Time: 11:09 A.M.
//
//The Jet Minion is a simple enemy that strafes a certain difference
//away from the player and attempts to shoot them down, after a certain
//amount of time the minion will leave. If the minion becomes damaged, 
//fire rate will increase

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using System;

enum JetMinionVisualState
{
    HEALTHY = 0,
    DAMAGED = 1,
    EXPLODING = 2,
    DEAD = 3
}


enum JetMinionBehaviourState
{
    IDLE = 0,
    SEEKSTRAFE = 1,
    ALERT = 2,
    STRAFE = 3,
    FIRE = 4,
    RETREAT = 5
}

enum GunState
{
    LEFT = 0,
    REST = 1,
    RIGHT = 2,
    DESTROYED = 3,
}
namespace ScrollingShooter
{
    //TODO:
    //1. Explosion class - define it an animation class
    //         These are a work in progress at the moment

    public class JetMinion : Enemy
    {
        #region Static and Constant Variables - For Tweaking some behaviour

        /// <summary>
        /// The distance away from the player on the y-axis that the Jet keeps
        /// </summary>
        private const int StrafeDistance = 150;

        /// <summary>
        /// The range at which the player needs to be before the Jet becomes alert
        /// </summary>
        private const int AlertRange = 400;

        /// <summary>
        /// The distance on the x-axis that a Jet needs to be before
        /// it will shoot
        /// </summary>
        private const int FireRange = 75;

        /// <summary>
        /// The time span(in seconds) before the Jet decides to leave
        /// </summary>
        private const int TotalLifeTime = 15;

        /// <summary>
        /// The total amount of time to spend displaying an explosion animation
        /// </summary>
        private const int TotalExplosionTime = 2;

        /// <summary>
        /// The amount of time to wait between frames in the Explosion animation
        /// </summary>
        private const float ExplosionIntervalTime = 0.06f;

        /// <summary>
        /// The sprite animation frames for the left side of the explosion
        /// </summary>
        private static Rectangle[] ExplosionLeftSpriteBounds =
        {
            new Rectangle(0,0,12,23),
            new Rectangle(12,0,12,23),
            new Rectangle(24,0,12,23),
            new Rectangle(36,0,12,23),
            new Rectangle(48,0,12,23),
            new Rectangle(60,0,12,23),
            new Rectangle(72,0,12,23),
            new Rectangle(84,0,12,23),
            new Rectangle(96,0,12,23),
            new Rectangle(108,0,12,23),
            new Rectangle(120,0,12,23),
            new Rectangle(132,0,12,23),
            new Rectangle(144,0,12,23),
            new Rectangle(156,0,12,23),
            new Rectangle(168,0,12,23),
            new Rectangle(180,0,12,23),
            new Rectangle(192,0,12,23)
        };

        /// <summary>
        /// The sprite animation frames for the right side of the explosion
        /// </summary>
        private static Rectangle[] ExplosionRightSpriteBounds =
        {
            new Rectangle(0,28,12,21),
            new Rectangle(12,28,12,21),
            new Rectangle(24,28,12,21),
            new Rectangle(36,28,12,21),
            new Rectangle(48,28,12,21),
            new Rectangle(60,28,12,21),
            new Rectangle(72,28,12,21),
            new Rectangle(84,28,12,21),
            new Rectangle(96,28,12,21),
            new Rectangle(108,28,12,21),
            new Rectangle(120,28,12,21),
            new Rectangle(132,28,12,21),
            new Rectangle(144,28,12,21),
            new Rectangle(156,28,12,21),
            new Rectangle(168,28,12,21),
            new Rectangle(180,28,12,21),
            new Rectangle(192,28,12,21)
        };

        /// <summary>
        /// The fire rate that the Jet has when at full health 
        /// </summary>
        private const float HealthyFireRate = 0.15f;

        /// <summary>
        /// The fire rate that the Jet has when in damaged state
        /// </summary>
        private const float DamagedFireRate = 0.08f;

        /// <summary>
        /// The maximum health that the Jet can have
        /// </summary>
        private const int FullHealth = 75;

        /// <summary>
        /// Maximum speed that a Jet may travel
        /// </summary>
        private const int MaxSpeed = 200;

        /// <summary>
        /// The amound of damage that the Jet can take before going to
        /// it's damaged State
        /// </summary>
        private const int DamageThreshold = 50;

        /// <summary>
        /// The position offset, from the Jets position, that projectiles will spawn from on the 
        /// left barrel
        /// </summary>
        private static Vector2 GunLeftBarrel = new Vector2(41, 75);

        /// <summary>
        /// The position offset, from the Jets position, that projectiles will spawn from on the
        /// right barrel
        /// </summary>
        private static Vector2 GunRightBarrel = new Vector2(48, 75);

        /// <summary>
        /// The location, relative to the origin of the ship, of the 
        /// gun barrels in the damaged state
        /// </summary>
        private static Vector2 GunSectionOffset = new Vector2(36, 53);

        /// <summary>
        /// The scale of the sprite
        /// </summary>
        private const float SpriteScale = 1f;
        
        /// <summary>
        /// The scale of the Explosion
        /// </summary>
        private const float ExplosionScale = 3f;

        /// <summary>
        /// This is temporary until we get something better in the game call, otherwise this just obfuscates code
        /// Represents the current Game screen
        /// </summary>
        private static Rectangle GameScreen = ScrollingShooterGame.Game.GraphicsDevice.Viewport.Bounds;

        #endregion

        #region Private Vars and States

        #region The States of the Jet

        /// <summary>
        /// Represents how the Jet will appear to the player
        /// </summary>
        private JetMinionVisualState _myVisualState;

        /// <summary>
        /// Represents how the Jet will behave in combat
        /// </summary>
        private JetMinionBehaviourState _myBehaviourState;

        /// <summary>
        /// Represents which state the barrel is in
        /// </summary>
        private GunState _myGunState;

        /// <summary>
        /// Represents the last barrel that the gun fired
        /// from (left or right)
        /// </summary>
        private GunState _oldGunState;

        #endregion

        #region The sprites for the jet and gun states

        /// <summary>
        /// An array indicating all of the sprite sheet locations
        /// for the visual states of the jet
        /// </summary>
        private Rectangle[] _spriteBounds = new Rectangle[4];

        /// <summary>
        /// An array indicating all of the sprite sheet locations
        /// for the visual states of the gun
        /// </summary>
        private Rectangle[] _spriteGun = new Rectangle[3];

        /// <summary>
        /// The sprite sheet with all of our sprites on it
        /// </summary>
        private Texture2D _spriteSheet;

        /// <summary>
        /// The sprite sheet with all of our explosion sprites on it
        /// </summary>
        private Texture2D _explosionSpriteSheet;

        #endregion

        #region Book Keeping Variables

        /// <summary>
        /// The current health of the Jet
        /// </summary>
        private int _health;

        /// <summary>
        /// The current velocity vector of this Jet
        /// </summary>
        private Vector2 _velocity;

        /// <summary>
        /// The position, relative to (0,0), of the Jet on the screen
        /// </summary>
        private Vector2 _position;

        /// <summary>
        /// The time left before the Jet will retreat
        /// </summary>
        private float _timerLife;

        /// <summary>
        /// The time left before the Jet can fire another shot
        /// </summary>
        private float _timerFire;

        /// <summary>
        /// Keeps track of how long to display the explosion animation
        /// </summary>
        private float _timerExplosion;

        /// <summary>
        /// Determines which direction the Jet should be strafing
        /// </summary>
        private bool _isMovingLeft;

        /// <summary>
        /// Handles the explosion animations
        /// </summary>
        private ExplodeAnim _explosionAnimator;

        #endregion

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets the current health of this Jet
        /// </summary>
        public int Health { get { return this._health; } }

        /// <summary>
        /// Gets or sets the current position of this Jet
        /// </summary>
        public Vector2 Position { get { return this._position; } set { _position = value; } }

        public override Rectangle Bounds
        {
            get
            {
                return new Rectangle((int)Position.X, (int)Position.Y,
                   (int)(_spriteBounds[(int)_myVisualState].Width * SpriteScale), (int)(_spriteBounds[(int)_myVisualState].Height * SpriteScale));
            }
        }

        #endregion

        #region Constructor

        /// <summary>
        /// Sets up the initialization
        /// </summary>
        /// <param name="id">The ID of this object</param>
        /// <param name="content">The Content Manager</param>
        /// <param name="position">The initial position of this Jet</param>
        public JetMinion(uint id, ContentManager content, Vector2 position)
            : base(id)
        {
            Initiailize(content, position);
        }

        /// <summary>
        /// Initializes the values of this Jet
        /// </summary>
        /// <param name="content"></param>
        /// <param name="position"></param>
        private void Initiailize(ContentManager content, Vector2 position)
        {

            //Setting Health
            _health = FullHealth;

            //Initializing States
            _myVisualState = JetMinionVisualState.HEALTHY;
            _myBehaviourState = JetMinionBehaviourState.IDLE;
            _myGunState = GunState.REST;
            _oldGunState = GunState.RIGHT;

            //Setting the initial position of the ship
            _position = position;

            //Setting up the initial direction
            _isMovingLeft = true;

            //Setting up the timers
            _timerLife = TotalLifeTime;
            _timerFire = HealthyFireRate;
            _timerExplosion = TotalExplosionTime;

            //Setting up the Explosion Animator
            _explosionAnimator = new ExplodeAnim(ExplosionType.SINGLE, 0.2f, new Rectangle((int)position.X, (int)position.Y, _spriteBounds[1].Width, _spriteBounds[1].Height));
            
            _spriteSheet = content.Load<Texture2D>("Spritesheets/newshi.shp.000000");
            _explosionSpriteSheet = content.Load<Texture2D>("Spritesheets/newsh6.shp.000000");

            #region Initializing _spriteBounds

            _spriteBounds[(int)JetMinionVisualState.HEALTHY].X = 98;
            _spriteBounds[(int)JetMinionVisualState.HEALTHY].Y = 143;
            _spriteBounds[(int)JetMinionVisualState.HEALTHY].Width = 93;
            _spriteBounds[(int)JetMinionVisualState.HEALTHY].Height = 80;

            _spriteBounds[(int)JetMinionVisualState.DAMAGED].X = 2;
            _spriteBounds[(int)JetMinionVisualState.DAMAGED].Y = 143;
            _spriteBounds[(int)JetMinionVisualState.DAMAGED].Width = 93;
            _spriteBounds[(int)JetMinionVisualState.DAMAGED].Height = 53;

            //This makes returning bounds easier, no check required
            _spriteBounds[(int)JetMinionVisualState.EXPLODING] = Rectangle.Empty;
            _spriteBounds[(int)JetMinionVisualState.DEAD] = Rectangle.Empty;

            #endregion

            #region Initializing _spriteGun

            _spriteGun[(int)GunState.LEFT].X = 62;
            _spriteGun[(int)GunState.LEFT].Y = 196;
            _spriteGun[(int)GunState.LEFT].Width = 21;
            _spriteGun[(int)GunState.LEFT].Height = 21;

            _spriteGun[(int)GunState.REST].X = 38;
            _spriteGun[(int)GunState.REST].Y = 196;
            _spriteGun[(int)GunState.REST].Width = 21;
            _spriteGun[(int)GunState.REST].Height = 20;

            _spriteGun[(int)GunState.RIGHT].X = 14;
            _spriteGun[(int)GunState.RIGHT].Y = 196;
            _spriteGun[(int)GunState.RIGHT].Width = 21;
            _spriteGun[(int)GunState.RIGHT].Height = 21;

            #endregion

        }

        #endregion

        #region Update and Draw

        /// <summary>
        /// The update method for the Jet, sets up the State Machine
        /// </summary>
        /// <param name="elapsedTime">The time elapsed since the last Update</param>
        public override void Update(float elapsedTime)
        {
            onFrame(elapsedTime);
        }

        /// <summary>
        /// The draw method for the Jet, draws the Jet to the screen if it is alive
        /// </summary>
        /// <param name="elapsedTime">The time elapsed since the last Draw</param>
        /// <param name="spriteBatch">The sprite batch to draw this Jet into</param>
        public override void Draw(float elapsedTime, SpriteBatch spriteBatch)
        {
            bool isInHealthyRetreat = (_myVisualState == JetMinionVisualState.HEALTHY) && (_myBehaviourState == JetMinionBehaviourState.RETREAT);
            bool isInDamagedRetreat = (_myVisualState == JetMinionVisualState.DAMAGED) && (_myBehaviourState == JetMinionBehaviourState.RETREAT);
            bool isDestroyedWhileRetreating = (_myVisualState == JetMinionVisualState.EXPLODING) && (_myBehaviourState == JetMinionBehaviourState.RETREAT);

            if (_myVisualState == JetMinionVisualState.HEALTHY || isInHealthyRetreat)
            {
                spriteBatch.Draw(_spriteSheet, _position, _spriteBounds[(int)_myVisualState], Color.White, 0f, Vector2.Zero, SpriteScale, SpriteEffects.None, 0);
            }
            else if (_myVisualState == JetMinionVisualState.DAMAGED || isInDamagedRetreat)
            {
                //The position of the gun turrets will change depending on the sprite scale, so
                //we need to take that into account

                Vector2 gunSectionPosition = _position + (GunSectionOffset * SpriteScale);

                //We need to draw both the Jet and the gun turrets in the damaged state

                spriteBatch.Draw(_spriteSheet, _position, _spriteBounds[(int)_myVisualState], Color.White, 0f, Vector2.Zero, SpriteScale, SpriteEffects.None, 0);
                spriteBatch.Draw(_spriteSheet, gunSectionPosition, _spriteGun[(int)_myGunState], Color.White, 0f, Vector2.Zero, SpriteScale, SpriteEffects.None, 0);
            }
            else if (_myVisualState == JetMinionVisualState.EXPLODING || isDestroyedWhileRetreating)
            {
                if (_timerExplosion <= 0)
                {
                    _explosionAnimator.StopAndFinish();
                    _timerExplosion = TotalExplosionTime;
                }
                else
                {
                    _timerExplosion -= elapsedTime;
                }

                _explosionAnimator.Draw(elapsedTime, spriteBatch);
            }

        }

        #endregion

        #region Methods that alter health

        /// <summary>
        /// Deals damage to this Jet
        /// </summary>
        /// <param name="amount">The amount of damage to deal</param>
        public void DamageMe(int amount)
        {
            _health -= amount;
        }

        /// <summary>
        /// Heals this Jet
        /// </summary>
        /// <param name="amount">The amount to heal</param>
        public void HealMe(int amount)
        {
            _health += amount;
        }

        #endregion

        //All methods below this are private

        #region Sensor Checks (and subsequent transitions)

        /// <summary>
        /// Checks if this Jet is On screen, if so then Alert it
        /// </summary>
        private void checkIfOnScreen()
        {
            switch (_myBehaviourState)
            {
                //Only need to do this if in the IDLE state
                case JetMinionBehaviourState.IDLE:

                    //I realize I *might* be able to do this with the Intersects() method with
                    //the sprite bounds and Game screen, but I'm not sure if that will work
                    //and I know that if I do do that, I will just have to change it later -_-

                    //Check if sprite is past left and top sides
                    if ((Position.X + Bounds.Width) > 0 && (Position.Y + Bounds.Height) > 0)
                    {
                        //Check if the sprite has not passed the right or bottom sides
                        if (Position.X < GameScreen.Width && Position.Y < GameScreen.Height)
                        {
                            _myBehaviourState = JetMinionBehaviourState.ALERT;
                        }
                    }
                    break;
            }

        }

        /// <summary>
        /// Checks if the Jet can sense the player yet
        /// </summary>
        private void checkIfInAlertRange(Vector2 playerPosition)
        {
            switch (_myBehaviourState)
            {
                //Only need to do this in the ALERT state
                case JetMinionBehaviourState.ALERT:

                    //Gets the vector from the player to this jet and gets its length (squared for efficiency) 
                    //and compares it with the alert range (squared)
                    if ((Position - playerPosition).LengthSquared() <= (AlertRange * AlertRange))
                    {
                        _myBehaviourState = JetMinionBehaviourState.SEEKSTRAFE;
                    }

                    break;
            }
        }

        /// <summary>
        /// Checks if this Jet is in strafing range of the player, 
        /// if so then begin strafing
        /// </summary>
        private void checkIfInStrafingRange(Vector2 playerPosition)
        {
            switch (_myBehaviourState)
            {
                case JetMinionBehaviourState.SEEKSTRAFE:

                    //StrafeDistance is just a distance between the tip of the Jet
                    //and the player position, so no fancy math here

                    float distanceAway = playerPosition.Y - (Position.Y + Bounds.Height);

                    if (distanceAway > StrafeDistance - 5 && distanceAway < StrafeDistance + 5)
                    {
                        _myBehaviourState = JetMinionBehaviourState.STRAFE;
                    }

                    break;
            }
        }

        /// <summary>
        /// Checks if this Jet is within firing range of the player,
        /// if so then begin firing
        /// </summary>
        private void checkIfInFiringRange(Vector2 playerPosition, int playerWidth)
        {
            switch (_myBehaviourState)
            {
                //Only need to check when in Fire or Strafe states
                case JetMinionBehaviourState.STRAFE:

                    if (Math.Abs((playerPosition.X + playerWidth * 0.5) - (Position.X + Bounds.Width * 0.5)) <= FireRange)
                    {
                        _myBehaviourState = JetMinionBehaviourState.FIRE;
                        onEntry();
                    }

                    break;

                case JetMinionBehaviourState.FIRE:

                    if (Math.Abs(playerPosition.X - Position.X) > FireRange)
                    {
                        _myBehaviourState = JetMinionBehaviourState.STRAFE;
                    }

                    break;
            }
        }

        /// <summary>
        /// Checks if this Jet has taken a lot of damage (indicated by DamageThreshold),
        /// if so then change the sprite to a more damaged one
        /// </summary>
        private void checkIfDamaged()
        {
            switch (_myVisualState)
            {
                //Really only need to check this if the current visual state is Healthy

                case JetMinionVisualState.HEALTHY:

                    if (Health < DamageThreshold)
                    {
                        _myVisualState = JetMinionVisualState.DAMAGED;
                    }

                    break;
            }
        }

        /// <summary>
        /// Checks if this Jets guns are ready to fire,
        /// if so then fire a shot
        /// </summary>
        private bool checkIfGunReady()
        {
            switch (_myBehaviourState)
            {

                case JetMinionBehaviourState.FIRE:

                    if (_timerFire <= 0)
                    {
                        if (_myVisualState == JetMinionVisualState.HEALTHY)
                            _timerFire = HealthyFireRate;
                        else
                            _timerFire = DamagedFireRate;

                        return true;
                    }

                    break;

            }

            return false;
        }

        /// <summary>
        /// Checks if this Jets life time is up,
        /// if so then start retreating
        /// </summary>
        private void checkIfLifeTimeOver()
        {
            switch (_myVisualState)
            {
                //Need to check this for both Healthy and Damaged states
                case JetMinionVisualState.HEALTHY:
                case JetMinionVisualState.DAMAGED:

                    if (_timerLife <= 0)
                    {
                        _myBehaviourState = JetMinionBehaviourState.RETREAT;
                    }

                    break;
            }
        }

        /// <summary>
        /// Checks if this Jet is even alive,
        /// if not then initiate death
        /// </summary>
        private void checkIfIsAlive()
        {
            //Only needs to occur in these two states since the transition between healthy and damaged is based on 
            //health as well

            if (_myVisualState == JetMinionVisualState.DAMAGED ||
                _myBehaviourState == JetMinionBehaviourState.RETREAT)
            {
                if (_health <= 0)
                {
                    _myVisualState = JetMinionVisualState.EXPLODING;
                    onEntry();
                }
            }
        }

        /// <summary>
        /// Checks to see if the Jet that was killed is finished exploding
        /// if so, transition to the dead state
        /// </summary>
        private bool checkIfFinishedExploding()
        {
            //TODO:
            //Will probably do some call to the explosion anim class here

            if (_explosionAnimator.IsStopped)
            {
                _myVisualState = JetMinionVisualState.DEAD;

                return true;
            }
            return false;
        }

        #endregion

        #region Actuators

        private void Idle()
        {
            _velocity = Vector2.Zero;
        }

        /// <summary>
        /// Handles when the Jet is just flying through the screen
        /// If the player stays out of the alert range, it is possible for
        /// this enemy to just leave without engaging, this is intentional
        /// </summary>
        private void Alert()
        {
            switch (_myBehaviourState)
            {
                case JetMinionBehaviourState.ALERT:
                    _velocity = (new Vector2(0, 1)) * MaxSpeed;
                    break;
            }
        }

        /// <summary>
        /// Handles when the Jet is getting ready to strafe the character
        /// </summary>
        private void SeekStrafe(Vector2 playerPosition)
        {
            switch (_myBehaviourState)
            {
                case JetMinionBehaviourState.SEEKSTRAFE:

                    Vector2 targetPosition = new Vector2(playerPosition.X, playerPosition.Y - StrafeDistance);
                    Vector2 directionToTarget = targetPosition - Position;
                    directionToTarget.Normalize();

                    _velocity = directionToTarget * MaxSpeed;

                    if (directionToTarget.X > 0)
                    {
                        _isMovingLeft = false;
                    }
                    else
                    {
                        _isMovingLeft = true;
                    }

                    break;
            }
        }

        /// <summary>
        /// Handles the Strafing action that the ship takes (i.e. moving left and right
        /// across the screen)
        /// </summary>
        private void Strafe(Vector2 playerPosition)
        {
            switch (_myBehaviourState)
            {
                case JetMinionBehaviourState.STRAFE:
                case JetMinionBehaviourState.FIRE:

                    float currentX = MathHelper.Clamp(Position.X, GameScreen.Left, (GameScreen.Right - this.Bounds.Width));

                    _position.Y = playerPosition.Y - StrafeDistance - Bounds.Height;

                    if (_isMovingLeft)
                    {
                        _velocity = (new Vector2(-1, 0)) * MaxSpeed;

                        if (currentX == GameScreen.Left)
                        {
                            _velocity = (new Vector2(1, 0)) * MaxSpeed;

                            _isMovingLeft = false;
                        }
                    }
                    else
                    {
                        _velocity = (new Vector2(1, 0)) * MaxSpeed;

                        if (currentX == (GameScreen.Right - this.Bounds.Width))
                        {
                            _velocity = (new Vector2(-1, 0)) * MaxSpeed;

                            _isMovingLeft = true;
                        }
                    }

                    break;
            }
        }

        /// <summary>
        /// Handles the Firing action when in the firing state
        /// </summary>
        private void Fire()
        {
            switch (_myBehaviourState)
            {

                case JetMinionBehaviourState.FIRE:

                    if (_oldGunState == GunState.LEFT)
                    {
                        _myGunState = GunState.RIGHT;

                        ScrollingShooterGame.GameObjectManager.CreateProjectile(ProjectileType.JetMinionBullet, Position + (GunRightBarrel * SpriteScale));

                        _oldGunState = GunState.RIGHT;
                    }
                    else
                    {
                        _myGunState = GunState.LEFT;

                        ScrollingShooterGame.GameObjectManager.CreateProjectile(ProjectileType.JetMinionBullet, Position + (GunLeftBarrel * SpriteScale));

                        _oldGunState = GunState.LEFT;
                    }

                    break;
            }
        }

        /// <summary>
        /// Handles the retreating action
        /// </summary>
        private void Retreat()
        {
            switch (_myBehaviourState)
            {
                case JetMinionBehaviourState.RETREAT:

                    _velocity = (new Vector2(0, 1)) * MaxSpeed;

                    break;
            }
        }

        /// <summary>
        /// Handles the explosion action
        /// </summary>
        private void Explode()
        {
            switch (_myVisualState)
            {
                case JetMinionVisualState.EXPLODING:

                    //TODO:
                    //Explosion logic here

                    break;
            }
        }

        #endregion

        #region State Methods (these handle initial state transitions and behaviour)

        /// <summary>
        /// This gets called whenever there is a transition to a new state
        /// </summary>
        private void onEntry()
        {
            //These are, so far, the only two states that need an onEntry method,
            //but we'll leave the structure here in case more is needed

            switch (_myBehaviourState)
            {
                case JetMinionBehaviourState.FIRE:

                    _timerFire = 0;

                    break;

                default:
                    break;
            }

            switch (_myVisualState)
            {
                case JetMinionVisualState.EXPLODING:

                    //TODO:
                    //This will probably insantiate the animation class, this
                    //way it is only called once when this state is entered

                    Vector2 explosionSize = new Vector2(ExplosionLeftSpriteBounds[0].Width, ExplosionRightSpriteBounds[0].Height) * ExplosionScale;

                    Rectangle explosionBounds = new Rectangle(0, 0, (int)explosionSize.X, (int)explosionSize.Y);

                    AnimSet explosionLeftSide = new AnimSet(ExplosionLeftSpriteBounds, _explosionSpriteSheet, explosionBounds, AnimType.LOOP, ExplosionIntervalTime);
                    AnimSet explosionRightSide = new AnimSet(ExplosionRightSpriteBounds, _explosionSpriteSheet, explosionBounds, AnimType.LOOP, ExplosionIntervalTime);

                    ExplosionSet newExplosion = new ExplosionSet(_position);

                    newExplosion.AddAnimSet(explosionLeftSide, Vector2.Zero);
                    newExplosion.AddAnimSet(explosionRightSide, new Vector2(explosionBounds.Width, 0) * ExplosionScale);

                    _explosionAnimator.AddExplosion(newExplosion, Vector2.Zero);

                    _explosionAnimator.Start();
                    break;

                default:
                    break;
            }
        }

        /// <summary>
        /// This gets called be default, handles the behaviour and appearence of the
        /// Jet as it remains in its current state
        /// </summary>
        /// <param name="elapsedTime">Time since last update, mostly for managing timers</param>
        private void onFrame(float elapsedTime)
        {
            Vector2 playerPosition = new Vector2(ScrollingShooterGame.Game.Player.Bounds.X, ScrollingShooterGame.Game.Player.Bounds.Y);
            int playerWidth = ScrollingShooterGame.Game.Player.Bounds.Width;

            if (_myVisualState == JetMinionVisualState.HEALTHY || _myVisualState == JetMinionVisualState.DAMAGED)
            {
                switch (_myBehaviourState)
                {
                    case JetMinionBehaviourState.IDLE:

                        Idle();

                        checkIfOnScreen();

                        break;

                    case JetMinionBehaviourState.ALERT:

                        Alert();

                        checkIfInAlertRange(playerPosition);

                        break;

                    case JetMinionBehaviourState.SEEKSTRAFE:

                        SeekStrafe(playerPosition);

                        checkIfInStrafingRange(playerPosition);

                        break;

                    case JetMinionBehaviourState.STRAFE:

                        Strafe(playerPosition);

                        checkIfInFiringRange(playerPosition, playerWidth);

                        break;

                    case JetMinionBehaviourState.FIRE:

                        if (checkIfGunReady())
                        {
                            Fire();
                        }
                        else
                        {
                            _myGunState = GunState.REST;
                        }

                        Strafe(playerPosition);

                        checkIfInFiringRange(playerPosition, playerWidth);

                        _timerFire -= elapsedTime;

                        break;

                    case JetMinionBehaviourState.RETREAT:

                        Retreat();

                        checkIfIsAlive();

                        break;

                    default:
                        break;
                }
            }

            //We don't do anything specific based on the visual state except change some
            //sprites around, except for the exploding state, so there is no need
            //to include their empty cases here
            switch (_myVisualState)
            {
                case JetMinionVisualState.HEALTHY:

                    checkIfDamaged();
                    checkIfLifeTimeOver();

                    _timerLife -= elapsedTime;

                    break;

                case JetMinionVisualState.DAMAGED:

                    checkIfLifeTimeOver();
                    checkIfIsAlive();

                    _timerLife -= elapsedTime;

                    break;

                case JetMinionVisualState.EXPLODING:

                    if (!checkIfFinishedExploding())
                    {
                        Explode();
                    }

                    break;

                default:
                    break;
            }

            _position += _velocity * elapsedTime;
        }

        #endregion

    }
}