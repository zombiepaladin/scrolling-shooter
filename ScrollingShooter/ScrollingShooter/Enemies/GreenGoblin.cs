﻿using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace ScrollingShooter
{

    /// <summary>
    /// Represents the three animation frames for the Green Goblin ship
    /// </summary>
    enum GreenGoblinSteeringState
    {
        Left = 0,
        Straight = 1,
        Right = 2,
    }

    /// <summary>
    /// An enemy ship that flies in a diagonal zig zag pattern. The length of the diagonal is randomly assigned when object is created.
    /// </summary>
    public class GreenGoblin : Enemy
    {
        // Green Goblin state variables
        Rectangle[] spriteBounds = new Rectangle[3];
        GreenGoblinSteeringState steeringState = GreenGoblinSteeringState.Straight;
        Random rand = new Random();
        // The number of pixels that the ship will move to the right or left before switching direction 
        int diagFlightLength;
        // Count of pixels currently moved 
        int diagCount;
        float gunTimer = 0;
        int screenWidth = 360; //Hardcoded until I get a better way to get the width

        /// <summary>
        /// The bounding rectangle of the Green Goblin
        /// </summary>
        public override Rectangle Bounds
        {
            get { return new Rectangle((int)position.X, (int)position.Y, spriteBounds[(int)steeringState].Width, spriteBounds[(int)steeringState].Height); }
        }

        /// <summary>
        /// Creates a new instance of a Green Goblin enemy ship
        /// </summary>
        /// <param name="content">A ContentManager to load resources with</param>
        /// <param name="position">The position of the Green Goblin ship in the game world</param>
        public GreenGoblin(uint id, ContentManager content, Vector2 position)
            : base(id)
        {
            // Randomly generates the diagonal flight length
            diagFlightLength = rand.Next(20, 150);
            this.position = position;
            diagCount = 0;
            this.Score = 12;

            spritesheet = content.Load<Texture2D>("Spritesheets/newshf.shp.000000");

            spriteBounds[(int)GreenGoblinSteeringState.Left].X = 0;
            spriteBounds[(int)GreenGoblinSteeringState.Left].Y = 0;
            spriteBounds[(int)GreenGoblinSteeringState.Left].Width = 25;
            spriteBounds[(int)GreenGoblinSteeringState.Left].Height = 26;

            spriteBounds[(int)GreenGoblinSteeringState.Straight].X = 25;
            spriteBounds[(int)GreenGoblinSteeringState.Straight].Y = 0;
            spriteBounds[(int)GreenGoblinSteeringState.Straight].Width = 25;
            spriteBounds[(int)GreenGoblinSteeringState.Straight].Height = 26;

            spriteBounds[(int)GreenGoblinSteeringState.Right].X = 50;
            spriteBounds[(int)GreenGoblinSteeringState.Right].Y = 0;
            spriteBounds[(int)GreenGoblinSteeringState.Right].Width = 25;
            spriteBounds[(int)GreenGoblinSteeringState.Right].Height = 26;

            steeringState = GreenGoblinSteeringState.Straight;
        }

        /// <summary>
        /// Updates the Green Goblin ship
        /// </summary>
        /// <param name="elapsedTime">The in-game time between the previous and current frame</param>
        public override void Update(float elapsedTime)
        {
            // Sense the player's position
            PlayerShip player = ScrollingShooterGame.Game.Player;
            Vector2 playerPosition = new Vector2(player.Bounds.Center.X, player.Bounds.Center.Y);

            gunTimer += elapsedTime;

            if (playerPosition.Y + 70 < this.position.Y) return;

            // Get the distance between the player and this along the X axis
            float playerDistance = Math.Abs(playerPosition.X - this.position.X);

            // Make sure the player is within range
            if (playerDistance < 60 && gunTimer > 0.25 && playerPosition.Y > (this.position.Y + 30))
            {
                ScrollingShooterGame.GameObjectManager.CreateProjectile(ProjectileType.EnemyFlameball, position);
                gunTimer = 0;
            }

            //Ship flies from top to bottom
            this.position.Y += 1;

            // Once diagonal motion has reach its length, switch directions
            if (diagCount == diagFlightLength || this.position.X >= screenWidth || this.position.X <= 5)
            {
                // If going right, switch to left
                if (0 == steeringState.CompareTo(GreenGoblinSteeringState.Right))
                {
                    this.position.X -= 2;
                    steeringState = GreenGoblinSteeringState.Left;
                    diagCount = 0;
                    rand = new Random();
                    diagFlightLength = rand.Next(20, 100);
                }

                // If going left, switch to right    
                else if (0 == steeringState.CompareTo(GreenGoblinSteeringState.Left))
                {
                    this.position.X += 2;
                    steeringState = GreenGoblinSteeringState.Right;
                    diagCount = 0;
                    rand = new Random();
                    diagFlightLength = rand.Next(20, 100);
                }

            }

            // If the ship hasn't reached the turning point
            else if (diagCount < diagFlightLength)
            {
                //If the ship has just been created
                if (0 == steeringState.CompareTo(GreenGoblinSteeringState.Straight))
                {
                    this.position.X += 2;
                    rand = new Random();
                    int direction = rand.Next(0, 10);
                    if (direction >= 5)
                    {
                        steeringState = GreenGoblinSteeringState.Right;
                    }
                    else if (direction < 5)
                    {
                        steeringState = GreenGoblinSteeringState.Left;
                    }

                    else
                    {
                        diagCount++;
                    }
                        diagCount++;
                }

                // If the ship is going right
                else if (0 == steeringState.CompareTo(GreenGoblinSteeringState.Right))
                {
                    this.position.X += 2;
                    diagCount++;
                }

                // If the ship is going left
                else if (0 == steeringState.CompareTo(GreenGoblinSteeringState.Left))
                {
                    this.position.X -= 2;
                    diagCount++;
                }
            }

            else
            {
                steeringState = GreenGoblinSteeringState.Straight;
            }

        }

        /// <summary>
        /// Draw the Green Goblin ship on-screen
        /// </summary>
        /// <param name="elapsedTime">The in-game time between the previous and current frame</param>
        /// <param name="spriteBatch">An already initialized SpriteBatch, ready for Draw() commands</param>
        public override void Draw(float elapsedTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(spritesheet, Bounds, spriteBounds[(int)steeringState], Color.White);
        }
		
		/// <summary>
        /// Scrolls the object with the map
        /// </summary>
        /// <param name="elapsedTime">The in-game time between the previous and current frame</param>
		public override void ScrollWithMap(float elapsedTime)
		{
			position.Y += elapsedTime * ScrollingSpeed;
		}

    }
}