using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace ScrollingShooter.Gui
{
    public abstract class GUIObject : GameObject
    {
        protected Vector2 position;
        protected Texture2D texture;
        protected  Boolean IsClicked;

        public override Microsoft.Xna.Framework.Rectangle Bounds
        {
            get { return new Rectangle((int)position.X, (int)position.Y, texture.Width, texture.Height); }
        }

        public override void Update(float elapsedTime)
        {
            MouseState mouse = Mouse.GetState();
            if (MouseOverSprite(new Vector2(mouse.X, mouse.Y)))
            {
                IsClicked = true;
            }
            else IsClicked = false;
        }

        public override void Draw(float elapsedTime, Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, Bounds, Color.White);
        }

        bool MouseOverSprite(Vector2 mousePosition)
        {
            if ((mousePosition.X >= position.X) && mousePosition.X < (position.X + texture.Width) &&
            mousePosition.Y >= position.Y && mousePosition.Y < (position.Y + texture.Height))
                return true;
            else return false;
        } 
    }
}
