using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

//Author: Josh Zavala
namespace ScrollingShooter
{
    /// <summary>
    /// Represents the 8 positions Panzer can face
    /// </summary>
    enum PanzerAimState
    {
        North = 0,
        NorthEast,
        East,
        SouthEast,
        South,
        SouthWest,
        West,
        NorthWest,
    }

    /// <summary>
    /// A ground tank that shoot at the player if they are in range,
    /// moves slowly towards them.
    /// </summary>
    public class Panzer : Enemy
    {
        //the vars
        Texture2D spritesheet;
        Vector2 position;
        Rectangle[] spriteBounds = new Rectangle[8];
        PanzerAimState aimState = PanzerAimState.South;
        float defaultGunTimer = 0;

        /// <summary>
        /// The bounding rectangle of the Panzer
        /// </summary>
        public override Rectangle Bounds
        {
            get
            {
                return new Rectangle((int)position.X, (int)position.Y,
                    spriteBounds[(int)aimState].Width, spriteBounds[(int)aimState].Height);
            }
        }

        /// <summary>
        /// Create a new instance of a Panzer enemy tank.
        /// </summary>
        /// <param name="id">Obj id</param>
        /// <param name="content">A ContentManager to load resources with</param>
        /// <param name="position">The position of the Dart ship in the game world</param>
        public Panzer(uint id, ContentManager content, Vector2 position)
            : base(id)
        {
            this.position = position;
            spritesheet = content.Load<Texture2D>("Spritesheets/newsh2.shp.000000");

            spriteBounds[(int)PanzerAimState.North].X = 74;
            spriteBounds[(int)PanzerAimState.North].Y = 28;
            spriteBounds[(int)PanzerAimState.North].Width = 19;
            spriteBounds[(int)PanzerAimState.North].Height = 23;

            spriteBounds[(int)PanzerAimState.NorthEast].X = 193;
            spriteBounds[(int)PanzerAimState.NorthEast].Y = 58;
            spriteBounds[(int)PanzerAimState.NorthEast].Width = 25;
            spriteBounds[(int)PanzerAimState.NorthEast].Height = 21;

            spriteBounds[(int)PanzerAimState.East].X = 48;
            spriteBounds[(int)PanzerAimState.East].Y = 32;
            spriteBounds[(int)PanzerAimState.East].Width = 24;
            spriteBounds[(int)PanzerAimState.East].Height = 18;

            spriteBounds[(int)PanzerAimState.SouthEast].X = 193;
            spriteBounds[(int)PanzerAimState.SouthEast].Y = 89;
            spriteBounds[(int)PanzerAimState.SouthEast].Width = 24;
            spriteBounds[(int)PanzerAimState.SouthEast].Height = 19;

            spriteBounds[(int)PanzerAimState.South].X = 26;
            spriteBounds[(int)PanzerAimState.South].Y = 32;
            spriteBounds[(int)PanzerAimState.South].Width = 18;
            spriteBounds[(int)PanzerAimState.South].Height = 22;

            spriteBounds[(int)PanzerAimState.SouthWest].X = 170;
            spriteBounds[(int)PanzerAimState.SouthWest].Y = 88;
            spriteBounds[(int)PanzerAimState.SouthWest].Width = 20;
            spriteBounds[(int)PanzerAimState.SouthWest].Height = 19;

            spriteBounds[(int)PanzerAimState.West].X = 2;
            spriteBounds[(int)PanzerAimState.West].Y = 32;
            spriteBounds[(int)PanzerAimState.West].Width = 22;
            spriteBounds[(int)PanzerAimState.West].Height = 17;

            spriteBounds[(int)PanzerAimState.NorthWest].X = 170;
            spriteBounds[(int)PanzerAimState.NorthWest].Y = 58;
            spriteBounds[(int)PanzerAimState.NorthWest].Width = 20;
            spriteBounds[(int)PanzerAimState.NorthWest].Height = 20;

            aimState = PanzerAimState.South;

        }

        /// <summary>
        /// Updates the Panzer tank
        /// </summary>
        /// <param name="elapsedTime">In-game time between previous and current frame</param>
        public override void Update(float elapsedTime)
        {
            //Sense the player's position
            PlayerShip player = ScrollingShooterGame.Game.Player;
            Vector2 playerPosition = new Vector2(player.Bounds.Center.X,
                player.Bounds.Center.Y);

            //Get the vector from Panzer's position to the player's position
            Vector2 toPlayer = playerPosition - this.position;

            if ((toPlayer.LengthSquared() < 70000) && (toPlayer.LengthSquared() > 5000))
            {
                //points the Panzer towards the player
                toPlayer.Normalize();

                //chase the player
                this.position += toPlayer * elapsedTime * 25;
                
                //update the steering
                SteeringState(toPlayer);

                //fire cannon
                FireCannon(elapsedTime);
            }

            if (toPlayer.LengthSquared() <= 5000) //distance to sense
            {
                SteeringState(toPlayer); //updates the steering
                FireCannon(elapsedTime); //fires the cannon
            }
        }

        /// <summary>
        /// Draws the Panzer on-screen
        /// </summary>
        /// <param name="elapsedTime">In-game time between previous and current frame</param>
        /// <param name="spriteBatch">SpriteBatch using Draw()</param>
        public override void Draw(float elapsedTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(spritesheet, Bounds, spriteBounds[(int)aimState],
                Color.White, 0f, new Vector2(Bounds.Width / 2, Bounds.Height / 2),
                SpriteEffects.None, 1f);
        }

        /// <summary>
        /// Changes direction the Panzer faces relative to the Player
        /// </summary>
        /// <param name="toPlayer">Unit Vector in the direction of the Player</param>
        public void SteeringState(Vector2 toPlayer)
        {
            toPlayer.Normalize();
            if ((toPlayer.X < -0.5f) && (toPlayer.Y > 0.5f)) aimState = PanzerAimState.SouthWest;
            else if ((toPlayer.X < -0.5f) && (toPlayer.Y < -0.5f)) aimState = PanzerAimState.NorthWest;
            else if ((toPlayer.X > 0.5f) && (toPlayer.Y < -0.5f)) aimState = PanzerAimState.NorthEast;
            else if ((toPlayer.X > 0.5f) && (toPlayer.Y > 0.5f)) aimState = PanzerAimState.SouthEast;
            else if (toPlayer.X < -0.5f) aimState = PanzerAimState.West;
            else if (toPlayer.X > 0.5f) aimState = PanzerAimState.East;
            else if (toPlayer.Y < -0.5f) aimState = PanzerAimState.North;
            else aimState = PanzerAimState.South;
        }

        /// <summary>
        /// Fires a bullet from the Panzer towards the player
        /// </summary>
        /// <param name="elapsedTime">In-game time between previous and current frame</param>
        public void FireCannon(float elapsedTime)
        {
            defaultGunTimer += elapsedTime;
            if (defaultGunTimer > 2f)
            {                
                //Make use of the ToPlayerBullet class
                ScrollingShooterGame.GameObjectManager.CreateProjectile(ProjectileType.ToPlayerBullet, position);
                defaultGunTimer = 0;
            }
        }
    }
}
