using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using NotSoForgottenCemetery.Helpers;
using NotSoForgottenCemetery.Models;
using NotSoForgottenCemetery.Services;
using System.Collections.ObjectModel;

namespace NotSoForgottenCemetery.Pages.HomePage
{
    public partial class HomeViewModel : ObservableObject, IDisposable
    {
        private readonly IDatabase _database;
        private readonly IYouTubeService _youtubeService;
        private readonly System.Timers.Timer _clockTimer;
        private readonly CancellationTokenSource _cts = new();
        private bool _disposed;

        private static readonly string[] RadioSongs =
        {
            "Playing: October Rust by Type O Negative...",
            "Playing: Disintegration by The Cure...",
            "Playing: Black No. 1 by Type O Negative...",
            "Playing: Lullaby by The Cure...",
            "Playing: The Ghost In You by Psychedelic Furs..."
        };

#pragma warning disable MVVMTK0045
        [ObservableProperty] private string _clockTime = "00:00:00";
        [ObservableProperty] private string _currentSong = "Tuning in...";
        [ObservableProperty] private string _currentWhisper = "The mist is quiet...";
        [ObservableProperty] private string _newMemoryTitle = string.Empty;
        [ObservableProperty] private string _newMemoryDescription = string.Empty;
#pragma warning restore MVVMTK0045

        public ObservableCollection<MemoryDb> Memories { get; } = new();

        public IAsyncRelayCommand AddMemoryCommand { get; }
        public IAsyncRelayCommand GoToMemoryBoardCommand { get; }
        public IAsyncRelayCommand GoToPlaylistsCommand { get; }
        public IAsyncRelayCommand WatchOnYouTubeCommand { get; }

        public HomeViewModel(IDatabase database, IYouTubeService youtubeService)
        {
            _database = database;
            _youtubeService = youtubeService;

            AddMemoryCommand = new AsyncRelayCommand(AddMemoryAsync);
            GoToMemoryBoardCommand = new AsyncRelayCommand(NavigateToMemoryBoardAsync);
            GoToPlaylistsCommand = new AsyncRelayCommand(NavigateToPlaylistsAsync);
            WatchOnYouTubeCommand = new AsyncRelayCommand(WatchOnYouTubeAsync);

            _clockTimer = new System.Timers.Timer(1000);
            _clockTimer.Elapsed += (s, e) =>
                MainThread.BeginInvokeOnMainThread(() => ClockTime = DateTime.Now.ToString("HH:mm:ss"));
            _clockTimer.Start();

            StartRadioStation(_cts.Token).FireAndForgetSafeAsync();
            StartMemoryWhispers(_cts.Token).FireAndForgetSafeAsync();

            LoadMemoriesAsync().FireAndForgetSafeAsync();
        }

        private async Task StartRadioStation(CancellationToken ct)
        {
            int index = 0;
            while (!ct.IsCancellationRequested)
            {
                var song = RadioSongs[index];
                MainThread.BeginInvokeOnMainThread(() => CurrentSong = song);
                index = (index + 1) % RadioSongs.Length;
                
                try { await Task.Delay(10000, ct); }
                catch (TaskCanceledException) { break; }
            }
        }

        private async Task StartMemoryWhispers(CancellationToken ct)
        {
            while (!ct.IsCancellationRequested)
            {
                var allMemories = await _database.GetMemoriesAsync();
                string whisper;
                if (allMemories.Any())
                {
                    var random = new Random();
                    var randomMemory = allMemories[random.Next(allMemories.Count)];
                    whisper = $"\"{randomMemory.Title}\" ... whispers from the mist.";
                }
                else
                {
                    whisper = "The cemetery is quiet. No memories rest here yet.";
                }
                
                MainThread.BeginInvokeOnMainThread(() => CurrentWhisper = whisper);
                
                try { await Task.Delay(15000, ct); }
                catch (TaskCanceledException) { break; }
            }
        }

        private async Task LoadMemoriesAsync()
        {
            var memories = await _database.GetMemoriesAsync();
            MainThread.BeginInvokeOnMainThread(() =>
            {
                Memories.Clear();
                foreach (var memory in memories.OrderByDescending(m => m.Date))
                    Memories.Add(memory);
            });
        }

        private async Task AddMemoryAsync()
        {
            if (string.IsNullOrWhiteSpace(NewMemoryTitle))
            {
                await Shell.Current.DisplayAlert("Validation Error", "Please provide a title for the memory.", "OK");
                return;
            }

            if (string.IsNullOrWhiteSpace(NewMemoryDescription))
            {
                await Shell.Current.DisplayAlert("Validation Error", "Please provide a description for the memory.", "OK");
                return;
            }

            var newMemory = new MemoryDb
            {
                Title = NewMemoryTitle.Trim(),
                Description = NewMemoryDescription.Trim(),
                Date = DateTime.Now
            };

            try
            {
                await _database.SaveMemoryAsync(newMemory);
                NewMemoryTitle = string.Empty;
                NewMemoryDescription = string.Empty;
                await LoadMemoriesAsync();
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Database Error", "Failed to save memory: " + ex.Message, "OK");
            }
        }

        private async Task NavigateToMemoryBoardAsync()
            => await Shell.Current.GoToAsync(nameof(MemoryBoardPage.MemoryBoardPage));

        private async Task NavigateToPlaylistsAsync()
            => await Shell.Current.GoToAsync(nameof(PlaylistPage.PlayListPage));

        private async Task WatchOnYouTubeAsync()
        {
            if (string.IsNullOrWhiteSpace(CurrentSong) || CurrentSong.StartsWith("Tuning"))
                return;

            var query = CurrentSong.Replace("Playing: ", "").Replace("...", "");
            
            var videoId = await _youtubeService.SearchVideoIdAsync(query, "");
            if (!string.IsNullOrEmpty(videoId))
            {
                await Launcher.Default.OpenAsync($"https://www.youtube.com/watch?v={videoId}");
            }
            else
            {
                await Launcher.Default.OpenAsync($"https://www.youtube.com/results?search_query={Uri.EscapeDataString(query)}");
            }
        }

        public void Dispose()
        {
            if (_disposed) return;
            _cts.Cancel();
            _cts.Dispose();
            _clockTimer.Stop();
            _clockTimer.Dispose();
            _disposed = true;
        }
    }
}
    }
}