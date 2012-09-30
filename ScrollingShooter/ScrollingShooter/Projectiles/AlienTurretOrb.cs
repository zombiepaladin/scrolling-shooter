using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using System.Collections.Generic;
using System;

namespace ScrollingShooter
{
    /// <summary>
    /// An orb used as a projectile for the alien turret
    /// </summary>
    public class AlienTurretOrb : Projectile
    {
        // Orb state variables
        Texture2D spritesheet;
        Vector2 position;
        Rectangle spriteBounds = new Rectangle();

        float angle;

        Vector2 angleVector;

        /// <summary>
        /// The bounding rectangle of the Orb
        /// </summary>
        public override Rectangle Bounds
        {
            get { return new Rectangle((int)position.X, (int)position.Y, spriteBounds.Width, spriteBounds.Height); }
        }

        /// <summary>
        /// Creates a new instance of the Orb 
        /// </summary>
        /// <param name="content">A ContentManager to load resources with</param>
        /// <param name="position">The position of the Orb in the game world</param>
        public AlienTurretOrb(uint id, ContentManager content, Vector2 position)
            : base(id)
        {
            this.position = position;

            spritesheet = content.Load<Texture2D>("Spritesheets/newshg.shp.000000");

            spriteBounds = new Rectangle(194, 88, 20, 20);
    
            //determine the angle

            // Sense the player's position
            PlayerShip player = ScrollingShooterGame.Game.Player;
            Vector2 playerPosition = new Vector2(player.Bounds.Center.X, player.Bounds.Center.Y);

            // Get a vector from our position to the player's position and normalize it
            angleVector = playerPosition - this.position;

            //normalize the angleVector
            angleVector.Normalize();
            playerPosition.Normalize();

            angle = (float)Math.Acos(Vector2.Dot(playerPosition, angleVector)) - .75f;
        }

        /// <summary>
        /// Updates the Pinchers
        /// </summary>
        /// <param name="elapsedTime">The in-game time between the previous and current frame</param>
        public override void Update(float elapsedTime)
        {
            position += angleVector;

        }

        /// <summary>
        /// Draw the Pincher on-screen
        /// </summary>
        /// <param name="elapsedTime">The in-game time between the previous and current frame</param>
        /// <param name="spriteBatch">An already initialized SpriteBatch, ready for Draw() commands</param>
        public override void Draw(float elapsedTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(spritesheet, Bounds, spriteBounds, Color.White, angle, new Vector2(Bounds.Width / 2, Bounds.Height / 2), SpriteEffects.None, 1f);
        }

    }
}