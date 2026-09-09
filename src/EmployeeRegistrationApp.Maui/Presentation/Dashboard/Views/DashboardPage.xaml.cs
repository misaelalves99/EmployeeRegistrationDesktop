// src/EmployeeRegistrationApp.Maui/Presentation/Dashboard/Views/DashboardPage.xaml.cs
using EmployeeRegistrationApp.Maui.Presentation.Dashboard.ViewModels;
using Microsoft.Maui.Controls;

namespace EmployeeRegistrationApp.Maui.Presentation.Dashboard.Views
{
    /// <summary>
    /// Tela principal do dashboard de gestão de pessoas.
    /// </summary>
    public partial class DashboardPage : ContentPage
    {
        private readonly DashboardViewModel _viewModel;

        public DashboardPage(DashboardViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = _viewModel = viewModel;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await _viewModel.LoadAsync();
        }
    }
}
