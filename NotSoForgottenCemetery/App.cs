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

            // Temporary test page to isolate startup crash:
            MainPage = new ContentPage
            {
                Content = new Label
                {
                    Text = "Startup test - Hello",
                    VerticalOptions = LayoutOptions.Center,
                    HorizontalOptions = LayoutOptions.Center
                }
            };
        }
    }
}