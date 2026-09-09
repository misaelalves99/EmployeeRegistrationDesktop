using System;
using System.Globalization;
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
    public sealed class EmployeeFormViewModel : ViewModelBase
    {
        private readonly IEmployeeAppService _employeeAppService;
        private readonly INavigationService _navigationService;
        private readonly IDialogService _dialogService;

        private EmployeeDto _form = new();
        private Guid? _editingId;

        private string _salaryText = "0";

        public EmployeeFormViewModel(
            IEmployeeAppService employeeAppService,
            INavigationService navigationService,
            IDialogService dialogService)
        {
            _employeeAppService = employeeAppService;
            _navigationService = navigationService;
            _dialogService = dialogService;

            SaveCommand = new Command(async () => await SaveAsync(), () => IsNotBusy);
            CancelCommand = new Command(async () => await _navigationService.GoBackAsync(), () => IsNotBusy);
        }

        public string PageTitle => _editingId.HasValue ? "Editar colaborador" : "Novo colaborador";

        public EmployeeDto Form
        {
            get => _form;
            set => SetProperty(ref _form, value);
        }

        public string SalaryText
        {
            get => _salaryText;
            set => SetProperty(ref _salaryText, value);
        }

        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }

        public Task LoadForCreateAsync()
        {
            _editingId = null;

            Form = new EmployeeDto
            {
                FullName = string.Empty,
                Email = string.Empty,
                Cpf = string.Empty,
                PhoneNumber = string.Empty,
                SalaryCurrency = "BRL",
                SalaryAmount = 0
            };

            SalaryText = "0";
            OnPropertyChanged(nameof(PageTitle));
            return Task.CompletedTask;
        }

        public async Task LoadForEditAsync(Guid id)
        {
            try
            {
                IsBusy = true;
                _editingId = id;

                var details = await _employeeAppService.GetByIdAsync(id);
                if (details == null)
                {
                    await _dialogService.ShowAlertAsync("Não encontrado", "Colaborador não encontrado.");
                    await _navigationService.GoBackAsync();
                    return;
                }

                Form = new EmployeeDto
                {
                    FullName = details.FullName ?? string.Empty,
                    Email = details.Email ?? string.Empty,
                    Cpf = details.Cpf ?? string.Empty,
                    PhoneNumber = details.PhoneNumber,
                    SalaryCurrency = details.SalaryCurrency ?? "BRL",
                    SalaryAmount = details.SalaryAmount,
                    ContractType = default,
                    JobRole = default,
                    EmploymentStatus = default
                };

                SalaryText = details.SalaryAmount.ToString(CultureInfo.InvariantCulture);

                OnPropertyChanged(nameof(PageTitle));
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async Task SaveAsync()
        {
            try
            {
                IsBusy = true;

                if (!decimal.TryParse(SalaryText?.Replace(",", "."), NumberStyles.Any, CultureInfo.InvariantCulture, out var salary))
                    salary = 0;

                Form.SalaryAmount = salary;
                if (string.IsNullOrWhiteSpace(Form.SalaryCurrency))
                    Form.SalaryCurrency = "BRL";

                if (_editingId.HasValue)
                    await _employeeAppService.UpdateAsync(_editingId.Value, Form);
                else
                    await _employeeAppService.CreateAsync(Form);

                await _navigationService.GoBackAsync();
            }
            catch (Exception ex)
            {
                await _dialogService.ShowErrorAsync("Erro ao salvar colaborador.", ex);
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}
