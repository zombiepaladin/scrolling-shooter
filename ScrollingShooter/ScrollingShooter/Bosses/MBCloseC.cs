using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;


namespace ScrollingShooter
{
    class MBCloseC:Enemy
    {
        enum CCState
        {
            none,
            close,
            fire,
        }
        float dgt1 = 0;
        float dgt2 = 0;
        float rc1 = 3f;
        float rc2 = 3f;
        Texture2D spritesheet;
        Vector2 position;
        Rectangle[] spriteBounds = new Rectangle[3];
        MBState state = MBState.Full;
        CCState cs = CCState.none;
        public override Rectangle Bounds
        {
            get { return new Rectangle((int)position.X, (int)position.Y, spriteBounds[(int)cs].Width, spriteBounds[(int)cs].Height); }
        }
        /// <summary>
        /// This creates the gun on the boss.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="content"></param>
        /// <param name="position"></param>
        public MBCloseC(uint id, ContentManager content, Vector2 position)
            : base(id)
        {
            this.position = position;
            Health = 600;

            spritesheet = content.Load<Texture2D>("Spritesheets/newshb.shp.000000");

            spriteBounds[(int)CCState.none].X = 0;
            spriteBounds[(int)CCState.none].Y = 140;
            spriteBounds[(int)CCState.none].Width = 48;
            spriteBounds[(int)CCState.none].Height = 28;

            spriteBounds[(int)CCState.close].X = 0;
            spriteBounds[(int)CCState.close].Y = 168;
            spriteBounds[(int)CCState.close].Width = 48;
            spriteBounds[(int)CCState.close].Height = 28;

            spriteBounds[(int)CCState.fire].X = 0;
            spriteBounds[(int)CCState.fire].Y = 196;
            spriteBounds[(int)CCState.fire].Width = 48;
            spriteBounds[(int)CCState.fire].Height = 28;

            state = MBState.Full;
            cs = CCState.none;
        }
        /// <summary>
        /// this updates the gun.
        /// </summary>
        /// <param name="elapsedTime"></param>
        public override void Update(float elapsedTime)
        {
            dgt1 += elapsedTime;
            dgt2 += elapsedTime;

            if (dgt1 < .5f && dgt1 > .4f)
            {
                cs = CCState.close;
            }
            if (dgt1 > .75f && state == MBState.Full)
            {
                ScrollingShooterGame.GameObjectManager.CreateProjectile(ProjectileType.EBullet, position);
                dgt1 = 0;
                cs = CCState.fire;
            }
            if (dgt1 > .66f && state == MBState.Half)
            {
                ScrollingShooterGame.GameObjectManager.CreateProjectile(ProjectileType.EBullet, position);
                dgt1 = 0;
                cs = CCState.fire;
            }
            if (dgt2 > .76f && state == MBState.Full)
            {
                ScrollingShooterGame.GameObjectManager.CreateProjectile(ProjectileType.EBullet, position);
                dgt2 = 0;
                cs = CCState.none;
            }
            if (dgt2 > .67f && state == MBState.Half)
            {
                ScrollingShooterGame.GameObjectManager.CreateProjectile(ProjectileType.EBullet, position);
                dgt2 = 0;
                cs = CCState.none;
            }
        }
        /// <summary>
        /// this draws the guns.
        /// </summary>
        /// <param name="elapsedTime"></param>
        /// <param name="spriteBatch"></param>
        public override void Draw(float elapsedTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(spritesheet, Bounds, spriteBounds[(int)cs], Color.White);
        }
    }
}
