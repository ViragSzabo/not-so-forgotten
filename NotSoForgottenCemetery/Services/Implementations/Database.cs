using SQLite;
using Cemetery.Services.Abstractions;

namespace Cemetery.Services.Implementations
{
    public class Database : IDatabase
    {
        private SQLiteAsyncConnection? _db;
        private readonly string _path;
        public Database(string path) => _path = path;

        public async Task InitializeAsync()
        {
            if (_db != null) return;
            _db = new SQLiteAsyncConnection(_path);
            await _db.CreateTableAsync<MemoryDb>();
            await _db.CreateTableAsync<WhisperDb>();
            await _db.CreateTableAsync<PlaylistDb>();
        }

        private async Task EnsureInitializedAsync() => await InitializeAsync();

        public async Task<List<MemoryDb>> GetMemoriesAsync()
        {
            await EnsureInitializedAsync();
            return await _db!.Table<MemoryDb>().ToListAsync();
        }

        public async Task SaveMemoryAsync(MemoryDb m)
        {
            await EnsureInitializedAsync();
            await _db!.InsertOrReplaceAsync(m);
        }

        public async Task DeleteMemoryAsync(MemoryDb m)
        {
            await EnsureInitializedAsync();
            await _db!.DeleteAsync(m);
        }

        public async Task<List<PlaylistDb>> GetPlaylistsAsync()
        {
            await EnsureInitializedAsync();
            return await _db!.Table<PlaylistDb>().ToListAsync();
        }

        public async Task SavePlaylistAsync(PlaylistDb p)
        {
            await EnsureInitializedAsync();
            await _db!.InsertOrReplaceAsync(p);
        }

        public async Task DeletePlaylistAsync(PlaylistDb p)
        {
            await EnsureInitializedAsync();
            await _db!.DeleteAsync(p);
        }
    }
}
