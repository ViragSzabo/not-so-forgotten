public class MemoryBoardViewModelTests
{
    private readonly Mock<IDatabase> _dbMock;
    private readonly Mock<ISpotifyService> _spotifyMock;
    private readonly Mock<IYouTubeService> _youtubeMock;

    public MemoryBoardViewModelTests()
    {
        _dbMock = TestHelpers.CreateDatabaseMock();
        _spotifyMock = TestHelpers.CreateSpotifyMock();
        _youtubeMock = TestHelpers.CreateYouTubeMock();
    }

    private MemoryBoardViewModel CreateViewModel(List<MemoryDb>? memories = null)
    {
        _dbMock.Setup(d => d.GetMemoriesAsync()).ReturnsAsync(memories ?? []);
        return new MemoryBoardViewModel(_dbMock.Object, _spotifyMock.Object, _youtubeMock.Object);
    }

    [Fact]
    public void Constructor_InitialMemoriesCollection_IsNotNull()
    {
        var vm = CreateViewModel();
        Assert.NotNull(vm.Memories);
    }

    [Fact]
    public void Constructor_InitializesAllCommands()
    {
        var vm = CreateViewModel();
        Assert.NotNull(vm.ListenOnSpotifyCommand);
        Assert.NotNull(vm.WatchOnYouTubeCommand);
        Assert.NotNull(vm.DeleteMemoryCommand);
    }

    [Fact]
    public async Task DeleteMemoryCommand_WithValidMemory_CallsDatabaseDelete()
    {
        var memory = TestHelpers.SampleMemory();
        var vm = CreateViewModel(new List<MemoryDb> { memory });
        await Task.Delay(50);

        await vm.DeleteMemoryCommand.ExecuteAsync(memory);

        _dbMock.Verify(d => d.DeleteMemoryAsync(memory), Times.Once);
    }

    [Fact]
    public async Task DeleteMemoryCommand_WithNullMemory_DoesNotCallDatabase()
    {
        var vm = CreateViewModel();

        await vm.DeleteMemoryCommand.ExecuteAsync(null);

        _dbMock.Verify(d => d.DeleteMemoryAsync(It.IsAny<MemoryDb>()), Times.Never);
    }

    [Fact]
    public async Task DeleteMemoryCommand_RemovesMemoryFromCollection()
    {
        var memory = TestHelpers.SampleMemory();
        var vm = CreateViewModel();
        vm.Memories.Add(memory);

        await vm.DeleteMemoryCommand.ExecuteAsync(memory);

        Assert.DoesNotContain(memory, vm.Memories);
    }
}
