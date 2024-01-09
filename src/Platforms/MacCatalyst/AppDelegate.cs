using Foundation;
using SpaceShooter.Game;
using UIKit;

namespace SpaceShooter
{
    [Register("AppDelegate")]
    public class AppDelegate : MauiUIApplicationDelegate
    {
        public override void PressesBegan(NSSet<UIPress> presses, UIPressesEvent evt)
        {
            //base.PressesBegan(presses, evt);

            foreach (UIPress press in presses)
            {
                if (press.Type == (UIPressType)2044) //SPACE
                {
                    MauiGame.KeyboardPressed(GameKey.Space);
                }
                else
                if (press.Type == (UIPressType)2080) //LEFT
                {
                    MauiGame.KeyboardPressed(GameKey.Left);
                }
                else
                if (press.Type == (UIPressType)2079) //RIGHT
                {
                    MauiGame.KeyboardPressed(GameKey.Right);
                }
            }
        }

        void ReleaseKeys(NSSet<UIPress> presses)
        {
            foreach (UIPress press in presses)
            {
                if (press.Type == (UIPressType)2044) //SPACE
                {
                    MauiGame.KeyboardReleased(GameKey.Space);
                }
                else
                if (press.Type == (UIPressType)2080) //LEFT
                {
                    MauiGame.KeyboardReleased(GameKey.Left);
                }
                else
                if (press.Type == (UIPressType)2079) //RIGHT
                {
                    MauiGame.KeyboardReleased(GameKey.Right);
                }
            }
        }

        public override void PressesEnded(NSSet<UIPress> presses, UIPressesEvent evt)
        {
            base.PressesEnded(presses, evt);

            ReleaseKeys(presses);
        }



        protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();
    }
}