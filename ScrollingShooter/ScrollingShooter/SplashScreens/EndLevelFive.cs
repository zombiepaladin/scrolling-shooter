using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;

namespace ScrollingShooter
{
    /// <summary>
    /// Class to handle the cutscene after level five
    /// </summary>
    public class EndLevelFive : SplashScreen
    {
        /// <summary>
        /// Handles which state of the dialog the cutscene is currently in
        /// </summary>
        private enum Dialog
        {
            conv1,
            conv2,
            conv3,
            conv4,
            conv5,
            conv6,
            conv7,
            conv8,
            conv9,
            conv10,
            conv11,
            conv12,
            conv13,
            conv14,
            conv15,
            conv16,
            conv17,
            conv18,
            conv19,
            end
        }

        //base variables to be used throughout
        private SpriteFont spriteFont;
        private CutSceneImages images = new CutSceneImages();
        private Rectangle bounds;
        private Rectangle image;

        //the current line to display
        private string line;

        //boolean that checks if the enter button was released.  This is so that the conversation will not skip if the enter key 
        //is held down
        private bool keyUp;

        //the line that prints off information about how to go to the next dialog or skip the dialog completely
        private string exitLine;

        //enum to handle which dialog state the scene is currently in
        Dialog dialog;

        public EndLevelFive()
        {
            //initiates variables
            keyUp = true;
            Done = false;

            //sets the scene to the first dialog text
            dialog = Dialog.conv1;

            //sets up the exit line
            exitLine = "Press [enter] to continue or press [spacebar] to skip";

            //declares which sprite font to use
            spriteFont = ScrollingShooterGame.Game.Content.Load<SpriteFont>("SpriteFonts/Pescadero");

            //the bounds for each image
            bounds = images.GetBounds(new Vector2(300, 300));
        }

        /// <summary>
        /// updates the cut scene and allows it to progress through the conversation
        /// </summary>
        /// <param name="elapsedTime">The time passed between this and the previous frame</param>
        public override void Update(float elapsedTime)
        {
            //don't allow them to go to the next dialog without releasing the enter button
            if (Keyboard.GetState().IsKeyUp(Keys.Enter))
                keyUp = true;

            switch (dialog)
            {
                case Dialog.conv1:
                    image = images.GetKiefer4();
                    line = "(coughing) That was a good hit.  I don't think I'll make it.";
                    if (Keyboard.GetState().IsKeyDown(Keys.Enter) && keyUp)
                    {
                        keyUp = false;
                        dialog = Dialog.conv2;
                    }
                    break;
                case Dialog.conv2:
                    image = images.GetJaxon();
                    line = "What happened to you Kiefer?  Your face doesn't even look human anymore.";
                    if (Keyboard.GetState().IsKeyDown(Keys.Enter) && keyUp)
                    {
                        keyUp = false;
                        dialog = Dialog.conv3;
                    }
                    break;
                case Dialog.conv3:
                    image = images.GetKiefer4();
                    line = "They implanted something in me to make me different than everyone else.  It was supposed \nto make me invincible.  I guess you proved that wrong.";
                    if (Keyboard.GetState().IsKeyDown(Keys.Enter) && keyUp)
                    {
                        keyUp = false;
                        dialog = Dialog.conv4;
                    }
                    break;
                case Dialog.conv4:
                    image = images.GetJaxon();
                    line = "Why did you do this Kiefer?";
                    if (Keyboard.GetState().IsKeyDown(Keys.Enter) && keyUp)
                    {
                        keyUp = false;
                        dialog = Dialog.conv5;
                    }
                    break;
                case Dialog.conv5:
                    image = images.GetKiefer4();
                    line = "I was lost in enemy territory and no one was coming back for me.  They gave me a way out \nthat didn't involve getting put in prison or killed.  I had to take it.";
                    if (Keyboard.GetState().IsKeyDown(Keys.Enter) && keyUp)
                    {
                        keyUp = false;
                        dialog = Dialog.conv6;
                    }
                    break;
                case Dialog.conv6:
                    image = images.GetJaxon();
                    line = "How do I stop this?";
                    if (Keyboard.GetState().IsKeyDown(Keys.Enter) && keyUp)
                    {
                        keyUp = false;
                        dialog = Dialog.conv7;
                    }
                    break;
                case Dialog.conv7:
                    image = images.GetKiefer4();
                    line = "They're a hive mind...kill the queen and the rest will fall.  Jaxon...I'm sorry.";
                    if (Keyboard.GetState().IsKeyDown(Keys.Enter) && keyUp)
                    {
                        keyUp = false;
                        dialog = Dialog.conv8;
                    }
                    break;
                case Dialog.conv8:
                    image = images.GetJaxon();
                    line = "They corrupted you, my friend.  I will put an end to this.";
                    if (Keyboard.GetState().IsKeyDown(Keys.Enter) && keyUp)
                    {
                        keyUp = false;
                        dialog = Dialog.conv9;
                    }
                    break;
                case Dialog.conv9:
                    image = images.GetKiefer4();
                    line = "Good...luck.  I hope...you do.";
                    if (Keyboard.GetState().IsKeyDown(Keys.Enter) && keyUp)
                    {
                        keyUp = false;
                        dialog = Dialog.conv10;
                    }
                    break;
                case Dialog.conv10:
                    image = new Rectangle();
                    line = "(Kiefer dies)";
                    if (Keyboard.GetState().IsKeyDown(Keys.Enter) && keyUp)
                    {
                        keyUp = false;
                        dialog = Dialog.conv11;
                    }
                    break;
                case Dialog.conv11:
                    image = images.GetJaxon();
                    line = "(turning on the radio) Aster...Kiefer is dead.";
                    if (Keyboard.GetState().IsKeyDown(Keys.Enter) && keyUp)
                    {
                        keyUp = false;
                        dialog = Dialog.conv12;
                    }
                    break;
                case Dialog.conv12:
                    image = images.GetAster();
                    line = "Jaxon?  Thank God you're all right.  It's too bad about Kiefer but we've got a major \nsituation here.  We're getting shot at from the moon.";
                    if (Keyboard.GetState().IsKeyDown(Keys.Enter) && keyUp)
                    {
                        keyUp = false;
                        dialog = Dialog.conv13;
                    }
                    break;
                case Dialog.conv13:
                    image = images.GetAster();
                    line = "They just hit the ocean with enough force to completely drown the coast.  The entire \neastern seaboard is underwater right now.";
                    if (Keyboard.GetState().IsKeyDown(Keys.Enter) && keyUp)
                    {
                        keyUp = false;
                        dialog = Dialog.conv14;
                    }
                    break;
                case Dialog.conv14:
                    image = images.GetJaxon();
                    line = "There's an alien base up here.  I'm close to it now.";
                    if (Keyboard.GetState().IsKeyDown(Keys.Enter) && keyUp)
                    {
                        keyUp = false;
                        dialog = Dialog.conv15;
                    }
                    break;
                case Dialog.conv15:
                    image = images.GetAster();
                    line = "Jaxon...we've scanned the surface of the moon and we can see where the weapon will fire \nnext.  They've got it over central Europe.";
                    if (Keyboard.GetState().IsKeyDown(Keys.Enter) && keyUp)
                    {
                        keyUp = false;
                        dialog = Dialog.conv16;
                    }
                    break;
                case Dialog.conv16:
                    image = images.GetJaxon();
                    line = "Central Europe?  Could it hit Italy?";
                    if (Keyboard.GetState().IsKeyDown(Keys.Enter) && keyUp)
                    {
                        keyUp = false;
                        dialog = Dialog.conv17;
                    }
                    break;
                case Dialog.conv17:
                    image = images.GetAster();
                    line = "Yes.  If they get that off again a lot of people including your wife and family are going to die.";
                    if (Keyboard.GetState().IsKeyDown(Keys.Enter) && keyUp)
                    {
                        keyUp = false;
                        dialog = Dialog.conv18;
                    }
                    break;
                case Dialog.conv18:
                    image = images.GetJaxon();
                    line = "Don't worry, I will stop them.";
                    if (Keyboard.GetState().IsKeyDown(Keys.Enter) && keyUp)
                    {
                        Done = true;
                        keyUp = false;
                        dialog = Dialog.conv19;
                    }
                    break;
                case Dialog.conv19:
                    image = images.GetAster();
                    line = "I hope so.  Good luck Jaxon.";
                    if (Keyboard.GetState().IsKeyDown(Keys.Enter) && keyUp)
                    {
                        Done = true;
                        keyUp = false;
                        dialog = Dialog.end;
                    }
                    break;
            }
        }

        /// <summary>
        /// draws each of the elements on the screen
        /// </summary>
        /// <param name="elapsedTime">The time passed between this and the previous frame</param>
        /// <param name="spriteBatch">An already-initialized spritebatch, ready for Draw() commands</param>
        public override void Draw(float elapsedTime, Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch)
        {
            //draw the dialog text
            spriteBatch.DrawString(spriteFont, line, new Vector2(320, 230), Color.White);

            //draw the character portrait
            spriteBatch.Draw(images.GetSpriteSheet(), bounds, image, Color.White, 0f, new Vector2(bounds.Width / 2, bounds.Height / 2), SpriteEffects.None, 1f);

            //draw information on how to go to the next dialog or skip the cutscene
            spriteBatch.DrawString(spriteFont, exitLine, new Vector2(420, 550), Color.White);

        }
    }
}
