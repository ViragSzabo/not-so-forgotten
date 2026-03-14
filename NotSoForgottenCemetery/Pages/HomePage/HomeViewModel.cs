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
        private readonly Database _database;
        private readonly YouTubeService _youtubeService;
        private readonly System.Timers.Timer _clockTimer;
        private bool _disposed;

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

        public HomeViewModel(Database database, YouTubeService youtubeService)
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

            StartRadioStation();
            StartMemoryWhispers();

            LoadMemoriesAsync().FireAndForgetSafeAsync();
        }

        private void StartRadioStation()
        {
            Task.Run(async () =>
            {
                string[] songs =
                {
                    "Playing: October Rust by Type O Negative...",
                    "Playing: Disintegration by The Cure...",
                    "Playing: Black No. 1 by Type O Negative...",
                    "Playing: Lullaby by The Cure...",
                    "Playing: The Ghost In You by Psychedelic Furs..."
                };
                int index = 0;
                while (true)
                {
                    var song = songs[index];
                    MainThread.BeginInvokeOnMainThread(() => CurrentSong = song);
                    index = (index + 1) % songs.Length;
                    await Task.Delay(10000);
                }
            });
        }

        private void StartMemoryWhispers()
        {
            Task.Run(async () =>
            {
                while (true)
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
                    await Task.Delay(15000);
                }
            });
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
            if (string.IsNullOrWhiteSpace(NewMemoryTitle) || string.IsNullOrWhiteSpace(NewMemoryDescription))
                return;

            var newMemory = new MemoryDb
            {
                Title = NewMemoryTitle,
                Description = NewMemoryDescription,
                Date = DateTime.Now
            };

            await _database.SaveMemoryAsync(newMemory);

            NewMemoryTitle = string.Empty;
            NewMemoryDescription = string.Empty;

            await LoadMemoriesAsync();
        }

        private async Task NavigateToMemoryBoardAsync()
            => await Shell.Current.GoToAsync(nameof(MemoryBoardPage.MemoryBoardPage));

        private async Task NavigateToPlaylistsAsync()
            => await Shell.Current.GoToAsync(nameof(PlaylistPage.PlayListPage));

        private async Task WatchOnYouTubeAsync()
        {
            if (string.IsNullOrWhiteSpace(CurrentSong) || CurrentSong.StartsWith("Tuning"))
                return;

            // Extract artist and title if possible, or just use the whole string
            var query = CurrentSong.Replace("Playing: ", "").Replace("...", "");
            
            var videoId = await _youtubeService.SearchVideoIdAsync(query, "");
            if (!string.IsNullOrEmpty(videoId))
            {
                await Launcher.Default.OpenAsync($"https://www.youtube.com/watch?v={videoId}");
            }
            else
            {
                // Fallback to general search if no direct video ID found
                await Launcher.Default.OpenAsync($"https://www.youtube.com/results?search_query={Uri.EscapeDataString(query)}");
            }
        }

        public void Dispose()
        {
            if (_disposed) return;
            _clockTimer.Stop();
            _clockTimer.Dispose();
            _disposed = true;
        }
    }
}