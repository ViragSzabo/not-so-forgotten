namespace Cemetery.Services.Abstractions
{
    public interface ISettingsStore
    {
        Task<string?> GetAsync(string key);
        Task SetAsync(string key, string value);
    }
}
