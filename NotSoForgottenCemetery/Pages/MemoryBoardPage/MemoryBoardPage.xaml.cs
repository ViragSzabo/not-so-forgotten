using Cemetery.ViewModels;

namespace Cemetery
{
    public partial class MemoryBoardPage : ContentPage
    {
        public MemoryBoardPage(MemoryBoardViewModel vm) { InitializeComponent(); BindingContext = vm; }
    }
}
