namespace Cemetery.Services.Abstractions
{
    public interface ISpotifyService
    {
        Task InitializeAsync();
        Task<bool> AuthenticateAsync();
        Task LogoutAsync();
        Task<List<Song>> SearchSongsAsync(string q);
    }

    public interface IYouTubeService
    {
        Task<string> SearchVideoIdAsync(string q, string t);
    }
}
