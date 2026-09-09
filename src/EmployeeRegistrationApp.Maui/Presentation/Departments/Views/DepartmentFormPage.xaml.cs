// src/EmployeeRegistrationApp.Maui/Presentation/Departments/Views/DepartmentFormPage.xaml.cs
using System;
using EmployeeRegistrationApp.Maui.Presentation.Departments.ViewModels;
using Microsoft.Maui.Controls;

namespace EmployeeRegistrationApp.Maui.Presentation.Departments.Views
{
    public partial class DepartmentFormPage : ContentPage
    {
        private readonly DepartmentFormViewModel _viewModel;

        public DepartmentFormPage(DepartmentFormViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = _viewModel = viewModel;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            try
            {
                // Pode carregar dados quando estiver em modo edição
                await _viewModel.InitializeAsync();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);
            }
        }
    }
}
