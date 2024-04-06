global using DrawnUi.Maui.Draw;
using Microsoft.Extensions.Logging;


namespace SpaceShooter
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {

            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                    fonts.AddFont("OpenSans-Regular.ttf", "FontText");

                    fonts.AddFont("Orbitron-Regular.ttf", "FontGame"); //400
                    fonts.AddFont("Orbitron-Medium.ttf", "FontGameMedium"); //500
                    fonts.AddFont("Orbitron-SemiBold.ttf", "FontGameSemiBold"); //600
                    fonts.AddFont("Orbitron-Bold.ttf", "FontGameBold"); //700
                    fonts.AddFont("Orbitron-ExtraBold.ttf", "FontGameExtraBold"); //800
                });

            builder.UseDrawnUi(new()
            {
                UseDesktopKeyboard = true, //capture keys on desktop
                DesktopWindow = new()
                {
                    Width = 550,
                    Height = 750,
                    IsFixedSize = true //user cannot resize window
                    //todo disable maximize btn 
                }
            });

            //to avoid returning many copies of same sprite bitmap for different images
            SkiaImageManager.ReuseBitmaps = true;

#if WINDOWS
            // game mode !!!
            Thread.CurrentThread.Priority = ThreadPriority.Highest;
#endif

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }

        public static bool IsDebug
        {
            get
            {
#if DEBUG
                return true;
#else
                return false;
#endif
            }
        }
    }
}