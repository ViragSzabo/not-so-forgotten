using Microsoft.Windows.ApplicationModel.WindowsAppRuntime;

namespace Cemetery.WinUI
{
    public partial class App : Microsoft.Maui.MauiWinUIApplication
    {
        public App()
        {
            // DeploymentManager requires package identity - skip for unpackaged development
        }

        protected override Microsoft.Maui.Hosting.MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();
    }
}