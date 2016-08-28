using System;
using System.Windows;
using EclipsePlugInRunner.ViewModels;

namespace EclipsePlugInRunner.Views
{
    internal partial class MainWindow : Window
    {
        private readonly MainViewModel _viewModel;

        public MainWindow(MainViewModel viewModel)
        {
            InitializeComponent();

            _viewModel = viewModel;
            _viewModel.ExitRequested += (o, e) => Close();
            _viewModel.UserMessaged += ViewModelOnUserMessaged;

            DataContext = _viewModel;
        }

        private void ViewModelOnUserMessaged(object sender, string message)
        {
            MessageBox.Show(message, "Alert", MessageBoxButton.OK);
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
