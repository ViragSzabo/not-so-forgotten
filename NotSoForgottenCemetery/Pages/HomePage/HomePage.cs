namespace NotSoForgottenCemetery.Pages.HomePage
{
    public partial class HomePage : ContentPage
    {
        public HomePage()
        {
            InitializeComponent();
            BindingContext = App.Services.GetRequiredService<NotSoForgottenCemetery.Pages.HomePage.HomeViewModel>();
        }
    }
}