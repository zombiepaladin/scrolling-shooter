using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace ScrollingShooter
{
    /// <summary>
    /// Powerup for BubbleBeam
    /// </summary>
    public class BubbleBeamPowerup : Powerup
    {
        //Constant values for the Bubble Beam Powerup
        private const int X_BOUNDS = 22;
        private const int Y_BOUNDS = 22;
        private const String SPRITE_SHEET = "Spritesheets/tyrian.shp.010008";
        private static readonly Rectangle SPRITE_SOURCE = new Rectangle(49, 142, X_BOUNDS, Y_BOUNDS);

        /// <summary>
        /// Creates a new BubbleBeam Powerup
        /// </summary>
        /// <param name="contentManager">ContentManager to load resources with.</param>
        /// <param name="position">Starting position for the power up.</param>
        public BubbleBeamPowerup(uint id, ContentManager contentManager, Vector2 position)
            : base(id)
        {
            this.type = PowerupType.BubbleBeam;
            this.spriteSource = SPRITE_SOURCE;
            this.spriteSheet = contentManager.Load<Texture2D>(SPRITE_SHEET);
            this.spriteBounds = new Rectangle((int)position.X, (int)position.Y, X_BOUNDS, Y_BOUNDS);
        }
    }
}