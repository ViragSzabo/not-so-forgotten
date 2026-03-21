/// <summary>
/// Tests for service interface contracts and model integrity.
/// </summary>
public class ServiceContractTests
{
    [Fact]
    public async Task IDatabase_GetMemoriesAsync_ReturnsEmptyListByDefault()
    {
        var mock = TestHelpers.CreateDatabaseMock();
        var result = await mock.Object.GetMemoriesAsync();
        Assert.Empty(result);
    }

    [Fact]
    public async Task IDatabase_GetMemoriesAsync_ReturnsSeedData()
    {
        var memories = new List<MemoryDb> { TestHelpers.SampleMemory("Ghost A"), TestHelpers.SampleMemory("Ghost B", 2) };
        var mock = TestHelpers.CreateDatabaseMock(memories: memories);

        var result = await mock.Object.GetMemoriesAsync();

        Assert.Equal(2, result.Count);
        Assert.Contains(result, m => m.Title == "Ghost A");
        Assert.Contains(result, m => m.Title == "Ghost B");
    }

    [Fact]
    public async Task IDatabase_SaveMemoryAsync_VerifiesCall()
    {
        var mock = TestHelpers.CreateDatabaseMock();
        var memory = TestHelpers.SampleMemory();

        await mock.Object.SaveMemoryAsync(memory);

        mock.Verify(d => d.SaveMemoryAsync(memory), Times.Once);
    }

    [Fact]
    public async Task IDatabase_DeleteMemoryAsync_VerifiesCall()
    {
        var mock = TestHelpers.CreateDatabaseMock();
        var memory = TestHelpers.SampleMemory();

        await mock.Object.DeleteMemoryAsync(memory);

        mock.Verify(d => d.DeleteMemoryAsync(memory), Times.Once);
    }

    [Fact]
    public async Task IDatabase_GetPlaylistsAsync_ReturnsSeedData()
    {
        var playlists = new List<PlaylistDb> { TestHelpers.SamplePlaylist("Mournful") };
        var mock = TestHelpers.CreateDatabaseMock(playlists: playlists);

        var result = await mock.Object.GetPlaylistsAsync();

        Assert.Single(result);
        Assert.Equal("Mournful", result[0].Name);
    }

    [Fact]
    public async Task ISpotifyService_AuthenticateAsync_ReturnsTrue()
    {
        var mock = TestHelpers.CreateSpotifyMock();
        Assert.True(await mock.Object.AuthenticateAsync());
    }

    [Fact]
    public async Task ISpotifyService_SearchSongsAsync_ReturnsEmptyList()
    {
        var mock = TestHelpers.CreateSpotifyMock();
        Assert.Empty(await mock.Object.SearchSongsAsync("gothic"));
    }

    [Fact]
    public async Task IYouTubeService_SearchVideoIdAsync_ReturnsEmptyStringByDefault()
    {
        var mock = TestHelpers.CreateYouTubeMock();
        Assert.Equal("", await mock.Object.SearchVideoIdAsync("query", "music"));
    }

    [Fact]
    public async Task IYouTubeService_SearchVideoIdAsync_ReturnsConfiguredId()
    {
        var mock = TestHelpers.CreateYouTubeMock("dQw4w9WgXcQ");
        Assert.Equal("dQw4w9WgXcQ", await mock.Object.SearchVideoIdAsync("Never Gonna Give You Up", "music"));
    }

    [Fact]
    public void MemoryDb_DefaultValues_AreEmpty()
    {
        var m = new MemoryDb();
        Assert.Equal(string.Empty, m.Title);
        Assert.Equal(string.Empty, m.Description);
        Assert.Equal(string.Empty, m.ImagePath);
        Assert.Equal(string.Empty, m.FavoriteSong);
    }

    [Fact]
    public void MemoryDb_AssignedValues_AreRetained()
    {
        var m = TestHelpers.SampleMemory("Revenant");
        Assert.Equal("Revenant", m.Title);
        Assert.Equal(new DateTime(2020, 10, 31), m.Date);
        Assert.Equal("October Rust", m.FavoriteSong);
    }

    [Fact]
    public void PlaylistDb_DefaultValues_AreEmpty()
    {
        var p = new PlaylistDb();
        Assert.Equal(string.Empty, p.Name);
    }

    [Fact]
    public void PlaylistDb_AssignedValues_AreRetained()
    {
        var p = TestHelpers.SamplePlaylist("Mournful", "spotify123");
        Assert.Equal("Mournful", p.Name);
        Assert.Equal("spotify123", p.SpotifyId);
    }
}
