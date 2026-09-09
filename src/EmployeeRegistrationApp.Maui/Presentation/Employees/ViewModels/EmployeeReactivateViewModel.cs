using System;
using System.Threading.Tasks;
using System.Windows.Input;
using EmployeeRegistrationApp.Application.Interfaces.Services;
using EmployeeRegistrationApp.Maui.Core.Base;
using EmployeeRegistrationApp.Maui.Core.Services.Dialogs;
using EmployeeRegistrationApp.Maui.Core.Services.Navigation;
using Microsoft.Maui.Controls;

namespace EmployeeRegistrationApp.Maui.Presentation.Employees.ViewModels
{
    public sealed class EmployeeReactivateViewModel : ViewModelBase
    {
        private readonly IEmployeeAppService _employeeAppService;
        private readonly INavigationService _navigationService;
        private readonly IDialogService _dialogService;

        private Guid _employeeId;
        private string _employeeName = string.Empty;
        private DateTime _reactivationDate = DateTime.Today;

        public EmployeeReactivateViewModel(
            IEmployeeAppService employeeAppService,
            INavigationService navigationService,
            IDialogService dialogService)
        {
            _employeeAppService = employeeAppService;
            _navigationService = navigationService;
            _dialogService = dialogService;

            ConfirmCommand = new Command(async () => await ConfirmAsync(), () => IsNotBusy);
            CancelCommand = new Command(async () => await _navigationService.GoBackAsync(), () => IsNotBusy);
        }

        public string EmployeeName
        {
            get => _employeeName;
            set => SetProperty(ref _employeeName, value);
        }

        public DateTime ReactivationDate
        {
            get => _reactivationDate;
            set => SetProperty(ref _reactivationDate, value);
        }

        public ICommand ConfirmCommand { get; }
        public ICommand CancelCommand { get; }

        public async Task LoadAsync(Guid id)
        {
            _employeeId = id;

            try
            {
                IsBusy = true;

                var details = await _employeeAppService.GetByIdAsync(id);
                EmployeeName = details?.FullName ?? "Colaborador";
                ReactivationDate = DateTime.Today;
            }
            catch (Exception ex)
            {
                await _dialogService.ShowErrorAsync("Erro ao carregar colaborador.", ex);
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async Task ConfirmAsync()
        {
            try
            {
                IsBusy = true;
                await _employeeAppService.ReactivateAsync(_employeeId);
                await _navigationService.GoBackAsync();
            }
            catch (Exception ex)
            {
                await _dialogService.ShowErrorAsync("Erro ao reativar colaborador.", ex);
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}
