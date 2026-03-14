using Microsoft.Extensions.DependencyInjection;
namespace NotSoForgottenCemetery.Pages.PlaylistPage
{
    public partial class PlayListPage : ContentPage
    {
        public PlayListPage(PlaylistViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;
        }

        public PlayListPage() : this(App.Services?.GetService<PlaylistViewModel>()!) { }
    }
}