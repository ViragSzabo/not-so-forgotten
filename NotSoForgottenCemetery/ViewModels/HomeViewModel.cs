using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;

namespace Cemetery
{
    public partial class HomeViewModel : ObservableObject, IDisposable
    {
        private readonly IDatabase _database;
        private readonly IYouTubeService _youtubeService;
        private readonly System.Timers.Timer _clockTimer;
        private readonly CancellationTokenSource _cts = new();

        [ObservableProperty] private string _clockTime = "00:00:00";
        [ObservableProperty] private string _currentSong = "Tuning in...";
        [ObservableProperty] private string _currentWhisper = "The mist is quiet...";
        [ObservableProperty] private double _signalStrength = 0.85;
        [ObservableProperty] private string _threadStatus = "Engines warm";
        [ObservableProperty] private bool _isRadioActive;
        [ObservableProperty] private bool _isWhisperActive;
        [ObservableProperty] private string _newMemoryTitle = string.Empty;
        [ObservableProperty] private string _newMemoryDescription = string.Empty;

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
            GoToMemoryBoardCommand = new AsyncRelayCommand(() => Shell.Current.GoToAsync("MemoryBoardPage"));
            GoToPlaylistsCommand = new AsyncRelayCommand(() => Shell.Current.GoToAsync("PlayListPage"));
            WatchOnYouTubeCommand = new AsyncRelayCommand(WatchOnYouTubeAsync);

            _clockTimer = new System.Timers.Timer(1000);
            _clockTimer.Elapsed += (s, e) => MainThread.BeginInvokeOnMainThread(() => ClockTime = DateTime.Now.ToString("HH:mm:ss"));
            _clockTimer.Start();

            _ = StartRadioStation(_cts.Token);
            _ = StartMemoryWhispers(_cts.Token);
            _ = LoadMemoriesAsync();
        }

        private async Task StartRadioStation(CancellationToken ct)
        {
            var songs = new[] { "October Rust", "Disintegration", "Black No. 1", "Lullaby", "Nightmare" };
            int i = 0;
            var rng = new Random();
            while (!ct.IsCancellationRequested)
            {
                IsRadioActive = true;
                ThreadStatus = "Radio Uplink: Active";
                SignalStrength = 0.7 + (rng.NextDouble() * 0.3);
                
                CurrentSong = $"Playing: {songs[i]}...";
                i = (i + 1) % songs.Length;
                
                try { await Task.Delay(2000, ct); IsRadioActive = false; await Task.Delay(8000, ct); } catch { break; }
            }
        }

        private async Task StartMemoryWhispers(CancellationToken ct)
        {
            while (!ct.IsCancellationRequested)
            {
                IsWhisperActive = true;
                ThreadStatus = "Database Query: Scanning";
                
                var all = await _database.GetMemoriesAsync();
                CurrentWhisper = all.Any() ? $"\"{all[new Random().Next(all.Count)].Title}\" ... whispers from the mist." : "Quiet...";
                
                try { await Task.Delay(3000, ct); IsWhisperActive = false; await Task.Delay(12000, ct); } catch { break; }
            }
        }

        private async Task LoadMemoriesAsync()
        {
            var items = await _database.GetMemoriesAsync();
            MainThread.BeginInvokeOnMainThread(() => {
                Memories.Clear();
                foreach (var m in items.OrderByDescending(x => x.Date)) Memories.Add(m);
            });
        }

        private async Task AddMemoryAsync()
        {
            if (string.IsNullOrWhiteSpace(NewMemoryTitle) || string.IsNullOrWhiteSpace(NewMemoryDescription)) 
                return;

            await _database.SaveMemoryAsync(new MemoryDb 
            { 
                Title = NewMemoryTitle, 
                Description = NewMemoryDescription, 
                Date = DateTime.Now 
            });

            NewMemoryTitle = string.Empty;
            NewMemoryDescription = string.Empty;
            await LoadMemoriesAsync();
        }

        private async Task WatchOnYouTubeAsync()
        {
            var query = CurrentSong.Replace("Playing: ", "").Replace("...", "");
            var id = await _youtubeService.SearchVideoIdAsync(query, "");
            await Launcher.Default.OpenAsync(!string.IsNullOrEmpty(id) ? $"https://www.youtube.com/watch?v={id}" : $"https://www.youtube.com/results?search_query={Uri.EscapeDataString(query)}");
        }

        public void Dispose() { _cts.Cancel(); _clockTimer.Stop(); }
    }
}
