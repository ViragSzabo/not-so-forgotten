using Moq;
using NotSoForgottenCemetery.Models;
using NotSoForgottenCemetery.Pages.HomePage;
using NotSoForgottenCemetery.Services;
using Xunit;

namespace NotSoForgottenCemetery.Tests
{
    public class HomeViewModelTests
    {
        [Fact]
        public async Task AddMemoryAsync_WithValidInput_SavesToDatabase()
        {
            // Arrange
            var mockDb = new Mock<IDatabase>();
            var mockYoutube = new Mock<IYouTubeService>();
            var viewModel = new HomeViewModel(mockDb.Object, mockYoutube.Object);
            
            viewModel.NewMemoryTitle = "Test Memory";
            viewModel.NewMemoryDescription = "Test Description";

            // Act
            // await viewModel.AddMemoryCommand.ExecuteAsync(null); // Assuming Command is exposed

            // Assert
            mockDb.Verify(db => db.SaveMemoryAsync(It.IsAny<MemoryDb>()), Times.Once);
        }
    }
}
