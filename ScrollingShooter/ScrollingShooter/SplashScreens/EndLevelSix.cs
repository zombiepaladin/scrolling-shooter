using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;

namespace ScrollingShooter
{
    /// <summary>
    /// Class to handle the cutscene after level six
    /// </summary>
    public class EndLevelSix : SplashScreen
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

        public EndLevelSix()
        {
            //initiates variables
            keyUp = true;
            Done = false;
            NextLevel = 7;

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
                    line = "Finally!  That ends it...Whoah! I'm being sucked in!";
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
