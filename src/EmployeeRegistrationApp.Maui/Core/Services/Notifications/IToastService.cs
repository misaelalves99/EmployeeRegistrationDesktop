// src/EmployeeRegistrationApp.Maui/Core/Services/Notifications/IToastService.cs
using System.Threading.Tasks;

namespace EmployeeRegistrationApp.Maui.Core.Services.Notifications
{
    public interface IToastService
    {
        Task ShowToastAsync(string message);
        Task ShowSuccessAsync(string message);
        Task ShowErrorAsync(string message);
        Task ShowInfoAsync(string message);
    }
}
