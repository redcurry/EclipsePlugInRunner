using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using EclipsePlugInRunner.ViewModels;
using VMS.TPS.Common.Model.API;

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

        private void PatientIdTextBox_OnPreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            _viewModel.UpdatePatientResults(PatientIdTextBox.Text + e.Text);
            PatientIdTextBox.IsDropDownOpen = true;
        }

        // Happens when user selects an item from the drop down
        private void PatientIdTextBox_OnDropDownClosed(object sender, EventArgs e)
        {
            var patientSummary = PatientIdTextBox.SelectedItem as PatientSummary;
            if (patientSummary != null)
            {
                PatientIdTextBox.Text = patientSummary.Id;
            }
        }
    }
}
