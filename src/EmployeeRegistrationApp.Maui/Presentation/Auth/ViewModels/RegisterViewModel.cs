// src/EmployeeRegistrationApp.Maui/Presentation/Auth/ViewModels/RegisterViewModel.cs
using EmployeeRegistrationApp.Maui.Core.Base;
using EmployeeRegistrationApp.Maui.Core.Config;
using EmployeeRegistrationApp.Maui.Core.Services.Auth;
using EmployeeRegistrationApp.Maui.Core.Services.Navigation;
using EmployeeRegistrationApp.Maui.Core.Services.Notifications;
using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace EmployeeRegistrationApp.Maui.Presentation.Auth.ViewModels
{
    /// <summary>
    /// ViewModel da tela de registro/criação de conta administrativa.
    /// </summary>
    public sealed class RegisterViewModel : ViewModelBase
    {
        private readonly IAuthService _authService;
        private readonly INavigationService _navigationService;
        private readonly IToastService _toastService;

        private string _fullName = string.Empty;
        private string _email = string.Empty;
        private string _password = string.Empty;
        private string _confirmPassword = string.Empty;
        private bool _acceptTerms;
        private bool _hasError;
        private string _errorMessage = string.Empty;

        public RegisterViewModel(
            IAuthService authService,
            INavigationService navigationService,
            IToastService toastService)
        {
            _authService = authService ?? throw new ArgumentNullException(nameof(authService));
            _navigationService = navigationService ?? throw new ArgumentNullException(nameof(navigationService));
            _toastService = toastService ?? throw new ArgumentNullException(nameof(toastService));

            RegisterCommand = new AsyncCommand(ExecuteRegisterAsync, () => CanRegister);
            NavigateToLoginCommand = new AsyncCommand(ExecuteNavigateToLoginAsync);
        }

        public string FullName
        {
            get => _fullName;
            set
            {
                if (SetProperty(ref _fullName, value))
                    RaiseCanRegisterChanged();
            }
        }

        public string Email
        {
            get => _email;
            set
            {
                if (SetProperty(ref _email, value))
                    RaiseCanRegisterChanged();
            }
        }

        public string Password
        {
            get => _password;
            set
            {
                if (SetProperty(ref _password, value))
                    RaiseCanRegisterChanged();
            }
        }

        public string ConfirmPassword
        {
            get => _confirmPassword;
            set
            {
                if (SetProperty(ref _confirmPassword, value))
                    RaiseCanRegisterChanged();
            }
        }

        public bool AcceptTerms
        {
            get => _acceptTerms;
            set
            {
                if (SetProperty(ref _acceptTerms, value))
                    RaiseCanRegisterChanged();
            }
        }

        public bool HasError
        {
            get => _hasError;
            private set => SetProperty(ref _hasError, value);
        }

        public string ErrorMessage
        {
            get => _errorMessage;
            private set => SetProperty(ref _errorMessage, value);
        }

        /// <summary>
        /// Propriedade usada no binding do botão (IsEnabled).
        /// </summary>
        public bool CanRegister =>
            !IsBusy &&
            !string.IsNullOrWhiteSpace(FullName) &&
            !string.IsNullOrWhiteSpace(Email) &&
            !string.IsNullOrWhiteSpace(Password) &&
            Password == ConfirmPassword &&
            AcceptTerms;

        public AsyncCommand RegisterCommand { get; }
        public ICommand NavigateToLoginCommand { get; }

        private void RaiseCanRegisterChanged()
        {
            OnPropertyChanged(nameof(CanRegister));
            RegisterCommand.RaiseCanExecuteChanged();
        }

        private async Task ExecuteRegisterAsync()
        {
            if (IsBusy)
                return;

            ClearError();

            if (!CanRegister)
            {
                SetError("Preencha todos os campos, aceite os termos e confirme a senha corretamente.");
                return;
            }

            try
            {
                IsBusy = true;

                var success = await _authService.RegisterAsync(
                    fullName: FullName.Trim(),
                    email: Email.Trim(),
                    password: Password);

                if (!success)
                {
                    SetError("Não foi possível criar sua conta. Verifique os dados informados.");
                    return;
                }

                await _toastService.ShowSuccess("Conta criada com sucesso. Faça login para acessar o painel.");
                await _navigationService.NavigateToAsync(NavigationRoutes.Root.Login);
            }
            catch (Exception ex)
            {
                SetError("Ocorreu um erro ao criar a conta. Tente novamente.");
                await _toastService.ShowError($"Erro ao registrar: {ex.Message}");
            }
            finally
            {
                IsBusy = false;
                RaiseCanRegisterChanged();
            }
        }

        private async Task ExecuteNavigateToLoginAsync()
        {
            if (IsBusy)
                return;

            ClearError();
            await _navigationService.NavigateToAsync(NavigationRoutes.Root.Login);
        }

        private void ClearError()
        {
            HasError = false;
            ErrorMessage = string.Empty;
        }

        private void SetError(string message)
        {
            HasError = true;
            ErrorMessage = message;
        }
    }
}
