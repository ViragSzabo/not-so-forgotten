namespace Cemetery
{
    public partial class SettingsPage : ContentPage
    {
        public SettingsPage(SettingsViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;
        }

        public SettingsPage() : this(App.ServiceProvider?.GetService<SettingsViewModel>()!) { }
    }
}
