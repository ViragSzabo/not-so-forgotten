using Microsoft.Extensions.Logging;

namespace Cemetery
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder.UseMauiApp<App>().ConfigureFonts(f => {
                f.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                f.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

            var dbPath = Path.Combine(FileSystem.AppDataDirectory, "cemetery_v3.db3");
            builder.Services.AddSingleton<IDatabase>(sp => new Database(dbPath));
            builder.Services.AddSingleton<ISpotifyService, SpotifyService>();
            builder.Services.AddSingleton<IYouTubeService, YouTubeService>();

            builder.Services.AddTransient<HomePage>();
            builder.Services.AddTransient<HomeViewModel>();
            builder.Services.AddTransient<MemoryBoardPage>();
            builder.Services.AddTransient<MemoryBoardViewModel>();
            builder.Services.AddTransient<PlayListPage>();
            builder.Services.AddTransient<PlaylistViewModel>();
            builder.Services.AddTransient<SettingsPage>();
            builder.Services.AddTransient<SettingsViewModel>();

            var app = builder.Build();
            App.ServiceProvider = app.Services;
            return app;
        }
    }

    public partial class App : Application
    {
        public static IServiceProvider? ServiceProvider { get; set; }
        public App() { MainPage = new AppShell(); }
    }

}
