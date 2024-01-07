using AppoMobi.Maui.DrawnUi;
using AppoMobi.Maui.DrawnUi.Infrastructure.Extensions;
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

            builder.UseDrawnUi<App>();

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}