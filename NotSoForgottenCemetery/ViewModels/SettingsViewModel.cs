using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Cemetery
{
    /// <summary>Abstraction over platform secure storage, making it mockable.</summary>
    public interface ISettingsStore
    {
        Task<string?> GetAsync(string key);
        Task SetAsync(string key, string value);
    }

    public partial class SettingsViewModel : ObservableObject
    {
        private readonly IDatabase _dbService;
        private readonly ISpotifyService _spotifyService;
        private readonly ISettingsStore _settings;

        [ObservableProperty] private string _spotifyClientId = string.Empty;
        [ObservableProperty] private string _youtubeApiKey = string.Empty;
        [ObservableProperty] private string _statusMessage = string.Empty;

        public IAsyncRelayCommand SaveCommand { get; }

        public SettingsViewModel() : this(
            App.ServiceProvider?.GetService<IDatabase>()!,
            App.ServiceProvider?.GetService<ISpotifyService>()!,
            App.ServiceProvider?.GetService<ISettingsStore>()!) { }

        public SettingsViewModel(IDatabase dbService, ISpotifyService spotifyService, ISettingsStore? settings = null)
        {
            _dbService = dbService;
            _spotifyService = spotifyService;
            _settings = settings ?? new SecureSettingsStore();
            SaveCommand = new AsyncRelayCommand(SaveSettingsAsync);
            LoadSettingsAsync().ConfigureAwait(false);
        }

        private async Task LoadSettingsAsync()
        {
            SpotifyClientId = await _settings.GetAsync("spotify_client_id") ?? string.Empty;
            YoutubeApiKey   = await _settings.GetAsync("youtube_api_key")  ?? string.Empty;
        }

        private async Task SaveSettingsAsync()
        {
            try
            {
                await _settings.SetAsync("spotify_client_id", SpotifyClientId);
                await _settings.SetAsync("youtube_api_key", YoutubeApiKey);
                StatusMessage = "Settings saved.";
            }
            catch (Exception ex)
            {
                StatusMessage = $"Error: {ex.Message}";
            }
            await Task.Delay(3000);
            StatusMessage = string.Empty;
        }
    }

    /// <summary>Production implementation backed by MAUI SecureStorage.</summary>
    internal class SecureSettingsStore : ISettingsStore
    {
        public Task<string?> GetAsync(string key) => SecureStorage.GetAsync(key);
        public Task SetAsync(string key, string value) => SecureStorage.SetAsync(key, value);
    }
}
