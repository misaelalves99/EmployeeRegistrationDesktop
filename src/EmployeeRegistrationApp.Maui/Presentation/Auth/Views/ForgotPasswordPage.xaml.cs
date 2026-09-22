using EmployeeRegistrationApp.Maui.Presentation.Auth.ViewModels;
using Microsoft.Maui.Controls;

namespace EmployeeRegistrationApp.Maui.Presentation.Auth.Views;

public partial class ForgotPasswordPage : ContentPage
{
    public ForgotPasswordPage(ForgotPasswordViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}