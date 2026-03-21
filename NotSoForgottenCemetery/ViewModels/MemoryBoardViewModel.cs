using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;

namespace Cemetery
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
            _db = db; _spotify = spotify; _youtube = youtube;
            DeleteMemoryCommand = new AsyncRelayCommand<MemoryDb>(async m => { if (m != null) { await _db.DeleteMemoryAsync(m); Memories.Remove(m); } });
            ListenOnSpotifyCommand = new AsyncRelayCommand<MemoryDb>(async m => await Launcher.Default.OpenAsync($"https://open.spotify.com/search/{Uri.EscapeDataString(m?.FavoriteSong ?? "")}"));
            WatchOnYouTubeCommand = new AsyncRelayCommand<MemoryDb>(async m => await Launcher.Default.OpenAsync($"https://www.youtube.com/results?search_query={Uri.EscapeDataString(m?.FavoriteSong ?? "")}"));
            _ = LoadMemoriesAsync();
        }

        private async Task LoadMemoriesAsync()
        {
            var items = await _db.GetMemoriesAsync();
            MainThread.BeginInvokeOnMainThread(() => { Memories.Clear(); foreach (var m in items) Memories.Add(m); });
        }
    }
}
