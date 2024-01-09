using Foundation;
using UIKit;

namespace SpaceShooter
{
    [Register("AppDelegate")]
    public class AppDelegate : MauiUIApplicationDelegate
    {
        public override void PressesBegan(NSSet<UIPress> presses, UIPressesEvent evt)
        {
            base.PressesBegan(presses, evt);

            foreach (UIPress press in presses)
            {
                if (press.Type == (UIPressType)2044) //SPACE
                {

                }
                else
                if (press.Type == (UIPressType)2080) //LEFT
                {

                }
                else
                if (press.Type == (UIPressType)2079) //RIGHT
                {

                }
            }
        }



        protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();
    }
}