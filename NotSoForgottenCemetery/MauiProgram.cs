using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using NotSoForgottenCemetery.Database;
using NotSoForgottenCemetery.Services.Database;

namespace NotSoForgottenCemetery
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
                });

            // Database path
            var dbPath = Path.Combine(FileSystem.AppDataDirectory, "not-so-forgotten-cemetery.db3");

            // Register DatabaseService as a singleton
            builder.Services.AddSingleton(sp => new DatabaseService(dbPath));

            // Register SpotifyService and LyricsService
            builder.Services.AddSingleton<SpotifyService>();
            builder.Services.AddSingleton(sp => new LyricsService("YOUR_MUSIXMATCH_API_KEY"));

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}