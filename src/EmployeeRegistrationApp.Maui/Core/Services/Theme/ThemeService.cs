// src/EmployeeRegistrationApp.Maui/Core/Services/Theme/ThemeService.cs
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using EmployeeRegistrationApp.Maui.Core.Config;
using Microsoft.Maui;
using Microsoft.Maui.ApplicationModel;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Storage;

namespace EmployeeRegistrationApp.Maui.Core.Services.Theme
{
    /// <summary>
    /// Implementação padrão de IThemeService.
    /// Usa Microsoft.Maui.Controls.Application.Current.UserAppTheme + Preferences para persistir
    /// a escolha de tema entre execuções.
    /// </summary>
    public sealed class ThemeService : IThemeService
    {
        private const string PreferenceKey = "EmployeeApp.Theme";
        private const AppTheme DefaultThemeFallback = AppTheme.Dark;

        private AppTheme _currentTheme = DefaultThemeFallback;

        public AppTheme CurrentTheme => _currentTheme;

        public event EventHandler<AppTheme>? ThemeChanged;

        // =======================
        // Implementação da interface
        // =======================

        public Task InitializeAsync()
        {
            InitializeInternal();
            return Task.CompletedTask;
        }

        public Task ApplyInitialThemeAsync()
        {
            ApplyTheme(_currentTheme, raiseEvent: false);
            return Task.CompletedTask;
        }

        public Task<ThemeInfo> GetCurrentThemeAsync()
        {
            var info = new ThemeInfo
            {
                AppTheme = _currentTheme,
                DisplayName = _currentTheme == AppTheme.Dark ? "Tema escuro" : "Tema claro",
                IsDark = _currentTheme == AppTheme.Dark
            };

            return Task.FromResult(info);
        }

        public Task SetThemeAsync(AppTheme theme)
        {
            SetThemeInternal(theme);
            return Task.CompletedTask;
        }

        public Task ToggleThemeAsync()
        {
            ToggleThemeInternal();
            return Task.CompletedTask;
        }

        // =======================
        // Implementação interna síncrona
        // =======================

        private void InitializeInternal()
        {
            try
            {
                if (Preferences.ContainsKey(PreferenceKey))
                {
                    var storedValue = Preferences.Get(PreferenceKey, nameof(DefaultThemeFallback));
                    if (Enum.TryParse<AppTheme>(storedValue, out var parsedTheme))
                    {
                        _currentTheme = parsedTheme;
                    }
                    else
                    {
                        _currentTheme = DefaultThemeFallback;
                    }
                }
                else
                {
                    _currentTheme = AppConfig.UseDarkThemeByDefault
                        ? AppTheme.Dark
                        : AppTheme.Light;
                }

                ApplyTheme(_currentTheme, raiseEvent: false);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[THEME][ERROR] Initialize: {ex}");
                ApplyTheme(DefaultThemeFallback, raiseEvent: false);
            }
        }

        private void SetThemeInternal(AppTheme theme)
        {
            if (_currentTheme == theme)
                return;

            _currentTheme = theme;
            Preferences.Set(PreferenceKey, theme.ToString());

            ApplyTheme(theme, raiseEvent: true);
        }

        private void ToggleThemeInternal()
        {
            var newTheme = _currentTheme == AppTheme.Dark
                ? AppTheme.Light
                : AppTheme.Dark;

            SetThemeInternal(newTheme);
        }

        private void ApplyTheme(AppTheme theme, bool raiseEvent)
        {
            try
            {
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    if (Microsoft.Maui.Controls.Application.Current is null)
                    {
                        Debug.WriteLine("[THEME] Microsoft.Maui.Controls.Application.Current é nulo, não foi possível aplicar tema.");
                        return;
                    }

                    Microsoft.Maui.Controls.Application.Current.UserAppTheme = theme;
                });

                if (raiseEvent)
                {
                    ThemeChanged?.Invoke(this, theme);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[THEME][ERROR] ApplyTheme: {ex}");
            }
        }
    }
}
