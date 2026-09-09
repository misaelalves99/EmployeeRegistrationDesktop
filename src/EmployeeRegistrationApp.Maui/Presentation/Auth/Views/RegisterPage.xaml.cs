// src/EmployeeRegistrationApp.Maui/Presentation/Auth/Views/RegisterPage.xaml.cs
using EmployeeRegistrationApp.Maui.Presentation.Auth.ViewModels;
using Microsoft.Maui.Controls;

namespace EmployeeRegistrationApp.Maui.Presentation.Auth.Views
{
    /// <summary>
    /// Tela de criação de conta (primeiro usuário/admin do sistema).
    /// </summary>
    public partial class RegisterPage : ContentPage
    {
        public RegisterPage(RegisterViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;
        }
    }
}
