using Microsoft.Maui.Controls;
// src/EmployeeRegistrationApp.Maui/Presentation/Settings/Views/CompanySettingsPage.xaml.cs
using EmployeeRegistrationApp.Maui.Presentation.Settings.ViewModels;

namespace EmployeeRegistrationApp.Maui.Presentation.Settings.Views;

public partial class CompanySettingsPage : ContentPage
{
    private readonly CompanySettingsViewModel _viewModel;

    public CompanySettingsPage(CompanySettingsViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = _viewModel = viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        if (_viewModel != null)
        {
            await _viewModel.InitializeAsync();
        }
    }
}
