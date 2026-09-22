using Microsoft.Maui.Controls;
// src/EmployeeRegistrationApp.Maui/Presentation/Reports/Views/SalarySummaryPage.xaml.cs
using EmployeeRegistrationApp.Maui.Presentation.Reports.ViewModels;

namespace EmployeeRegistrationApp.Maui.Presentation.Reports.Views;

public partial class SalarySummaryPage : ContentPage
{
    private readonly SalarySummaryViewModel _viewModel;

    public SalarySummaryPage(SalarySummaryViewModel viewModel)
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
