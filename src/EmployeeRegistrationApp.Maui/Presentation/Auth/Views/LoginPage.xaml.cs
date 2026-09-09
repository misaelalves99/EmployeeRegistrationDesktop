// src/EmployeeRegistrationApp.Maui/Presentation/Auth/Views/LoginPage.xaml.cs
using EmployeeRegistrationApp.Maui.Presentation.Auth.ViewModels;
using Microsoft.Maui.Controls;

namespace EmployeeRegistrationApp.Maui.Presentation.Auth.Views
{
    /// <summary>
    /// Tela de login do sistema Employee Registration (versão desktop MAUI).
    /// </summary>
    public partial class LoginPage : ContentPage
    {
        // ViewModel é resolvido pelo container de DI em MauiProgram.cs
        public LoginPage(LoginViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;
        }
    }
}
