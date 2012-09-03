using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
namespace ScrollingShooter.Gui.GUIObjects
{
    public class GUIHealthBar : GUIObject
    {
        private int mHealth;
        private int mMaxHealth;

        public int Health
        {
            get
            {
                return mHealth;
            }
        }
        public GUIHealthBar(ContentManager content,Vector2 position,int health, int maxHealth) :
            base()
        {
            this.texture = content.Load<Texture2D>("healthbar");
            this.position = position;
            mHealth = health;
            mMaxHealth = maxHealth;
        }

        public void AddHealth(int amount)
        {
            mHealth++;
        }
    }
}
