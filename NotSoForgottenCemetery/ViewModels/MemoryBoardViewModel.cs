using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using Cemetery.Services.Abstractions;

namespace Cemetery.ViewModels
{
    public partial class MemoryBoardViewModel : ObservableObject
    {
        private readonly IDatabase _db;
        private readonly ISpotifyService _spotify;
        private readonly IYouTubeService _youtube;

        [ObservableProperty] private ObservableCollection<MemoryDb> _memories = new();

        public IAsyncRelayCommand<MemoryDb> DeleteMemoryCommand { get; }
        public IAsyncRelayCommand<MemoryDb> ListenOnSpotifyCommand { get; }
        public IAsyncRelayCommand<MemoryDb> WatchOnYouTubeCommand { get; }

        public MemoryBoardViewModel(IDatabase db, ISpotifyService spotify, IYouTubeService youtube)
        {
            _db = db;
            _spotify = spotify;
            _youtube = youtube;

            DeleteMemoryCommand = new AsyncRelayCommand<MemoryDb>(DeleteMemoryAsync);
            ListenOnSpotifyCommand = new AsyncRelayCommand<MemoryDb>(ListenOnSpotifyAsync);
            WatchOnYouTubeCommand = new AsyncRelayCommand<MemoryDb>(WatchOnYouTubeAsync);

            _ = LoadMemoriesAsync();
        }

        private async Task LoadMemoriesAsync()
        {
            var items = await _db.GetMemoriesAsync();
            MainThread.BeginInvokeOnMainThread(() => 
            { 
                Memories.Clear(); 
                foreach (var m in items) Memories.Add(m); 
            });
        }

        private async Task DeleteMemoryAsync(MemoryDb? m)
        {
            if (m != null)
            {
                await _db.DeleteMemoryAsync(m);
                Memories.Remove(m);
            }
        }

        private async Task ListenOnSpotifyAsync(MemoryDb? m)
        {
            if (m != null)
            {
                var query = Uri.EscapeDataString(m.FavoriteSong ?? string.Empty);
                await Launcher.Default.OpenAsync($"https://open.spotify.com/search/{query}");
            }
        }

        private async Task WatchOnYouTubeAsync(MemoryDb? m)
        {
            if (m != null)
            {
                var query = Uri.EscapeDataString(m.FavoriteSong ?? string.Empty);
                await Launcher.Default.OpenAsync($"https://www.youtube.com/results?search_query={query}");
            }
        }
    }
}
