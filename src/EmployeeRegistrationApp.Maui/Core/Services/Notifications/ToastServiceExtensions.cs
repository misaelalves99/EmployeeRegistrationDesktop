// src/EmployeeRegistrationApp.Maui/Core/Services/Notifications/ToastServiceExtensions.cs
using System.Threading.Tasks;

namespace EmployeeRegistrationApp.Maui.Core.Services.Notifications
{
    /// <summary>
    /// Helpers para deixar o uso do IToastService mais enxuto nos ViewModels.
    /// </summary>
    public static class ToastServiceExtensions
    {
        public static Task ShowSuccess(this IToastService? toastService, string message)
        {
            return toastService?.ShowSuccessAsync(message) ?? Task.CompletedTask;
        }

        public static Task ShowError(this IToastService? toastService, string message)
        {
            return toastService?.ShowErrorAsync(message) ?? Task.CompletedTask;
        }

        public static Task ShowInfo(this IToastService? toastService, string message)
        {
            return toastService?.ShowInfoAsync(message) ?? Task.CompletedTask;
        }
    }
}
