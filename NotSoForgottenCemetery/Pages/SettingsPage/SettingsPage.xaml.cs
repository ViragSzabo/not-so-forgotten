namespace NotSoForgottenCemetery.Pages.SettingsPage
{
    public partial class SettingsPage : ContentPage
    {
        public SettingsPage(SettingsViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;
        }

        public SettingsPage() : this(App.Services?.GetService<SettingsViewModel>()!) { }
    }
}
