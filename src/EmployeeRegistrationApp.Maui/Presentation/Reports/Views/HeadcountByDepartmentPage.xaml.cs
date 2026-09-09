// src/EmployeeRegistrationApp.Maui/Presentation/Reports/Views/HeadcountByDepartmentPage.xaml.cs
using EmployeeRegistrationApp.Maui.Presentation.Reports.ViewModels;

namespace EmployeeRegistrationApp.Maui.Presentation.Reports.Views;

public partial class HeadcountByDepartmentPage : ContentPage
{
    private readonly HeadcountByDepartmentViewModel _viewModel;

    public HeadcountByDepartmentPage(HeadcountByDepartmentViewModel viewModel)
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
