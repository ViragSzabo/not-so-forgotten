namespace Cemetery
{
    public partial class HomePage : ContentPage
    {
        public HomePage(HomeViewModel vm) { InitializeComponent(); BindingContext = vm; }
        public HomePage() : this(App.ServiceProvider?.GetService<HomeViewModel>()!) { }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            StartFlickerAnimation();
        }

        private async void StartFlickerAnimation()
        {
            var rng = new Random();
            while (true)
            {
                if (CandleLabel == null) break;
                await CandleLabel.FadeTo(0.6 + (rng.NextDouble() * 0.4), (uint)rng.Next(100, 300));
                await CandleLabel.FadeTo(0.8 + (rng.NextDouble() * 0.2), (uint)rng.Next(100, 300));
            }
        }
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
