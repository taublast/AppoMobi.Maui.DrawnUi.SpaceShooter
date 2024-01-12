using Microsoft.UI.Xaml;
using SpaceShooter.Game;
using Windows.System;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace SpaceShooter.WinUI
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    public partial class App : MauiWinUIApplication
    {
        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {
            this.InitializeComponent();
        }

        protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();

        protected override void OnLaunched(LaunchActivatedEventArgs args)
        {
            base.OnLaunched(args);

            var appWindow = this.Application.Windows.First() as Microsoft.Maui.Controls.Window;
            var window = appWindow.Handler.PlatformView as Microsoft.Maui.MauiWinUIWindow;

            //hook keys
            var native = window.Content as Microsoft.UI.Xaml.Controls.Panel;
            native.PreviewKeyUp += (sender, args) =>
            {
                if (args.Key == VirtualKey.Space)
                {
                    MauiGame.KeyboardReleased(GameKey.Space);
                }
                else
                if (args.Key == VirtualKey.Left)
                {
                    MauiGame.KeyboardReleased(GameKey.Left);
                }
                else
                if (args.Key == VirtualKey.Right)
                {
                    MauiGame.KeyboardReleased(GameKey.Right);
                }
            };
            native.PreviewKeyDown += (sender, args) =>
            {
                if (args.Key == VirtualKey.Space)
                {
                    MauiGame.KeyboardPressed(GameKey.Space);
                }
                else
                if (args.Key == VirtualKey.Left)
                {
                    MauiGame.KeyboardPressed(GameKey.Left);
                }
                else
                if (args.Key == VirtualKey.Right)
                {
                    MauiGame.KeyboardPressed(GameKey.Right);
                }

            };
        }
    }
}