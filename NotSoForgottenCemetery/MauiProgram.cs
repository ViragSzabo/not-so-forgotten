using System;
using Microsoft.Extensions.Logging;
using NotSoForgottenCemetery.Services;
using System.IO;

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

            // Services, Database, and Logging
            var dbPath = Path.Combine(FileSystem.AppDataDirectory, "not-so-forgotten-cemetery.db3");
            builder.Services.AddSingleton(sp => new Database(dbPath));
            builder.Services.AddSingleton<SpotifyService>();
            
            // YouTubeService for searching music videos
            builder.Services.AddSingleton<YouTubeService>();
            
            // Register UI Pages and ViewModels
            builder.Services.AddTransient<Pages.HomePage.HomePage>();
            builder.Services.AddTransient<Pages.HomePage.HomeViewModel>();
            builder.Services.AddTransient<Pages.MemoryBoardPage.MemoryBoardPage>();
            builder.Services.AddTransient<Pages.MemoryBoard.MemoryBoardViewModel>();
            builder.Services.AddTransient<Pages.PlaylistPage.PlayListPage>();
            builder.Services.AddTransient<Pages.PlaylistPage.PlaylistViewModel>();
            builder.Services.AddTransient<Pages.SettingsPage.SettingsPage>();
            builder.Services.AddTransient<Pages.SettingsPage.SettingsViewModel>();

            var app = builder.Build();
            App.Services = app.Services;

            return app;
        }
    }
}