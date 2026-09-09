// src/EmployeeRegistrationApp.Maui/Core/Services/Navigation/INavigationService.cs
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EmployeeRegistrationApp.Maui.Core.Services.Navigation
{
    public interface INavigationService
    {
        Task NavigateToAsync(string route, IDictionary<string, object>? parameters = null);
        Task NavigateToRootAsync(string route, IDictionary<string, object>? parameters = null);

        Task GoToAsync(string route, IDictionary<string, object>? parameters = null);
        Task GoToRootAsync(string route, IDictionary<string, object>? parameters = null);

        Task GoBackAsync();

        Task NavigateToLoginAsync();
        Task NavigateToDashboardAsync();

        void SetGlobalParameter(string key, object value);
        T? GetGlobalParameter<T>(string key);
    }
}
