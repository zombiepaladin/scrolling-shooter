using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;

namespace ScrollingShooter.Bosses
{
    public class BrainBossWrapper: Boss
    {
        private Enemy boss;

       public BrainBossWrapper(uint id, ContentManager content, Vector2 position)
            : base(id)
        {
            boss = ScrollingShooterGame.GameObjectManager.CreateEnemy(EnemyType.BrainBoss, position);
        }


        public override void Update(float elapsedTime)
        {
            //throw new NotImplementedException();
            if (boss == null || boss.Health <= 0)
                this.Health = 0;
        }

        public override void Draw(float elapsedTime, Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch)
        {
            //throw new NotImplementedException();
        }
    }
}
