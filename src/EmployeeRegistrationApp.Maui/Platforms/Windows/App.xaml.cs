// src/EmployeeRegistrationApp.Maui/Platforms/Windows/App.xaml.cs
using Microsoft.Maui;
using Microsoft.Maui.Hosting;
using Microsoft.UI.Xaml;

namespace EmployeeRegistrationApp.Maui.WinUI;

/// <summary>
/// Classe de inicialização da aplicação para Windows (WinUI).
/// </summary>
public sealed partial class App : MauiWinUIApplication
{
    public App()
    {
        InitializeComponent();
    }

    protected override MauiApp CreateMauiApp()
    {
        // Usa a configuração principal definida em MauiProgram.cs
        return MauiProgram.CreateMauiApp();
    }
}
