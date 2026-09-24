using EmployeeRegistrationApp.Maui.Core.Services.Departments;
using EmployeeRegistrationApp.Application.Interfaces.Services;
// src/EmployeeRegistrationApp.Maui/MauiProgram.cs
using CommunityToolkit.Maui;
using EmployeeRegistrationApp.Application.DependencyInjection;
using EmployeeRegistrationApp.Infrastructure.DependencyInjection;
using EmployeeRegistrationApp.Infrastructure.Repositories.InMemory;
using EmployeeRegistrationApp.Maui.Core.Config;
using EmployeeRegistrationApp.Maui.Core.Services.Auth;
using EmployeeRegistrationApp.Maui.Core.Services.Dialogs;
using EmployeeRegistrationApp.Maui.Core.Services.Navigation;
using EmployeeRegistrationApp.Maui.Core.Services.Notifications;
using EmployeeRegistrationApp.Maui.Core.Services.Theme;
using EmployeeRegistrationApp.Maui.Presentation.Auth.ViewModels;
using EmployeeRegistrationApp.Maui.Presentation.Auth.Views;
using EmployeeRegistrationApp.Maui.Presentation.Dashboard.ViewModels;
using EmployeeRegistrationApp.Maui.Presentation.Dashboard.Views;
using EmployeeRegistrationApp.Maui.Presentation.Departments.ViewModels;
using EmployeeRegistrationApp.Maui.Presentation.Departments.Views;
using EmployeeRegistrationApp.Maui.Presentation.Employees.ViewModels;
using EmployeeRegistrationApp.Maui.Presentation.Employees.Views;
using EmployeeRegistrationApp.Maui.Presentation.Positions.ViewModels;
using EmployeeRegistrationApp.Maui.Presentation.Positions.Views;
using EmployeeRegistrationApp.Maui.Presentation.Reports.ViewModels;
using EmployeeRegistrationApp.Maui.Presentation.Reports.Views;
using EmployeeRegistrationApp.Maui.Presentation.Settings.ViewModels;
using EmployeeRegistrationApp.Maui.Presentation.Settings.Views;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Maui.Hosting;

namespace EmployeeRegistrationApp.Maui;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();

        builder
            .UseMauiApp<App>()
            .UseMauiCommunityToolkit()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

#if WINDOWS
        // .NET MAUI 10.0.101 maps Picker.Title to WinUI ComboBox.Header.
        // Windows desktop guidance uses native ComboBox.PlaceholderText for
        // the unselected prompt. Keep field labels in XAML and render Title
        // inside the closed selector instead of as an external heading.
        Microsoft.Maui.Handlers.PickerHandler.Mapper.AppendToMapping(
            nameof(Microsoft.Maui.Controls.Picker.Title),
            (handler, picker) =>
            {
                handler.PlatformView.Header = null;
                handler.PlatformView.HeaderTemplate = null;
                handler.PlatformView.PlaceholderText = picker.Title ?? string.Empty;
            });

        // PickerHandler.MapTitleColor calls the same Windows UpdateTitle path,
        // so re-apply the placeholder after that mapping as well.
        Microsoft.Maui.Handlers.PickerHandler.Mapper.AppendToMapping(
            nameof(Microsoft.Maui.Controls.Picker.TitleColor),
            (handler, picker) =>
            {
                handler.PlatformView.Header = null;
                handler.PlatformView.HeaderTemplate = null;
                handler.PlatformView.PlaceholderText = picker.Title ?? string.Empty;
            });
#endif

        // Camadas (Application + Infra InMemory)
        builder.Services.AddApplicationServices();
        builder.Services.AddInMemoryPersistence();

        // Core
        builder.Services.AddSingleton<INavigationService, ShellNavigationService>();
        builder.Services.AddSingleton<IDialogService, DialogService>();
        builder.Services.AddSingleton<IThemeService, ThemeService>();
        builder.Services.AddSingleton<IAuthService, AuthService>();
        builder.Services.AddSingleton<IToastService, ToastService>();
        // ViewModels
        builder.Services.AddTransient<LoginViewModel>();
        builder.Services.AddTransient<RegisterViewModel>();
        builder.Services.AddTransient<ForgotPasswordViewModel>();

        builder.Services.AddTransient<DashboardViewModel>();

        builder.Services.AddTransient<EmployeesListViewModel>();
        builder.Services.AddTransient<EmployeeDetailsViewModel>();
        builder.Services.AddTransient<EmployeeFormViewModel>();
        builder.Services.AddTransient<EmployeeReactivateViewModel>();

        builder.Services.AddTransient<DepartmentsListViewModel>();
        builder.Services.AddTransient<DepartmentDetailsViewModel>();
        builder.Services.AddTransient<DepartmentFormViewModel>();

        builder.Services.AddTransient<PositionsListViewModel>();
        builder.Services.AddTransient<PositionDetailsViewModel>();
        builder.Services.AddTransient<PositionFormViewModel>();

        builder.Services.AddTransient<ReportsViewModel>();
        builder.Services.AddTransient<HeadcountByDepartmentViewModel>();
        builder.Services.AddTransient<SalarySummaryViewModel>();

        builder.Services.AddTransient<SettingsViewModel>();
        builder.Services.AddTransient<CompanySettingsViewModel>();

        // Views
        builder.Services.AddTransient<LoginPage>();
        builder.Services.AddTransient<RegisterPage>();
        builder.Services.AddTransient<ForgotPasswordPage>();
        builder.Services.AddTransient<AppShell>();

        builder.Services.AddTransient<DashboardPage>();

        builder.Services.AddTransient<EmployeesListPage>();
        builder.Services.AddTransient<EmployeeDetailsPage>();
        builder.Services.AddTransient<EmployeeFormPage>();
        builder.Services.AddTransient<EmployeeReactivatePage>();

        builder.Services.AddTransient<DepartmentsListPage>();
        builder.Services.AddTransient<DepartmentDetailsPage>();
        builder.Services.AddTransient<DepartmentFormPage>();

        builder.Services.AddTransient<PositionsListPage>();
        builder.Services.AddTransient<PositionDetailsPage>();
        builder.Services.AddTransient<PositionFormPage>();

        builder.Services.AddTransient<ReportsPage>();
        builder.Services.AddTransient<HeadcountByDepartmentPage>();
        builder.Services.AddTransient<SalarySummaryPage>();

        builder.Services.AddTransient<SettingsPage>();
        builder.Services.AddTransient<CompanySettingsPage>();

        // Department source: local/in-memory in demo mode.
        // Remote is opt-in for non-demo runs through an explicit environment variable.
        var departmentApiBaseAddress =
            Environment.GetEnvironmentVariable("EMPLOYEE_REGISTRATION_DEPARTMENT_API_BASE_ADDRESS");

        if (!AppConfig.IsDemoMode && !string.IsNullOrWhiteSpace(departmentApiBaseAddress))
        {
            builder.Services.AddScoped<IDepartmentAppService>(_ =>
            {
                var client = new HttpClient
                {
                    BaseAddress = new Uri(departmentApiBaseAddress, UriKind.Absolute),
                    Timeout = TimeSpan.FromSeconds(15)
                };

                return new RemoteDepartmentAppService(client);
            });
        }
        var app = builder.Build();

        // ✅ seed depois do build (mesmo container)
        var db = app.Services.GetRequiredService<InMemoryDatabase>();
        db.SeedIfEmpty();

        // ✅ inicializa estado de auth (Preferences)
        app.Services.GetRequiredService<IAuthService>().Initialize();

        return app;
    }
}
