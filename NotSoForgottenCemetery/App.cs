using NotSoForgottenCemetery.Services;
using NotSoForgottenCemetery.Models;
using System.Threading.Tasks;

namespace NotSoForgottenCemetery
{
    public partial class App : Application
    {
        // Static service provider for dependency injection
        public static IServiceProvider? Services { get; set; }

        // Constructor
        public App()
        {
            InitializeComponent();
            MainPage = new AppShell();

            // Initialize database asynchronously to avoid UI deadlock on startup
            Task.Run(async () =>
            {
                try
                {
                    var db = Services?.GetService<Database>();
                    if (db != null) await db.InitializeAsync();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Late-stage database initialization error: {ex.Message}");
                }
            });
        }
    }
}