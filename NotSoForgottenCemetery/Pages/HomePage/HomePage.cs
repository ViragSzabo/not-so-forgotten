using Microsoft.Extensions.DependencyInjection;
namespace NotSoForgottenCemetery.Pages.HomePage
{
    public partial class HomePage : ContentPage
    {
        public HomePage(HomeViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;
        }

        public HomePage() : this(App.Services?.GetService<HomeViewModel>()!) { }
    }
}