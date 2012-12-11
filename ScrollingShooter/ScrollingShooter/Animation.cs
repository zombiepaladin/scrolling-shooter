//Animation Class:
//Coders: Nicholas Boen
//Date: 9/9/2012
//Time: 3:22 P.M.
//
//This file will hold the classes related
//to animations for the game

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;

namespace ScrollingShooter
{

    public enum AnimType
    {
        PLAYONCE = 0,
        LOOP,
        BACKANDFORTH,
        MANUAL
    }

    public class AnimSet
    {
        #region Private Fields

        /// <summary>
        /// The total time to spend between slides
        /// </summary>
        private float slideInterval;

        /// <summary>
        /// The time left before the next slide
        /// </summary>
        private float timerToSlide;

        /// <summary>
        /// The time left before the animation begins
        /// playing
        /// </summary>
        private float timerToStart;

        /// <summary>
        /// The positions and sizes of the sprites
        /// </summary>
        private Rectangle[] slides;

        /// <summary>
        /// The sprite sheet to get the animation from
        /// </summary>
        private Texture2D spriteSheet;

        /// <summary>
        /// The current frame or slide that is being displayed
        /// </summary>
        private int currentSlide;

        /// <summary>
        /// The current anim type of this animset
        /// </summary>
        private AnimType myType;

        /// <summary>
        /// The position and bounds of the drawn animation. Beware
        /// that the sprite will be scaled to fit the width and
        /// height of this rectangle
        /// </summary>
        private Rectangle spriteBounds;

        /// <summary>
        /// Sets this animset to stop animating when it finishes
        /// with it's animation (good for stopping loops)
        /// </summary>
        private bool softStop;

        /// <summary>
        /// Stops the animation immediately, regardless of current
        /// frame and without waiting for the animation to finish
        /// </summary>
        private bool hardStop;

        /// <summary>
        /// Determines whether the animation should be playing forward or backward
        /// </summary>
        private bool isMovingForward;

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets whether the current animation is stopped or not
        /// </summary>
        public bool IsStopped { get { return hardStop; } }

        /// <summary>
        /// Gets whether the current animation is waiting to stop or not
        /// </summary>
        public bool IsStopping { get { return softStop; } }

        /// <summary>
        /// Gets or sets the time interval (in seconds) between slides
        /// </summary>
        public float Interval { get { return slideInterval; } set { slideInterval = value; } }

        /// <summary>
        /// Gets or sets the bounds and position of the animation being drawn
        /// </summary>
        public Rectangle Bounds { get { return spriteBounds; } set { spriteBounds = value; } }

        /// <summary>
        /// Gets or sets the current position of the animation
        /// </summary>
        public Vector2 Position { get { return new Vector2(spriteBounds.X, spriteBounds.Y); } set { spriteBounds.X = (int)value.X; spriteBounds.Y = (int)value.Y; } }
        #endregion

        #region Constructors

        /// <summary>
        /// Constructs a new AnimSet
        /// </summary>
        /// <param name="newSlides">An array of sprite locations on the sprite sheet for each from of the animation</param>
        /// <param name="newSpriteSheet">The sprite sheet to get the slides from</param>
        /// <param name="newBounds">The bounds and position that the animation will be drawn as. Note that the sprite retrieved
        /// from the spritesheet will be scaled to fit this width and height
        /// </param>
        /// <param name="newType">The animation type of this set</param>
        /// <param name="newInterval">The interval to wait between each frame/slide</param>
        public AnimSet(Rectangle[] newSlides, Texture2D newSpriteSheet, Rectangle newBounds, AnimType newType, float newInterval)
        {
            slides = newSlides;
            spriteSheet = newSpriteSheet;
            spriteBounds = newBounds;
            myType = newType;
            slideInterval = newInterval;
            timerToSlide = slideInterval;

            timerToStart = 0;
            softStop = false;
            hardStop = true;
            isMovingForward = true;
        }

        /// <summary>
        /// Constructs a new AnimSet
        /// </summary>
        /// <param name="newSlides">An array of sprite locations on the sprite sheet for each from of the animation</param>
        /// <param name="newSpriteSheet">The sprite sheet to get the slides from</param>
        /// <param name="newBounds">The bounds and position that the animation will be drawn as. Note that the sprite retrieved
        /// from the spritesheet will be scaled to fit this width and height
        /// </param>
        /// <param name="newType">The animation type of this set</param>
        /// <param name="totalTimeToPlay">The total time it will take to go through one entire animation</param>
        public AnimSet(Rectangle[] newSlides, Texture2D newSpriteSheet, Rectangle newBounds, AnimType newType, int totalTimeToPlay)
        {
            slides = newSlides;
            spriteSheet = newSpriteSheet;
            spriteBounds = newBounds;
            myType = newType;
            slideInterval = totalTimeToPlay / slides.Length;

            softStop = false;
            hardStop = true;
            isMovingForward = true;
        }

        #endregion

        #region Draw Method

        /// <summary>
        /// Draws the current animation frame to the screen
        /// </summary>
        /// <param name="elapsedTime">The time elapsed since the last Draw</param>
        /// <param name="spriteBatch">The Draw batch to include this call into</param>
        public void Draw(float elapsedTime, SpriteBatch spriteBatch)
        {
            if (!hardStop) //Make sure that the animation isn't stopped
            {
                if (timerToStart <= 0) //Make sure that the timer to start is up
                {
                    switch (myType)
                    {
                        case AnimType.PLAYONCE:
                            PlayOnce();
                            break;
                        case AnimType.LOOP:
                            Loop();
                            break;
                        case AnimType.BACKANDFORTH:
                            BackAndForth();
                            break;
                        case AnimType.MANUAL:
                            //This is meant to let an outside controller take over
                            //so we don't need to touch anything
                            break;
                        default:
                            return;
                    }

                    timerToSlide -= elapsedTime;

                    spriteBatch.Draw(spriteSheet, spriteBounds, slides[currentSlide], Color.White, 0f, Vector2.Zero, SpriteEffects.None, 0);

                }
                else //Otherwise continue the count down to start
                {
                    timerToStart -= elapsedTime;
                }
            }

        }

        #endregion

        #region General Controls

        /// <summary>
        /// Starts the animation
        /// </summary>
        /// <param name="setDelayTimer">Optional timer to delay starting from</param>
        public void Start(float setDelayTimer = 0)
        {
            hardStop = false;
            softStop = true;
            timerToStart = setDelayTimer;
        }

        /// <summary>
        /// Stops the animation when it has finished it's current animation
        /// </summary>
        public void StopAndFinish()
        {
            softStop = true;
        }

        /// <summary>
        /// Stops the animation immediately
        /// </summary>
        public void StopNow()
        {
            hardStop = true;
        }

        /// <summary>
        /// Change the type of animation this animset has
        /// </summary>
        /// <param name="newType">The new animation type</param>
        public void ChangeAnimType(AnimType newType)
        {
            myType = newType;
        }

        #endregion

        #region Manual Control Methods

        /// <summary>
        /// Proceeds to the next frame of animation if possible
        /// </summary>
        public void NextFrame()
        {
            switch (myType)
            {
                case AnimType.MANUAL:

                    currentSlide++;

                    if (currentSlide > slides.Length)
                        currentSlide = 0;

                    break;
                default:
                    return;
            }
        }

        /// <summary>
        /// Goes back to the previous frame of animation if possible
        /// </summary>
        public void PreviousFrame()
        {
            switch (myType)
            {
                case AnimType.MANUAL:

                    currentSlide--;

                    if (currentSlide < 0)
                        currentSlide = slides.Length;

                    break;
                default:
                    return;
            }
        }

        /// <summary>
        /// Sets the current frame of animation to the given one
        /// </summary>
        /// <param name="newFrame">The animation frame to use</param>
        public void SetFrame(int newFrame)
        {
            switch (myType)
            {
                case AnimType.MANUAL:
                    currentSlide = (int)MathHelper.Clamp(newFrame, 0, slides.Length);
                    break;
                default:
                    return;
            }
        }

        /// <summary>
        /// Sets the animation to the last frame
        /// </summary>
        /// <returns>The index of the last animation</returns>
        public int SetToLastFrame()
        {
            switch (myType)
            {
                case AnimType.MANUAL:
                    currentSlide = slides.Length;
                    return slides.Length;
                default:
                    return 0;
            }
        }

        #endregion

        #region Automatic Control Methods

        /// <summary>
        /// Handles the looping functionality
        /// </summary>
        private void Loop()
        {
            switch (myType)
            {
                case AnimType.LOOP:

                    if (timerToSlide <= 0)
                    {
                        currentSlide++;
                        if (currentSlide > slides.Length - 1)
                            currentSlide = 0;

                        timerToSlide = slideInterval;
                    }

                    break;
                default:
                    return;
            }
        }

        /// <summary>
        /// Handles the back and forth functionality
        /// </summary>
        private void BackAndForth()
        {
            switch (myType)
            {
                case AnimType.BACKANDFORTH:

                    if (timerToSlide <= 0)
                    {
                        if (isMovingForward)
                        {
                            currentSlide++;
                            if (currentSlide > slides.Length - 1)
                            {
                                currentSlide = slides.Length - 1;
                                isMovingForward = false;
                            }
                        }
                        else
                        {
                            currentSlide--;
                            if (currentSlide < 0)
                            {
                                currentSlide = 1;
                                isMovingForward = true;
                            }
                        }

                        timerToSlide = slideInterval;
                    }

                    break;
                default:
                    return;
            }
        }

        /// <summary>
        /// Handles the play once functionality
        /// </summary>
        private void PlayOnce()
        {
            switch (myType)
            {
                case AnimType.PLAYONCE:
                    if (timerToSlide <= 0)
                    {
                        currentSlide++;

                        if (currentSlide > slides.Length - 1)
                        {
                            currentSlide = slides.Length - 1;
                            hardStop = true;
                        }

                        timerToSlide = slideInterval;
                    }

                    break;
                default:
                    return;
            }
        }

        #endregion

    }

    public class ExplosionSet
    {
        #region Private Fields

        /// <summary>
        /// The list of AnimSets that make up this explosion
        /// </summary>
        private List<AnimSet> _spriteList;

        /// <summary>
        /// The position (top left corner) that this explosion will contain
        /// </summary>
        private Vector2 _position;

        /// <summary>
        /// The interval between each slide of the animation
        /// </summary>
        private float _slideInterval;

        /// <summary>
        /// The delay it takes to start the animation, used primarily
        /// for random explosions
        /// </summary>
        private float _timeToStart;

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the position of this Explosion Set
        /// </summary>
        public Vector2 Position { get { return _position; } set { _position = value; } }

        /// <summary>
        /// Gets or sets the timer interval (in seconds) between each slide
        /// </summary>
        public float SlideInterval { get { return _slideInterval; } set { _slideInterval = value; } }

        /// <summary>
        /// Gets whether all current animations are stopped or not
        /// </summary>
        public bool IsStopped 
        { 
            get 
            {
                foreach (AnimSet anim in _spriteList)
                {
                    if (!anim.IsStopped)
                        return false;
                }

                return true;
            } 
        }

        /// <summary>
        /// Gets whether all current animations is waiting to stop or not
        /// </summary>
        public bool IsStopping 
        { 
            get 
            {
                foreach (AnimSet anim in _spriteList)
                {
                    if (!anim.IsStopping)
                        return false;
                }

                return true;
            } 
        }


        #endregion

        #region Constructors

        /// <summary>
        /// Creates an empty Explosion Set
        /// </summary>
        /// <param name="position">The position of this set</param>
        /// <param name="delayTimer">The optional time delay when starting the animation</param>
        public ExplosionSet(Vector2 position, float delayTimer = 0)
        {
            _spriteList = new List<AnimSet>();
            _position = position;
            _timeToStart = delayTimer;
        }

        /// <summary>
        /// Creates an Explosion set with one initial Anim Set
        /// </summary>
        /// <param name="initialSet">The set to start out with</param>
        /// <param name="position">The position of this set</param>
        /// <param name="delayTimer">The optional time delay when starting the animation</param>
        public ExplosionSet(AnimSet initialSet, Vector2 position, float delayTimer = 0)
        {
            _spriteList = new List<AnimSet>();
            _position = position;
            _timeToStart = delayTimer;

            _spriteList.Add(initialSet);
        }

        #endregion

        #region Draw Method

        /// <summary>
        /// Draws the current frame of each of the AnimSets
        /// </summary>
        /// <param name="elapsedTime">Time elapsed since last draw</param>
        /// <param name="spriteBatch">Sprite Batch to include this set into</param>
        public void Draw(float elapsedTime, SpriteBatch spriteBatch)
        {
            foreach (AnimSet anim in _spriteList)
            {
                anim.Draw(elapsedTime, spriteBatch);
            }
        }

        #endregion

        #region Control Methods

        //// <summary>
        /// Adds an animation set to this set
        /// </summary>
        /// <param name="newSet">The new Set to add</param>
        /// <param name="offsetFromPosition">The offset from the explosion sets position to where
        /// the animation should be displayed (useful for piecing together animations that 
        /// have been split apart)
        /// </param>
        public void AddAnimSet(AnimSet newSet, Vector2 offsetFromPosition)
        {
            newSet.ChangeAnimType(AnimType.LOOP);
            newSet.Position = _position + offsetFromPosition;

            _spriteList.Add(newSet);
        }

        /// <summary>
        /// Starts all of the AnimSets together
        /// </summary>
        /// <param name="delayTimer">The time to delay starting them</param>
        public void Start(float delayTimer = -1)
        {
            if (delayTimer >= 0)
                _timeToStart = delayTimer;

            foreach (AnimSet anim in _spriteList)
            {
                anim.Start(_timeToStart);
            }
        }

        /// <summary>
        /// Has all of the AnimSets stop animating when they complete their
        /// current animation
        /// </summary>
        public void StopAndFinish()
        {
            foreach (AnimSet anim in _spriteList)
            {
                anim.StopAndFinish();
            }
        }

        /// <summary>
        /// Immediately stops all animation sets in their current 
        /// animation and ceases to draw them
        /// </summary>
        public void StopNow()
        {
            foreach (AnimSet anim in _spriteList)
            {
                anim.StopNow();
            }
        }

        #endregion

    }

    public enum ExplosionType
    {
        SINGLE = 0,
        RANDOM,
        MULTIPLE
    }

    public class ExplodeAnim
    {

        #region Private Fields

        /// <summary>
        /// The list of explosion sets that this animator controls
        /// </summary>
        private List<ExplosionSet> _explosionList;

        /// <summary>
        /// The type of explosion that this animator will implement
        /// </summary>
        private ExplosionType _myExplosionType;

        /// <summary>
        /// The time step between slides that should be used for all explosion sets
        /// in the explosion list
        /// </summary>
        private float _universalSlideInterval;

        /// <summary>
        /// The bounds and position of this Explosion Set, the bounds are primarily used in
        /// random explosion patterns to denote a bounding box
        /// </summary>
        private Rectangle _bounds;

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the position of this Explosion Set
        /// </summary>
        public Vector2 Position { get { return new Vector2(_bounds.X, _bounds.Y); } set { _bounds = new Rectangle((int)value.X, (int)value.Y, _bounds.Width, _bounds.Height); } }

        /// <summary>
        /// Gets or sets the timer interval (in seconds) between each slide
        /// </summary>
        public float SlideInterval { get { return _universalSlideInterval; } set { _universalSlideInterval = value; } }


        /// <summary>
        /// Gets whether all current animations are stopped or not
        /// </summary>
        public bool IsStopped
        {
            get
            {
                foreach (ExplosionSet es in _explosionList)
                {
                    if (!es.IsStopped)
                        return false;
                }

                return true;
            }
        }

        /// <summary>
        /// Gets whether all current animations is waiting to stop or not
        /// </summary>
        public bool IsStopping
        {
            get
            {
                foreach (ExplosionSet es in _explosionList)
                {
                    if (!es.IsStopping)
                        return false;
                }

                return true;
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Creates an ExplodeAnim animator
        /// </summary>
        /// <param name="initialExplosionSet">The initial set to use for an explosion, the position of this will be set to the bounds</param>
        /// <param name="initialType">The initial explosion type of this animator</param>
        /// <param name="initialSlideInterval">The interval between frames to use for this animator</param>
        /// <param name="bounds">The bounds and position of this explosion animator, bounds are primarily used for the RANDOM type</param>
        public ExplodeAnim(ExplosionSet initialExplosionSet, ExplosionType initialType, float initialSlideInterval, Rectangle bounds)
        {
            _explosionList = new List<ExplosionSet>();

            _universalSlideInterval = initialSlideInterval;
            _bounds = bounds;
            _myExplosionType = initialType;

            initialExplosionSet.Position = new Vector2(bounds.X, bounds.Y);

            AddExplosion(initialExplosionSet, Vector2.Zero);

        }

        /// <summary>
        /// Creates an instance of an empty ExplodeAnim animator
        /// </summary>
        /// <param name="initialType">The initial explosion type of this animator</param>
        /// <param name="initialSlideInterval">The interval between frames to use for this animator</param>
        /// <param name="bounds">The bounds and position of this explosion animator, bounds are primarily used for the RANDOM type</param>
        public ExplodeAnim(ExplosionType initialType, float initialSlideInterval, Rectangle bounds)
        {
            _explosionList = new List<ExplosionSet>();

            _universalSlideInterval = initialSlideInterval;
            _bounds = bounds;
            _myExplosionType = initialType;
        }

        #endregion

        #region Draw Method

        public void Draw(float elapsedTime, SpriteBatch spriteBatch)
        {
            foreach (ExplosionSet es in _explosionList)
            {
                es.Draw(elapsedTime, spriteBatch);
            }
        }

        #endregion

        #region Control Methods

        /// <summary>
        /// Starts all timers for each member of the explosion list
        /// </summary>
        public void Start()
        {
            switch (_myExplosionType)
            {
                case ExplosionType.SINGLE:

                    _explosionList[0].Start();

                    break;
                case ExplosionType.RANDOM:

                    Random r = new Random();

                    foreach (ExplosionSet es in _explosionList)
                    {
                        es.Start((float)r.NextDouble());
                    }

                    break;
                case ExplosionType.MULTIPLE:

                    foreach (ExplosionSet es in _explosionList)
                    {
                        es.Start();
                    }

                    break;
                default:
                    break;
            }


        }

        /// <summary>
        /// Adds an Explosion Set to the list
        /// </summary>
        /// <param name="newSet">Explosion set to add</param>
        /// <param name="xOffset">The offset from this explosion animator's position 
        /// on the x-axis, only used for the MULTIPLE explosion pattern
        /// </param>
        /// <param name="yOffset">The offset from this explosion animator's position 
        /// on the y-axis, only used for the MULTIPLE explosion pattern
        /// </param>
        public void AddExplosion(ExplosionSet newSet, Vector2 offsetPosition)
        {
            Vector2 newVector = new Vector2(_bounds.X, _bounds.Y);

            switch (_myExplosionType)
            {
                case ExplosionType.SINGLE:
                    newSet.Position = newVector + offsetPosition;
                    break;

                case ExplosionType.RANDOM:

                    Random r = new Random();

                    newVector.X += r.Next(0, _bounds.Width);
                    newVector.Y += r.Next(0, _bounds.Height);

                    newSet.Position = newVector;

                    break;

                case ExplosionType.MULTIPLE:
                    newSet.Position = newVector + offsetPosition;
                    break;

                default:
                    break;
            }

            _explosionList.Add(newSet);
        }

        /// <summary>
        /// Tells the ExplosionSets to stop animating and waits for them
        /// to finish their cycle
        /// </summary>
        public void StopAndFinish()
        {
            foreach (ExplosionSet es in _explosionList)
            {
                es.StopAndFinish();
            }
        }

        /// <summary>
        /// Immediately stops all ExplosionSets in the middle of their animations
        /// </summary>
        public void StopNow()
        {
            foreach (ExplosionSet es in _explosionList)
            {
                es.StopNow();
            }
        }

        #endregion

    }

}