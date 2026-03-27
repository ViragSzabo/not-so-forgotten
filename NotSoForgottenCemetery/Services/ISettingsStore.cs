namespace Cemetery
{
    /// <summary>Abstraction over platform secure storage, making it mockable.</summary>
    public interface ISettingsStore
    {
        Task<string?> GetAsync(string key);
        Task SetAsync(string key, string value);
    }
}
