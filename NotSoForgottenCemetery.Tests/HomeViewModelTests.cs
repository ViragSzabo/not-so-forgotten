/// <summary>
/// Unit tests for HomeViewModel — focuses on business logic, input validation,
/// and property change notification. MAUI UI calls (Shell, Launcher, MainThread)
/// are not tested here as they require a live MAUI host.
/// </summary>
public class HomeViewModelTests
{
    private readonly Mock<IDatabase> _dbMock;
    private readonly Mock<IYouTubeService> _youtubeMock;

    public HomeViewModelTests()
    {
        _dbMock = TestHelpers.CreateDatabaseMock();
        _youtubeMock = TestHelpers.CreateYouTubeMock();
    }

    private HomeViewModel CreateViewModel(List<MemoryDb>? memories = null)
    {
        _dbMock.Setup(d => d.GetMemoriesAsync()).ReturnsAsync(memories ?? []);
        return new HomeViewModel(_dbMock.Object, _youtubeMock.Object);
    }

    [Fact]
    public void Constructor_ClockTime_DefaultsToPlaceholder()
    {
        using var vm = CreateViewModel();
        Assert.Equal("00:00:00", vm.ClockTime);
    }

    [Fact]
    public void Constructor_CurrentSong_StartsPlayingFirstSong()
    {
        using var vm = CreateViewModel();
        Assert.Equal("Playing: October Rust...", vm.CurrentSong);
    }

    [Fact]
    public void Constructor_CurrentWhisper_EvaluatesToQuietIfNoMemories()
    {
        using var vm = CreateViewModel();
        Assert.Equal("Quiet...", vm.CurrentWhisper);
    }

    [Fact]
    public void Constructor_NewMemoryTitle_DefaultsToEmpty()
    {
        using var vm = CreateViewModel();
        Assert.Equal(string.Empty, vm.NewMemoryTitle);
    }

    [Fact]
    public void Constructor_NewMemoryDescription_DefaultsToEmpty()
    {
        using var vm = CreateViewModel();
        Assert.Equal(string.Empty, vm.NewMemoryDescription);
    }

    [Fact]
    public void Constructor_Memories_IsInitialized()
    {
        using var vm = CreateViewModel();
        Assert.NotNull(vm.Memories);
    }

    [Fact]
    public void Constructor_AllCommands_AreInitialized()
    {
        using var vm = CreateViewModel();
        Assert.NotNull(vm.AddMemoryCommand);
        Assert.NotNull(vm.GoToMemoryBoardCommand);
        Assert.NotNull(vm.GoToPlaylistsCommand);
        Assert.NotNull(vm.WatchOnYouTubeCommand);
    }

    [Fact]
    public async Task AddMemoryCommand_WithEmptyTitle_DoesNotSaveToDatabase()
    {
        using var vm = CreateViewModel();
        vm.NewMemoryTitle = "";

        await vm.AddMemoryCommand.ExecuteAsync(null);

        _dbMock.Verify(d => d.SaveMemoryAsync(It.IsAny<MemoryDb>()), Times.Never);
    }

    [Fact]
    public async Task AddMemoryCommand_WithWhitespaceTitle_DoesNotSaveToDatabase()
    {
        using var vm = CreateViewModel();
        vm.NewMemoryTitle = "   ";

        await vm.AddMemoryCommand.ExecuteAsync(null);

        _dbMock.Verify(d => d.SaveMemoryAsync(It.IsAny<MemoryDb>()), Times.Never);
    }

    [Fact]
    public async Task AddMemoryCommand_WithValidTitle_SavesMemoryToDatabase()
    {
        using var vm = CreateViewModel();
        vm.NewMemoryTitle = "A Haunting Day";
        vm.NewMemoryDescription = "The fog rolled in.";

        await vm.AddMemoryCommand.ExecuteAsync(null);

        _dbMock.Verify(d => d.SaveMemoryAsync(It.Is<MemoryDb>(m =>
            m.Title == "A Haunting Day" &&
            m.Description == "The fog rolled in."
        )), Times.Once);
    }

    [Fact]
    public async Task AddMemoryCommand_WithValidTitle_ClearsTitleAndDescription()
    {
        using var vm = CreateViewModel();
        vm.NewMemoryTitle = "A Haunting Day";
        vm.NewMemoryDescription = "Some details.";

        await vm.AddMemoryCommand.ExecuteAsync(null);

        Assert.Equal(string.Empty, vm.NewMemoryTitle);
        Assert.Equal(string.Empty, vm.NewMemoryDescription);
    }

    [Fact]
    public async Task AddMemoryCommand_WithValidTitle_ReloadsMemories()
    {
        using var vm = CreateViewModel();
        vm.NewMemoryTitle = "A Haunting Day";
        vm.NewMemoryDescription = "The fog rolled in.";

        await vm.AddMemoryCommand.ExecuteAsync(null);

        _dbMock.Verify(d => d.GetMemoriesAsync(), Times.AtLeastOnce);
    }

    [Fact]
    public async Task AddMemoryCommand_SavedMemory_HasCurrentDateAssigned()
    {
        using var vm = CreateViewModel();
        vm.NewMemoryTitle = "A Timely Memory";
        vm.NewMemoryDescription = "A description.";
        var before = DateTime.Now.AddSeconds(-1);

        await vm.AddMemoryCommand.ExecuteAsync(null);

        _dbMock.Verify(d => d.SaveMemoryAsync(It.Is<MemoryDb>(m =>
            m.Date > before && m.Date < DateTime.Now.AddSeconds(1)
        )), Times.Once);
    }

    [Fact]
    public void ClockTime_SetValue_RaisesPropertyChanged()
    {
        using var vm = CreateViewModel();
        var raised = false;
        vm.PropertyChanged += (s, e) => { if (e.PropertyName == nameof(vm.ClockTime)) raised = true; };
        vm.ClockTime = "12:30:00";
        Assert.True(raised);
    }

    [Fact]
    public void CurrentSong_SetValue_RaisesPropertyChanged()
    {
        using var vm = CreateViewModel();
        var raised = false;
        vm.PropertyChanged += (s, e) => { if (e.PropertyName == nameof(vm.CurrentSong)) raised = true; };
        vm.CurrentSong = "Playing: Black No. 1...";
        Assert.True(raised);
    }

    [Fact]
    public void NewMemoryTitle_SetValue_RaisesPropertyChanged()
    {
        using var vm = CreateViewModel();
        var raised = false;
        vm.PropertyChanged += (s, e) => { if (e.PropertyName == nameof(vm.NewMemoryTitle)) raised = true; };
        vm.NewMemoryTitle = "Changed";
        Assert.True(raised);
    }

    [Fact]
    public void Dispose_DoesNotThrow()
    {
        var vm = CreateViewModel();
        var ex = Record.Exception(() => vm.Dispose());
        Assert.Null(ex);
    }
}
