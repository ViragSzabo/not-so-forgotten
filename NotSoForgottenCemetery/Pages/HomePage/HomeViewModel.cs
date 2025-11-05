using Microsoft.VisualStudio.PlatformUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            await Shell.Current.GoToAsync(nameof(MemoryBoard.MemoryBoardPage));
        }

        private async Task NavigateToPlaylistsAsync()
        {
            await Shell.Current.GoToAsync(nameof(PlaylistPage.PlaylistPage));
        }
    }
}
