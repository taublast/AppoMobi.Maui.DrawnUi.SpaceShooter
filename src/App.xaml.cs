using AppoMobi.Specials;
using System.Runtime.InteropServices;

namespace SpaceShooter
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new MainPage();
        }

        protected override void OnStart()
        {
            base.OnStart();

            Tasks.StartDelayed(TimeSpan.FromSeconds(3), () =>
            {
                Dispatcher.Dispatch(() =>
                {
                    DeviceDisplay.Current.KeepScreenOn = true;
                });
            });
        }

        protected override void OnSleep()
        {
            base.OnSleep();

            Dispatcher.Dispatch(() =>
            {
                DeviceDisplay.Current.KeepScreenOn = false;
            });

            Game.SpaceShooter.Instance.Pause();
        }

        protected override void OnResume()
        {
            base.OnResume();

            Dispatcher.Dispatch(() =>
            {
                DeviceDisplay.Current.KeepScreenOn = true;
            });

            Game.SpaceShooter.Instance.Resume();
        }

    }
}
