using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using System;

namespace ScrollingShooter
{
    /// <summary>
    /// Represents the state of the blimp
    /// </summary>
    enum BlimpState
    {
        Normal = 0,
        Below25,
    }
    /// <summary>
    /// A blimp boss
    /// </summary>
    public class Blimp : Boss
    {
        // Blimp state variables
        Texture2D spritesheet;
        Vector2 position;
        Rectangle[] spriteBounds = new Rectangle[2];
        BlimpState state;
        int maxHealth = 300;
        Vector2 velocity;
        int screenWidth = 384; //Hardcoded until I get a better way to get the width
        float gunTimer;

        /// <summary>
        /// The bounding rectangle of the SuicideBomber
        /// </summary>
        public override Rectangle Bounds
        {
            get { return new Rectangle((int)position.X, (int)position.Y, spriteBounds[(int)state].Width, spriteBounds[(int)state].Height); }
        }

        /// <summary>
        /// The position of the blimp in the world
        /// </summary>
        public Vector2 Position
        {
            get { return this.position; }
        }

        /// <summary>
        /// Creates a new Blimp
        /// </summary>
        /// <param name="content">A ContentManager to load resources with</param>
        /// <param name="position">The position of the Blimp in the game world</param>
        public Blimp(uint id, Vector2 position, ContentManager content)
            : base(id)
        {
            this.position = position;

            spritesheet = content.Load<Texture2D>("Spritesheets/newshi.shp.000000");

            this.Health = (float)maxHealth;

            spriteBounds[(int)BlimpState.Normal].X = 13;
            spriteBounds[(int)BlimpState.Normal].Y = 10;
            spriteBounds[(int)BlimpState.Normal].Width = 70;
            spriteBounds[(int)BlimpState.Normal].Height = 130;

            spriteBounds[(int)BlimpState.Below25].X = 121;
            spriteBounds[(int)BlimpState.Below25].Y = 10;
            spriteBounds[(int)BlimpState.Below25].Width = 24;
            spriteBounds[(int)BlimpState.Below25].Height = 130;

            this.state = BlimpState.Normal;

            velocity = new Vector2(50, 0);

            this.gunTimer = 0;
        }

        /// <summary>
        /// Updates the Blimp
        /// </summary>
        /// <param name="elapsedTime">The in-game time between the previous and current frame</param>
        public override void Update(float elapsedTime)
        {
            // If the blimp is below 25% health switch the sprite
            if (this.Health / maxHealth < 0.25f) state = BlimpState.Below25;

            // If the blimp is at 0 health delete it
            else if (Health == 0)
            {
                ScrollingShooterGame.GameObjectManager.DestroyObject(this.ID);
                return;
            }

            // Move the blimp
            if (position.X - 11 <= 5 || position.X + 69 >= this.screenWidth - 30) velocity.X *= -1;
            position.X -= elapsedTime * velocity.X;

            this.gunTimer += elapsedTime;

            if (gunTimer >= 1f)
            {
                // Sense the player's position
                PlayerShip player = ScrollingShooterGame.Game.Player;
                Vector2 playerPosition = new Vector2(player.Bounds.Center.X, player.Bounds.Center.Y);

                // Get a vector from our position to the player's position
                Vector2 toPlayer = playerPosition - this.position;

                // Shoot the shotgun if the player is within 200 units of the blimp
                if (toPlayer.LengthSquared() < 25000)
                {
                    Vector2 travel = position;
                    travel.X += Bounds.Width / 2;
                    travel.Y += Bounds.Height / 2;
                    ScrollingShooterGame.GameObjectManager.CreateProjectile(ProjectileType.BlimpShotgun, travel);
                    gunTimer = 0;
                }
            }
        }

        /// <summary>
        /// Draw the Blimp body on-screen
        /// </summary>
        /// <param name="elapsedTime">The in-game time between the previous and current frame</param>
        /// <param name="spriteBatch">An already initialized SpriteBatch, ready for Draw() commands</param>
        public override void Draw(float elapsedTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(spritesheet, Bounds, spriteBounds[(int)state], Color.White);
        }
    }

    /// <summary>
    /// The LeftGun for the Blimp Boss
    /// </summary>
    public class LeftGun : Enemy
    {
        //LeftGun State Variables
        Texture2D spritesheet;
        Vector2 position;
        Rectangle spriteBounds;
        Blimp ship;
        float gunTimer;

        /// <summary>
        /// The bounding rectangle of the LeftGun
        /// </summary>
        public override Rectangle Bounds
        {
            get { return new Rectangle((int)position.X, (int)position.Y, spriteBounds.Width, spriteBounds.Height); }
        }

        /// <summary>
        /// Creates a new LeftGun
        /// </summary>
        /// <param name="content">A ContentManager to load resources with</param>
        /// <param name="position">The blimp the LeftGun is attached to</param>
        public LeftGun(uint id, ContentManager content, Blimp ship)
            : base(id)
        {
            this.position.X = ship.Position.X - 11;

            this.position.Y = ship.Position.Y + 36;
            spritesheet = content.Load<Texture2D>("Spritesheets/newshi.shp.000000");

            spriteBounds = new Rectangle(0, 47, 12, 63);

            this.Health = 100;

            this.ship = ship;

            this.gunTimer = 0;
        }

        /// <summary>
        /// Updates the LeftGun
        /// </summary>
        /// <param name="elapsedTime">The in-game time between the previous and current frame</param>
        public override void Update(float elapsedTime)
        {
            // If the LeftGun is dead delete it
            if (Health == 0)
            {
                ScrollingShooterGame.GameObjectManager.DestroyObject(this.ID);
                return;
            }

            // Updates the position of the LeftGun based on the position of the ship
            this.position.X = ship.Position.X - 11;
            this.position.Y = ship.Position.Y + 36;

            this.gunTimer += elapsedTime;

            if (gunTimer >= 0.50f)
            {
                // Sense the player's position
                PlayerShip player = ScrollingShooterGame.Game.Player;
                Vector2 playerPosition = new Vector2(player.Bounds.Center.X, player.Bounds.Center.Y);

                // Get a vector from our position to the player's position
                Vector2 toPlayer = playerPosition - this.position;

                // Shoots if the player is at the correct range
                if (toPlayer.LengthSquared() < 200000 && toPlayer.LengthSquared() > 22050)
                {
                    Vector2 travel = position;
                    travel.X += 8;
                    travel.Y += 62;
                    ScrollingShooterGame.GameObjectManager.CreateProjectile(ProjectileType.BlimpBullet, travel);
                    gunTimer = 0;
                }
            }
        }

        /// <summary>
        /// Draw the LeftGun on-screen
        /// </summary>
        /// <param name="elapsedTime">The in-game time between the previous and current frame</param>
        /// <param name="spriteBatch">An already initialized SpriteBatch, ready for Draw() commands</param>
        public override void Draw(float elapsedTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(spritesheet, Bounds, spriteBounds, Color.White);
        }
    }

    /// <summary>
    /// The RightGun for the Blimp Boss
    /// </summary>
    public class RightGun : Enemy
    {
        //LeftGun State Variables
        Texture2D spritesheet;
        Vector2 position;
        Rectangle spriteBounds;
        float gunTimer;
        Blimp ship;

        /// <summary>
        /// The bounding rectangle of the LeftGun
        /// </summary>
        public override Rectangle Bounds
        {
            get { return new Rectangle((int)position.X, (int)position.Y, spriteBounds.Width, spriteBounds.Height); }
        }

        /// <summary>
        /// Creates a new RightGun
        /// </summary>
        /// <param name="content">A ContentManager to load resources with</param>
        /// <param name="position">The blimp the RightGun is attached to</param>
        public RightGun(uint id, ContentManager content, Blimp ship)
            : base(id)
        {
            this.position.X = ship.Position.X + 69;
            this.position.Y = ship.Position.Y + 36;

            spritesheet = content.Load<Texture2D>("Spritesheets/newshi.shp.000000");

            spriteBounds = new Rectangle(84, 47, 13, 63);

            this.Health = 100;

            this.gunTimer = 0;

            this.ship = ship;
        }

        /// <summary>
        /// Updates the RightGun
        /// </summary>
        /// <param name="elapsedTime">The in-game time between the previous and current frame</param>
        public override void Update(float elapsedTime)
        {
            this.gunTimer += elapsedTime;

            // If the gun is dead please delete it
            if (Health == 0)
            {
                ScrollingShooterGame.GameObjectManager.DestroyObject(this.ID);
                return;
            }

            //Updates the position of the gun relative to the ship
            this.position.X = ship.Position.X + 69;
            this.position.Y = ship.Position.Y + 36;

            if (gunTimer >= 0.50f)
            {
                // Sense the player's position
                PlayerShip player = ScrollingShooterGame.Game.Player;
                Vector2 playerPosition = new Vector2(player.Bounds.Center.X, player.Bounds.Center.Y);

                // Get a vector from our position to the player's position
                Vector2 toPlayer = playerPosition - this.position;

                //Shoots if the player is at the correct range
                if (toPlayer.LengthSquared() < 200000 && toPlayer.LengthSquared() > 22050)
                {
                    Vector2 travel = position;
                    travel.X += 8;
                    travel.Y += 62;
                    ScrollingShooterGame.GameObjectManager.CreateProjectile(ProjectileType.BlimpBullet, travel);
                    gunTimer = 0;
                }
            }
        }

        /// <summary>
        /// Draw the RightGun on-screen
        /// </summary>
        /// <param name="elapsedTime">The in-game time between the previous and current frame</param>
        /// <param name="spriteBatch">An already initialized SpriteBatch, ready for Draw() commands</param>
        public override void Draw(float elapsedTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(spritesheet, Bounds, spriteBounds, Color.White);
        }
    }

    public class BlimpBullet : Projectile
    {
        /// <summary>
        /// Creates a new blimp bullet
        /// </summary>
        /// <param name="content">A ContentManager to load content from</param>
        /// <param name="position">A position on the screen</param>
        public BlimpBullet(uint id, ContentManager content, Vector2 position)
            : base(id)
        {
            this.spriteSheet = content.Load<Texture2D>("Spritesheets/tyrian.shp.000000");

            this.spriteBounds = new Rectangle(203, 56, 13, 14);

            this.position = position;

            // Sense the player's position
            PlayerShip player = ScrollingShooterGame.Game.Player;
            Vector2 playerPosition = new Vector2(player.Bounds.Center.X, player.Bounds.Center.Y);

            // Get a vector from our position to the player's position
            Vector2 toPlayer = playerPosition - this.position;

            velocity = toPlayer;

        }
    }

    /// <summary>
    /// Represents a bullet that is shot when the shotgun powerup is active
    /// </summary>
    public class BlimpShotgun : Projectile
    {
        // The direction the bullet is traveling
        BulletDirection direction;

        /// <summary>
        /// Creates a new shotgun bullet
        /// </summary>
        /// <param name="content">A ContentManager to load content from</param>
        /// <param name="position">The starting position of the bullet</param>
        /// <param name="bulletDirection"></param>
        public BlimpShotgun(uint id, ContentManager content, Vector2 position, BulletDirection bulletDirection)
            : base(id)
        {
            this.spriteSheet = content.Load<Texture2D>("Spritesheets/newsh(.shp.000000");

            direction = bulletDirection;

            this.spriteBounds = new Rectangle(146, 99, 7, 13);

            // Sets the velocity based on the direction the bullet should be headed in
            if (bulletDirection == BulletDirection.Right)
                this.velocity = new Vector2(-100, 500);

            else if (bulletDirection == BulletDirection.Left)
                this.velocity = new Vector2(100, 500);

            else if (bulletDirection == BulletDirection.HardLeft)
                this.velocity = new Vector2(200, 400);

            else if (bulletDirection == BulletDirection.HardRight)
                this.velocity = new Vector2(-200, 400);

            else
                this.velocity = new Vector2(0, 600);

            this.position = position;
        }

        /// <summary>
        /// Draws a shotgun bullet and rotates it in the direction it is traveling
        /// </summary>
        /// <param name="elapsedTime">The elapsed time</param>
        /// <param name="spriteBatch">An already-initialized spritebatch, ready for Draw() commands</param>
        public override void Draw(float elapsedTime, SpriteBatch spriteBatch)
        {
            if (direction == BulletDirection.Right)
                spriteBatch.Draw(spriteSheet, Bounds, spriteBounds, Color.White, 0.393f + (float)Math.PI, new Vector2(0, 0), new SpriteEffects(), 0);

            else if (direction == BulletDirection.Left)
                spriteBatch.Draw(spriteSheet, Bounds, spriteBounds, Color.White, -0.393f + (float)Math.PI, new Vector2(0, 0), new SpriteEffects(), 0);

            else if (direction == BulletDirection.HardLeft)
                spriteBatch.Draw(spriteSheet, Bounds, spriteBounds, Color.White, -0.785f + (float)Math.PI, new Vector2(0, 0), new SpriteEffects(), 0);

            else if (direction == BulletDirection.HardRight)
                spriteBatch.Draw(spriteSheet, Bounds, spriteBounds, Color.White, 0.785f + (float)Math.PI, new Vector2(0, 0), new SpriteEffects(), 0);

            else
                spriteBatch.Draw(spriteSheet, Bounds, spriteBounds, Color.White, (float)Math.PI, new Vector2(0, 0), new SpriteEffects(), 0);
        }
    }
}
