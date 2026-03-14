using SQLite;
using NotSoForgottenCemetery.Models;

namespace NotSoForgottenCemetery.Services
{
    public class Database
    {
        private readonly SQLiteAsyncConnection _database;

        public Database(string dbPath)
        {
            var dir = Path.GetDirectoryName(dbPath);
            if (!string.IsNullOrEmpty(dir) && !Directory.Exists(dir))
                Directory.CreateDirectory(dir);

            _database = new SQLiteAsyncConnection(dbPath);
        }

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

        // Generic get by ID
        public Task<T> GetByIdAsync<T>(int id) where T : new() => _database.FindAsync<T>(id);
    }
}