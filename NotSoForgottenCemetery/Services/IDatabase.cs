namespace Cemetery
{
    public interface IDatabase
    {
        Task InitializeAsync();
        Task<List<MemoryDb>> GetMemoriesAsync();
        Task SaveMemoryAsync(MemoryDb m);
        Task DeleteMemoryAsync(MemoryDb m);
        Task<List<PlaylistDb>> GetPlaylistsAsync();
        Task SavePlaylistAsync(PlaylistDb p);
        Task DeletePlaylistAsync(PlaylistDb p);
    }
}
