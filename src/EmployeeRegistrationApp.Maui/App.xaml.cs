// src/EmployeeRegistrationApp.Maui/App.xaml.cs
using EmployeeRegistrationApp.Infrastructure.Repositories.InMemory;
using EmployeeRegistrationApp.Maui.Core.Services.Theme;
using EmployeeRegistrationApp.Maui.Presentation.Auth.Views;
using Microsoft.Maui.Controls;

namespace EmployeeRegistrationApp.Maui;

public partial class App : Application
{
    private readonly IThemeService _themeService;

    public App(
        IThemeService themeService,
        InMemoryDatabase database,
        LoginPage loginPage)
    {
        InitializeComponent();

        _themeService = themeService;

        // Tema inicial
        _themeService.ApplyInitialTheme();

        // ✅ Seed do "banco" in-memory (agora com DI correto)
        database.SeedIfEmpty();

        // ✅ Fluxo inicial
        MainPage = new NavigationPage(loginPage);
    }

    public static new App Current => (App)Application.Current!;
}
