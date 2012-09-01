using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using System;

namespace ScrollingShooter
{
    /// <summary>
    /// One of the 3 balls that will circle the ship after collecting a trishield powerup
    /// </summary>
    public class TriShieldBall : Projectile
    {
        //To circle around the player ship, we need a rotation value (presumably 0-2*PI)
        private float rotation;

        //NOTE: This needs to be changed in the future to calling something like Game.Player to save space.
        private PlayerShip player;

        //Constant value for rotation speed: increase to make it rotate faster
        private const float ROTATION_INCREMENT = MathHelper.Pi / 32;

        //Constant value for distance from player: increase to expand the diameter
        private const float DIST_FROM_PLAYER = 24;

        /// <summary>
        /// Creates a new Tri Shield Ball Projectile
        /// </summary>
        /// <param name="content">A ContentManager to load content from</param>
        /// <param name="rotation">The rotation that it's at around the player (0-2*PI)</param>
        /// <param name="player">Reference back to the player</param>
        public TriShieldBall(ContentManager content, float rotation, PlayerShip player)
        {   
            this.spriteSheet = content.Load<Texture2D>("Spritesheets/tyrian.shp.000000");

            this.spriteBounds = new Rectangle(84, 182, 12, 14);

            this.velocity = new Vector2(0, -300);

            this.rotation = rotation;

            this.player = player;
        }

        /// <summary>
        /// Updates the trishield ball, locking its position on the player and rotating around it
        /// </summary>
        /// <param name="elapsedTime"></param>
        public override void Update(float elapsedTime)
        {
            this.rotation += ROTATION_INCREMENT;

            if (this.rotation >= 2 * MathHelper.Pi)
                this.rotation -= 2 * MathHelper.Pi;

            this.position.X = (float)(player.Bounds.Center.X + DIST_FROM_PLAYER * Math.Cos(this.rotation));
            this.position.Y = (float)(player.Bounds.Center.Y + DIST_FROM_PLAYER * Math.Sin(this.rotation));
        }
    }
}
