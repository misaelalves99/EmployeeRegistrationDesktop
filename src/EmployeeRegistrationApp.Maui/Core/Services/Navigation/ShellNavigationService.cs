using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using EmployeeRegistrationApp.Maui.Core.Config;
using EmployeeRegistrationApp.Maui.Presentation.Auth.Views;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Maui.ApplicationModel;
using Microsoft.Maui.Controls;

namespace EmployeeRegistrationApp.Maui.Core.Services.Navigation;

public sealed class ShellNavigationService : INavigationService
{
    private readonly Dictionary<string, object> _globalParameters = new();
    private readonly IServiceProvider _serviceProvider;

    public ShellNavigationService(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
    }

    public Task NavigateToAsync(string route, IDictionary<string, object>? parameters = null)
        => GoToAsync(route, parameters);

    public Task NavigateToRootAsync(string route, IDictionary<string, object>? parameters = null)
        => GoToRootAsync(route, parameters);

    public async Task GoToAsync(string route, IDictionary<string, object>? parameters = null)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(route))
                return;

            if (AppConfig.EnableNavigationLogging)
                Debug.WriteLine($"[NAVIGATION] GoToAsync -> {route}");

            if (await TryHandleBootstrapRouteAsync(route, forceRoot: false))
                return;

            if (Shell.Current is null)
            {
                Debug.WriteLine($"[NAVIGATION] Shell.Current está nulo e a rota '{route}' não pertence ao bootstrap.");
                return;
            }

            if (parameters is null || parameters.Count == 0)
                await Shell.Current.GoToAsync(route);
            else
                await Shell.Current.GoToAsync(route, parameters);
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"[NAVIGATION][ERROR] {ex}");
        }
    }

    public async Task GoToRootAsync(string route, IDictionary<string, object>? parameters = null)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(route))
                return;

            if (AppConfig.EnableNavigationLogging)
                Debug.WriteLine($"[NAVIGATION] GoToRootAsync -> {route}");

            if (await TryHandleBootstrapRouteAsync(route, forceRoot: true))
                return;

            if (Shell.Current is null)
            {
                Debug.WriteLine($"[NAVIGATION] Shell.Current está nulo e a rota root '{route}' não pertence ao bootstrap.");
                return;
            }

            var finalRoute = route.StartsWith("//", StringComparison.Ordinal)
                ? route
                : $"//{route.TrimStart('/')}";

            if (parameters is null || parameters.Count == 0)
                await Shell.Current.GoToAsync(finalRoute);
            else
                await Shell.Current.GoToAsync(finalRoute, parameters);
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"[NAVIGATION][ERROR] {ex}");
        }
    }

    public async Task GoBackAsync()
    {
        try
        {
            if (AppConfig.EnableNavigationLogging)
                Debug.WriteLine("[NAVIGATION] GoBackAsync");

            if (Shell.Current is not null)
            {
                await Shell.Current.GoToAsync("..");
                return;
            }

            var navigationPage = CurrentWindow()?.Page as NavigationPage;

            if (navigationPage is not null && navigationPage.Navigation.NavigationStack.Count > 1)
            {
                await MainThread.InvokeOnMainThreadAsync(
                    async () => await navigationPage.PopAsync());
                return;
            }

            Debug.WriteLine("[NAVIGATION] Nenhuma pilha disponível para voltar.");
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"[NAVIGATION][ERROR] {ex}");
        }
    }

    public Task NavigateToLoginAsync()
        => ShowLoginRootAsync();

    public Task NavigateToDashboardAsync()
        => ShowDashboardRootAsync();

    public void SetGlobalParameter(string key, object value)
    {
        if (string.IsNullOrWhiteSpace(key))
            return;

        _globalParameters[key] = value;
    }

    public T? GetGlobalParameter<T>(string key)
    {
        if (string.IsNullOrWhiteSpace(key))
            return default;

        return _globalParameters.TryGetValue(key, out var obj) && obj is T typed
            ? typed
            : default;
    }

    private async Task<bool> TryHandleBootstrapRouteAsync(string route, bool forceRoot)
    {
        var normalized = NormalizeRoute(route);

        if (string.Equals(normalized, NavigationRoutes.AuthLogin, StringComparison.Ordinal))
        {
            await ShowLoginRootAsync();
            return true;
        }

        if (string.Equals(normalized, NavigationRoutes.Dashboard, StringComparison.Ordinal))
        {
            await ShowDashboardRootAsync();
            return true;
        }

        if (Shell.Current is not null && !forceRoot)
            return false;

        if (string.Equals(normalized, NavigationRoutes.AuthRegister, StringComparison.Ordinal))
        {
            await PushBootstrapPageAsync(
                _serviceProvider.GetRequiredService<RegisterPage>());
            return true;
        }

        if (string.Equals(normalized, NavigationRoutes.AuthForgotPassword, StringComparison.Ordinal))
        {
            await PushBootstrapPageAsync(
                _serviceProvider.GetRequiredService<ForgotPasswordPage>());
            return true;
        }

        return false;
    }

    private async Task ShowLoginRootAsync()
    {
        var window = CurrentWindow()
            ?? throw new InvalidOperationException("No active MAUI window is available.");

        var loginPage = _serviceProvider.GetRequiredService<LoginPage>();

        await MainThread.InvokeOnMainThreadAsync(() =>
        {
            window.Page = new NavigationPage(loginPage);
        });
    }

    private async Task ShowDashboardRootAsync()
    {
        var window = CurrentWindow()
            ?? throw new InvalidOperationException("No active MAUI window is available.");

        var shell = _serviceProvider.GetRequiredService<AppShell>();

        await MainThread.InvokeOnMainThreadAsync(() =>
        {
            window.Page = shell;
        });
    }

    private async Task PushBootstrapPageAsync(Page page)
    {
        var navigationPage = CurrentWindow()?.Page as NavigationPage
            ?? throw new InvalidOperationException(
                "The authentication bootstrap NavigationPage is not available.");

        await MainThread.InvokeOnMainThreadAsync(
            async () => await navigationPage.PushAsync(page));
    }

    private static Window? CurrentWindow()
        => Microsoft.Maui.Controls.Application.Current?.Windows.Count > 0
            ? Microsoft.Maui.Controls.Application.Current.Windows[0]
            : null;

    private static string NormalizeRoute(string route)
        => route.Trim().Trim('/');
}