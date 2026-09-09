// src/EmployeeRegistrationApp.Maui/Presentation/Departments/Views/DepartmentDetailsPage.xaml.cs
using System;
using EmployeeRegistrationApp.Maui.Presentation.Departments.ViewModels;
using Microsoft.Maui.Controls;

namespace EmployeeRegistrationApp.Maui.Presentation.Departments.Views
{
    public partial class DepartmentDetailsPage : ContentPage
    {
        private readonly DepartmentDetailsViewModel _viewModel;

        // O departamento (Id) deve ser passado via NavigationService com query params
        public DepartmentDetailsPage(DepartmentDetailsViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = _viewModel = viewModel;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            try
            {
                await _viewModel.LoadAsync();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);
            }
        }
    }
}
