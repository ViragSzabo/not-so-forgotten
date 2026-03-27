namespace Cemetery
{
    public interface ISpotifyService
    {
        Task InitializeAsync();
        Task<bool> AuthenticateAsync();
        Task LogoutAsync();
        Task<List<Song>> SearchSongsAsync(string q);
    }
}
