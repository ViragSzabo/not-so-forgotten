namespace NotSoForgottenCemetery
{
    public partial class App : Application
    {
        // Static service provider for dependency injection
        public static IServiceProvider Services { get; set; }

        // Constructor
        public App(IServiceProvider services)
        {
            InitializeComponent();
            Services = services;
            MainPage = new AppShell();
        }
    }
}