using Microsoft.Extensions.DependencyInjection;
using NotSoForgottenCemetery.Pages.MemoryBoard;
namespace NotSoForgottenCemetery.Pages.MemoryBoardPage
{
    public partial class MemoryBoardPage : ContentPage
    {
        public MemoryBoardPage(MemoryBoardViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;
        }

        public MemoryBoardPage() : this(App.Services?.GetService<MemoryBoardViewModel>()!) { }
    }
}