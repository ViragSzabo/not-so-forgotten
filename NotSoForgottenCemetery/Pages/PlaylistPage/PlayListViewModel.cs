using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using NotSoForgottenCemetery.Services;
using NotSoForgottenCemetery.Models;
using System.Collections.ObjectModel;

namespace NotSoForgottenCemetery.Pages.PlaylistPage
{
    public partial class PlaylistViewModel : ObservableObject
    {
        private readonly Database _dbService;
        private readonly SpotifyService _spotifyService;
        private readonly YouTubeService _youtubeService;

        [ObservableProperty] private ObservableCollection<PlaylistDb> _playlists = new();
        [ObservableProperty] private string _newPlaylistName = string.Empty;
        [ObservableProperty] private string _newPlaylistDescription = string.Empty;
        [ObservableProperty] private string _newPlaylistSpotifyId = string.Empty;
#pragma warning restore MVVMTK0045

        public IAsyncRelayCommand LoadPlaylistsCommand { get; }
        public IAsyncRelayCommand<PlaylistDb> OpenPlaylistCommand { get; }
        public IAsyncRelayCommand<PlaylistDb> WatchOnYouTubeCommand { get; }
        public IAsyncRelayCommand AddPlaylistCommand { get; }
        public IAsyncRelayCommand<PlaylistDb> DeletePlaylistCommand { get; }

        public PlaylistViewModel(Database dbService, SpotifyService spotifyService, YouTubeService youtubeService)
        {
            _dbService = dbService;
            _spotifyService = spotifyService;
            _youtubeService = youtubeService;
            _playlists = new ObservableCollection<PlaylistDb>();

            LoadPlaylistsCommand = new AsyncRelayCommand(LoadPlaylistsAsync);
            OpenPlaylistCommand = new AsyncRelayCommand<PlaylistDb>(OpenPlaylistAsync);
            WatchOnYouTubeCommand = new AsyncRelayCommand<PlaylistDb>(WatchOnYouTubeAsync);
            AddPlaylistCommand = new AsyncRelayCommand(AddPlaylistAsync);
            DeletePlaylistCommand = new AsyncRelayCommand<PlaylistDb>(DeletePlaylistAsync);

            LoadPlaylistsAsync().ConfigureAwait(false);
        }

        // Default constructor for XAML
        public PlaylistViewModel() : this(
            App.Services?.GetService<Database>()!,
            App.Services?.GetService<SpotifyService>()!,
            App.Services?.GetService<YouTubeService>()!)
        { }

        private async Task LoadPlaylistsAsync()
        {
            var all = await _dbService.GetPlaylistsAsync();
            MainThread.BeginInvokeOnMainThread(() =>
            {
                Playlists.Clear();
                foreach (var p in all) Playlists.Add(p);
            });
        }

        private async Task OpenPlaylistAsync(PlaylistDb? playlist)
        {
            if (playlist == null) return;
            if (!string.IsNullOrEmpty(playlist.SpotifyId))
            {
                // If it's a full URL, use it, otherwise assume it's an ID
                var url = playlist.SpotifyId.StartsWith("http") 
                    ? playlist.SpotifyId 
                    : $"https://open.spotify.com/playlist/{playlist.SpotifyId}";
                await Launcher.Default.OpenAsync(url);
            }
        }

        private async Task WatchOnYouTubeAsync(PlaylistDb? playlist)
        {
            if (playlist == null) return;
            
            var query = $"{playlist.Name} {playlist.Description} full playlist music";
            var videoId = await _youtubeService.SearchVideoIdAsync(playlist.Name, "playlist");
            
            if (!string.IsNullOrEmpty(videoId))
            {
                await Launcher.Default.OpenAsync($"https://www.youtube.com/watch?v={videoId}");
            }
            else
            {
                await Launcher.Default.OpenAsync($"https://www.youtube.com/results?search_query={Uri.EscapeDataString(query)}");
            }
        }

        private async Task AddPlaylistAsync()
        {
            if (string.IsNullOrWhiteSpace(NewPlaylistName)) return;

            var newPlaylist = new PlaylistDb
            {
                Name = NewPlaylistName,
                Description = NewPlaylistDescription,
                SpotifyId = NewPlaylistSpotifyId
            };

            await _dbService.SavePlaylistAsync(newPlaylist);

            NewPlaylistName = string.Empty;
            NewPlaylistDescription = string.Empty;
            NewPlaylistSpotifyId = string.Empty;

            await LoadPlaylistsAsync();
        }

        private async Task DeletePlaylistAsync(PlaylistDb? playlist)
        {
            if (playlist == null) return;
            await _dbService.DeletePlaylistAsync(playlist);
            await LoadPlaylistsAsync();
        }
    }
}