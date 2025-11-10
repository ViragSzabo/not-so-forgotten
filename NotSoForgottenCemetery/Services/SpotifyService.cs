using NotSoForgottenCemetery.Features;
using System.Net;
using System.Net.Http.Headers;
using System.Security.Cryptography; // for SHA256 + RNG
using System.Text;
using System.Text.Json;

namespace NotSoForgottenCemetery.Services
{
    public class SpotifyService
    {
        private string _accessToken;
        private string _refreshToken;
        private string _codeVerifier; // PKCE code verifier
        private string _clientId;
        private DateTime _accessTokenExpiresAtUtc; // UTC time when token expires

        private readonly HttpClient _httpClient = new();
        private readonly SemaphoreSlim _refreshLock = new(1, 1);

        private const string TOKEN_URL = "https://accounts.spotify.com/api/token";
        private const string API_BASE_URL = "https://api.spotify.com/v1/";
        private const string REDIRECT_URI = "sociallyanxioushub://callback"; // Must be registered in Spotify Dev Dashboard

        // Constructor
        public SpotifyService()
        {
            _httpClient.BaseAddress = new Uri(API_BASE_URL);
        }

        // Initialize by loading client ID and refresh token from secure storage
        public async Task InitializeAsync()
        {
            _clientId = await SecureStorage.GetAsync("spotify_client_id"); // ensure this is set during onboarding
            _refreshToken = await SecureStorage.GetAsync("spotify_refresh_token");
            var expiresAt = await SecureStorage.GetAsync("spotify_access_token_expires_at_utc");

            if (DateTime.TryParse(expiresAt, out var dt)) _accessTokenExpiresAtUtc = dt;

            _accessToken = await SecureStorage.GetAsync("spotify_access_token");
            if (!string.IsNullOrEmpty(_accessToken))
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _accessToken);
        }

        // Authenticate user using Authorization Code Flow with PKCE
        public async Task<bool> AuthenticateAsync()
        {
            try
            {
                if (string.IsNullOrEmpty(_clientId))
                    throw new InvalidOperationException("Spotify client ID not set. Set it in SecureStorage under 'spotify_client_id'.");

                _codeVerifier = GenerateCodeVerifier();
                var codeChallenge = GenerateCodeChallenge(_codeVerifier);
                var scopes = "user-read-private playlist-read-private";

                var authUrl = $"https://accounts.spotify.com/authorize?client_id={_clientId}&response_type=code&redirect_uri={Uri.EscapeDataString(REDIRECT_URI)}&scope={Uri.EscapeDataString(scopes)}&code_challenge={codeChallenge}&code_challenge_method=S256";

                var authResult = await WebAuthenticator.AuthenticateAsync(new Uri(authUrl), new Uri(REDIRECT_URI));
                if (!authResult.Properties.TryGetValue("code", out var code) || string.IsNullOrEmpty(code))
                    return false;

                await ExchangeCodeForTokenAsync(code, _codeVerifier);
                return true;
            }
            catch (Exception ex)
            {
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    Console.WriteLine($"Spotify AuthenticateAsync failed: {ex.Message}");
                });
                return false;
            }
        }

        // Clears stored tokens (logout)
        public async Task LogoutAsync()
        {
            _accessToken = null;
            _refreshToken = null;
            _accessTokenExpiresAtUtc = default;
            _httpClient.DefaultRequestHeaders.Authorization = null;

            await SecureStorage.SetAsync("spotify_access_token", "");
            await SecureStorage.SetAsync("spotify_refresh_token", "");
            await SecureStorage.SetAsync("spotify_access_token_expires_at_utc", "");
        }

        // Exchange code for tokens
        private async Task ExchangeCodeForTokenAsync(string code, string codeVerifier)
        {
            using var request = new HttpRequestMessage(HttpMethod.Post, TOKEN_URL);
            request.Content = new FormUrlEncodedContent(new Dictionary<string, string>
            {
                { "grant_type", "authorization_code" },
                { "code", code },
                { "redirect_uri", REDIRECT_URI },
                { "client_id", _clientId },
                { "code_verifier", codeVerifier }
            });

            using var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            var tokenResponse = JsonSerializer.Deserialize<TokenResponse>(content);

            _accessToken = tokenResponse.Access_token;
            _refreshToken = tokenResponse.Refresh_token;
            _accessTokenExpiresAtUtc = DateTime.UtcNow.AddSeconds(tokenResponse.Expires_in - 30); // buffer 30s

            // store securely
            await SecureStorage.SetAsync("spotify_access_token", _accessToken ?? "");
            if (!string.IsNullOrEmpty(_refreshToken))
                await SecureStorage.SetAsync("spotify_refresh_token", _refreshToken);
            await SecureStorage.SetAsync("spotify_access_token_expires_at_utc", _accessTokenExpiresAtUtc.ToString("o"));

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _accessToken);
        }

        // Try refresh token flow
        private async Task<bool> TryRefreshTokenAsync()
        {
            if (string.IsNullOrEmpty(_refreshToken)) return false;

            await _refreshLock.WaitAsync();
            try
            {
                // If another thread already refreshed while waiting, skip
                if (!string.IsNullOrEmpty(_accessToken) && _accessTokenExpiresAtUtc > DateTime.UtcNow.AddSeconds(10))
                    return true;

                var request = new FormUrlEncodedContent(new Dictionary<string, string>
                {
                    { "grant_type", "refresh_token" },
                    { "refresh_token", _refreshToken },
                    { "client_id", _clientId }
                });

                var response = await _httpClient.PostAsync(TOKEN_URL, request);
                if (!response.IsSuccessStatusCode)
                {
                    Console.WriteLine($"Spotify refresh token failed: {response.StatusCode}");
                    return false;
                }

                var content = await response.Content.ReadAsStringAsync();
                var tokenResponse = JsonSerializer.Deserialize<TokenResponse>(content);

                _accessToken = tokenResponse.Access_token;
                _accessTokenExpiresAtUtc = DateTime.UtcNow.AddSeconds(tokenResponse.Expires_in - 30);

                await SecureStorage.SetAsync("spotify_access_token", _accessToken ?? "");
                await SecureStorage.SetAsync("spotify_access_token_expires_at_utc", _accessTokenExpiresAtUtc.ToString("o"));

                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _accessToken);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error refreshing token: {ex.Message}");
                return false;
            }
            finally
            {
                _refreshLock.Release();
            }
        }

        // Ensure token exists and is not expired. Refresh if necessary
        private async Task EnsureAccessTokenAsync()
        {
            if (!string.IsNullOrEmpty(_accessToken) && _accessTokenExpiresAtUtc > DateTime.UtcNow.AddSeconds(10))
                return;

            var refreshed = await TryRefreshTokenAsync();
            if (!refreshed)
                throw new InvalidOperationException("User not authenticated or refresh failed.");
        }

        // High-level helper that calls Spotify endpoints and auto-refreshes once on 401
        private async Task<HttpResponseMessage> SendApiRequestAsync(Func<Task<HttpResponseMessage>> apiCall)
        {
            HttpResponseMessage response = await apiCall();

            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                // Try refresh and retry once
                var refreshed = await TryRefreshTokenAsync();
                if (refreshed)
                {
                    response.Dispose();
                    response = await apiCall();
                }
            }

            return response;
        }

        // Search tracks
        public async Task<List<Song>> SearchSongsAsync(string query)
        {
            try
            {
                await EnsureAccessTokenAsync();

                var response = await SendApiRequestAsync(() => _httpClient.GetAsync($"search?q={Uri.EscapeDataString(query)}&type=track&limit=20"));
                if (!response.IsSuccessStatusCode)
                {
                    Console.WriteLine($"Spotify search failed: {response.StatusCode}");
                    return [];
                }

                var content = await response.Content.ReadAsStringAsync();
                using var doc = JsonDocument.Parse(content);
                var list = new List<Song>();

                if (doc.RootElement.TryGetProperty("tracks", out var tracks) && tracks.TryGetProperty("items", out var items))
                {
                    foreach (var item in items.EnumerateArray())
                    {
                        var title = item.GetProperty("name").GetString() ?? "";
                        var artist = item.GetProperty("artists").EnumerateArray().FirstOrDefault().GetProperty("name").GetString() ?? "";
                        var album = item.GetProperty("album").GetProperty("name").GetString() ?? "";
                        var duration = TimeSpan.FromMilliseconds(item.GetProperty("duration_ms").GetInt32());
                        var cover = item.GetProperty("album").GetProperty("images").EnumerateArray().FirstOrDefault().GetProperty("url").GetString() ?? "";
                        var spotifyUrl = item.GetProperty("external_urls").GetProperty("spotify").GetString() ?? "";

                        list.Add(new Song(title, artist, duration, album, cover, spotifyUrl));
                    }
                }

                return list;
            }
            catch (InvalidOperationException)
            {
                // Not authenticated
                return [];
            }
            catch (Exception ex)
            {
                Console.WriteLine($"SearchSongsAsync error: {ex.Message}");
                return [];
            }
        }

        // Helpers / models
        private static string GenerateCodeVerifier()
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789-._~";
            var bytes = new byte[128];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(bytes);
            var result = new char[128];
            for (int i = 0; i < bytes.Length; i++)
                result[i] = chars[bytes[i] % chars.Length];
            return new string(result);
        }

        private static string GenerateCodeChallenge(string codeVerifier)
        {
            var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(codeVerifier));
            var base64 = Convert.ToBase64String(bytes);
            return base64.Replace('+', '-').Replace('/', '_').Replace("=", "");
        }

        private class TokenResponse
        {
            public string Access_token { get; set; }
            public string Token_type { get; set; }
            public int Expires_in { get; set; }
            public string Refresh_token { get; set; }
        }
    }
}