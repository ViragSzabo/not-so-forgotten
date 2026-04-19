using Cemetery.Services.Abstractions;

namespace Cemetery.Services.Implementations
{
    /// <summary>Production implementation backed by MAUI SecureStorage.</summary>
    public class SecureSettingsStore : ISettingsStore
    {
        public Task<string?> GetAsync(string key) => SecureStorage.GetAsync(key);
        public Task SetAsync(string key, string value) => SecureStorage.SetAsync(key, value);
    }
}
