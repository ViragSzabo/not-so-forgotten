// ─────────────────────────────────────────────────────────────────
// MauiStubs.cs
// Minimal shim implementations of MAUI APIs so that ViewModels can be
// compiled and tested in a plain net8.0 xUnit project without a MAUI host.
// These stubs do NOT call into the real MAUI runtime.
// ─────────────────────────────────────────────────────────────────

#pragma warning disable CS1998

using System.Collections.ObjectModel;

// --- Service interfaces (normally defined in MauiProgram.cs) ---
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

    // --- Minimal App stub so ViewModels can access App.ServiceProvider ---
    public partial class App
    {
        public static IServiceProvider? ServiceProvider { get; set; }
    }
}

// --- MAUI Shell stub ---
namespace Microsoft.Maui.Controls
{
    public abstract class Shell
    {
        public static Shell? Current => null;
        public Task GoToAsync(string route) => Task.CompletedTask;
    }

    public class Launcher
    {
        public static Launcher Default { get; } = new();
        public Task OpenAsync(string uri) => Task.CompletedTask;
        public Task OpenAsync(Uri uri) => Task.CompletedTask;
    }
}

// --- MAUI MainThread stub ---
namespace Microsoft.Maui
{
    public static class MainThread
    {
        public static void BeginInvokeOnMainThread(Action action) => action();
    }
}

// --- MAUI FileSystem stub ---
namespace Microsoft.Maui.Storage
{
    public class FileSystem
    {
        public static FileSystem Current { get; } = new();
        public static string AppDataDirectory => Path.GetTempPath();
    }

    public class SecureStorage
    {
        public static Task<string?> GetAsync(string key) => Task.FromResult<string?>(null);
        public static Task SetAsync(string key, string value) => Task.CompletedTask;
    }
}

// --- Test-only ISettingsStore mock helper ---
namespace Cemetery
{
    /// <summary>In-memory ISettingsStore for use in unit tests.</summary>
    public class InMemorySettingsStore : ISettingsStore
    {
        private readonly Dictionary<string, string> _store = new();
        public Task<string?> GetAsync(string key) => Task.FromResult<string?>(_store.TryGetValue(key, out var v) ? v : null);
        public Task SetAsync(string key, string value) { _store[key] = value; return Task.CompletedTask; }
    }
}
