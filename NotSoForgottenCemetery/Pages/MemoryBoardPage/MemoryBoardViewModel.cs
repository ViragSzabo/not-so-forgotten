using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using NotSoForgottenCemetery.Services;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using NotSoForgottenCemetery.Features;

namespace NotSoForgottenCemetery.Pages.MemoryBoard
{
    public partial class MemoryBoardViewModel : ObservableObject
    {
        private readonly Database _dbService;

        [ObservableProperty]
        public ObservableCollection<MemoryDb> memories = new ObservableCollection<MemoryDb>();

        public MemoryBoardViewModel(Database dbService)
        {
            _dbService = dbService;
            LoadMemoriesCommand = new AsyncRelayCommand(LoadMemoriesAsync);
            DeleteMemoryCommand = new AsyncRelayCommand<MemoryDb>(DeleteMemoryAsync);
        }

        // Default constructor required for XAML previewer
        public MemoryBoardViewModel() { }

        public IAsyncRelayCommand LoadMemoriesCommand { get; }
        public IAsyncRelayCommand<MemoryDb> DeleteMemoryCommand { get; }

        private async Task LoadMemoriesAsync()
        {
            var allMemories = await _dbService.GetMemoriesAsync();
            Memories.Clear();
            foreach (var memory in allMemories)
                Memories.Add(memory);
        }

        private async Task DeleteMemoryAsync(MemoryDb memory)
        {
            if (memory == null) return;
            await _dbService.DeleteMemoryAsync(memory);
            Memories.Remove(memory);
        }
    }
}