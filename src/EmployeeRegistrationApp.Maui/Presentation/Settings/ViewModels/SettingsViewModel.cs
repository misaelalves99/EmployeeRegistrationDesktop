// src/EmployeeRegistrationApp.Maui/Presentation/Settings/ViewModels/SettingsViewModel.cs
using System;
using System.Threading.Tasks;
using EmployeeRegistrationApp.Application.Interfaces.Services;
using EmployeeRegistrationApp.Maui.Core.Base;
using EmployeeRegistrationApp.Maui.Core.Services.Navigation;
using EmployeeRegistrationApp.Maui.Core.Services.Notifications;
using EmployeeRegistrationApp.Maui.Core.Services.Theme;

namespace EmployeeRegistrationApp.Maui.Presentation.Settings.ViewModels
{
    public sealed class SettingsViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private readonly ISettingsAppService _settingsAppService;
        private readonly IThemeService _themeService;
        private readonly IToastService _toastService;

        private string _companyNameSummary = "Empresa não configurada";
        public string CompanyNameSummary
        {
            get => _companyNameSummary;
            set => SetProperty(ref _companyNameSummary, value);
        }

        private string _currentThemeSummary = "Tema padrão";
        public string CurrentThemeSummary
        {
            get => _currentThemeSummary;
            set => SetProperty(ref _currentThemeSummary, value);
        }

        private string _themeToggleButtonText = "Alternar tema";
        public string ThemeToggleButtonText
        {
            get => _themeToggleButtonText;
            set => SetProperty(ref _themeToggleButtonText, value);
        }

        public AsyncCommand InitializeCommand { get; }
        public AsyncCommand OpenCompanySettingsCommand { get; }
        public AsyncCommand ToggleThemeCommand { get; }

        public SettingsViewModel(
            INavigationService navigationService,
            ISettingsAppService settingsAppService,
            IThemeService themeService,
            IToastService toastService)
        {
            _navigationService = navigationService;
            _settingsAppService = settingsAppService;
            _themeService = themeService;
            _toastService = toastService;

            InitializeCommand = new AsyncCommand(InitializeAsync);
            OpenCompanySettingsCommand = new AsyncCommand(OpenCompanySettingsAsync);
            ToggleThemeCommand = new AsyncCommand(ToggleThemeAsync);
        }

        public async Task InitializeAsync()
        {
            if (IsBusy) return;

            try
            {
                IsBusy = true;

                var settings = await _settingsAppService.GetCompanySettingsAsync();

                if (settings is null || string.IsNullOrWhiteSpace(settings.CompanyName))
                {
                    CompanyNameSummary = "Empresa não configurada";
                }
                else
                {
                    CompanyNameSummary = $"Empresa: {settings.CompanyName}";
                }

                var themeInfo = await _themeService.GetCurrentThemeAsync();
                CurrentThemeSummary = $"Tema atual: {themeInfo.DisplayName}";
                ThemeToggleButtonText = themeInfo.IsDark
                    ? "Usar tema claro"
                    : "Usar tema escuro";
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);
                _toastService.ShowError("Não foi possível carregar as configurações.");
            }
            finally
            {
                IsBusy = false;
            }
        }

        private Task OpenCompanySettingsAsync()
        {
            // route absoluta no Shell (ajuste se seu AppShell usar outro path)
            return _navigationService.NavigateToAsync("//Settings/CompanySettingsPage");
        }

        private async Task ToggleThemeAsync()
        {
            try
            {
                await _themeService.ToggleThemeAsync();

                var themeInfo = await _themeService.GetCurrentThemeAsync();
                CurrentThemeSummary = $"Tema atual: {themeInfo.DisplayName}";
                ThemeToggleButtonText = themeInfo.IsDark
                    ? "Usar tema claro"
                    : "Usar tema escuro";

                _toastService.ShowInfo("Tema atualizado.");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);
                _toastService.ShowError("Não foi possível alterar o tema.");
            }
        }
    }
}
