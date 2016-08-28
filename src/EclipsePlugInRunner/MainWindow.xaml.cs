using System;
using System.Windows;

namespace EclipsePlugInRunner
{
    internal partial class MainWindow : Window
    {
        private readonly MainViewModel _viewModel;

        public MainWindow(MainViewModel viewModel)
        {
            InitializeComponent();

            _viewModel = viewModel;
            _viewModel.ExitRequested += (o, e) => Close();

            DataContext = _viewModel;
        }

        private void MainWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
            _viewModel.StartEclipse();
        }

        private void MainWindow_OnClosed(object sender, EventArgs e)
        {
            _viewModel.StopEclipse();
        }
    }
}
