using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace ScrollingShooter
{
    enum AlienClawParts
    {
        Claw,
        Shaft
    }

    enum AlienClawPhase
    {
        Wait,
        Begin,
        Hover,
        Attack,
        Extend,
        Retract,
        Stop
    }

    /// <summary>
    /// A claw that extends towards the player from the right side of the screen
    /// </summary>
    public class RightClaw : Enemy
    {
        Texture2D spritesheet;
        Vector2 position;
        Rectangle[] spriteBounds = new Rectangle[2];

        Rectangle[] drawBounds = new Rectangle[2];
        float timer;
        AlienClawPhase phase;

        int screenWidth = ScrollingShooterGame.Game.GraphicsDevice.Viewport.Width;
        /// <summary>
        /// The bounding rectangle of the Claw
        /// </summary>
        public override Rectangle Bounds
        {
            get { return new Rectangle((int)position.X, (int)position.Y, spriteBounds[(int)AlienClawParts.Shaft].Width + spriteBounds[(int)AlienClawParts.Claw].Width + 50, spriteBounds[(int)AlienClawParts.Claw].Height); }
        }

        /// <summary>
        /// Creates a new instance of the Right Claw
        /// </summary>
        /// <param name="content">A ContentManager to load resources with</param>
        /// <param name="position">The position of the claw in the game world</param>
        public RightClaw(uint id, ContentManager content, Vector2 position)
            : base(id)
        {
            this.position.Y = position.Y;
            this.position.X = ScrollingShooterGame.Game.GraphicsDevice.Viewport.Width + 200;
            phase = AlienClawPhase.Wait;
           
            spritesheet = content.Load<Texture2D>("Spritesheets/newsh4.shp.000000");

            spriteBounds[(int)AlienClawParts.Claw].X = 26;
            spriteBounds[(int)AlienClawParts.Claw].Y = 171;
            spriteBounds[(int)AlienClawParts.Claw].Width = 70;
            spriteBounds[(int)AlienClawParts.Claw].Height = 23;
            drawBounds[(int)AlienClawParts.Claw] = new Rectangle((int)this.position.X, (int)this.position.Y, spriteBounds[(int)AlienClawParts.Claw].Width, spriteBounds[(int)AlienClawParts.Claw].Height);
 
            spriteBounds[(int)AlienClawParts.Shaft].X = 0;
            spriteBounds[(int)AlienClawParts.Shaft].Y = 197;
            spriteBounds[(int)AlienClawParts.Shaft].Width = 96;
            spriteBounds[(int)AlienClawParts.Shaft].Height = 22;
            drawBounds[(int)AlienClawParts.Shaft] = new Rectangle((int)this.position.X + spriteBounds[(int)AlienClawParts.Claw].Width, (int)this.position.Y, spriteBounds[(int)AlienClawParts.Shaft].Width, spriteBounds[(int)AlienClawParts.Shaft].Height);
        
            
}
        /// <summary>
        /// Updates the Claw
        /// </summary>
        /// <param name="elapsedTime">The in-game time between the previous and current frame</param>
        public override void Update(float elapsedTime)
        {
            timer += elapsedTime;
            switch (phase)
            {
               case AlienClawPhase.Wait:
                    if (timer < 1)
                        drawBounds[(int)AlienClawParts.Claw].X = screenWidth + 120;
                    else
                    {
                        timer = 0;
                        phase = AlienClawPhase.Begin;
                    }
                    break;

                case AlienClawPhase.Begin:
                    if (timer >= .1)
                    {
                        timer = 0;
                        drawBounds[(int)AlienClawParts.Claw].X -= 5;
                        if (drawBounds[(int)AlienClawParts.Claw].X < screenWidth + 60)
                            phase = AlienClawPhase.Hover;
                        
                    }
                    break;

                case AlienClawPhase.Hover:
                     if (timer >= 1)
                     {
                        timer = 0;
                        phase = AlienClawPhase.Attack;
                     }
                     break;

                case AlienClawPhase.Attack:
                    if(timer >= .1)
                    {
                        timer = 0;
                        drawBounds[(int)AlienClawParts.Claw].X -= 30;
                        if (drawBounds[(int)AlienClawParts.Claw].X < screenWidth - 25)
                            phase = AlienClawPhase.Extend;
                    }
                    break;

                case AlienClawPhase.Extend:
                    if (timer >= 1)
                    {
                        timer = 0;
                        phase = AlienClawPhase.Retract;
                    }
                    break;

                case AlienClawPhase.Retract:
                    if (timer >= .1)
                    {
                        timer = 0;
                        drawBounds[(int)AlienClawParts.Claw].X += 20;
                        if (drawBounds[(int)AlienClawParts.Claw].X >= screenWidth + 120)
                            phase = AlienClawPhase.Wait;
                    }
                    break;
            }

            drawBounds[(int)AlienClawParts.Shaft].X = drawBounds[(int)AlienClawParts.Claw].X + spriteBounds[(int)AlienClawParts.Claw].Width;
        }

        /// <summary>
        /// Draw the Claw on-screen
        /// </summary>
        /// <param name="elapsedTime">The in-game time between the previous and current frame</param>
        /// <param name="spriteBatch">An already initialized SpriteBatch, ready for Draw() commands</param>
        public override void Draw(float elapsedTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(spritesheet, drawBounds[(int)AlienClawParts.Shaft], spriteBounds[(int)AlienClawParts.Shaft], Color.White, 0f, new Vector2(Bounds.Width / 2, Bounds.Height / 2), SpriteEffects.None, 1f);
            spriteBatch.Draw(spritesheet, drawBounds[(int)AlienClawParts.Claw], spriteBounds[(int)AlienClawParts.Claw], Color.White, 0f, new Vector2(Bounds.Width / 2, Bounds.Height / 2), SpriteEffects.None, 1f);
    
        }

        public void Attack(int y)
        {
            position.Y = y;
            phase = AlienClawPhase.Wait;
        }

    }
}