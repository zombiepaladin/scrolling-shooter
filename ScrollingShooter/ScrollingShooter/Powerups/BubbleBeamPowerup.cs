using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace ScrollingShooter
{
    public class BubbleBeamPowerup : Powerup
    {
        private const String SPRITE_SHEET = "Spritesheets/tyrian.shp.010008";
        private static readonly Rectangle SPRITE_SOURCE = new Rectangle(42, 142, 23, 23);

        public BubbleBeamPowerup(ContentManager contentManager, Vector2 position)
        {
            this.spriteSource = SPRITE_SOURCE;
            this.spriteSheet = contentManager.Load<Texture2D>(SPRITE_SHEET);
            this.spriteBounds = new Rectangle((int)position.X, (int)position.Y, 23, 23);
        }
    }
}
