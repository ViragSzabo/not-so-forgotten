using NotSoForgottenCemetery.Models;

namespace NotSoForgottenCemetery.Services
{
    /// <summary>
    /// Provides integration with the Spotify Web API.
    /// </summary>
    public interface ISpotifyService
    {
        /// <summary>
        /// Initializes the service with stored credentials.
        /// </summary>
        Task InitializeAsync();

        /// <summary>
        /// Authenticates the user using Spotify's OAuth flow.
        /// </summary>
        Task<bool> AuthenticateAsync();

        /// <summary>
        /// Clears stored tokens and logs the user out.
        /// </summary>
        Task LogoutAsync();

        /// <summary>
        /// Searches for songs on Spotify.
        /// </summary>
        Task<List<Song>> SearchSongsAsync(string query);
    }
}
