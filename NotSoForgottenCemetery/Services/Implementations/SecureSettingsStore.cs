using Cemetery.Services.Abstractions;
using System.Text.Json;

namespace Cemetery.Services.Implementations
{
    /// <summary>Plain JSON file store — works without MSIX package identity.</summary>
    public class SecureSettingsStore : ISettingsStore
    {
        private readonly string _path = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            "cemetery_settings.json");

        private Dictionary<string, string> Load()
        {
            try { return File.Exists(_path) ? JsonSerializer.Deserialize<Dictionary<string, string>>(File.ReadAllText(_path)) ?? new() : new(); }
            catch { return new(); }
        }

        public Task<string?> GetAsync(string key)
        {
            var d = Load();
            return Task.FromResult(d.TryGetValue(key, out var v) ? v : null);
        }

        public Task SetAsync(string key, string value)
        {
            var d = Load();
            d[key] = value;
            File.WriteAllText(_path, JsonSerializer.Serialize(d));
            return Task.CompletedTask;
        }
    }
}
