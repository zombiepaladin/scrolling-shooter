using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;

namespace ScrollingShooter
{
    /// <summary>
    /// Class to handle the beginning cutscene
    /// </summary>
    public class Beginning : SplashScreen
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

        public Beginning()
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
                    image = images.GetAster();
                    line = "I'm glad you could make it Jaxon.";
                    if (Keyboard.GetState().IsKeyDown(Keys.Enter) && keyUp)
                    {
                        keyUp = false;
                        dialog = Dialog.conv2;
                    }
                    break;
                case Dialog.conv2:
                    image = images.GetJaxon();
                    line = "Why did you call me in Commander Aster?  I'm supposed to be on my way to Italy to visit my wife's family.";
                    if (Keyboard.GetState().IsKeyDown(Keys.Enter) && keyUp)
                    {
                        keyUp = false;
                        dialog = Dialog.conv3;
                    }
                    break;
                case Dialog.conv3:
                    image = images.GetAster();
                    line = "You are not going to be able to keep those plans.  We've got a situation in Kradstand.  \nYou remember it don't you?";
                    if (Keyboard.GetState().IsKeyDown(Keys.Enter) && keyUp)
                    {
                        keyUp = false;
                        dialog = Dialog.conv4;
                    }
                    break;
                case Dialog.conv4:
                    image = images.GetJaxon();
                    line = "...Yes.  I had hoped to never go back.";
                    if (Keyboard.GetState().IsKeyDown(Keys.Enter) && keyUp)
                    {
                        keyUp = false;
                        dialog = Dialog.conv5;
                    }
                    break;
                case Dialog.conv5:
                    image = images.GetAster();
                    line = "Well, guess what. They have just attacked several neighboring countries and are threatening \ncivilian lives if anyone stands against them.";
                    if (Keyboard.GetState().IsKeyDown(Keys.Enter) && keyUp)
                    {
                        keyUp = false;
                        dialog = Dialog.conv6;
                    }
                    break;
                case Dialog.conv6:
                    image = images.GetJaxon();
                    line = "So why am I here?";
                    if (Keyboard.GetState().IsKeyDown(Keys.Enter) && keyUp)
                    {
                        keyUp = false;
                        dialog = Dialog.conv7;
                    }
                    break;
                case Dialog.conv7:
                    image = images.GetAster();
                    line = "For some reason you are the exception.  They want you to go against them...alone.";
                    if (Keyboard.GetState().IsKeyDown(Keys.Enter) && keyUp)
                    {
                        keyUp = false;
                        dialog = Dialog.conv8;
                    }
                    break;
                case Dialog.conv8:
                    image = images.GetJaxon();
                    line = "That's suicide!";
                    if (Keyboard.GetState().IsKeyDown(Keys.Enter) && keyUp)
                    {
                        keyUp = false;
                        dialog = Dialog.conv9;
                    }
                    break;
                case Dialog.conv9:
                    image = images.GetAster();
                    line = "Normally yes, but we have a special ship for you.  You should be able to stand up to an \narmy with this beauty.";
                    if (Keyboard.GetState().IsKeyDown(Keys.Enter) && keyUp)
                    {
                        keyUp = false;
                        dialog = Dialog.conv10;
                    }
                    break;
                case Dialog.conv10:
                    image = images.GetJaxon();
                    line = "What's the target?";
                    if (Keyboard.GetState().IsKeyDown(Keys.Enter) && keyUp)
                    {
                        keyUp = false;
                        dialog = Dialog.conv11;
                    }
                    break;
                case Dialog.conv11:
                    image = images.GetAster();
                    line = "First, attack their forward base.  We need to get a look at their hardware.";
                    if (Keyboard.GetState().IsKeyDown(Keys.Enter) && keyUp)
                    {
                        keyUp = false;
                        dialog = Dialog.conv12;
                    }
                    break;
                case Dialog.conv12:
                    image = images.GetJaxon();
                    line = "I guess I should call my wife.  She'll have to go without me.";
                    if (Keyboard.GetState().IsKeyDown(Keys.Enter) && keyUp)
                    {
                        keyUp = false;
                        dialog = Dialog.conv13;
                    }
                    break;
                case Dialog.conv13:
                    image = images.GetAster();
                    line = "Good luck.";
                    if (Keyboard.GetState().IsKeyDown(Keys.Enter) && keyUp)
                    {
                        keyUp = false;
                        dialog = Dialog.end;
                        Done = true;
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
