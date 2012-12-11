using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace ScrollingShooter
{
    public class EnergyBlast : Projectile
    {
        /// <summary>
        /// Spawns the energy blast that corresponds to the energy blast powerup.  Different power levels (gained by collecting
        /// multiples of the powerup) will create bigger and/or faster bullets
        /// </summary>
        /// <param name="content">Content manager to load the spritesheet</param>
        /// <param name="position">Position to spawn at</param>
        /// <param name="power">The power level of this blast</param>
        public EnergyBlast(uint id, ContentManager content, Vector2 position, int power) : base (id)
        {
            this.spriteSheet = content.Load<Texture2D>("Spritesheets/tyrian.shp.01D8A7");

            if (power == 0)
            {
                this.spriteBounds = new Rectangle(26, 15, 8, 13);

                this.velocity = new Vector2(0, -300);

                Damage = 3;
            }
            else if (power == 1)
            {
                this.spriteBounds = new Rectangle(24, 28, 12, 14);

                this.velocity = new Vector2(0, -300);

                Damage = 5;
            }
            else if (power == 2)
            {
                this.spriteBounds = new Rectangle(85, 14, 10, 25);

                this.velocity = new Vector2(0, -350);

                Damage = 10;
            }
            else if (power >= 3)
            {
                this.spriteBounds = new Rectangle(25, 70, 22, 28);

                this.velocity = new Vector2(0, -350);

                Damage = 20;
            }

            this.position = position;

            // TODO: When it collides with an enemy, this projectile will spawn an explosion that will damage enemies it comes in contact with
        }

    }
}
