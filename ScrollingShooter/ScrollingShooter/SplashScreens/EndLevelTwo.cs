using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;

namespace ScrollingShooter
{
    /// <summary>
    /// Class to handle the cutscene after level two
    /// </summary>
    public class EndLevelTwo : SplashScreen
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

        public EndLevelTwo()
        {
            //initiates variables
            keyUp = true;
            Done = false;
            NextLevel = 3;

            //sets the scene to the first dialog text
            dialog = Dialog.conv1;

            //sets up the exit line
            exitLine = "Press [enter] to continue or press [S] to skip";

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
                    image = images.GetJaxon();
                    line = "This is pathetic. For all of their firepower they can't even stand up to a single ship.  \nI'm not sure what they are trying to prove here.";
                    if (Keyboard.GetState().IsKeyDown(Keys.Enter) && keyUp)
                    {
                        keyUp = false;
                        dialog = Dialog.conv2;
                    }
                    break;
                case Dialog.conv2:
                    image = images.GetAster();
                    line = "We still don't know where they got the equipment.  There may be more to this than we think.";
                    if (Keyboard.GetState().IsKeyDown(Keys.Enter) && keyUp)
                    {
                        keyUp = false;
                        dialog = Dialog.conv3;
                    }
                    break;
                case Dialog.conv3:
                    image = images.GetJaxon();
                    line = "There shouldn't be much more to clean up.  I'll be able to get back to my vact...";
                    if (Keyboard.GetState().IsKeyDown(Keys.Enter) && keyUp)
                    {
                        keyUp = false;
                        dialog = Dialog.conv4;
                    }
                    break;
                case Dialog.conv4:
                    image = images.GetKiefer1();
                    line = "(breaking in) You never did fully understand your enemy did you?";
                    if (Keyboard.GetState().IsKeyDown(Keys.Enter) && keyUp)
                    {
                        keyUp = false;
                        dialog = Dialog.conv5;
                    }
                    break;
                case Dialog.conv5:
                    image = images.GetJaxon();
                    line = "Who's that?";
                    if (Keyboard.GetState().IsKeyDown(Keys.Enter) && keyUp)
                    {
                        keyUp = false;
                        dialog = Dialog.conv6;
                    }
                    break;
                case Dialog.conv6:
                    image = images.GetAster();
                    line = "We're not sure.  Someone is in our channel and we can't get him out.  We're tracing the \nsource.";
                    if (Keyboard.GetState().IsKeyDown(Keys.Enter) && keyUp)
                    {
                        keyUp = false;
                        dialog = Dialog.conv7;
                    }
                    break;
                case Dialog.conv7:
                    image = images.GetKiefer1();
                    line = "Don't bother Aster.  I'll tell you who this is.  It's your old frend...the one you left \nbehind!";
                    if (Keyboard.GetState().IsKeyDown(Keys.Enter) && keyUp)
                    {
                        keyUp = false;
                        dialog = Dialog.conv8;
                    }
                    break;
                case Dialog.conv8:
                    image = images.GetJaxon();
                    line = "Brendon Kiefer?  How?";
                    if (Keyboard.GetState().IsKeyDown(Keys.Enter) && keyUp)
                    {
                        keyUp = false;
                        dialog = Dialog.conv9;
                    }
                    break;
                case Dialog.conv9:
                    image = images.GetKiefer1();
                    line = "I'm sure you have questions but I'm not here to answer them.  I asked for you Jaxon so I \ncould be the one to kill you.  Come to my mountain stronghold and face me.";
                    if (Keyboard.GetState().IsKeyDown(Keys.Enter) && keyUp)
                    {
                        keyUp = false;
                        dialog = Dialog.conv10;
                    }
                    break;
                case Dialog.conv10:
                    image = images.GetJaxon();
                    line = "Wait Kiefer!  How are you alive?";
                    if (Keyboard.GetState().IsKeyDown(Keys.Enter) && keyUp)
                    {
                        keyUp = false;
                        dialog = Dialog.conv11;
                    }
                    break;
                case Dialog.conv11:
                    image = images.GetAster();
                    line = "He is no longer in our channel.";
                    if (Keyboard.GetState().IsKeyDown(Keys.Enter) && keyUp)
                    {
                        keyUp = false;
                        dialog = Dialog.conv12;
                    }
                    break;
                case Dialog.conv12:
                    image = images.GetJaxon();
                    line = "I saw his plane explode over this country.  Could he have survived that?";
                    if (Keyboard.GetState().IsKeyDown(Keys.Enter) && keyUp)
                    {
                        keyUp = false;
                        dialog = Dialog.conv13;
                    }
                    break;
                case Dialog.conv13:
                    image = images.GetAster();
                    line = "We believe he did survive.  It was just too risky to go after him when you botched that mission.";
                    if (Keyboard.GetState().IsKeyDown(Keys.Enter) && keyUp)
                    {
                        keyUp = false;
                        dialog = Dialog.conv14;
                    }
                    break;
                case Dialog.conv14:
                    image = images.GetJaxon();
                    line = "You told me he was confirmed dead!";
                    if (Keyboard.GetState().IsKeyDown(Keys.Enter) && keyUp)
                    {
                        keyUp = false;
                        dialog = Dialog.conv15;
                    }
                    break;
                case Dialog.conv15:
                    image = images.GetAster();
                    line = "Jaxon, you don't have time for this now.  It may be a trap but it seems Kiefer is in league \nwith these people.  You must go to his stronghold and fight him.";
                    if (Keyboard.GetState().IsKeyDown(Keys.Enter) && keyUp)
                    {
                        keyUp = false;
                        dialog = Dialog.conv16;
                    }
                    break;
                case Dialog.conv16:
                    image = images.GetJaxon();
                    line = "Fine.  I want to know why you lied to me though.";
                    if (Keyboard.GetState().IsKeyDown(Keys.Enter) && keyUp)
                    {
                        keyUp = false;
                        dialog = Dialog.conv17;
                    }
                    break;
                case Dialog.conv17:
                    image = images.GetAster();
                    line = "We knew how close you where to him.  We didn't want you to go off and get yourself killed \ntrying to get him back.";
                    if (Keyboard.GetState().IsKeyDown(Keys.Enter) && keyUp)
                    {
                        keyUp = false;
                        dialog = Dialog.conv18;
                    }
                    break;
                case Dialog.conv18:
                    image = images.GetJaxon();
                    line = "...";
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
