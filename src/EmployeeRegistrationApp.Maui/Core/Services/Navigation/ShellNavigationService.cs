// src/EmployeeRegistrationApp.Maui/Core/Services/Navigation/ShellNavigationService.cs
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using EmployeeRegistrationApp.Maui.Core.Config;
using Microsoft.Maui.Controls;

namespace EmployeeRegistrationApp.Maui.Core.Services.Navigation;

public sealed class ShellNavigationService : INavigationService
{
    private readonly Dictionary<string, object> _globalParameters = new();

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

            if (Shell.Current is null)
            {
                Debug.WriteLine("[NAVIGATION] Shell.Current está nulo. Navegação ignorada.");
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

            if (Shell.Current is null)
            {
                Debug.WriteLine("[NAVIGATION] Shell.Current está nulo. Navegação ignorada.");
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

            if (Shell.Current is null)
            {
                Debug.WriteLine("[NAVIGATION] Shell.Current está nulo. GoBack ignorado.");
                return;
            }

            await Shell.Current.GoToAsync("..");
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"[NAVIGATION][ERROR] {ex}");
        }
    }

    public Task NavigateToLoginAsync()
        => GoToRootAsync(NavigationRoutes.Root.Login);

    public Task NavigateToDashboardAsync()
        => GoToRootAsync(NavigationRoutes.Root.Dashboard);

    public void SetGlobalParameter(string key, object value)
    {
        if (string.IsNullOrWhiteSpace(key)) return;
        _globalParameters[key] = value;
    }

    public T? GetGlobalParameter<T>(string key)
    {
        if (string.IsNullOrWhiteSpace(key)) return default;
        return _globalParameters.TryGetValue(key, out var obj) && obj is T typed ? typed : default;
    }
}
