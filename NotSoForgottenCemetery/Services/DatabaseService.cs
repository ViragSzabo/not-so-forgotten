using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using SociallyAnxiousHub.Features;

namespace NotSoForgottenCemetery.Services
{
    public class DatabaseService
    {
        private readonly SQLiteAsyncConnection _database;

        // Constructor
        public DatabaseService(string dbPath)
        {
            // Ensure directory exists
            var dir = Path.GetDirectoryName(dbPath);
            if (!string.IsNullOrEmpty(dir) && !Directory.Exists(dir))
                Directory.CreateDirectory(dir);

            // Initialize SQLite connection
            _database = new SQLiteAsyncConnection(dbPath);
        }

        // Initialize
        public async Task InitializeAsync()
        {
            try
            {
                await _database.CreateTableAsync<UserProfileDb>();
                await _database.CreateTableAsync<MemoryDb>();
                await _database.CreateTableAsync<WhisperDb>();
                await _database.CreateTableAsync<HabitDb>();
                await _database.CreateTableAsync<PlaylistDb>();
                await _database.CreateTableAsync<ChallengeDb>();
                await _database.CreateTableAsync<BadgeDb>();
            }
            catch (Exception ex)
            {
                // Log or handle initialization errors
                Console.WriteLine($"Database initialization error: {ex.Message}");
            }
        }

        // MEMORY
        public Task<int> SaveMemoryAsync(MemoryDb memory) => _database.InsertOrReplaceAsync(memory);
        public Task<List<MemoryDb>> GetMemoriesAsync() => _database.Table<MemoryDb>().ToListAsync();
        public Task<int> DeleteMemoryAsync(MemoryDb memory) => _database.DeleteAsync(memory);

        // USER PROFILE
        public Task<int> SaveUserProfileAsync(UserProfileDb profile) => _database.InsertOrReplaceAsync(profile);
        public Task<List<UserProfileDb>> GetUserProfilesAsync() => _database.Table<UserProfileDb>().ToListAsync();

        // WHISPER
        public Task<int> SaveWhisperAsync(WhisperDb whisper) => _database.InsertOrReplaceAsync(whisper);
        public Task<List<WhisperDb>> GetWhispersAsync() => _database.Table<WhisperDb>().ToListAsync();
        public Task<int> DeleteWhisperAsync(WhisperDb whisper) => _database.DeleteAsync(whisper);

        // HABIT
        public Task<int> SaveHabitAsync(HabitDb habit) => _database.InsertOrReplaceAsync(habit);
        public Task<List<HabitDb>> GetHabitsAsync() => _database.Table<HabitDb>().ToListAsync();

        // PLAYLIST
        public Task<int> SavePlaylistAsync(PlaylistDb playlist) => _database.InsertOrReplaceAsync(playlist);
        public Task<List<PlaylistDb>> GetPlaylistsAsync() => _database.Table<PlaylistDb>().ToListAsync();
        public Task<int> DeletePlaylistAsync(PlaylistDb playlist) => _database.DeleteAsync(playlist);

        // Optional: Generic method to get item by ID
        public Task<T> GetByIdAsync<T>(int id) where T : new()
        {
            return _database.FindAsync<T>(id);
        }
    }
}

// Database Models
[Table("UserProfiles")]
public class UserProfileDb
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string AvatarPath { get; set; }
}

[Table("Memories")]
public class MemoryDb
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public DateTime Date { get; set; }
    public string ImagePath { get; set; }
}

[Table("Whispers")]
public class WhisperDb
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }
    public DateTime Timestamp { get; set; }
    public string Message { get; set; }
}

[Table("Habits")]
public class HabitDb
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public bool IsCompleted { get; set; }
    public DateTime Date { get; set; }
}

[Table("Playlists")]
public class PlaylistDb
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string SpotifyId { get; set; }
}

[Table("Challenges")]
public class ChallengeDb
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public bool IsCompleted { get; set; }
}

[Table("Badges")]
public class BadgeDb
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public bool Unlocked { get; set; }
}