namespace Cemetery
{
    public interface IYouTubeService
    {
        Task<string> SearchVideoIdAsync(string q, string t);
    }
}
