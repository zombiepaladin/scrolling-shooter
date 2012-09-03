using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace ScrollingShooter.ObjectManagers
{

    public class ProjectileManager
    {
        private List<Projectile> mProjectiles = new List<Projectile>();

        public void Add(Projectile projectile);
        public void DrawAll(float elapsedTime, SpriteBatch spriteBatch)
        {
            if (mProjectiles.Count != 0)
            {
                foreach (Projectile projectile in mProjectiles)
                {
                    projectile.Draw(elapsedTime, spriteBatch);
                }
            }
        }
        public void UpdateAll(float elapsedTime)
        {
            if (mProjectiles.Count != 0)
            {
                foreach (Projectile projectile in mProjectiles)
                {
                    projectile.Update(elapsedTime);
                }
            }
        }
    }
}
