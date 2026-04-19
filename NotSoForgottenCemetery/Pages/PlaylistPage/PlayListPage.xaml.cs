using Cemetery.ViewModels;

namespace Cemetery
{
    public partial class PlayListPage : ContentPage
    {
        public PlayListPage(PlaylistViewModel vm) { InitializeComponent(); BindingContext = vm; }
        public PlayListPage() : this(App.ServiceProvider?.GetService<PlaylistViewModel>()!) { }
    }
}
