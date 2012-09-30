using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace ScrollingShooter
{
    /// <summary>
    /// Enum to hold the 8 turret directions.
    /// </summary>
    enum TurretSteeringState
    {
        TopLeft = 0,
        Top = 1,
        TopRight = 2,
        Right = 3,
        BottomRight = 4,
        Bottom = 5,
        BottomLeft = 6,
        Left = 7,
    }

    /// <summary>
    /// Basic turret class.
    /// </summary>
    public class Turret : Enemy
    {
        Texture2D spritesheet;
        Vector2 position;
        Rectangle[] spriteBounds = new Rectangle[8];
        TurretSteeringState steeringState = TurretSteeringState.Bottom;
        float turretGunTimer = 0;

        /// <summary>
        /// Not sure if this is needed for a moving turret.
        /// </summary>
        public override Rectangle Bounds
        {
            get { return new Rectangle((int)position.X, (int)position.Y, spriteBounds[(int)steeringState].Width, spriteBounds[(int)steeringState].Height); }
        }

        /// <summary>
        /// New turret class.
        /// </summary>
        /// <param name="content">Content Manager</param>
        /// <param name="position">The turrets sitting position.</param>
        public Turret(uint id, ContentManager content, Vector2 position) : base (id)
        {
            this.position = position;

            spritesheet = content.Load<Texture2D>("Spritesheets/newshp.shp.000000");

            spriteBounds[(int)TurretSteeringState.TopLeft].X = 144;
            spriteBounds[(int)TurretSteeringState.TopLeft].Y = 0;
            spriteBounds[(int)TurretSteeringState.TopLeft].Width = 24;
            spriteBounds[(int)TurretSteeringState.TopLeft].Height = 28;

            spriteBounds[(int)TurretSteeringState.Top].X = 168;
            spriteBounds[(int)TurretSteeringState.Top].Y = 0;
            spriteBounds[(int)TurretSteeringState.Top].Width = 24;
            spriteBounds[(int)TurretSteeringState.Top].Height = 28;

            spriteBounds[(int)TurretSteeringState.TopRight].X = 192;
            spriteBounds[(int)TurretSteeringState.TopRight].Y = 0;
            spriteBounds[(int)TurretSteeringState.TopRight].Width = 24;
            spriteBounds[(int)TurretSteeringState.TopRight].Height = 28;

            spriteBounds[(int)TurretSteeringState.Right].X = 192;
            spriteBounds[(int)TurretSteeringState.Right].Y = 28;
            spriteBounds[(int)TurretSteeringState.Right].Width = 24;
            spriteBounds[(int)TurretSteeringState.Right].Height = 28;

            spriteBounds[(int)TurretSteeringState.BottomRight].X = 192;
            spriteBounds[(int)TurretSteeringState.BottomRight].Y = 56;
            spriteBounds[(int)TurretSteeringState.BottomRight].Width = 24;
            spriteBounds[(int)TurretSteeringState.BottomRight].Height = 28;

            spriteBounds[(int)TurretSteeringState.Bottom].X = 168;
            spriteBounds[(int)TurretSteeringState.Bottom].Y = 56;
            spriteBounds[(int)TurretSteeringState.Bottom].Width = 24;
            spriteBounds[(int)TurretSteeringState.Bottom].Height = 28;

            spriteBounds[(int)TurretSteeringState.BottomLeft].X = 144;
            spriteBounds[(int)TurretSteeringState.BottomLeft].Y = 56;
            spriteBounds[(int)TurretSteeringState.BottomLeft].Width = 24;
            spriteBounds[(int)TurretSteeringState.BottomLeft].Height = 28;

            spriteBounds[(int)TurretSteeringState.Left].X = 144;
            spriteBounds[(int)TurretSteeringState.Left].Y = 28;
            spriteBounds[(int)TurretSteeringState.Left].Width = 24;
            spriteBounds[(int)TurretSteeringState.Left].Height = 28;

            steeringState = TurretSteeringState.Bottom;
        }

        /// <summary>
        /// It's update function that shoots out TurretFireballs if the PlayerShip
        /// is in range.
        /// </summary>
        /// <param name="elapsedTime">Elapsed time of the update.</param>
        public override void Update(float elapsedTime)
        {
            PlayerShip player = ScrollingShooterGame.Game.Player;
            Vector2 playerPosition = new Vector2(player.Bounds.Center.X, player.Bounds.Center.Y);

            Vector2 toPlayer = playerPosition - this.position;
            turretGunTimer+=elapsedTime;

            if (toPlayer.LengthSquared() < 95000)
            {
                toPlayer.Normalize();

                // Unit vector numbers. Should turn evenly in 8 directions.
                if (toPlayer.X > 0.92f) steeringState = TurretSteeringState.Right;
                else if (toPlayer.X < -0.92f) steeringState = TurretSteeringState.Left;
                else if (-toPlayer.Y > 0.92f) steeringState = TurretSteeringState.Top;
                else if (-toPlayer.Y < -0.92f) steeringState = TurretSteeringState.Bottom;
                else if (toPlayer.X > 0.38f && -toPlayer.Y > 0.38f) steeringState = TurretSteeringState.TopRight;
                else if (toPlayer.X > 0.38f && -toPlayer.Y < -0.38f) steeringState = TurretSteeringState.BottomRight;
                else if (toPlayer.X < -0.38f && -toPlayer.Y > 0.38f) steeringState = TurretSteeringState.TopLeft;
                else steeringState = TurretSteeringState.BottomLeft;

                // Turret firing speed can be adjusted here.
                if (turretGunTimer > 1.75f)
                {
                    ScrollingShooterGame.GameObjectManager.CreateProjectile(ProjectileType.TurretFireball, position);
                    turretGunTimer = 0f;
                }

               
            }
        }

        /// <summary>
        /// Draw function.
        /// </summary>
        /// <param name="elapsedTime">Game elapsed time.</param>
        /// <param name="spriteBatch">Game spritebatch.</param>
        public override void Draw(float elapsedTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(spritesheet, Bounds, spriteBounds[(int)steeringState], Color.White);
        }
    }
}