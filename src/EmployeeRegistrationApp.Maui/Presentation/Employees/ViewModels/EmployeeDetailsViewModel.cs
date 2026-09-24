using System;
using System.Threading.Tasks;
using System.Windows.Input;
using EmployeeRegistrationApp.Application.DTOs.Employees;
using EmployeeRegistrationApp.Application.Interfaces.Services;
using EmployeeRegistrationApp.Maui.Core.Base;
using EmployeeRegistrationApp.Maui.Core.Services.Dialogs;
using EmployeeRegistrationApp.Maui.Core.Services.Navigation;
using Microsoft.Maui.Controls;
using EmployeeRegistrationApp.Maui.Core.Config;

namespace EmployeeRegistrationApp.Maui.Presentation.Employees.ViewModels
{
    public sealed class EmployeeDetailsViewModel : ViewModelBase
    {
        private readonly IEmployeeAppService _employeeAppService;
        private readonly INavigationService _navigationService;
        private readonly IDialogService _dialogService;

        private EmployeeDetailsDto? _employee;

        public EmployeeDetailsViewModel(
            IEmployeeAppService employeeAppService,
            INavigationService navigationService,
            IDialogService dialogService)
        {
            _employeeAppService = employeeAppService;
            _navigationService = navigationService;
            _dialogService = dialogService;

            EditCommand = new Command(async () => await EditAsync(), () => IsNotBusy && Employee != null);
            DeactivateCommand = new Command(
                async () => await DeactivateAsync(),
                () => IsNotBusy && Employee?.IsActive == true);
            ReactivateCommand = new Command(
                async () => await ReactivateAsync(),
                () => IsNotBusy && Employee?.IsActive == false);
            DeleteCommand = new Command(async () => await DeleteAsync(), () => IsNotBusy && Employee != null);
        }

        public EmployeeDetailsDto? Employee
        {
            get => _employee;
            private set
            {
                if (SetProperty(ref _employee, value))
                {
                    RefreshActionCommandStates();
                }
            }
        }

        public ICommand EditCommand { get; }
        public ICommand DeactivateCommand { get; }
        public ICommand ReactivateCommand { get; }
        public ICommand DeleteCommand { get; }

        public async Task LoadAsync(Guid id)
        {
            if (IsBusy) return;

            try
            {
                IsBusy = true;
                RefreshActionCommandStates();
                Employee = await _employeeAppService.GetByIdAsync(id);
            }
            catch (Exception ex)
            {
                await _dialogService.ShowErrorAsync("Erro ao carregar detalhes do colaborador.", ex);
            }
            finally
            {
                IsBusy = false;
                RefreshActionCommandStates();
            }
        }

        private Task EditAsync()
        {
            if (Employee == null) return Task.CompletedTask;

            var route = $"{NavigationRoutes.EmployeeFormPage}?id={Employee.Id:D}";
            return _navigationService.NavigateToAsync(route);
        }

        private async Task DeactivateAsync()
        {
            if (Employee == null || !Employee.IsActive)
                return;

            var employeeId = Employee.Id;

            var confirmed = await _dialogService.ShowDangerConfirmationAsync(
                "Desativar colaborador",
                "Tem certeza que deseja desativar este colaborador?",
                "Sim, desativar",
                "Cancelar");

            if (!confirmed)
                return;

            try
            {
                IsBusy = true;
                RefreshActionCommandStates();

                var deactivated = await _employeeAppService.DeactivateAsync(employeeId);
                if (!deactivated)
                {
                    await _dialogService.ShowWarningAsync(
                        "Colaborador não encontrado",
                        "Não foi possível desativar o colaborador selecionado.");
                    return;
                }

                Employee = await _employeeAppService.GetByIdAsync(employeeId);
                if (Employee == null)
                {
                    await _dialogService.ShowErrorAsync(
                        "Erro ao atualizar colaborador",
                        "O colaborador foi desativado, mas não foi possível recarregar os detalhes.");
                    return;
                }

                await _dialogService.ShowInfoAsync(
                    "Colaborador desativado",
                    "O colaborador foi desativado com sucesso.");
            }
            catch (Exception ex)
            {
                await _dialogService.ShowErrorAsync("Erro ao desativar colaborador.", ex);
            }
            finally
            {
                IsBusy = false;
                RefreshActionCommandStates();
            }
        }

        private Task ReactivateAsync()
        {
            if (Employee == null || Employee.IsActive)
                return Task.CompletedTask;

            var route = $"{NavigationRoutes.EmployeeReactivatePage}?id={Employee.Id:D}";
            return _navigationService.NavigateToAsync(route);
        }

        private async Task DeleteAsync()
        {
            if (Employee == null) return;

            var confirm = await _dialogService.ShowConfirmationAsync(
                "Excluir colaborador",
                "Tem certeza que deseja excluir este colaborador?");

            if (!confirm) return;

            try
            {
                IsBusy = true;
                RefreshActionCommandStates();
                await _employeeAppService.DeleteAsync(Employee.Id);
                await _navigationService.GoBackAsync();
            }
            catch (Exception ex)
            {
                await _dialogService.ShowErrorAsync("Erro ao excluir colaborador.", ex);
            }
            finally
            {
                IsBusy = false;
                RefreshActionCommandStates();
            }
        }

        private void RefreshActionCommandStates()
        {
            ((Command)EditCommand).ChangeCanExecute();
            ((Command)DeactivateCommand).ChangeCanExecute();
            ((Command)ReactivateCommand).ChangeCanExecute();
            ((Command)DeleteCommand).ChangeCanExecute();
        }
    }
}
