using System;
using System.Threading.Tasks;
using System.Windows.Input;
using EmployeeRegistrationApp.Application.DTOs.Employees;
using EmployeeRegistrationApp.Application.Interfaces.Services;
using EmployeeRegistrationApp.Maui.Core.Base;
using EmployeeRegistrationApp.Maui.Core.Services.Dialogs;
using EmployeeRegistrationApp.Maui.Core.Services.Navigation;
using Microsoft.Maui.Controls;

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
            DeactivateCommand = new Command(async () => await DeactivateAsync(), () => IsNotBusy && Employee != null);
            ReactivateCommand = new Command(async () => await ReactivateAsync(), () => IsNotBusy && Employee != null);
            DeleteCommand = new Command(async () => await DeleteAsync(), () => IsNotBusy && Employee != null);
        }

        public EmployeeDetailsDto? Employee
        {
            get => _employee;
            private set
            {
                if (SetProperty(ref _employee, value))
                {
                    ((Command)EditCommand).ChangeCanExecute();
                    ((Command)DeactivateCommand).ChangeCanExecute();
                    ((Command)ReactivateCommand).ChangeCanExecute();
                    ((Command)DeleteCommand).ChangeCanExecute();
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
                Employee = await _employeeAppService.GetByIdAsync(id);
            }
            catch (Exception ex)
            {
                await _dialogService.ShowErrorAsync("Erro ao carregar detalhes do colaborador.", ex);
            }
            finally
            {
                IsBusy = false;
            }
        }

        private Task EditAsync()
        {
            if (Employee == null) return Task.CompletedTask;

            return _navigationService.NavigateToAsync(
                NavigationRoutes.EmployeeFormPage,
                new System.Collections.Generic.Dictionary<string, object> { ["id"] = Employee.Id });
        }

        private async Task DeactivateAsync()
        {
            if (Employee == null) return;

            try
            {
                IsBusy = true;
                await _employeeAppService.DeactivateAsync(Employee.Id);
                await LoadAsync(Employee.Id);
            }
            catch (Exception ex)
            {
                await _dialogService.ShowErrorAsync("Erro ao desativar colaborador.", ex);
            }
            finally
            {
                IsBusy = false;
            }
        }

        private Task ReactivateAsync()
        {
            if (Employee == null) return Task.CompletedTask;

            return _navigationService.NavigateToAsync(
                NavigationRoutes.EmployeeReactivatePage,
                new System.Collections.Generic.Dictionary<string, object> { ["id"] = Employee.Id });
        }

        private async Task DeleteAsync()
        {
            if (Employee == null) return;

            var confirm = await _dialogService.ShowConfirmAsync(
                "Excluir colaborador",
                "Tem certeza que deseja excluir este colaborador?");

            if (!confirm) return;

            try
            {
                IsBusy = true;
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
            }
        }
    }
}
