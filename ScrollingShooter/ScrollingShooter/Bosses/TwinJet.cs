//Twin Jet Class:
//Coders: Nicholas Boen
//Date: 9/16/2012
//Time: 6:32 P.M.
//
//The Twin Jet Boss

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

//TODO:
//1. Implement rotation of the jet toward the player (may need rotation Matrix)

namespace ScrollingShooter
{
    /// <summary>
    /// The Visual States of the boss
    /// </summary>
    enum TwinJetVisualState
    {
        HEALTHY = 0,
        DAMAGED,
        GUNDESTROYED,
        EXPLODING,
        DEAD
    }

    /// <summary>
    /// The Behavioural States of the boss
    /// </summary>
    enum TwinJetBehaviouralState
    {
        INTRO = 0,
        FIGHT,
        PAUSED
    }

    /// <summary>
    /// The Fire States of the boss
    /// </summary>
    enum TwinJetFireState
    {
        STAGE1 = 0,
        STAGE2,
        STAGE3
    }

    class TwinJet : Enemy
    {
        #region Twin Jet Variables

        #region Static vars for tweaking

        /// <summary>
        /// Affects how quickly the jets fix to their
        /// next nodes, the higher the value, the tighter
        /// turns they will make
        /// </summary>
        private const int HomingVectorScalar = 100;

        /// <summary>
        /// The fire rate that the Jet has when at full health 
        /// </summary>
        private const float HealthyGunFireRate = 0.35f;

        /// <summary>
        /// The Missile Fire Rate in the Healthy State,
        /// no missiles fire, so it is marked with -1
        /// </summary>
        private const float HealthyMissileFireRate = -1f;

        /// <summary>
        /// The fire rate that the Jet has when in damaged state
        /// </summary>
        private const float DamagedGunFireRate = 0.25f;

        /// <summary>
        /// The Missile Fire Rate in the Damaged state
        /// </summary>
        private const float DamagedMissileFireRate = 2f;

        /// <summary>
        /// The Gun Fire rate when the gun has been lost,
        /// obviously -1, so no bullets
        /// </summary>
        private const float GunLostGunFireRate = -1f;
        
        /// <summary>
        /// The missile fire rate after the gun is lost
        /// </summary>
        private const float GunLostMissileFireRate = 1.5f;

        /// <summary>
        /// The maximum health that the Jet can have
        /// </summary>
        private const int FullHealth = 75;

        /// <summary>
        /// Maximum speed that a Jet may travel
        /// </summary>
        private const int MaxHealthySpeed = 200;

        /// <summary>
        /// The max speed when the jet is damaged
        /// </summary>
        private const int MaxDamagedSpeed = 300;

        /// <summary>
        /// The mas speed when the guns have
        /// been lost
        /// </summary>
        private const int MaxGunLostSpeed = 400;

        /// <summary>
        /// The amound of damage that the Jet can take before going to
        /// it's damaged State
        /// </summary>
        private const int DamageThreshold = 50;

        /// <summary>
        /// The damage threshold before the guns
        /// are lost
        /// </summary>
        private const int GunLossThreshold = 25;

        /// <summary>
        /// The position offset, from the Jets position, that projectiles will spawn from on the 
        /// left barrel
        /// </summary>
        private static Vector2 GunLeftBarrel = new Vector2(43, 75);

        /// <summary>
        /// The position offset, from the Jets position, that projectiles will spawn from on the
        /// right barrel
        /// </summary>
        private static Vector2 GunRightBarrel = new Vector2(50, 75);

        /// <summary>
        /// The position offset, from the Jets position, that missiles will spawn from on the 
        /// left barrel
        /// </summary>
        private static Vector2 MissileLeftBarrel = new Vector2(30, 54);

        /// <summary>
        /// The position offset, from the Jets position, that missiles will spawn from on the
        /// right barrel
        /// </summary>
        private static Vector2 MissileRightBarrel = new Vector2(62, 54);

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

        #region This Jet's States

        /// <summary>
        /// The visual state of this jet
        /// </summary>
        private TwinJetVisualState _myVisualState;

        /// <summary>
        /// The behaviour state of this jet
        /// </summary>
        private TwinJetBehaviouralState _myBehaviorState;

        /// <summary>
        /// The fire state of this jet
        /// </summary>
        private TwinJetFireState _myFireState;

        /// <summary>
        /// The current gun state of this jet
        /// </summary>
        private GunState _myGunState;

        /// <summary>
        /// The previous gun state of this jet
        /// </summary>
        private GunState _oldGunState;

        /// <summary>
        /// The current flight pattern of this jet
        /// </summary>
        private BossFlightPattern _myPattern;

        #endregion

        #region Sprite Related Variables

        /// <summary>
        /// The sprite sheet this jet is on
        /// </summary>
        private Texture2D _spriteSheet;

        /// <summary>
        /// The bounds of the healthy, damaged, and gun lost
        /// states
        /// </summary>
        private Rectangle[] _spriteBounds = new Rectangle[5];

        /// <summary>
        /// The bounds of the left, rest, right, and destroyed
        /// gun textures
        /// </summary>
        private Rectangle[] _spriteGunBounds = new Rectangle[4];

        #endregion

        #region Personal Variables

        /// <summary>
        /// The current position
        /// </summary>
        private Vector2 _position;

        /// <summary>
        /// The current velocity
        /// </summary>
        private Vector2 _velocity;

        /// <summary>
        /// The current max speed
        /// </summary>
        private int _myMaxSpeed;

        /// <summary>
        /// The current rotation
        /// </summary>
        private float _myRotation;

        /// <summary>
        /// The current max gun fire rate
        /// </summary>
        private float _myGunFireRate;

        /// <summary>
        /// The current max missile fire rate
        /// </summary>
        private float _myMissileFireRate;

        /// <summary>
        /// The time before this jet can fire
        /// another bullet
        /// </summary>
        private float _timerGunFireRate;

        /// <summary>
        /// the time before this jet can fire
        /// another missile
        /// </summary>
        private float _timerMissileFireRate;

        /// <summary>
        /// A flag determining whether this jet
        /// is alive or not
        /// </summary>
        private bool _isAlive;

        /// <summary>
        /// A flag indicating whether the introduction
        /// sequence is over or not
        /// </summary>
        private bool _isIntroOver;

        #endregion

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets a Vector2 of the center of the ship
        /// </summary>
        public Vector2 CenterPosition { get { return new Vector2(_position.X + Bounds.Width, _position.Y + Bounds.Height); } }

        /// <summary>
        /// Getsa nd sets the current (top left) position of the sprite
        /// </summary>
        public Vector2 Position { get { return this._position; } set { _position = value; } }

        /// <summary>
        /// Gets and sets the current Velocity of this jet
        /// </summary>
        public Vector2 Velocity { get { return this._velocity; } set { Vector2 newVector = value; newVector.Normalize(); _velocity = newVector * _myMaxSpeed; } }

        /// <summary>
        /// Gets and sets the current rotation of this jet
        /// </summary>
        public float Rotation { get { return _myRotation; } set { _myRotation = value; } }

        /// <summary>
        /// Gets the bounds of this ship
        /// </summary>
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
        /// Creates a shiny new jet
        /// </summary>
        /// <param name="id">the factory id on the underside of this jet</param>
        /// <param name="content">The content manager to use</param>
        /// <param name="newPosition">The position to spawn the jet</param>
        public TwinJet(uint id, ContentManager content, Vector2 newPosition)
            : base(id)
        {
            //Setting the health
            this.Health = FullHealth;

            //Setting alive to 'true'
            _isAlive = true;

            //Initializing the states
            _myVisualState = TwinJetVisualState.HEALTHY;
            _myBehaviorState = TwinJetBehaviouralState.INTRO;
            _myFireState = TwinJetFireState.STAGE1;
            _myGunState = GunState.REST;
            _oldGunState = GunState.LEFT;

            //Initializing rotation
            _myRotation = 0;

            //Setting the flight pattern to null
            _myPattern = null;

            //Setting position and velocity
            _position = newPosition;
            _velocity = Vector2.Zero;

            //Marking the intro as still occurring
            _isIntroOver = false;

            //Setup firerates
            _timerGunFireRate = 0;
            _timerMissileFireRate = 0;

            //Bring in the spritesheet
            _spriteSheet = content.Load<Texture2D>("Spritesheets/newshi.shp.000000");

            //Setting up Sprite Bounds for the jet and gun
            _spriteBounds[(int)TwinJetVisualState.HEALTHY] = new Rectangle(98, 143, 93, 80);
            _spriteBounds[(int)TwinJetVisualState.DAMAGED] = new Rectangle(2, 143, 93, 53);
            _spriteBounds[(int)TwinJetVisualState.GUNDESTROYED] = new Rectangle(2, 143, 93, 53);
            _spriteBounds[(int)TwinJetVisualState.EXPLODING] = Rectangle.Empty;
            _spriteBounds[(int)TwinJetVisualState.DEAD] = Rectangle.Empty;

            _spriteGunBounds[(int)GunState.LEFT] = new Rectangle(62, 196, 21, 21);
            _spriteGunBounds[(int)GunState.REST] = new Rectangle(38, 196, 21, 20);
            _spriteGunBounds[(int)GunState.RIGHT] = new Rectangle(14, 196, 21, 21);
            _spriteGunBounds[(int)GunState.DESTROYED] = new Rectangle(38, 196, 21, 11);

            //Calls the on entry method to initialize some things
            onEntry();
        }

        #endregion

        #region Update and Draw

        /// <summary>
        /// The update method that gets called each update cycle
        /// </summary>
        /// <param name="elapsedTime">The time elapsed since the last call</param>
        public override void Update(float elapsedTime)
        {
            //Calls the onFrame method
            onFrame(elapsedTime);
        }

        /// <summary>
        /// The draw method that gets called each draw cycle
        /// </summary>
        /// <param name="elapsedTime">The time elapsed since the last call</param>
        /// <param name="spriteBatch">The sprite batch to draw things into</param>
        public override void Draw(float elapsedTime, SpriteBatch spriteBatch)
        {
            //No need to do anything if this jet is dead
            if (!_isAlive)
                return;

            //See what all we need to draw
            if (_myVisualState == TwinJetVisualState.HEALTHY)
            {
                //Jet is healthy, only need to draw the jet body
                spriteBatch.Draw(_spriteSheet, _position, _spriteBounds[(int)_myVisualState], Color.White, _myRotation, Vector2.Zero, SpriteScale, SpriteEffects.None, 0);
            }
            else if (_myVisualState == TwinJetVisualState.DAMAGED || _myVisualState == TwinJetVisualState.GUNDESTROYED)
            {
                //Jet is damaged so we need to draw the body and the gun section

                Vector2 gunSectionPosition = _position + (GunSectionOffset * SpriteScale);

                spriteBatch.Draw(_spriteSheet, _position, _spriteBounds[(int)_myVisualState], Color.White, _myRotation, Vector2.Zero, SpriteScale, SpriteEffects.None, 0);

                spriteBatch.Draw(_spriteSheet, gunSectionPosition, _spriteGunBounds[(int)_myGunState], Color.White, _myRotation, Vector2.Zero, SpriteScale, SpriteEffects.None, 0);
            }

        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Sets the current flight pattern to the new pattern
        /// </summary>
        /// <param name="newPattern">The new pattern to use</param>
        public void SetPattern(BossFlightPattern newPattern)
        {
            _myPattern = newPattern;

            onEntry();
        }

        /// <summary>
        /// Changes the firing stage that this jet is at if necessary
        /// </summary>
        /// <param name="newState">The new state to enter</param>
        public void SetFireStage(TwinJetFireState newState)
        {
            _myFireState = newState;
        }

        /// <summary>
        /// Notifies this jet that it's intro is completed
        /// </summary>
        public void StopIntro()
        {
            _isIntroOver = true;
        }

        /// <summary>
        /// Damages this jet for the amount passed in
        /// </summary>
        /// <param name="amount">The amount of damage to deal</param>
        public void DamageMe(int amount)
        {
            Health -= amount;
        }
        
        #endregion

        #region Sensors

        /// <summary>
        /// Checks if the intro for this jet is over
        /// </summary>
        private void checkIfIntroIsOver()
        {
            if (_isIntroOver)
            {
                _myBehaviorState = TwinJetBehaviouralState.FIGHT;

                onEntry();
            }
        }

        /// <summary>
        /// Checks if this jet is damaged yet
        /// </summary>
        private void checkIfDamaged()
        {
            switch (_myVisualState)
            {

                case TwinJetVisualState.HEALTHY:
                    if (this.Health <= DamageThreshold)
                    {
                        _myVisualState = TwinJetVisualState.DAMAGED;

                        onEntry();
                    }
                    break;
            }
        }

        /// <summary>
        /// checks if this jet still has its guns
        /// </summary>
        private void checkIfGunDestroyed()
        {
            switch (_myVisualState)
            {

                case TwinJetVisualState.DAMAGED:
                    if (this.Health <= GunLossThreshold)
                    {
                        _myVisualState = TwinJetVisualState.GUNDESTROYED;

                        onEntry();
                    }
                    break;
            }
        }

        /// <summary>
        /// Check if this jet is even alive
        /// </summary>
        private void checkIfIsAlive()
        {
            switch (_myVisualState)
            {

                case TwinJetVisualState.GUNDESTROYED:
                    if (this.Health <= 0)
                    {
                        _isAlive = false;
                        _myVisualState = TwinJetVisualState.EXPLODING;

                        onEntry();
                    }
                    break;
            }
        }

        /// <summary>
        /// Check if this jet is ready to fire a bullet
        /// </summary>
        /// <returns>True if ready to fire</returns>
        private bool checkIfGunIsReady()
        {
            switch (_myBehaviorState)
            {

                case TwinJetBehaviouralState.FIGHT:
                    if (_timerGunFireRate <= 0)
                    {
                        _timerGunFireRate = _myGunFireRate;

                        return true;
                    }
                    break;
            }

            return false;
        }

        /// <summary>
        /// Check if this jet is ready to fire a missile
        /// </summary>
        /// <returns>True if ready to fire</returns>
        private bool checkIfMissilesReady()
        {
            switch (_myBehaviorState)
            {

                case TwinJetBehaviouralState.FIGHT:
                    if (_timerMissileFireRate <= 0)
                    {
                        _timerMissileFireRate = _myMissileFireRate;

                        return true;
                    }
                    break;
            }

            return false;
        }

        #endregion

        #region Actuators

        /// <summary>
        /// Handles flying for this jet
        /// </summary>
        private void Fly()
        {
            Vector2 vectorToTarget;

            //Cannot do anything if there is no pattern
            if (_myPattern != null)
            {
                //See if we are close enough to the node to get the
                //next one
                if (_myPattern.CheckProximity(CenterPosition))
                {
                    _myPattern.NextNode();
                }

                //Vector to the next node
                vectorToTarget = _myPattern.GetDirectionToNode(CenterPosition);

                //Normalize for unit vector
                vectorToTarget.Normalize();

                //Run some checks and make sure Velocity never registers as {NAN, NAN}
                //I had some troubles with this in earlier testing =\
                if (!float.IsNaN(_velocity.X) && !float.IsNaN(_velocity.Y))
                    this._velocity += (vectorToTarget * HomingVectorScalar);
                else
                    this._velocity = (vectorToTarget * HomingVectorScalar);

                this._velocity.Normalize();

                //Set the velocity to a magnitude of the mas speed
                this._velocity = _myMaxSpeed * this._velocity;
            }
        }

        /// <summary>
        /// Handles firing the gun for this jet
        /// </summary>
        private void FireGun()
        {
            switch (_myBehaviorState)
            {
                case TwinJetBehaviouralState.FIGHT:

                    //See which barrel should be firing

                    if (_oldGunState == GunState.LEFT)
                    {
                        _myGunState = GunState.RIGHT;

                        ScrollingShooterGame.GameObjectManager.CreateProjectile(ProjectileType.TwinJetBullet, _position + (GunRightBarrel * SpriteScale));

                        _oldGunState = GunState.RIGHT;
                    }
                    else
                    {
                        _myGunState = GunState.LEFT;

                        ScrollingShooterGame.GameObjectManager.CreateProjectile(ProjectileType.TwinJetBullet, _position + (GunLeftBarrel * SpriteScale));

                        _oldGunState = GunState.LEFT;
                    }

                    break;
            }
        }

        /// <summary>
        /// Handles firing missiles for this jet
        /// </summary>
        private void FireMissiles()
        {
            switch (_myBehaviorState)
            {
                case TwinJetBehaviouralState.FIGHT:

                    ScrollingShooterGame.GameObjectManager.CreateProjectile(ProjectileType.TwinJetMissile, _position + (MissileLeftBarrel * SpriteScale));
                    ScrollingShooterGame.GameObjectManager.CreateProjectile(ProjectileType.TwinJetMissile, _position + (MissileRightBarrel * SpriteScale));

                    break;
            }
        }

        /// <summary>
        /// Handles exploding for this jet
        /// </summary>
        private void Explode()
        {
            ScrollingShooterGame.GameObjectManager.CreateExplosion(this.ID);
        }

        #endregion

        #region State Management Methods

        /// <summary>
        /// Called when the overall state of this jet is changed
        /// </summary>
        private void onEntry()
        {
            //Mostly setting variables to appropriate values
            //or calling some helper functions
            switch (_myVisualState)
            {
                case TwinJetVisualState.HEALTHY:

                    _myMaxSpeed = MaxHealthySpeed;
                    _myGunFireRate = HealthyGunFireRate;
                    _myMissileFireRate = HealthyMissileFireRate;
                    _myFireState = TwinJetFireState.STAGE1;

                    break;

                case TwinJetVisualState.DAMAGED:

                    _myMaxSpeed = MaxDamagedSpeed;
                    _myGunFireRate = DamagedGunFireRate;
                    _myMissileFireRate = DamagedMissileFireRate;
                    _myFireState = TwinJetFireState.STAGE2;

                    break;

                case TwinJetVisualState.GUNDESTROYED:

                    _myMaxSpeed = MaxGunLostSpeed;
                    _myGunFireRate = GunLostGunFireRate;
                    _myMissileFireRate = GunLostMissileFireRate;
                    _myFireState = TwinJetFireState.STAGE3;
                    _myGunState = GunState.DESTROYED;

                    break;

                case TwinJetVisualState.EXPLODING:
                    _myBehaviorState = TwinJetBehaviouralState.PAUSED;
                    break;
            }

            switch (_myBehaviorState)
            {
                case TwinJetBehaviouralState.FIGHT:
                    _myPattern.SetToNearestNode(CenterPosition);
                    break;
            }
        }

        /// <summary>
        /// Called every update and performs according to the current state
        /// </summary>
        /// <param name="elapsedTime">Time elapsed since last update</param>
        private void onFrame(float elapsedTime)
        {
            switch (_myVisualState)
            {
                case TwinJetVisualState.HEALTHY:
                    checkIfDamaged();
                    break;
                case TwinJetVisualState.DAMAGED:
                    checkIfGunDestroyed();
                    break;
                case TwinJetVisualState.GUNDESTROYED:
                    checkIfIsAlive();
                    break;
                case TwinJetVisualState.EXPLODING:
                    Explode();
                    _myVisualState = TwinJetVisualState.DEAD;
                    break;
                case TwinJetVisualState.DEAD:
                    break;
            }

            switch (_myBehaviorState)
            {
                case TwinJetBehaviouralState.INTRO:
                    checkIfIntroIsOver();
                    break;
                case TwinJetBehaviouralState.FIGHT:

                    Fly();

                    if (_myFireState != TwinJetFireState.STAGE3)
                    {
                        if (checkIfGunIsReady())
                        {
                            FireGun();
                        }
                        else
                        {
                            _myGunState = GunState.REST;
                        }
                    }

                    if (_myFireState != TwinJetFireState.STAGE1)
                    {
                        if (checkIfMissilesReady())
                        {
                            FireMissiles();
                        }
                    }

                    _timerGunFireRate -= elapsedTime;
                    _timerMissileFireRate -= elapsedTime;

                    break;

                case TwinJetBehaviouralState.PAUSED:

                    break;
            }

            if (!float.IsNaN(_velocity.X) && !float.IsNaN(_velocity.Y))
                _position += _velocity * elapsedTime;
        }

        #endregion
    }
}
