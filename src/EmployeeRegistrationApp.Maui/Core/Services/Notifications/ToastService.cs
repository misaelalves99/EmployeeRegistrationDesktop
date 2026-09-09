// src/EmployeeRegistrationApp.Maui/Core/Services/Notifications/ToastService.cs
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using Microsoft.Maui.ApplicationModel;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace EmployeeRegistrationApp.Maui.Core.Services.Notifications
{
    public sealed class ToastService : IToastService
    {
        private const ToastDuration DefaultDuration = ToastDuration.Short;
        private const double DefaultTextSize = 14;

        public Task ShowToastAsync(string message) => ShowInternalAsync(message);
        public Task ShowSuccessAsync(string message) => ShowInternalAsync($"✅ {message}");
        public Task ShowErrorAsync(string message) => ShowInternalAsync($"❌ {message}");
        public Task ShowInfoAsync(string message) => ShowInternalAsync($"ℹ️ {message}");

        private static async Task ShowInternalAsync(string message)
        {
            try
            {
                await MainThread.InvokeOnMainThreadAsync(async () =>
                {
                    var toast = Toast.Make(message, DefaultDuration, DefaultTextSize);
                    await toast.Show();
                });
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[TOAST][ERROR] {ex}");
            }
        }
    }
}
