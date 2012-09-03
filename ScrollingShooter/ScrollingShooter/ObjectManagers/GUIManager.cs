using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ScrollingShooter.Gui;
using Microsoft.Xna.Framework.Graphics;

namespace ScrollingShooter.ObjectManagers
{
    public class GUIManager
    {
        private List<GUIObject> mGuiObjects = new List<GUIObject>();

        public void Add(GUIObject guiObj)
        {
            mGuiObjects.Add(guiObj);
        }
        public void DrawAll(float elapsedTime, SpriteBatch spriteBatch)
        {
            if (mGuiObjects.Count != 0)
            {
                foreach (GUIObject guiObj in mGuiObjects)
                {
                    guiObj.Draw(elapsedTime, spriteBatch);
                }
            }
        }
        public void UpdateAll(float elapsedTime)
        {
            if (mGuiObjects.Count != 0)
            {
                foreach (GUIObject guiObj in mGuiObjects)
                {
                    guiObj.Update(elapsedTime);
                }
            }
        }
    }
}
