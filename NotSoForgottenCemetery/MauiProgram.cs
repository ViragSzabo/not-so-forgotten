using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using NotSoForgottenCemetery.Services;

namespace NotSoForgottenCemetery
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            // Create a MAUI app builder
            var builder = MauiApp.CreateBuilder();

            // Configure the app
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            // Database path
            var dbPath = Path.Combine(FileSystem.AppDataDirectory, "not-so-forgotten-cemetery.db3");

            builder.Services.AddSingleton(sp => new Database(dbPath));
            builder.Services.AddSingleton<SpotifyService>();
            builder.Services.AddSingleton(sp => new LyricsService("YOUR_MUSIXMATCH_API_KEY"));

#if DEBUG
            builder.Logging.AddDebug();
#endif

            var app = builder.Build();

            // Set the static service provider in App class
            App.Services = app.Services;

            return app;
        }
    }
}