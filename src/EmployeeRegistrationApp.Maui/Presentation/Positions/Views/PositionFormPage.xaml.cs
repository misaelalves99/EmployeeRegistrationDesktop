// src/EmployeeRegistrationApp.Maui/Presentation/Positions/Views/PositionFormPage.xaml.cs
using System;
using EmployeeRegistrationApp.Maui.Presentation.Positions.ViewModels;
using Microsoft.Maui.Controls;

namespace EmployeeRegistrationApp.Maui.Presentation.Positions.Views
{
    public partial class PositionFormPage : ContentPage
    {
        private readonly PositionFormViewModel _viewModel;

        public PositionFormPage(PositionFormViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = _viewModel = viewModel;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            try
            {
                await _viewModel.InitializeAsync();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);
            }
        }
    }
}
