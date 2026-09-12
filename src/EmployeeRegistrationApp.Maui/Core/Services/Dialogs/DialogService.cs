// src/EmployeeRegistrationApp.Maui/Core/Services/Dialogs/DialogService.cs
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;

namespace EmployeeRegistrationApp.Maui.Core.Services.Dialogs
{
    /// <summary>
    /// Implementação padrão de IDialogService usando DisplayAlert.
    /// </summary>
    public sealed class DialogService : IDialogService
    {
        private Page? MainPage => Microsoft.Maui.Controls.Application.Current?.MainPage;

        public Task ShowInfoAsync(string title, string message, string accept = "OK")
            => ShowAlertInternalAsync(title, message, accept);

        public Task ShowInfoAsync(string message)
            => ShowAlertInternalAsync("Informação", message, "OK");

        public Task ShowWarningAsync(string title, string message, string accept = "OK")
            => ShowAlertInternalAsync(title, message, accept);

        public Task ShowWarningAsync(string message)
            => ShowAlertInternalAsync("Atenção", message, "OK");

        public Task ShowErrorAsync(string title, string message, string accept = "OK")
            => ShowAlertInternalAsync(title, message, accept);

        public Task ShowErrorAsync(string message)
            => ShowAlertInternalAsync("Erro", message, "OK");

        public Task ShowErrorAsync(string message, Exception ex)
        {
            Debug.WriteLine($"[DIALOG][ERROR] {message}");
            Debug.WriteLine(ex);
            return ShowAlertInternalAsync("Erro", message, "OK");
        }

        public async Task<bool> ShowConfirmationAsync(
            string title,
            string message,
            string accept = "OK",
            string cancel = "Cancelar")
        {
            if (MainPage is null)
                return false;

            return await MainPage.DisplayAlert(title, message, accept, cancel);
        }

        public Task<bool> ShowDangerConfirmationAsync(
            string title,
            string message,
            string accept = "Sim, continuar",
            string cancel = "Cancelar")
            => ShowConfirmationAsync(title, message, accept, cancel);

        public Task ShowAlertAsync(string title, string message, string accept = "OK")
            => ShowAlertInternalAsync(title, message, accept);

        private async Task ShowAlertInternalAsync(string title, string message, string accept)
        {
            if (MainPage is null)
                return;

            await MainPage.DisplayAlert(title, message, accept);
        }
    }
}
