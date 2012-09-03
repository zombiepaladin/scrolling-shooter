using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
        public GUIHealthBar(int health, int maxHealth) :
            base()
        {
            mHealth = health;
            mMaxHealth = maxHealth;
        }

        public void AddHealth(int amount)
        {
            mHealth++;
        }
    }
}
