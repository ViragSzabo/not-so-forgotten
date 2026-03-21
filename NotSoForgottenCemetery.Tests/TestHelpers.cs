/// <summary>
/// Shared test helpers and mock factories used across all test classes.
/// </summary>
internal static class TestHelpers
{
    public static Mock<IDatabase> CreateDatabaseMock(List<MemoryDb>? memories = null, List<PlaylistDb>? playlists = null)
    {
        memories ??= [];
        playlists ??= [];
        var mock = new Mock<IDatabase>();
        mock.Setup(d => d.GetMemoriesAsync()).ReturnsAsync(memories);
        mock.Setup(d => d.GetPlaylistsAsync()).ReturnsAsync(playlists);
        mock.Setup(d => d.SaveMemoryAsync(It.IsAny<MemoryDb>())).Returns(Task.CompletedTask);
        mock.Setup(d => d.DeleteMemoryAsync(It.IsAny<MemoryDb>())).Returns(Task.CompletedTask);
        mock.Setup(d => d.SavePlaylistAsync(It.IsAny<PlaylistDb>())).Returns(Task.CompletedTask);
        mock.Setup(d => d.DeletePlaylistAsync(It.IsAny<PlaylistDb>())).Returns(Task.CompletedTask);
        mock.Setup(d => d.InitializeAsync()).Returns(Task.CompletedTask);
        return mock;
    }

    public static Mock<ISpotifyService> CreateSpotifyMock()
    {
        var mock = new Mock<ISpotifyService>();
        mock.Setup(s => s.SearchSongsAsync(It.IsAny<string>())).ReturnsAsync([]);
        mock.Setup(s => s.AuthenticateAsync()).ReturnsAsync(true);
        mock.Setup(s => s.LogoutAsync()).Returns(Task.CompletedTask);
        mock.Setup(s => s.InitializeAsync()).Returns(Task.CompletedTask);
        return mock;
    }

    public static Mock<IYouTubeService> CreateYouTubeMock(string returnedVideoId = "")
    {
        var mock = new Mock<IYouTubeService>();
        mock.Setup(y => y.SearchVideoIdAsync(It.IsAny<string>(), It.IsAny<string>()))
            .ReturnsAsync(returnedVideoId);
        return mock;
    }

    public static MemoryDb SampleMemory(string title = "Test Ghost", int id = 1) =>
        new() { Id = id, Title = title, Description = "A haunting description.", Date = new DateTime(2020, 10, 31), FavoriteSong = "October Rust" };

    public static PlaylistDb SamplePlaylist(string name = "Gothic Selections", string spotifyId = "abc123", int id = 1) =>
        new() { Id = id, Name = name, SpotifyId = spotifyId };
}
