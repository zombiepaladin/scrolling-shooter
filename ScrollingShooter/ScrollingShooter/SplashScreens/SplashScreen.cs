using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ScrollingShooter
{
    enum SplashScreenType
    {
        GameStart,
        EndLevelOne,
    }

    /// <summary>
    /// A base class for representing splash screens.  
    /// </summary>
    public abstract class SplashScreen
    {
        /// <summary>
        /// The level to load after this splash screen exits
        /// </summary>
        public string NextLevel;

        /// <summary>
        /// Updates the splash screen. 
        /// </summary>
        /// <param name="elapsedTime">The time from the previous frame to this one</param>
        abstract public void Update(float elapsedTime);

        /// <summary>
        /// Draws the splash Screen
        /// </summary>
        /// <param name="elapsedTime">The time from the previous frame to this one</param>
        /// <param name="spriteBatch">An already-initialized spritebatch to draw the screen</param>
        abstract public void Draw(float elapsedTime, SpriteBatch spriteBatch);
    }
}
