using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace ScrollingShooter
{
    /// <summary>
    /// Represents the three animation frames for the Drill ship
    /// </summary>
    enum DrillSpinState
    {
        Spin1 = 0,
        Spin2 = 1,
    }

    enum DrillMoveState
    {
        Right = 0,
        Left = 1,
        Enter = 2,
    }

    class Drill : Enemy
    {
        // Drill state variables
        Texture2D spritesheet;
        Vector2 position;
        Vector2 velocity;
        Rectangle[] spriteBounds = new Rectangle[3];
        DrillSpinState SpinState = DrillSpinState.Spin1;
        DrillMoveState moveState;
        int timer;
        int attackTimer;
        bool attack;

        /// <summary>
        /// The bounding rectangle of the Drill
        /// </summary>
        public override Rectangle Bounds
        {
            get { return new Rectangle((int)position.X, (int)position.Y, spriteBounds[(int)SpinState].Width, spriteBounds[(int)SpinState].Height); }
        }

        /// <summary>
        /// Creates a new instance of a Drill enemy ship
        /// </summary>
        /// <param name="content">A ContentManager to load resources with</param>
        /// <param name="position">The position of the Drill ship in the game world</param>
        public Drill(uint id, ContentManager content, bool enterLeft) : base (id)
        {
            if (enterLeft)
            {
                position = new Vector2(0,-100);

            }
            else
            {
                position = new Vector2(800-25, -100);
            }

            moveState = DrillMoveState.Enter;

            spritesheet = content.Load<Texture2D>("Spritesheets/newshv.shp.000000");

            spriteBounds[(int)DrillSpinState.Spin1].X = 0;
            spriteBounds[(int)DrillSpinState.Spin1].Y = 0;
            spriteBounds[(int)DrillSpinState.Spin1].Width = 25;
            spriteBounds[(int)DrillSpinState.Spin1].Height = 57;

            spriteBounds[(int)DrillSpinState.Spin2].X = 24;
            spriteBounds[(int)DrillSpinState.Spin2].Y = 0;
            spriteBounds[(int)DrillSpinState.Spin2].Width = 25;
            spriteBounds[(int)DrillSpinState.Spin2].Height = 57;

            SpinState = DrillSpinState.Spin1;
            timer = 5;
            attackTimer = 20;
            attack = false;
            velocity = new Vector2(75, 400);
        }

        /// <summary>
        /// Updates the Drill ship
        /// </summary>
        /// <param name="elapsedTime">The in-game time between the previous and current frame</param>
        public override void  Update(float elapsedTime)
        {
            //Is the ship attacking?
            if (attack)
            {
                //The ship is attacking.  Animate drill.
                timer--;
                if (timer == 0)
                {
                    if (SpinState == DrillSpinState.Spin1)
                    {
                        SpinState = DrillSpinState.Spin2;
                    }
                    else
                    {
                        SpinState = DrillSpinState.Spin1;
                    }
                    timer = 5;
                }
                //Move down if ship attack timer is ready.
                if (attackTimer == 0)
                {
                    position.Y += elapsedTime * velocity.Y;
                }
                else
                {
                    attackTimer--;
                }
            }
            else
            {
                //The ship is not attacking.  Check for the player's position.
                PlayerShip player = ScrollingShooterGame.Game.Player;
                Vector2 playerPosition = new Vector2(player.Bounds.Center.X, player.Bounds.Center.Y);
                if (Math.Abs(position.X - playerPosition.X) <= 1.5 && moveState != DrillMoveState.Enter)
                {
                    attack = true;
                }
                else
                {
                    //Move depending on ship status.
                    switch (moveState)
                    {
                        case DrillMoveState.Enter:
                            position.Y += elapsedTime * 50;
                            if (Math.Abs(position.Y) <= 1 && position.X == 0) moveState = DrillMoveState.Right;
                            else if (Math.Abs(position.Y) <= 1) moveState = DrillMoveState.Left;
                            break;
                        case DrillMoveState.Right:
                            position.X += elapsedTime * velocity.X;
                            if (position.X > (800-25)) moveState = DrillMoveState.Left;
                            break;
                        case DrillMoveState.Left:
                            position.X -= elapsedTime * velocity.X;
                            if (position.X < 0) moveState = DrillMoveState.Right;
                            break;
                    }
                }
            }
        }

        /// <summary>
        /// Draw the Drill ship on-screen
        /// </summary>
        /// <param name="elapsedTime">The in-game time between the previous and current frame</param>
        /// <param name="spriteBatch">An already initialized SpriteBatch, ready for Draw() commands</param>
        public override void Draw(float elapsedTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(spritesheet, Bounds, spriteBounds[(int)SpinState], Color.White);
        }

    }
}