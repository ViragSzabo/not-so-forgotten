using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Maui.ApplicationModel;
using System.Security.Cryptography; // for SHA256 + RNG
using Microsoft.Maui.Storage;       // for SecureStorage
using Microsoft.Maui.Dispatching;
using Aspose.Pdf.Operators;

namespace NotSoForgottenCemetery.Services
{
    // Service to fetch song lyrics from Musixmatch API
    public class LyricsService
    {
        private readonly HttpClient _httpClient;
        private const string API_BASE_URL = "https://api.musixmatch.com/ws/1.1/";
        private readonly string _apiKey;

        // Constructor to initialize HttpClient and API key
        public LyricsService(string apikey)
        {
            _apiKey = apikey ?? throw new ArgumentNullException(nameof(apikey));
            _httpClient = new HttpClient { BaseAddress = new Uri(API_BASE_URL), Timeout = TimeSpan.FromSeconds(20) };
        }

        public async Task<string> GetLyricsAsync(string artist, string title)
        {
            if (string.IsNullOrWhiteSpace(_apiKey))
                return "Lyrics API key missing.";

            try
            {
                var url = $"matcher.lyrics.get?q_track={Uri.EscapeDataString(title)}&q_artist={Uri.EscapeDataString(artist)}&apikey={_apiKey}";
                var response = await _httpClient.GetAsync(url);

                if (!response.IsSuccessStatusCode)
                {
                    MainThread.BeginInvokeOnMainThread(() =>
                        Console.WriteLine($"LyricsService: non-success status {response.StatusCode}")
                    );
                    return "Error fetching lyrics.";
                }

                var content = await response.Content.ReadAsStringAsync();
                using var doc = JsonDocument.Parse(content);

                // safe navigation
                if (doc.RootElement.TryGetProperty("message", out var msg) &&
                    msg.TryGetProperty("body", out var body) &&
                    body.TryGetProperty("lyrics", out var lyricsNode) &&
                    lyricsNode.TryGetProperty("lyrics_body", out var lyricsBody))
                {
                    var lyrics = lyricsBody.GetString();
                    return string.IsNullOrWhiteSpace(lyrics) ? "Lyrics not found." : lyrics;
                }

                return "Lyrics not found.";
            }
            catch (Exception ex)
            {
                MainThread.BeginInvokeOnMainThread(() =>
                    Console.WriteLine($"LyricsService error: {ex.Message}")
                );
                return "An error occurred while fetching lyrics.";
            }
        }

        // Class to parse lyrics response
        private class LyricsResponse
        {
            public required string Lyrics { get; set; }
        }
    }
}