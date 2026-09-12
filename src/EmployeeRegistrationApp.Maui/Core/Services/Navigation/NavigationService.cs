// src/EmployeeRegistrationApp.Maui/Core/Services/Navigation/NavigationService.cs
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EmployeeRegistrationApp.Maui.Core.Config;
using Microsoft.Maui.Controls;

namespace EmployeeRegistrationApp.Maui.Core.Services.Navigation
{
    public sealed class NavigationService : INavigationService
    {
        private readonly Dictionary<string, object> _globalParameters = new();

        private Shell Shell =>
            Microsoft.Maui.Controls.Application.Current?.MainPage as Shell
            ?? throw new InvalidOperationException("MainPage não é um Shell.");

        public Task NavigateToAsync(string route, IDictionary<string, object>? parameters = null)
            => GoToAsync(route, parameters);

        public Task NavigateToRootAsync(string route, IDictionary<string, object>? parameters = null)
            => GoToRootAsync(route, parameters);

        public Task GoToAsync(string route, IDictionary<string, object>? parameters = null)
        {
            if (string.IsNullOrWhiteSpace(route))
                return Task.CompletedTask;

            var shellRoute = route;

            return parameters is null
                ? Shell.GoToAsync(shellRoute)
                : Shell.GoToAsync(shellRoute, parameters);
        }

        public Task GoToRootAsync(string route, IDictionary<string, object>? parameters = null)
        {
            if (string.IsNullOrWhiteSpace(route))
                return Task.CompletedTask;

            var rootRoute = route.StartsWith("//", StringComparison.Ordinal)
                ? route
                : $"//{route.TrimStart('/')}";

            return parameters is null
                ? Shell.GoToAsync(rootRoute)
                : Shell.GoToAsync(rootRoute, parameters);
        }

        public Task GoBackAsync() => Shell.GoToAsync("..");

        public Task NavigateToLoginAsync()
            => GoToRootAsync(NavigationRoutes.Root.Login);

        public Task NavigateToDashboardAsync()
            => GoToRootAsync(NavigationRoutes.Root.Dashboard);

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

            if (_globalParameters.TryGetValue(key, out var value) && value is T typed)
                return typed;

            return default;
        }
    }
}
