public class SettingsViewModelTests
{
    private readonly Mock<IDatabase> _dbMock;
    private readonly Mock<ISpotifyService> _spotifyMock;
    private readonly InMemorySettingsStore _settings;

    public SettingsViewModelTests()
    {
        _dbMock = TestHelpers.CreateDatabaseMock();
        _spotifyMock = TestHelpers.CreateSpotifyMock();
        _settings = new InMemorySettingsStore();
    }

    private SettingsViewModel CreateViewModel() =>
        new(_dbMock.Object, _spotifyMock.Object, _settings);

    [Fact]
    public void Constructor_SaveCommand_IsNotNull()
    {
        var vm = CreateViewModel();
        Assert.NotNull(vm.SaveCommand);
    }

    [Fact]
    public void Constructor_SpotifyClientId_DefaultsToEmpty()
    {
        var vm = CreateViewModel();
        Assert.Equal(string.Empty, vm.SpotifyClientId);
    }

    [Fact]
    public void Constructor_YoutubeApiKey_DefaultsToEmpty()
    {
        var vm = CreateViewModel();
        Assert.Equal(string.Empty, vm.YoutubeApiKey);
    }

    [Fact]
    public void Constructor_StatusMessage_DefaultsToEmpty()
    {
        var vm = CreateViewModel();
        Assert.Equal(string.Empty, vm.StatusMessage);
    }

    [Fact]
    public void SpotifyClientId_SetValue_RaisesPropertyChanged()
    {
        var vm = CreateViewModel();
        var raised = false;
        vm.PropertyChanged += (s, e) => { if (e.PropertyName == nameof(vm.SpotifyClientId)) raised = true; };
        vm.SpotifyClientId = "new-client-id";
        Assert.True(raised);
    }

    [Fact]
    public void YoutubeApiKey_SetValue_RaisesPropertyChanged()
    {
        var vm = CreateViewModel();
        var raised = false;
        vm.PropertyChanged += (s, e) => { if (e.PropertyName == nameof(vm.YoutubeApiKey)) raised = true; };
        vm.YoutubeApiKey = "new-api-key";
        Assert.True(raised);
    }

    [Fact]
    public void StatusMessage_SetValue_RaisesPropertyChanged()
    {
        var vm = CreateViewModel();
        var raised = false;
        vm.PropertyChanged += (s, e) => { if (e.PropertyName == nameof(vm.StatusMessage)) raised = true; };
        vm.StatusMessage = "Saved!";
        Assert.True(raised);
    }

    [Fact]
    public void SpotifyClientId_AssignedValue_ReturnsCorrectValue()
    {
        var vm = CreateViewModel();
        vm.SpotifyClientId = "test-id-123";
        Assert.Equal("test-id-123", vm.SpotifyClientId);
    }

    [Fact]
    public void YoutubeApiKey_AssignedValue_ReturnsCorrectValue()
    {
        var vm = CreateViewModel();
        vm.YoutubeApiKey = "yt-key-abc";
        Assert.Equal("yt-key-abc", vm.YoutubeApiKey);
    }

    [Fact]
    public async Task SaveCommand_PersistsSpotifyClientId()
    {
        var vm = CreateViewModel();
        vm.SpotifyClientId = "my-id";
        vm.YoutubeApiKey = "my-key";

        await vm.SaveCommand.ExecuteAsync(null);

        Assert.Equal("my-id",  await _settings.GetAsync("spotify_client_id"));
        Assert.Equal("my-key", await _settings.GetAsync("youtube_api_key"));
    }
}
