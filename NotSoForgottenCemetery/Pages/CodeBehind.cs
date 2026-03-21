namespace Cemetery
{
    public partial class HomePage : ContentPage
    {
        public HomePage(HomeViewModel vm) { InitializeComponent(); BindingContext = vm; }
        public HomePage() : this(App.ServiceProvider?.GetService<HomeViewModel>()!) { }
    }

    public partial class MemoryBoardPage : ContentPage
    {
        public MemoryBoardPage(MemoryBoardViewModel vm) { InitializeComponent(); BindingContext = vm; }
        public MemoryBoardPage() : this(App.ServiceProvider?.GetService<MemoryBoardViewModel>()!) { }
    }

    public partial class PlayListPage : ContentPage
    {
        public PlayListPage(PlaylistViewModel vm) { InitializeComponent(); BindingContext = vm; }
        public PlayListPage() : this(App.ServiceProvider?.GetService<PlaylistViewModel>()!) { }
    }
}
