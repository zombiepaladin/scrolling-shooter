using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using System;

namespace ScrollingShooter
{
    /// <summary>
    /// A green orb
    /// </summary>
    public class GreenOrb : Projectile
    {
        Vector2 angleVector;

        /// <summary>
        /// Creates a green orb
        /// </summary>
        /// <param name="content">A ContentManager to load content from</param>
        /// <param name="position">A position on the screen</param>
        public GreenOrb(uint id, ContentManager content, Vector2 position)
            : base(id)
        { 
            this.position = position;
            this.spriteSheet = content.Load<Texture2D>("Spritesheets/newsha.shp.000000");

            this.spriteBounds = new Rectangle(123, 201, 18, 17);

            Random rand = new Random(DateTime.Now.Millisecond * (int)id +1);


            //determine the angle

            int x = rand.Next(0, ScrollingShooterGame.Game.GraphicsDevice.Viewport.Width) - ScrollingShooterGame.Game.GraphicsDevice.Viewport.Width /2;
            int y = rand.Next((int)position.Y - 150, ScrollingShooterGame.Game.GraphicsDevice.Viewport.Height - 150);

            // Get a vector from our position to the player's position and normalize it
            angleVector = new Vector2(x, y);

            //normalize the angleVector
            angleVector.Normalize();
        }
   

        /// <summary>
        /// Custom update class to fly at a certain angle
        /// </summary>
        /// <param name="elapsedTime">The in-game time between the previous and current frame</param>
        public override void Update(float elapsedTime)
        {
            position += angleVector * 2;
        } 
    }
}