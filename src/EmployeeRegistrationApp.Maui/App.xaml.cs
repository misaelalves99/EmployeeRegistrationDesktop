// src/EmployeeRegistrationApp.Maui/App.xaml.cs
using EmployeeRegistrationApp.Infrastructure.Repositories.InMemory;
using EmployeeRegistrationApp.Maui.Core.Services.Theme;
using EmployeeRegistrationApp.Maui.Presentation.Auth.Views;
using Microsoft.Maui.Controls;

namespace EmployeeRegistrationApp.Maui;

public partial class App : Microsoft.Maui.Controls.Application
{
    private readonly IThemeService _themeService;

    public App(
        IThemeService themeService,
        InMemoryDatabase database,
        System.IServiceProvider serviceProvider)
    {
        InitializeComponent();

        _themeService = themeService;

        // Tema inicial
        _ = _themeService.ApplyInitialThemeAsync();

        // ✅ Seed do "banco" in-memory (agora com DI correto)
        database.SeedIfEmpty();

        // ✅ Fluxo inicial
        var loginPage = (LoginPage)(
            serviceProvider.GetService(typeof(LoginPage))
            ?? throw new System.InvalidOperationException("LoginPage service could not be resolved."));

        MainPage = new NavigationPage(loginPage);
    }

    public static new App Current => (App)Microsoft.Maui.Controls.Application.Current!;
}
