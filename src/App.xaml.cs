using AppoMobi.Maui.DrawnUi;
using System.Runtime.InteropServices;
using UIKit;


namespace SpaceShooter
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new MainPage();
        }

        const int DefaultWidth = 500;
        const int DefaultHeight = 800;

        void ResizeWindow(Window window)
        {


            // change window size.
            window.Width = DefaultWidth;
            window.Height = DefaultHeight;

            var disp = DeviceDisplay.Current.MainDisplayInfo;

            // move to screen center
            window.X = (disp.Width / disp.Density - window.Width) / 2;
            window.Y = (disp.Height / disp.Density - window.Height) / 2;
        }

        protected override Window CreateWindow(IActivationState activationState)
        {
            var window = base.CreateWindow(activationState);

#if WINDOWS

            ResizeWindow(window);

            window.Created += (sender, args) =>
            {
                if (sender is Microsoft.Maui.Controls.Window window)
                {
                    var platformWindow = window.Handler.PlatformView as Microsoft.Maui.MauiWinUIWindow;
                    var hWnd = platformWindow.WindowHandle;

                    // Now, use hWnd with P/Invoke to change window styles
                    MakeWindowNonResizable(hWnd);
                }
            };

#elif MACCATALYST

            window.Created += (sender, args) =>
            {
                foreach (var scene in UIApplication.SharedApplication.ConnectedScenes)
                {
                    if (scene is UIWindowScene windowScene)
                    {
                        windowScene.SizeRestrictions.MinimumSize = new(DefaultWidth, DefaultHeight);
                        windowScene.SizeRestrictions.MaximumSize = new(DefaultWidth, DefaultHeight);
                    }
                }
            };

#endif


            return window;
        }

#if WINDOWS

        // Win32 API constants and functions
        private const int GWL_STYLE = -16;
        private const int WS_THICKFRAME = 0x00040000;

        [DllImport("user32.dll", SetLastError = true)]
        private static extern IntPtr GetWindowLongPtr(IntPtr hWnd, int nIndex);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern int SetWindowLongPtr(IntPtr hWnd, int nIndex, IntPtr dwNewLong);

        private void MakeWindowNonResizable(IntPtr hWnd)
        {
            // Get the current window style
            IntPtr style = GetWindowLongPtr(hWnd, GWL_STYLE);

            // Remove the resize border (thick frame) from the style
            style = new IntPtr(style.ToInt64() & ~WS_THICKFRAME);

            // Set the modified style
            SetWindowLongPtr(hWnd, GWL_STYLE, style);
        }

#endif

    }
}
