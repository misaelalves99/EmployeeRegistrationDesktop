// src/EmployeeRegistrationApp.Maui/Presentation/Reports/Views/ReportsPage.xaml.cs
using EmployeeRegistrationApp.Maui.Presentation.Reports.ViewModels;

namespace EmployeeRegistrationApp.Maui.Presentation.Reports.Views;

public partial class ReportsPage : ContentPage
{
    private readonly ReportsViewModel _viewModel;

    public ReportsPage(ReportsViewModel viewModel)
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
