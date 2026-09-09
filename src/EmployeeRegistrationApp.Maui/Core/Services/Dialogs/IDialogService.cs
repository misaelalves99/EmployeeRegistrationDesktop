// src/EmployeeRegistrationApp.Maui/Core/Services/Dialogs/IDialogService.cs
using System;
using System.Threading.Tasks;

namespace EmployeeRegistrationApp.Maui.Core.Services.Dialogs
{
    public interface IDialogService
    {
        Task ShowInfoAsync(string title, string message, string accept = "OK");
        Task ShowInfoAsync(string message);

        Task ShowWarningAsync(string title, string message, string accept = "OK");
        Task ShowWarningAsync(string message);

        Task ShowErrorAsync(string title, string message, string accept = "OK");
        Task ShowErrorAsync(string message);
        Task ShowErrorAsync(string message, Exception ex);

        Task<bool> ShowConfirmationAsync(string title, string message, string accept = "OK", string cancel = "Cancelar");
        Task<bool> ShowDangerConfirmationAsync(string title, string message, string accept = "Sim, continuar", string cancel = "Cancelar");

        Task ShowAlertAsync(string title, string message, string accept = "OK");
    }
}
