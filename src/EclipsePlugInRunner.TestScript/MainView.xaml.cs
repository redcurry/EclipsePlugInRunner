using System.Windows.Controls;

namespace EclipsePlugInRunner.TestScript
{
    public partial class MainView : UserControl
    {
        public MainView(MainViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
        }
    }
}
