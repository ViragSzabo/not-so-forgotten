#pragma warning disable MVVMTK0045

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using Cemetery.Services.Abstractions;

namespace Cemetery.ViewModels
{
    public partial class PlaylistViewModel : ObservableObject
    {
        private readonly IDatabase _db;
        [ObservableProperty] private ObservableCollection<PlaylistDb> _playlists = new();
        [ObservableProperty] private string _newName = string.Empty;
        [ObservableProperty] private string _newDescription = string.Empty;
        [ObservableProperty] private string _newSpotifyId = string.Empty;

        public IAsyncRelayCommand AddCommand { get; }
        public IAsyncRelayCommand<PlaylistDb> OpenCommand { get; }
        public IAsyncRelayCommand<PlaylistDb> DeleteCommand { get; }

        public PlaylistViewModel(IDatabase db, ISpotifyService s, IYouTubeService y)
        {
            _db = db;
            AddCommand = new AsyncRelayCommand(async () => 
            { 
                if (!string.IsNullOrWhiteSpace(NewName)) 
                { 
                    await _db.SavePlaylistAsync(new PlaylistDb 
                    { 
                        Name = NewName,
                        Description = NewDescription,
                        SpotifyId = NewSpotifyId
                    }); 
                    NewName = string.Empty; 
                    NewDescription = string.Empty;
                    NewSpotifyId = string.Empty;
                    await LoadAsync(); 
                } 
            });
            OpenCommand = new AsyncRelayCommand<PlaylistDb>(async p => 
            { 
                if (p != null && !string.IsNullOrWhiteSpace(p.SpotifyId)) 
                    await Launcher.Default.OpenAsync(p.SpotifyId.StartsWith("http") ? p.SpotifyId : $"https://open.spotify.com/playlist/{p.SpotifyId}"); 
            });
            DeleteCommand = new AsyncRelayCommand<PlaylistDb>(async p =>
            {
                if (p != null)
                {
                    await _db.DeletePlaylistAsync(p);
                    await LoadAsync();
                }
            });
            _ = LoadAsync();
        }

        private async Task LoadAsync() 
        { 
            var items = await _db.GetPlaylistsAsync(); 
            MainThread.BeginInvokeOnMainThread(() => 
            { 
                Playlists.Clear(); 
                foreach (var i in items) Playlists.Add(i); 
            }); 
        }
    }
}
