using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace ScrollingShooter
{
    public class ShrikeShip : PlayerShip
    {
        public ShrikeShip(ContentManager content)
        {
            this.spriteSheet = content.Load<Texture2D>("Spritesheets/tyrian.shp.007D3C");

            this.spriteBounds[(int)SteeringState.HardLeft].X = 1;
            this.spriteBounds[(int)SteeringState.HardLeft].Y = 139;
            this.spriteBounds[(int)SteeringState.HardLeft].Width = 23;
            this.spriteBounds[(int)SteeringState.HardLeft].Height = 29;

            this.spriteBounds[(int)SteeringState.Left].X = 24;
            this.spriteBounds[(int)SteeringState.Left].Y = 139;
            this.spriteBounds[(int)SteeringState.Left].Width = 23;
            this.spriteBounds[(int)SteeringState.Left].Height = 29;

            this.spriteBounds[(int)SteeringState.Straight].X = 48;
            this.spriteBounds[(int)SteeringState.Straight].Y = 139;
            this.spriteBounds[(int)SteeringState.Straight].Width = 23;
            this.spriteBounds[(int)SteeringState.Straight].Height = 29;

            this.spriteBounds[(int)SteeringState.Right].X = 72;
            this.spriteBounds[(int)SteeringState.Right].Y = 139;
            this.spriteBounds[(int)SteeringState.Right].Width = 23;
            this.spriteBounds[(int)SteeringState.Right].Height = 29;

            this.spriteBounds[(int)SteeringState.HardRight].X = 97;
            this.spriteBounds[(int)SteeringState.HardRight].Y = 139;
            this.spriteBounds[(int)SteeringState.HardRight].Width = 23;
            this.spriteBounds[(int)SteeringState.HardRight].Height = 29;

            this.velocity = new Vector2(100, 100);
        }
    }
}
