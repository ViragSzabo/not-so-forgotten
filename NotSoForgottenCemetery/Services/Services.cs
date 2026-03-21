namespace Cemetery
{
    public class SpotifyService : ISpotifyService
    {
        public Task InitializeAsync() => Task.CompletedTask;
        public Task<bool> AuthenticateAsync() => Task.FromResult(true);
        public Task LogoutAsync() => Task.CompletedTask;
        public Task<List<Song>> SearchSongsAsync(string q) => Task.FromResult(new List<Song>());
    }

    public class YouTubeService : IYouTubeService
    {
        public Task<string> SearchVideoIdAsync(string q, string t) => Task.FromResult(string.Empty);
    }
}
