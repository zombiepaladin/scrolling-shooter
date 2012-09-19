using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

//Author: Josh Zavala
namespace ScrollingShooter
{
    //Enum for the Photon's changing graphics state
    enum PhotonState
    {
        One,
        Two,
        Three,
        Four,
        Five,
        Six,
        Seven,
        Eight,
        Nine,
        Ten,
    }

    /// <summary>
    /// Class controlling the Photon enemy projectile
    /// </summary>
    public class Photon : Projectile
    {
        //vars
        private PhotonState photonState;    //graphics state var
        private PlayerShip player;          //player object
        private Vector2 toPlayer;           //vector from Photon pos to player pos
        private Rectangle[] spriteBounds;   //can't use inherited spriteBounds because it's not defined as a []
        private Vector2 lastPlayerPosition; //When Photon stops homing, used for normal vector travel
        private float photonStateTimer;     //graphics timer var
        private bool homing;                //Photon homing bool

        /// <summary>
        /// The bounding rectangle of the Photon including
        /// the changing states
        /// </summary>
        public override Rectangle Bounds
        {
            get
            {
                return new Rectangle((int)position.X, (int)position.Y,
                    spriteBounds[(int)photonState].Width, spriteBounds[(int)photonState].Height);
            }
        }

        /// <summary>
        /// Create a new instance of a Photon projectile
        /// </summary>
        /// <param name="id">Obj id</param>
        /// <param name="content">A ContentManager to load resources with</param>
        /// <param name="position">The position of the Photon projectile in the game world</param>
        public Photon(uint id, ContentManager content, Vector2 position)
            : base(id)
        {
            this.position = position;
            this.velocity = new Vector2(100);
            this.spriteSheet = content.Load<Texture2D>("Spritesheets/accessories");
            spriteBounds = new Rectangle[10];
            player = ScrollingShooterGame.Game.player;
            photonStateTimer = 0;
            homing = true;

            spriteBounds[(int)PhotonState.One].X = 108;
            spriteBounds[(int)PhotonState.One].Y = 3;
            spriteBounds[(int)PhotonState.One].Width = 9;
            spriteBounds[(int)PhotonState.One].Height = 7;

            spriteBounds[(int)PhotonState.Two].X = 96;
            spriteBounds[(int)PhotonState.Two].Y = 2;
            spriteBounds[(int)PhotonState.Two].Width = 9;
            spriteBounds[(int)PhotonState.Two].Height = 9;

            spriteBounds[(int)PhotonState.Three].X = 83;
            spriteBounds[(int)PhotonState.Three].Y = 1;
            spriteBounds[(int)PhotonState.Three].Width = 11;
            spriteBounds[(int)PhotonState.Three].Height = 11;

            spriteBounds[(int)PhotonState.Four].X = 71;
            spriteBounds[(int)PhotonState.Four].Y = 1;
            spriteBounds[(int)PhotonState.Four].Width = 11;
            spriteBounds[(int)PhotonState.Four].Height = 11;

            spriteBounds[(int)PhotonState.Five].X = 59;
            spriteBounds[(int)PhotonState.Five].Y = 1;
            spriteBounds[(int)PhotonState.Five].Width = 11;
            spriteBounds[(int)PhotonState.Five].Height = 11;

            spriteBounds[(int)PhotonState.Six].X = 47;
            spriteBounds[(int)PhotonState.Six].Y = 1;
            spriteBounds[(int)PhotonState.Six].Width = 11;
            spriteBounds[(int)PhotonState.Six].Height = 11;

            spriteBounds[(int)PhotonState.Seven].X = 35;
            spriteBounds[(int)PhotonState.Seven].Y = 1;
            spriteBounds[(int)PhotonState.Seven].Width = 11;
            spriteBounds[(int)PhotonState.Seven].Height = 11;

            spriteBounds[(int)PhotonState.Eight].X = 23;
            spriteBounds[(int)PhotonState.Eight].Y = 1;
            spriteBounds[(int)PhotonState.Eight].Width = 11;
            spriteBounds[(int)PhotonState.Eight].Height = 11;

            spriteBounds[(int)PhotonState.Nine].X = 11;
            spriteBounds[(int)PhotonState.Nine].Y = 1;
            spriteBounds[(int)PhotonState.Nine].Width = 11;
            spriteBounds[(int)PhotonState.Nine].Height = 11;

            spriteBounds[(int)PhotonState.Ten].X = 0;
            spriteBounds[(int)PhotonState.Ten].Y = 0;
            spriteBounds[(int)PhotonState.Ten].Width = 10;
            spriteBounds[(int)PhotonState.Ten].Height = 13;

            photonState = PhotonState.One;
        }

        /// <summary>
        /// Updates the Photon's position and graphics state
        /// </summary>
        /// <param name="elapsedTime">The time elapsed between the previous and current frame</param>
        public override void Update(float elapsedTime)
        {
            //Get the vector to the player
            toPlayer = player.GetPosition() - this.position;
            
            //determine how to chase the player
            if (homing)
            {
                //Photon 1st fired and if player is far away
                if (toPlayer.LengthSquared() > 3000)
                {
                    //chase the player
                    toPlayer.Normalize();
                    this.position += toPlayer * elapsedTime * this.velocity;
                }
                else //Become a dummy and go straight
                {
                    homing = false;
                    lastPlayerPosition = toPlayer;
                } 
            }
            else //When near the player
            {
                lastPlayerPosition.Normalize();
                this.position += elapsedTime * lastPlayerPosition * this.velocity;
            }

            //Update the Photon graphics state
            UpdatePhotonState(elapsedTime);
        }

        /// <summary>
        /// Helper mehtod to update the Photon's graphic state
        /// </summary>
        /// <param name="elapsedTime">The time elapsed between the previous and current frame</param>
        public void UpdatePhotonState(float elapsedTime)
        {
            photonStateTimer += elapsedTime;
            if (photonStateTimer > .05f)
            {
                //enumerate PhotonState by 1 to next state
                if ((int)photonState < 9) photonState++;
                else photonState--;
                photonStateTimer = 0;
            }
        }

        /// <summary>
        /// Draws the Photon on-screen
        /// </summary>
        /// <param name="elapsedTime">The time elapsed between the previous and current frame</param>
        /// <param name="spriteBatch">An already initialized sprite batch</param>
        public override void Draw(float elapsedTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(this.spriteSheet, Bounds, spriteBounds[(int)photonState],
                Color.White, 0f, new Vector2(Bounds.Width / 2, Bounds.Height / 2),
                SpriteEffects.None, 1f);
        }
    }
}
