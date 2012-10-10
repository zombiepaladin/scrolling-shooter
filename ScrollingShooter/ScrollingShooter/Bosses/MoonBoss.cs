using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace ScrollingShooter
{
    enum MBState{
        Full,
        Half,
        AD
    }
    /// <summary>
    /// This is going to hopfuly finalize the MoonBoss Since git hub lost these files and I have no way of getting them back I hope this works. 
    /// </summary>
    class MoonBoss:Enemy
    {
        float dgt1 = 0;
        float dgt2 = 0;
        float rc1 = 3f;
        float rc2 = 3f;
        Texture2D spritesheet;
        Vector2 position;
        Rectangle[] spriteBounds = new Rectangle[3];
        MBState state = MBState.Full;
        MBCloseC CC;


        public override Rectangle Bounds
        {
            get { return new Rectangle((int)position.X, (int)position.Y, spriteBounds[(int)state].Width, spriteBounds[(int)state].Height); }
        }
        /// <summary>
        /// Creates the moon boss. 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="content">A Content manager</param>
        /// <param name="position"></param>
        public MoonBoss(uint id, ContentManager content, Vector2 position)
            : base(id)
        {
            Health = 600;
            this.position = position;
            CC = new MBCloseC(id, content, position);
            spritesheet = content.Load<Texture2D>("Spritesheets/MoonBoss.png");

            spriteBounds[(int)MBState.Full].X = 0;
            spriteBounds[(int)MBState.Full].Y = 0;
            spriteBounds[(int)MBState.Full].Width = 119;
            spriteBounds[(int)MBState.Full].Height = 140;

            spriteBounds[(int)MBState.Half].X = 0;
            spriteBounds[(int)MBState.Half].Y = 0;
            spriteBounds[(int)MBState.Half].Width = 119;
            spriteBounds[(int)MBState.Half].Height = 140;

            spriteBounds[(int)MBState.AD].X = 119;
            spriteBounds[(int)MBState.AD].Y = 0;
            spriteBounds[(int)MBState.AD].Width = 108;
            spriteBounds[(int)MBState.AD].Height = 140;

            state = MBState.Full;
        }
        /// <summary>
        /// Updates the moon boss
        /// </summary>
        /// <param name="elapsedTime">Time elapsed</param>
        public override void Update(float elapsedTime)
        {
            if (Health <= 300)
            {
                state = MBState.Half;
            }
            else if (Health <= 100)
            {
                state = MBState.AD;
            }

            if (dgt1 >= 2f)
            {
                dgt1 = 0;
                rc1 = 0;
            }
            else if (rc1 >= 3f)
            {
                ScrollingShooterGame.GameObjectManager.CreateProjectile(ProjectileType.Laser, Vector2.Zero);
                dgt1 += elapsedTime;
            }
            else if(rc1 < 3f)
            {
                rc1 += elapsedTime;
            }
            if (dgt2 >= 2.4f)
            {
                dgt2 = 0;
            }
            else if( rc2 >= 3f)
            {
                ScrollingShooterGame.GameObjectManager.CreateProjectile(ProjectileType.Laser, Vector2.Zero);
                dgt2 += elapsedTime;
            }
            else if (rc2 < 3f)
            {
                rc2 += elapsedTime;
            }
            CC.Update(elapsedTime);
        }
        /// <summary>
        /// Draws the Moon Boss.
        /// </summary>
        /// <param name="elapsedTime">Time elapsed</param>
        /// <param name="spriteBatch">Sprite grouping</param>
        public override void Draw(float elapsedTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(spritesheet, Bounds, spriteBounds[(int)state], Color.White);
        }
    }
}
