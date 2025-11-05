using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using NotSoForgottenCemetery.Database;
using NotSoForgottenCemetery.Features;
using NotSoForgottenCemetery.Services.Database;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace NotSoForgottenCemetery.Pages.PlaylistPage
{
    public partial class PlaylistViewModel : ObservableObject
    {
        private readonly DatabaseService _dbService;
        private readonly SpotifyService _spotifyService;

        [ObservableProperty]
        public ObservableCollection<PlaylistDb> playlists = new ObservableCollection<PlaylistDb>();

        public IAsyncRelayCommand LoadPlaylistsCommand { get; }
        public IAsyncRelayCommand<PlaylistDb> OpenPlaylistCommand { get; }

        public PlaylistViewModel(DatabaseService dbService, SpotifyService spotifyService)
        {
            _dbService = dbService;
            _spotifyService = spotifyService;

            LoadPlaylistsCommand = new AsyncRelayCommand(LoadPlaylistsAsync);
            OpenPlaylistCommand = new AsyncRelayCommand<PlaylistDb>(OpenPlaylistAsync);
        }

        private async Task LoadPlaylistsAsync()
        {
            var all = await _dbService.GetPlaylistsAsync();
            Playlists.Clear();
            foreach (var p in all) Playlists.Add(p);
        }

        private async Task OpenPlaylistAsync(PlaylistDb playlist)
        {
            if (playlist == null) return;
            // Open Spotify URL if exists
            if (!string.IsNullOrEmpty(playlist.SpotifyId))
                await Launcher.Default.OpenAsync($"https://open.spotify.com/playlist/{playlist.SpotifyId}");
        }
    }
}