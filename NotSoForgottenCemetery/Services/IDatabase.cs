using NotSoForgottenCemetery.Models;

namespace NotSoForgottenCemetery.Services
{
    /// <summary>
    /// Defines the operations for the Not-So-Forgotten Cemetery local database.
    /// </summary>
    public interface IDatabase
    {
        /// <summary>
        /// Initializes the database tables asynchronously.
        /// </summary>
        Task InitializeAsync();
        
        /// <summary>
        /// Saves or updates a memory in the database.
        /// </summary>
        Task<int> SaveMemoryAsync(MemoryDb memory);

        /// <summary>
        /// Retrieves all stored memories ordered by date.
        /// </summary>
        Task<List<MemoryDb>> GetMemoriesAsync();

        /// <summary>
        /// Removes a memory from the graveyard.
        /// </summary>
        Task<int> DeleteMemoryAsync(MemoryDb memory);

        /// <summary>
        /// Saves or updates a user profile.
        /// </summary>
        Task<int> SaveUserProfileAsync(UserProfileDb profile);

        /// <summary>
        /// Retrieves all user profiles.
        /// </summary>
        Task<List<UserProfileDb>> GetUserProfilesAsync();

        /// <summary>
        /// Saves a new whisper to the mist.
        /// </summary>
        Task<int> SaveWhisperAsync(WhisperDb whisper);

        /// <summary>
        /// Retrieves all whispers.
        /// </summary>
        Task<List<WhisperDb>> GetWhispersAsync();

        /// <summary>
        /// Removes a whisper.
        /// </summary>
        Task<int> DeleteWhisperAsync(WhisperDb whisper);

        /// <summary>
        /// Saves or updates a playlist.
        /// </summary>
        Task<int> SavePlaylistAsync(PlaylistDb playlist);

        /// <summary>
        /// Retrieves all playlists.
        /// </summary>
        Task<List<PlaylistDb>> GetPlaylistsAsync();

        /// <summary>
        /// Removes a playlist.
        /// </summary>
        Task<int> DeletePlaylistAsync(PlaylistDb playlist);

        /// <summary>
        /// Retrieves a record by its unique identifier.
        /// </summary>
        Task<T> GetByIdAsync<T>(int id) where T : new();
    }
}
