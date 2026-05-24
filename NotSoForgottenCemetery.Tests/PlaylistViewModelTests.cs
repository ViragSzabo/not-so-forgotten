public class PlaylistViewModelTests
{
    private readonly Mock<IDatabase> _dbMock;
    private readonly Mock<ISpotifyService> _spotifyMock;
    private readonly Mock<IYouTubeService> _youtubeMock;

    public PlaylistViewModelTests()
    {
        _dbMock = TestHelpers.CreateDatabaseMock();
        _spotifyMock = TestHelpers.CreateSpotifyMock();
        _youtubeMock = TestHelpers.CreateYouTubeMock();
    }

    private PlaylistViewModel CreateViewModel(List<PlaylistDb>? playlists = null)
    {
        _dbMock.Setup(d => d.GetPlaylistsAsync()).ReturnsAsync(playlists ?? []);
        return new PlaylistViewModel(_dbMock.Object, _spotifyMock.Object, _youtubeMock.Object);
    }

    [Fact]
    public void Constructor_InitializesPlaylists_AsNotNull()
    {
        var vm = CreateViewModel();
        Assert.NotNull(vm.Playlists);
    }

    [Fact]
    public void Constructor_InitializesAddCommand()
    {
        var vm = CreateViewModel();
        Assert.NotNull(vm.AddCommand);
    }

    [Fact]
    public void Constructor_InitializesOpenCommand()
    {
        var vm = CreateViewModel();
        Assert.NotNull(vm.OpenCommand);
    }

    [Fact]
    public async Task AddCommand_WithEmptyName_DoesNotCallDatabase()
    {
        var vm = CreateViewModel();
        vm.NewName = "";

        await vm.AddCommand.ExecuteAsync(null);

        _dbMock.Verify(d => d.SavePlaylistAsync(It.IsAny<PlaylistDb>()), Times.Never);
    }

    [Fact]
    public async Task AddCommand_WithWhitespaceName_DoesNotCallDatabase()
    {
        var vm = CreateViewModel();
        vm.NewName = "   ";

        await vm.AddCommand.ExecuteAsync(null);

        _dbMock.Verify(d => d.SavePlaylistAsync(It.IsAny<PlaylistDb>()), Times.Never);
    }

    [Fact]
    public async Task AddCommand_WithValidName_SavesPlaylistToDatabase()
    {
        var vm = CreateViewModel();
        vm.NewName = "Gothic Selections";

        await vm.AddCommand.ExecuteAsync(null);

        _dbMock.Verify(d => d.SavePlaylistAsync(It.Is<PlaylistDb>(p => p.Name == "Gothic Selections")), Times.Once);
    }

    [Fact]
    public async Task AddCommand_WithValidName_ClearsNewName()
    {
        var vm = CreateViewModel();
        vm.NewName = "Gothic Selections";

        await vm.AddCommand.ExecuteAsync(null);

        Assert.Equal("", vm.NewName);
    }

    [Fact]
    public async Task AddCommand_WithValidName_ReloadsPlaylists()
    {
        var vm = CreateViewModel();
        vm.NewName = "New List";

        await vm.AddCommand.ExecuteAsync(null);

        _dbMock.Verify(d => d.GetPlaylistsAsync(), Times.AtLeast(2));
    }

    [Fact]
    public void NewName_PropertyChanged_RaisesEvent()
    {
        var vm = CreateViewModel();
        var raised = false;
        vm.PropertyChanged += (s, e) => { if (e.PropertyName == nameof(vm.NewName)) raised = true; };

        vm.NewName = "Changed";

        Assert.True(raised);
    }

    [Fact]
    public void NewDescription_PropertyChanged_RaisesEvent()
    {
        var vm = CreateViewModel();
        var raised = false;
        vm.PropertyChanged += (s, e) => { if (e.PropertyName == nameof(vm.NewDescription)) raised = true; };

        vm.NewDescription = "New desc";

        Assert.True(raised);
    }

    [Fact]
    public void NewSpotifyId_PropertyChanged_RaisesEvent()
    {
        var vm = CreateViewModel();
        var raised = false;
        vm.PropertyChanged += (s, e) => { if (e.PropertyName == nameof(vm.NewSpotifyId)) raised = true; };

        vm.NewSpotifyId = "spotify:playlist:123";

        Assert.True(raised);
    }

    [Fact]
    public async Task AddCommand_WithValidFields_SavesFullPlaylistToDatabase()
    {
        var vm = CreateViewModel();
        vm.NewName = "Synthwave Nostalgia";
        vm.NewDescription = "Retro futuristic beats.";
        vm.NewSpotifyId = "spotify:playlist:retro";

        await vm.AddCommand.ExecuteAsync(null);

        _dbMock.Verify(d => d.SavePlaylistAsync(It.Is<PlaylistDb>(p =>
            p.Name == "Synthwave Nostalgia" &&
            p.Description == "Retro futuristic beats." &&
            p.SpotifyId == "spotify:playlist:retro"
        )), Times.Once);
    }

    [Fact]
    public async Task AddCommand_WithValidFields_ClearsAllFields()
    {
        var vm = CreateViewModel();
        vm.NewName = "Goth Rock";
        vm.NewDescription = "Classic 80s goth rock.";
        vm.NewSpotifyId = "spotify:playlist:goth";

        await vm.AddCommand.ExecuteAsync(null);

        Assert.Equal(string.Empty, vm.NewName);
        Assert.Equal(string.Empty, vm.NewDescription);
        Assert.Equal(string.Empty, vm.NewSpotifyId);
    }

    [Fact]
    public async Task DeleteCommand_WithValidPlaylist_CallsDatabaseDelete()
    {
        var playlist = new PlaylistDb { Id = 1, Name = "To Delete" };
        var vm = CreateViewModel(new List<PlaylistDb> { playlist });

        await vm.DeleteCommand.ExecuteAsync(playlist);

        _dbMock.Verify(d => d.DeletePlaylistAsync(playlist), Times.Once);
    }

    [Fact]
    public async Task DeleteCommand_WithValidPlaylist_ReloadsPlaylists()
    {
        var playlist = new PlaylistDb { Id = 2, Name = "Reload List" };
        var vm = CreateViewModel(new List<PlaylistDb> { playlist });

        await vm.DeleteCommand.ExecuteAsync(playlist);

        _dbMock.Verify(d => d.GetPlaylistsAsync(), Times.AtLeast(2));
    }
}
