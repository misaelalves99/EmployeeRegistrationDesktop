// src/EmployeeRegistrationApp.Maui/Presentation/Positions/Views/PositionDetailsPage.xaml.cs
using System;
using EmployeeRegistrationApp.Maui.Presentation.Positions.ViewModels;
using Microsoft.Maui.Controls;

namespace EmployeeRegistrationApp.Maui.Presentation.Positions.Views
{
    public partial class PositionDetailsPage : ContentPage
    {
        private readonly PositionDetailsViewModel _viewModel;

        public PositionDetailsPage(PositionDetailsViewModel viewModel)
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
