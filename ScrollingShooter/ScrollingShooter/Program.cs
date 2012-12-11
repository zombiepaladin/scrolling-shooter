using System;

namespace ScrollingShooter
{
#if WINDOWS || XBOX
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            using (ScrollingShooterGame game = new ScrollingShooterGame())
            {
                game.Run();
            }
        }
    }
#endif
}

