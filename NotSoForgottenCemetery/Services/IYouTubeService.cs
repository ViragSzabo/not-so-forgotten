namespace NotSoForgottenCemetery.Services
{
    /// <summary>
    /// Provides video search capabilities via the YouTube Data API.
    /// </summary>
    public interface IYouTubeService
    {
        /// <summary>
        /// Searches for a video ID based on artist and song title.
        /// </summary>
        Task<string?> SearchVideoIdAsync(string artist, string title);
    }
}
