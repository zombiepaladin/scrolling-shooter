using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;

namespace ScrollingShooter
{
    /// <summary>
    /// Class to handle the cutscene after level three
    /// </summary>
    public class EndLevelThree : SplashScreen
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

        public EndLevelThree()
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
                    image = images.GetJaxon();
                    line = "Please surrender Kiefer. I don't want to kill you.";
                    if (Keyboard.GetState().IsKeyDown(Keys.Enter) && keyUp)
                    {
                        keyUp = false;
                        dialog = Dialog.conv2;
                    }
                    break;
                case Dialog.conv2:
                    image = images.GetKiefer2();
                    line = "It's not even close to over!  You have no idea what resources I have!.";
                    if (Keyboard.GetState().IsKeyDown(Keys.Enter) && keyUp)
                    {
                        keyUp = false;
                        dialog = Dialog.conv3;
                    }
                    break;
                case Dialog.conv3:
                    image = images.GetJaxon();
                    line = "Where are you getting these things?  They don't even look like normal weapons.  What have you \ngotten yourself into?";
                    if (Keyboard.GetState().IsKeyDown(Keys.Enter) && keyUp)
                    {
                        keyUp = false;
                        dialog = Dialog.conv4;
                    }
                    break;
                case Dialog.conv4:
                    image = images.GetKiefer2();
                    line = "Follow me then.  If you dare.  I will show you where I am getting these if you can catch me.  \nThen I will kill you. ";
                    if (Keyboard.GetState().IsKeyDown(Keys.Enter) && keyUp)
                    {
                        keyUp = false;
                        dialog = Dialog.conv5;
                    }
                    break;
                case Dialog.conv5:
                    image = images.GetAster();
                    line = "Jaxon, it's over.  Kradstan just issued a surrender.";
                    if (Keyboard.GetState().IsKeyDown(Keys.Enter) && keyUp)
                    {
                        keyUp = false;
                        dialog = Dialog.conv6;
                    }
                    break;
                case Dialog.conv6:
                    image = images.GetJaxon();
                    line = "I'm going after Kiefer.";
                    if (Keyboard.GetState().IsKeyDown(Keys.Enter) && keyUp)
                    {
                        keyUp = false;
                        dialog = Dialog.conv7;
                    }
                    break;
                case Dialog.conv7:
                    image = images.GetAster();
                    line = "I am assigning that to other pilots.  Without the threat to civilians I am sending more forces \nthat way.";
                    if (Keyboard.GetState().IsKeyDown(Keys.Enter) && keyUp)
                    {
                        keyUp = false;
                        dialog = Dialog.conv8;
                    }
                    break;
                case Dialog.conv8:
                    image = images.GetJaxon();
                    line = "I think Kiefer has gone into space.";
                    if (Keyboard.GetState().IsKeyDown(Keys.Enter) && keyUp)
                    {
                        keyUp = false;
                        dialog = Dialog.conv9;
                    }
                    break;
                case Dialog.conv9:
                    image = images.GetAster();
                    line = "Really?  Space?";
                    if (Keyboard.GetState().IsKeyDown(Keys.Enter) && keyUp)
                    {
                        keyUp = false;
                        dialog = Dialog.conv10;
                    }
                    break;
                case Dialog.conv10:
                    image = images.GetJaxon();
                    line = "Yes.  This ship has all kinds of features.  Can it survive in space?";
                    if (Keyboard.GetState().IsKeyDown(Keys.Enter) && keyUp)
                    {
                        keyUp = false;
                        dialog = Dialog.conv11;
                    }
                    break;
                case Dialog.conv11:
                    image = images.GetAster();
                    line = "Well...yes...it can.  We were prototyping it as a replacement for the space shuttle.";
                    if (Keyboard.GetState().IsKeyDown(Keys.Enter) && keyUp)
                    {
                        keyUp = false;
                        dialog = Dialog.conv12;
                    }
                    break;
                case Dialog.conv12:
                    image = images.GetJaxon();
                    line = "Then I'm following Kiefer.";
                    if (Keyboard.GetState().IsKeyDown(Keys.Enter) && keyUp)
                    {
                        keyUp = false;
                        dialog = Dialog.conv13;
                    }
                    break;
                case Dialog.conv13:
                    image = images.GetAster();
                    line = "Wait! Jaxon!";
                    if (Keyboard.GetState().IsKeyDown(Keys.Enter) && keyUp)
                    {
                        keyUp = false;
                        dialog = Dialog.conv14;
                    }
                    break;
                case Dialog.conv14:
                    image = new Rectangle();
                    line = "(Aster is disconnected)";
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
