using System.Text.Json;

namespace NotSoForgottenCemetery.Services
{
    public class YouTubeService
    {
        private readonly HttpClient _httpClient;
        private const string API_BASE_URL = "https://www.googleapis.com/youtube/v3/";

        public YouTubeService()
        {
            _httpClient = new HttpClient { BaseAddress = new Uri(API_BASE_URL) };
        }

        public async Task<string?> SearchVideoIdAsync(string artist, string title)
        {
            var apiKey = await SecureStorage.GetAsync("youtube_api_key");
            if (string.IsNullOrWhiteSpace(apiKey))
                return null;

            try
            {
                var query = Uri.EscapeDataString($"{artist} {title} official music video");
                var url = $"search?part=snippet&q={query}&type=video&key={apiKey}&maxResults=1";
                
                var response = await _httpClient.GetAsync(url);
                if (!response.IsSuccessStatusCode) return null;

                var content = await response.Content.ReadAsStringAsync();
                using var doc = JsonDocument.Parse(content);
                
                if (doc.RootElement.TryGetProperty("items", out var items) && 
                    items.GetArrayLength() > 0)
                {
                    var firstItem = items[0];
                    if (firstItem.TryGetProperty("id", out var id) && 
                        id.TryGetProperty("videoId", out var videoId))
                    {
                        return videoId.GetString();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"YouTubeService error: {ex.Message}");
            }

            return null;
        }
    }
}
