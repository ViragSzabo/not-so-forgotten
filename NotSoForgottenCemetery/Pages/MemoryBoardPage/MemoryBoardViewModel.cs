using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using NotSoForgottenCemetery.Services;
using NotSoForgottenCemetery.Models;
using System.Collections.ObjectModel;

namespace NotSoForgottenCemetery.Pages.MemoryBoard
{
    public partial class MemoryBoardViewModel : ObservableObject
    {
        private readonly Database _dbService;
        private readonly SpotifyService _spotifyService;
        private readonly YouTubeService _youtubeService;

#pragma warning disable MVVMTK0045
        [ObservableProperty] private ObservableCollection<MemoryDb> _memories = new();
#pragma warning restore MVVMTK0045

        public IAsyncRelayCommand LoadMemoriesCommand { get; }
        public IAsyncRelayCommand<MemoryDb> DeleteMemoryCommand { get; }
        public IAsyncRelayCommand<MemoryDb> ListenOnSpotifyCommand { get; }
        public IAsyncRelayCommand<MemoryDb> WatchOnYouTubeCommand { get; }

        public MemoryBoardViewModel(Database dbService, SpotifyService spotifyService, YouTubeService youtubeService)
        {
            _dbService = dbService;
            _spotifyService = spotifyService;
            _youtubeService = youtubeService;

            LoadMemoriesCommand = new AsyncRelayCommand(LoadMemoriesAsync);
            DeleteMemoryCommand = new AsyncRelayCommand<MemoryDb>(DeleteMemoryAsync);
            ListenOnSpotifyCommand = new AsyncRelayCommand<MemoryDb>(ListenOnSpotifyAsync);
            WatchOnYouTubeCommand = new AsyncRelayCommand<MemoryDb>(WatchOnYouTubeAsync);

            LoadMemoriesAsync().ConfigureAwait(false);
        }

        // Default constructor for XAML
        public MemoryBoardViewModel() : this(
            App.Services?.GetService<Database>()!,
            App.Services?.GetService<SpotifyService>()!,
            App.Services?.GetService<YouTubeService>()!)
        { }

        private async Task LoadMemoriesAsync()
        {
            var allMemories = await _dbService.GetMemoriesAsync();
            MainThread.BeginInvokeOnMainThread(() =>
            {
                Memories.Clear();
                foreach (var memory in allMemories.OrderByDescending(m => m.Date))
                    Memories.Add(memory);
            });
        }

        private async Task DeleteMemoryAsync(MemoryDb? memory)
        {
            if (memory == null) return;
            await _dbService.DeleteMemoryAsync(memory);
            Memories.Remove(memory);
        }

        private async Task ListenOnSpotifyAsync(MemoryDb? memory)
        {
            if (memory == null || string.IsNullOrWhiteSpace(memory.FavoriteSong)) return;

            var songs = await _spotifyService.SearchSongsAsync(memory.FavoriteSong);
            var bestMatch = songs.FirstOrDefault();
            if (bestMatch != null && !string.IsNullOrEmpty(bestMatch.SpotifyUrl))
            {
                await Launcher.Default.OpenAsync(bestMatch.SpotifyUrl);
            }
            else
            {
                await Launcher.Default.OpenAsync($"https://open.spotify.com/search/{Uri.EscapeDataString(memory.FavoriteSong)}");
            }
        }

        private async Task WatchOnYouTubeAsync(MemoryDb? memory)
        {
            if (memory == null || string.IsNullOrWhiteSpace(memory.FavoriteSong)) return;

            var videoId = await _youtubeService.SearchVideoIdAsync(memory.FavoriteSong, "");
            if (!string.IsNullOrEmpty(videoId))
            {
                await Launcher.Default.OpenAsync($"https://www.youtube.com/watch?v={videoId}");
            }
            else
            {
                await Launcher.Default.OpenAsync($"https://www.youtube.com/results?search_query={Uri.EscapeDataString(memory.FavoriteSong)}");
            }
        }
    }
}