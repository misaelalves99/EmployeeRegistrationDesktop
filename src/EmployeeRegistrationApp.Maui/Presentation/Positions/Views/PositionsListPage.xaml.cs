// src/EmployeeRegistrationApp.Maui/Presentation/Positions/Views/PositionsListPage.xaml.cs
using System;
using EmployeeRegistrationApp.Application.DTOs.Positions;
using EmployeeRegistrationApp.Maui.Presentation.Positions.ViewModels;
using Microsoft.Maui.Controls;

namespace EmployeeRegistrationApp.Maui.Presentation.Positions.Views
{
    public partial class PositionsListPage : ContentPage
    {
        private readonly PositionsListViewModel _viewModel;

        public PositionsListPage(PositionsListViewModel viewModel)
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

        private async void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.CurrentSelection == null || e.CurrentSelection.Count == 0)
                return;

            if (e.CurrentSelection[0] is PositionDto selected)
            {
                await _viewModel.OpenDetailsAsync(selected);
            }

            // limpa seleção para permitir re-clique
            if (sender is CollectionView cv)
            {
                cv.SelectedItem = null;
            }
        }
    }
}
