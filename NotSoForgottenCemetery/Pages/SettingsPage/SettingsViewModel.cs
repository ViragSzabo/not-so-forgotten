using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace NotSoForgottenCemetery.Pages.SettingsPage
{
    public partial class SettingsViewModel : ObservableObject
    {
        [ObservableProperty]
        private string _spotifyClientId = string.Empty;

        [ObservableProperty]
        private string _youtubeApiKey = string.Empty;

        [ObservableProperty]
        private string _statusMessage = string.Empty;

        public IAsyncRelayCommand SaveCommand { get; }

        public SettingsViewModel()
        {
            SaveCommand = new AsyncRelayCommand(SaveSettingsAsync);
            LoadSettingsAsync().ConfigureAwait(false);
        }

        private async Task LoadSettingsAsync()
        {
            SpotifyClientId = await SecureStorage.GetAsync("spotify_client_id") ?? string.Empty;
            YoutubeApiKey = await SecureStorage.GetAsync("youtube_api_key") ?? string.Empty;
        }

        private async Task SaveSettingsAsync()
        {
            try
            {
                await SecureStorage.SetAsync("spotify_client_id", SpotifyClientId);
                await SecureStorage.SetAsync("youtube_api_key", YoutubeApiKey);
                StatusMessage = "Settings saved successfully!";
            }
            catch (Exception ex)
            {
                StatusMessage = $"Error saving settings: {ex.Message}";
            }

            await Task.Delay(3000);
            StatusMessage = string.Empty;
        }
    }
}
