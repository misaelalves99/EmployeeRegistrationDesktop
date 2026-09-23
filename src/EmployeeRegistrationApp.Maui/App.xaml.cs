// src/EmployeeRegistrationApp.Maui/App.xaml.cs
using EmployeeRegistrationApp.Infrastructure.Repositories.InMemory;
using EmployeeRegistrationApp.Maui.Core.Services.Theme;
using EmployeeRegistrationApp.Maui.Presentation.Auth.Views;
using Microsoft.Maui.Controls;

namespace EmployeeRegistrationApp.Maui;

public partial class App : Microsoft.Maui.Controls.Application
{
    private readonly IThemeService _themeService;
    private readonly System.IServiceProvider _serviceProvider;

    public App(
        IThemeService themeService,
        InMemoryDatabase database,
        System.IServiceProvider serviceProvider)
    {
        InitializeComponent();

        _themeService = themeService;
        _serviceProvider = serviceProvider;

        // Tema inicial
        _ = _themeService.InitializeAsync();

        // ✅ Seed do "banco" in-memory (agora com DI correto)
        database.SeedIfEmpty();

        // Fluxo inicial materializado em CreateWindow após a inicialização do App.
    }


    protected override Microsoft.Maui.Controls.Window CreateWindow(Microsoft.Maui.IActivationState? activationState)
    {
        var loginPage = (LoginPage)(
            _serviceProvider.GetService(typeof(LoginPage))
            ?? throw new System.InvalidOperationException("LoginPage service could not be resolved."));

        return new Microsoft.Maui.Controls.Window(new NavigationPage(loginPage));
    }
    public static new App Current => (App)Microsoft.Maui.Controls.Application.Current!;
}
