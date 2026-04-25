using Microsoft.Windows.ApplicationModel.WindowsAppRuntime;

namespace Cemetery.WinUI
{
    public partial class App : Microsoft.Maui.MauiWinUIApplication
    {
        public App()
        {
            DeploymentManager.Initialize(new DeploymentInitializeOptions { OnErrorShowUI = false });
        }

        protected override Microsoft.Maui.Hosting.MauiApp CreateMauiApp() => Cemetery.MauiProgram.CreateMauiApp();
    }
}