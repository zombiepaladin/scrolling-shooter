using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using System.Collections.Generic;
using System;

namespace ScrollingShooter
{
    enum AlienHeadPart
    {
        Forehead,
        Face,
        FaceAnimation1,
        FaceAnimation2,
        FaceAnimation3,
        FaceAnimation4
    }

    enum AlienHeadAnimation
    {
        Stage1 = 2,
        Stage2 = 3,
        Stage3 = 4,
        Stage4 = 5
    }

    enum AlienPhase
    {
        Wait,
        FirstPhase,
        SecondPhase,
        Breathing,
        Firing,
        Dead
    }

    enum AlienBreathingPhase
    {
        BreatheIn,
        Inhale,
        BreatheOut
    }

    /// <summary>
    /// The alien head enemy logic for the alien head boss
    /// </summary>
    public class AlienHead : Enemy
    {
        //sprite sheet containing the alien head
        Texture2D spritesheet;

        //position of the alien head on screen
        Vector2 position;

        //rectangle where the sprite exists on the spritesheet
        Rectangle[] spriteBounds = new Rectangle[6];

        //rectangle for each part of the head for drawing purposes
        Rectangle[] drawBounds = new Rectangle[3];

        AlienTurret[] turrets = new AlienTurret[4];

        //timer used for animation
        float timer;

        //integer used to determine animation frame
        int frame;

        //iteration to determine how many times the creature has fired
        int fired;

        //animated sequence
        List<AlienHeadAnimation> animationSequence;

        //a vector that represents the position of the mouth
        static Vector2 mouthPos = new Vector2(ScrollingShooterGame.Game.GraphicsDevice.Viewport.Width / 2, 100);

        //used to determine which stage the boss is on
        AlienPhase phase;
        AlienBreathingPhase breathingPhase;

        //random object to determine when to fire projectiles
        Random randomGen = new Random();

        //the generator objects that keep the boss from being vulnerable
        Enemy generator1;
        Enemy generator2;

        /// <summary>
        /// The bounding rectangle of the alien boss
        /// </summary>
        public override Rectangle Bounds
        {
            get { return new Rectangle((int)position.X, (int)position.Y, spriteBounds[(int)AlienHeadPart.Face].Width, spriteBounds[(int)AlienHeadPart.Face].Height); }
        }

        /// <summary>
        /// Creates a new instance of an alien head
        /// </summary>
        /// <param name="content">A ContentManager to load resources with</param>
        /// <param name="position">The position of the Dart ship in the game world</param>
        public AlienHead(uint id, ContentManager content, Vector2 position)
            : base(id)
        {
            this.position = position;
            phase = AlienPhase.Wait;
            timer = 0;

            spritesheet = content.Load<Texture2D>("Spritesheets/newshn.shp.000000");

            spriteBounds[(int)AlienHeadPart.Face].X = 7;
            spriteBounds[(int)AlienHeadPart.Face].Y = 56;
            spriteBounds[(int)AlienHeadPart.Face].Width = 113;
            spriteBounds[(int)AlienHeadPart.Face].Height = 94;
            drawBounds[(int)AlienHeadPart.Face] = new Rectangle((int)position.X, (int)position.Y, spriteBounds[(int)AlienHeadPart.Face].Width, spriteBounds[(int)AlienHeadPart.Face].Height);
  

            spriteBounds[(int)AlienHeadPart.Forehead].X = 24;
            spriteBounds[(int)AlienHeadPart.Forehead].Y = 2;
            spriteBounds[(int)AlienHeadPart.Forehead].Width = 72;
            spriteBounds[(int)AlienHeadPart.Forehead].Height = 54;
            drawBounds[(int)AlienHeadPart.Forehead] = new Rectangle((int)position.X + 18, (int)(position.Y - spriteBounds[(int)AlienHeadPart.Forehead].Height), spriteBounds[(int)AlienHeadPart.Forehead].Width, spriteBounds[(int)AlienHeadPart.Forehead].Height);

            spriteBounds[(int)AlienHeadPart.FaceAnimation1].X = 0;
            spriteBounds[(int)AlienHeadPart.FaceAnimation1].Y = 168;
            spriteBounds[(int)AlienHeadPart.FaceAnimation1].Width = 48;
            spriteBounds[(int)AlienHeadPart.FaceAnimation1].Height = 56;

            spriteBounds[(int)AlienHeadPart.FaceAnimation2].X = 48;
            spriteBounds[(int)AlienHeadPart.FaceAnimation2].Y = 168;
            spriteBounds[(int)AlienHeadPart.FaceAnimation2].Width = 48;
            spriteBounds[(int)AlienHeadPart.FaceAnimation2].Height = 56;

            spriteBounds[(int)AlienHeadPart.FaceAnimation3].X = 96;
            spriteBounds[(int)AlienHeadPart.FaceAnimation3].Y = 168;
            spriteBounds[(int)AlienHeadPart.FaceAnimation3].Width = 48;
            spriteBounds[(int)AlienHeadPart.FaceAnimation3].Height = 56;

            spriteBounds[(int)AlienHeadPart.FaceAnimation4].X = 144;
            spriteBounds[(int)AlienHeadPart.FaceAnimation4].Y = 168;
            spriteBounds[(int)AlienHeadPart.FaceAnimation4].Width = 48;
            spriteBounds[(int)AlienHeadPart.FaceAnimation4].Height = 56;
            
            
            //bounds for all of the face animations
            drawBounds[(int)AlienHeadPart.FaceAnimation1] = new Rectangle((int)position.X + 41, (int)position.Y + 28, spriteBounds[(int)AlienHeadPart.FaceAnimation1].Width, spriteBounds[(int)AlienHeadPart.FaceAnimation1].Height);
  
           //populating the list for the animation sequence
            animationSequence = new List<AlienHeadAnimation>();
            animationSequence.Add(AlienHeadAnimation.Stage1);
            animationSequence.Add(AlienHeadAnimation.Stage2);
            animationSequence.Add(AlienHeadAnimation.Stage3);
            animationSequence.Add(AlienHeadAnimation.Stage4);

            frame = -1;

            //setting up the shield generators
            generator1 = ScrollingShooterGame.GameObjectManager.CreateEnemy(EnemyType.ShieldGenerator, new Vector2(position.X + 70, position.Y + 100));
            generator2 = ScrollingShooterGame.GameObjectManager.CreateEnemy(EnemyType.ShieldGenerator, new Vector2(position.X - 70, position.Y + 100));

            //set up the turrets
            turrets[0] = (AlienTurret)ScrollingShooterGame.GameObjectManager.CreateEnemy(EnemyType.AlienTurret, new Vector2(position.X - 150, position.Y + 80));
            turrets[1] = (AlienTurret)ScrollingShooterGame.GameObjectManager.CreateEnemy(EnemyType.AlienTurret, new Vector2(position.X - 100, position.Y + 20));
            turrets[2] = (AlienTurret)ScrollingShooterGame.GameObjectManager.CreateEnemy(EnemyType.AlienTurret, new Vector2(position.X + 100, position.Y + 20));
            turrets[3] = (AlienTurret)ScrollingShooterGame.GameObjectManager.CreateEnemy(EnemyType.AlienTurret, new Vector2(position.X + 150, position.Y + 80));

            //set up the claws
            ScrollingShooterGame.GameObjectManager.CreateEnemy(EnemyType.LeftClaw, new Vector2(0, 250));
            ScrollingShooterGame.GameObjectManager.CreateEnemy(EnemyType.RightClaw, new Vector2(0, 250));
        }

        /// <summary>
        /// Updates the alien head
        /// </summary>
        /// <param name="elapsedTime">The in-game time between the previous and current frame</param>
        public override void Update(float elapsedTime)
        {
            //get a new random number
            int random = randomGen.Next(0, 100);
           

            switch (phase)
            {
                case AlienPhase.Wait:
                    timer += elapsedTime;
                    if (timer >= 1)
                    {
                        timer = 0;
                        phase = AlienPhase.FirstPhase;
                    }
                    break;
                
                case AlienPhase.FirstPhase:
                    if (generator1.Health <= 0 && generator2.Health <= 0)
                        phase = AlienPhase.SecondPhase;

                    if (random >= 1 && random <= 2)
                        ScrollingShooterGame.GameObjectManager.CreateProjectile(ProjectileType.Pincher, mouthPos);
                    break;
                case AlienPhase.SecondPhase:
                    timer += elapsedTime;

                    if (timer >= 5)
                    {
                        phase = AlienPhase.Breathing;
                        breathingPhase = AlienBreathingPhase.BreatheIn;
                        timer = 0;
                    }

                     if (random >= 1 && random <= 3)
                        ScrollingShooterGame.GameObjectManager.CreateProjectile(ProjectileType.Pincher, mouthPos);
                    break;

                case AlienPhase.Breathing:
                    timer += elapsedTime;
                    switch (breathingPhase)
                    {
                        case AlienBreathingPhase.BreatheIn:
                            if (timer >= .2)
                            {
                                timer = 0;
                                if (frame <= 2)
                                    frame++;
                                else
                                    breathingPhase = AlienBreathingPhase.Inhale;
                            }
                            break;

                        case AlienBreathingPhase.Inhale:
                            ScrollingShooterGame.Game.Player.MoveShip(mouthPos);
                            if(timer >= 3)
                                breathingPhase = AlienBreathingPhase.BreatheOut;
                            break;

                        case AlienBreathingPhase.BreatheOut:
                            if (timer >= .2)
                            {
                                timer = 0;
                                if (frame >= 1)
                                    frame--;
                                else
                                {
                                    phase = AlienPhase.Firing;
                                    fired = 0;
                                    timer = 1;
                                }
                            }
                        break;
                    }
                    break;

                case AlienPhase.Firing:
                    timer += elapsedTime;
                    if (timer >= 1)
                    {
                        if (fired <= 5)
                        {
                            for (int i = 0; i < 20; i++)
                            {
                                ScrollingShooterGame.GameObjectManager.CreateProjectile(ProjectileType.GreenOrb, mouthPos);
                            }
                            fired++;
                            timer = 0;
                        }
                        else
                        {
                            frame = -1;
                            timer = 0;

                            for (int i = 0; i < turrets.Length; i++)
                            {
                                if (turrets[i].isDead())
                                    turrets[i].ReviveTurret();
                            }

                            phase = AlienPhase.SecondPhase;
                        }
                    }
                        
                    
                    break;
            }
        

           /* 
            }*/
            
            /*
            // Sense the player's position
            PlayerShip player = ScrollingShooterGame.Game.player;
            Vector2 playerPosition = new Vector2(player.Bounds.Center.X, player.Bounds.Center.Y);

            // Get a vector from our position to the player's position
            Vector2 toPlayer = playerPosition - this.position;

            if (toPlayer.LengthSquared() < 40000)
            {
                // We sense the player's ship!                  
                // Get a normalized steering vector
                toPlayer.Normalize();

                // Steer towards them!
                //this.position += toPlayer * elapsedTime * 100;

                // Change the steering state to reflect our direction
                if (toPlayer.X < -0.5f) steeringState = DartSteeringState.Left;
                else if (toPlayer.X > 0.5f) steeringState = DartSteeringState.Right;
                else steeringState = DartSteeringState.Straight;
            }*/
        }

        /// <summary>
        /// Draw the alient head on-screen
        /// </summary>
        /// <param name="elapsedTime">The in-game time between the previous and current frame</param>
        /// <param name="spriteBatch">An already initialized SpriteBatch, ready for Draw() commands</param>
        public override void Draw(float elapsedTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(spritesheet, drawBounds[(int)AlienHeadPart.Forehead], spriteBounds[(int)AlienHeadPart.Forehead], Color.White, 0f, new Vector2(Bounds.Width / 2, Bounds.Height / 2), SpriteEffects.None, 1f);
            spriteBatch.Draw(spritesheet, drawBounds[(int)AlienHeadPart.Face], spriteBounds[(int)AlienHeadPart.Face], Color.White, 0f, new Vector2(Bounds.Width / 2, Bounds.Height / 2), SpriteEffects.None, 1f);
            if(frame >= 0)
                spriteBatch.Draw(spritesheet, drawBounds[(int)AlienHeadPart.FaceAnimation1], spriteBounds[(int)animationSequence[frame]], Color.White, 0f, new Vector2(Bounds.Width / 2, Bounds.Height / 2), SpriteEffects.None, 1f);            
        }

    }
}
