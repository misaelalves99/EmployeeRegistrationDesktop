// src/EmployeeRegistrationApp.Maui/Presentation/Auth/ViewModels/LoginViewModel.cs
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Input;
using EmployeeRegistrationApp.Maui.Core.Base;
using EmployeeRegistrationApp.Maui.Core.Config;
using EmployeeRegistrationApp.Maui.Core.Services.Auth;
using EmployeeRegistrationApp.Maui.Core.Services.Navigation;
using EmployeeRegistrationApp.Maui.Core.Services.Notifications;
using Microsoft.Maui.Controls;

namespace EmployeeRegistrationApp.Maui.Presentation.Auth.ViewModels
{
    /// <summary>
    /// ViewModel da tela de Login (.NET MAUI Desktop).
    /// Responsável por orquestrar autenticação e navegação para o dashboard.
    /// </summary>
    public sealed class LoginViewModel : ViewModelBase
    {
        private readonly IAuthService _authService;
        private readonly INavigationService _navigationService;
        private readonly IToastService _toastService;

        private string _email = string.Empty;
        private string _password = string.Empty;
        private bool _rememberMe;
        private bool _hasErrors;
        private string _errorMessage = string.Empty;

        public LoginViewModel(
            IAuthService authService,
            INavigationService navigationService,
            IToastService toastService)
        {
            _authService = authService ?? throw new ArgumentNullException(nameof(authService));
            _navigationService = navigationService ?? throw new ArgumentNullException(nameof(navigationService));
            _toastService = toastService ?? throw new ArgumentNullException(nameof(toastService));

            LoginCommand = new Command(async () => await ExecuteLoginAsync(), CanExecuteLogin);

            NavigateToRegisterCommand = new Command(
                async () => await _navigationService.GoToAsync(NavigationRoutes.AuthRegister));

            NavigateToForgotPasswordCommand = new Command(
                async () => await _navigationService.GoToAsync(NavigationRoutes.AuthForgotPassword));
        }

        public string Email
        {
            get => _email;
            set
            {
                if (SetProperty(ref _email, value))
                {
                    RaiseCanExecuteChanged();
                }
            }
        }

        public string Password
        {
            get => _password;
            set
            {
                if (SetProperty(ref _password, value))
                {
                    RaiseCanExecuteChanged();
                }
            }
        }

        public bool RememberMe
        {
            get => _rememberMe;
            set => SetProperty(ref _rememberMe, value);
        }

        public bool HasErrors
        {
            get => _hasErrors;
            private set => SetProperty(ref _hasErrors, value);
        }

        public string ErrorMessage
        {
            get => _errorMessage;
            private set => SetProperty(ref _errorMessage, value);
        }

        public ICommand LoginCommand { get; }
        public ICommand NavigateToRegisterCommand { get; }
        public ICommand NavigateToForgotPasswordCommand { get; }

        private bool CanExecuteLogin()
        {
            return !IsBusy
                   && !string.IsNullOrWhiteSpace(Email)
                   && !string.IsNullOrWhiteSpace(Password);
        }

        private async Task ExecuteLoginAsync()
        {
            if (!CanExecuteLogin())
                return;

            try
            {
                IsBusy = true;
                HasErrors = false;
                ErrorMessage = string.Empty;
                RaiseCanExecuteChanged();

                if (!Email.Contains("@"))
                {
                    HasErrors = true;
                    ErrorMessage = "Informe um e-mail válido.";
                    await _toastService.ShowInfoAsync(ErrorMessage);
                    return;
                }

                var succeeded = await _authService.SignInAsync(Email, Password);

                if (succeeded)
                {
                    _toastService.ShowSuccess("Bem-vindo(a) de volta!");
                    await _navigationService.NavigateToDashboardAsync();
                }
                else
                {
                    HasErrors = true;
                    ErrorMessage = "Não foi possível realizar o login. Verifique suas credenciais.";

                    _toastService.ShowError(ErrorMessage);
                }
            }
            catch (Exception ex)
            {
                HasErrors = true;
                ErrorMessage = "Ocorreu um erro inesperado ao tentar fazer login.";
                _toastService.ShowError(ErrorMessage);
                Debug.WriteLine($"[LOGIN][ERROR] {ex}");
            }
            finally
            {
                IsBusy = false;
                RaiseCanExecuteChanged();
            }
        }

        private void RaiseCanExecuteChanged()
        {
            if (LoginCommand is Command command)
            {
                command.ChangeCanExecute();
            }
        }
    }
}
