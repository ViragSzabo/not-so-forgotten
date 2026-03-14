using System;
using Microsoft.Maui;
using Microsoft.Maui.Hosting;
using System.IO;

namespace NotSoForgottenCemetery.WinUI
{
    public partial class App : MauiWinUIApplication
    {
        public App()
        {
            try { File.WriteAllText(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "NSF_Early_Log.txt"), $"WinApp constructor start at {DateTime.Now}\n"); } catch { }
            this.InitializeComponent();

            this.UnhandledException += (s, e) =>
            {
                try
                {
                    var logPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "NSF_Cemetery_Crash.txt");
                    File.AppendAllText(logPath, $"WINUI UNHANDLED EXCEPTION: {e.Exception}{Environment.NewLine}");
                }
                catch { }
            };
        }

        protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();
    }
}