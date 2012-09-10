using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace ScrollingShooter
{
    public class ToPlayerBullet : Projectile
    {
        //Constants
        private const string SPRITESHEET = "Spritesheets/tyrian.shp.01D8A7";
        private static readonly Rectangle SPRITEBOUNDS = new Rectangle(38, 57, 7, 11);
        private const int X_VELOCITY = 300;
        private const int Y_VELOCITY = 300;

        public ToPlayerBullet(uint id, ContentManager content, Vector2 position)
            : base(id)
        {
            this.spriteSheet = content.Load<Texture2D>(SPRITESHEET);

            this.spriteBounds = SPRITEBOUNDS;

            this.position = position;

            this.velocity = toPlayerVelocity();
        }

        private Vector2 toPlayerVelocity()
        {
            //Don't want to deal with trig. Just fire in one of eight directions.
            PlayerShip player = ScrollingShooterGame.Game.player;
            Vector2 positionVector = position - player.GetPosition();

            return new Vector2(X_VELOCITY * -Math.Sign(positionVector.X), Y_VELOCITY * -Math.Sign(positionVector.Y));
        }
    }
}
