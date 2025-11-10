using CommunityToolkit.Mvvm.Input;
using Microsoft.VisualStudio.PlatformUI;

namespace NotSoForgottenCemetery.Pages.HomePage
{
    public partial class HomeViewModel : ObservableObject
    {
        public IAsyncRelayCommand GoToMemoryBoardCommand { get; }
        public IAsyncRelayCommand GoToPlaylistsCommand { get; }

        public HomeViewModel()
        {
            GoToMemoryBoardCommand = new AsyncRelayCommand(NavigateToMemoryBoardAsync);
            GoToPlaylistsCommand = new AsyncRelayCommand(NavigateToPlaylistsAsync);
        }

        private async Task NavigateToMemoryBoardAsync()
        {
            await Shell.Current.GoToAsync(nameof(MemoryBoardPage.MemoryBoardPage));
        }

        private async Task NavigateToPlaylistsAsync()
        {
            await Shell.Current.GoToAsync(nameof(PlaylistPage.PlayListPage));
        }
    }
}