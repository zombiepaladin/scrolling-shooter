using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace ScrollingShooter
{
    public class CutSceneImages
    {
        private Texture2D spritesheet;
      
        public CutSceneImages()
        {
            spritesheet = ScrollingShooterGame.Game.Content.Load<Texture2D>("Spritesheets/characterPotraits");
        }

        public Rectangle GetJaxon()
        {
            Rectangle spriteBounds = new Rectangle();
            
            spriteBounds.X = 0;
            spriteBounds.Y = 0;
            spriteBounds.Width = 48;
            spriteBounds.Height = 48;

            return spriteBounds;
        }

        public Rectangle GetAster()
        {
            Rectangle spriteBounds = new Rectangle();

            spriteBounds.X = 48;
            spriteBounds.Y = 0;
            spriteBounds.Width = 48;
            spriteBounds.Height = 48;

            return spriteBounds;
        }

        public Rectangle GetKiefer1()
        {
            Rectangle spriteBounds = new Rectangle();

            spriteBounds.X = 96;
            spriteBounds.Y = 0;
            spriteBounds.Width = 48;
            spriteBounds.Height = 48;

            return spriteBounds;
        }

        public Rectangle GetKiefer2()
        {
            Rectangle spriteBounds = new Rectangle();

            spriteBounds.X = 0;
            spriteBounds.Y = 48;
            spriteBounds.Width = 48;
            spriteBounds.Height = 48;

            return spriteBounds;
        }

        public Rectangle GetKiefer3()
        {
            Rectangle spriteBounds = new Rectangle();

            spriteBounds.X = 48;
            spriteBounds.Y = 48;
            spriteBounds.Width = 48;
            spriteBounds.Height = 48;

            return spriteBounds;
        }

        public Rectangle GetKiefer4()
        {
            Rectangle spriteBounds = new Rectangle();

            spriteBounds.X = 96;
            spriteBounds.Y = 48;
            spriteBounds.Width = 48;
            spriteBounds.Height = 48;

            return spriteBounds;
        }

        public Texture2D GetSpriteSheet()
        {
            return spritesheet;
        }

        public Rectangle GetBounds(Vector2 pos)
        {
            Rectangle bounds = new Rectangle();

            bounds.X = (int)pos.X;
            bounds.Y = (int)pos.Y;
            bounds.Width = 96;
            bounds.Height = 96;

            return bounds;
        }
    }
}
