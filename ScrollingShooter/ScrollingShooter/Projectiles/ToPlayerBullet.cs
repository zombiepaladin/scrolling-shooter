using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace ScrollingShooter
{
    /// <summary>
    /// A bullet that travels towards the Player.
    /// </summary>
    public class ToPlayerBullet : Projectile
    {
        //Constants
        private const string SPRITESHEET = "Spritesheets/tyrian.shp.01D8A7";
        private static readonly Rectangle SPRITEBOUNDS = new Rectangle(38, 57, 7, 11);
        private static readonly Vector2 VELOCITY = new Vector2(300);

        /// <summary>
        /// Creates a new bullet that will travel towards the Player's current position.
        /// </summary>
        /// <param name="id">Id for the bullet.</param>
        /// <param name="content">ContentManager to load content with.</param>
        /// <param name="position">Starting position for the bullet.</param>
        public ToPlayerBullet(uint id, ContentManager content, Vector2 position, ProjectileType type)
            : base(id, type)
        {
            this.spriteSheet = content.Load<Texture2D>(SPRITESHEET);

            this.spriteBounds = SPRITEBOUNDS;

            this.position = position;

            //Fire at the Player.
            PlayerShip Player = ScrollingShooterGame.Game.Player;
            Vector2 positionVector = Player.GetPosition() - position;
            positionVector.Normalize();

            this.velocity = positionVector * VELOCITY;
        }
    }
}