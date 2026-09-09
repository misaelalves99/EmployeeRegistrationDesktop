// src/EmployeeRegistrationApp.Maui/Presentation/Departments/Views/DepartmentsListPage.xaml.cs
using System;
using System.Linq;
using EmployeeRegistrationApp.Application.DTOs.Departments;
using EmployeeRegistrationApp.Maui.Presentation.Departments.ViewModels;
using Microsoft.Maui.Controls;

namespace EmployeeRegistrationApp.Maui.Presentation.Departments.Views
{
    public partial class DepartmentsListPage : ContentPage
    {
        private readonly DepartmentsListViewModel _viewModel;

        public DepartmentsListPage(DepartmentsListViewModel viewModel)
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
                // Em um app real, podemos logar o erro
                System.Diagnostics.Debug.WriteLine(ex);
            }
        }

        private async void DepartmentsCollectionView_SelectionChanged(
            object sender,
            SelectionChangedEventArgs e)
        {
            if (e.CurrentSelection == null || e.CurrentSelection.Count == 0)
                return;

            var selected = e.CurrentSelection.FirstOrDefault();

            // limpa seleção para permitir novo toque no mesmo item
            if (sender is CollectionView collectionView)
            {
                collectionView.SelectedItem = null;
            }

            if (selected is DepartmentDto department)
            {
                await _viewModel.OpenDetailsAsync(department);
            }
        }
    }
}
