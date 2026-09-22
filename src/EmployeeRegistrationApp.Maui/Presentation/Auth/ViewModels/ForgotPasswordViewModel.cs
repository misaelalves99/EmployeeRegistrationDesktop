using System;
using System.Threading.Tasks;
using EmployeeRegistrationApp.Maui.Core.Base;
using EmployeeRegistrationApp.Maui.Core.Services.Navigation;
using EmployeeRegistrationApp.Maui.Core.Services.Notifications;

namespace EmployeeRegistrationApp.Maui.Presentation.Auth.ViewModels;

public sealed class ForgotPasswordViewModel : ViewModelBase
{
    private readonly INavigationService _navigationService;
    private readonly IToastService _toastService;

    private string _email = string.Empty;
    private bool _hasError;
    private string _errorMessage = string.Empty;

    public ForgotPasswordViewModel(
        INavigationService navigationService,
        IToastService toastService)
    {
        _navigationService = navigationService
            ?? throw new ArgumentNullException(nameof(navigationService));

        _toastService = toastService
            ?? throw new ArgumentNullException(nameof(toastService));

        SendResetCommand = new AsyncCommand(
            ExecuteSendResetAsync,
            () => CanSendReset);

        NavigateToLoginCommand = new AsyncCommand(
            async () => await _navigationService.NavigateToLoginAsync());
    }

    public string Email
    {
        get => _email;
        set
        {
            if (SetProperty(ref _email, value))
            {
                OnPropertyChanged(nameof(CanSendReset));
                SendResetCommand.RaiseCanExecuteChanged();
            }
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

    public bool CanSendReset =>
        !IsBusy &&
        !string.IsNullOrWhiteSpace(Email);

    public AsyncCommand SendResetCommand { get; }

    public AsyncCommand NavigateToLoginCommand { get; }

    private async Task ExecuteSendResetAsync()
    {
        ClearError();

        if (string.IsNullOrWhiteSpace(Email) ||
            !Email.Contains("@", StringComparison.Ordinal))
        {
            SetError("Informe um e-mail válido.");
            return;
        }

        try
        {
            IsBusy = true;
            OnPropertyChanged(nameof(CanSendReset));
            SendResetCommand.RaiseCanExecuteChanged();

            await Task.Delay(120);

            await _toastService.ShowInfoAsync(
                "Se existir uma conta para este e-mail, as instruções de recuperação foram solicitadas.");

            await _navigationService.NavigateToLoginAsync();
        }
        catch (Exception ex)
        {
            SetError("Não foi possível concluir a solicitação de recuperação.");
            await _toastService.ShowErrorAsync(
                $"Erro ao solicitar recuperação: {ex.Message}");
        }
        finally
        {
            IsBusy = false;
            OnPropertyChanged(nameof(CanSendReset));
            SendResetCommand.RaiseCanExecuteChanged();
        }
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