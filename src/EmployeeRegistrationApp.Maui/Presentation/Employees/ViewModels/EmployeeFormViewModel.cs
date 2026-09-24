using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using EmployeeRegistrationApp.Application.DTOs.Departments;
using EmployeeRegistrationApp.Application.DTOs.Employees;
using EmployeeRegistrationApp.Application.DTOs.Positions;
using EmployeeRegistrationApp.Application.Interfaces.Services;
using EmployeeRegistrationApp.Domain.Enums;
using EmployeeRegistrationApp.Maui.Core.Base;
using EmployeeRegistrationApp.Maui.Core.Services.Dialogs;
using EmployeeRegistrationApp.Maui.Core.Services.Navigation;
using Microsoft.Maui.Controls;

namespace EmployeeRegistrationApp.Maui.Presentation.Employees.ViewModels
{
    public sealed class ContractTypeOption
    {
        public ContractTypeOption(ContractType value, string label)
        {
            Value = value;
            Label = label;
        }

        public ContractType Value { get; }
        public string Label { get; }
    }

    public sealed class EmployeeFormViewModel : ViewModelBase
    {
        private readonly IEmployeeAppService _employeeAppService;
        private readonly IDepartmentAppService _departmentAppService;
        private readonly IPositionAppService _positionAppService;
        private readonly INavigationService _navigationService;
        private readonly IDialogService _dialogService;

        private EmployeeDto _form = new();
        private Guid? _editingId;
        private string _salaryText = "0";
        private DepartmentDto? _selectedDepartment;
        private PositionDto? _selectedPosition;
        private ContractTypeOption? _selectedContractType;

        public EmployeeFormViewModel(
            IEmployeeAppService employeeAppService,
            IDepartmentAppService departmentAppService,
            IPositionAppService positionAppService,
            INavigationService navigationService,
            IDialogService dialogService)
        {
            _employeeAppService = employeeAppService;
            _departmentAppService = departmentAppService;
            _positionAppService = positionAppService;
            _navigationService = navigationService;
            _dialogService = dialogService;

            Departments = new ObservableCollection<DepartmentDto>();
            Positions = new ObservableCollection<PositionDto>();
            ContractTypes = new ObservableCollection<ContractTypeOption>
            {
                new(ContractType.CLT, "CLT"),
                new(ContractType.PJ, "PJ"),
                new(ContractType.Intern, "Estágio"),
                new(ContractType.Temporary, "Temporário")
            };

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

        public ObservableCollection<DepartmentDto> Departments { get; }
        public ObservableCollection<PositionDto> Positions { get; }
        public ObservableCollection<ContractTypeOption> ContractTypes { get; }

        public DepartmentDto? SelectedDepartment
        {
            get => _selectedDepartment;
            set
            {
                if (SetProperty(ref _selectedDepartment, value))
                    Form.DepartmentId = value?.Id;
            }
        }

        public PositionDto? SelectedPosition
        {
            get => _selectedPosition;
            set
            {
                if (SetProperty(ref _selectedPosition, value))
                    Form.PositionId = value?.Id;
            }
        }

        public ContractTypeOption? SelectedContractType
        {
            get => _selectedContractType;
            set
            {
                if (SetProperty(ref _selectedContractType, value) && value != null)
                    Form.ContractType = value.Value;
            }
        }

        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }

        public async Task LoadForCreateAsync()
        {
            if (IsBusy) return;

            try
            {
                IsBusy = true;
                RefreshCommandStates();

                _editingId = null;

                Form = new EmployeeDto
                {
                    FullName = string.Empty,
                    Email = string.Empty,
                    Cpf = string.Empty,
                    PhoneNumber = string.Empty,
                    HireDate = DateTime.Today,
                    EmploymentStatus = EmploymentStatus.Active,
                    ContractType = ContractType.CLT,
                    JobRole = JobRole.Other,
                    SalaryCurrency = "BRL",
                    SalaryAmount = 0
                };

                SalaryText = "0";
                await LoadLookupsAsync();

                _selectedDepartment = null;
                _selectedPosition = null;
                _selectedContractType = null;

                OnPropertyChanged(nameof(SelectedDepartment));
                OnPropertyChanged(nameof(SelectedPosition));
                OnPropertyChanged(nameof(SelectedContractType));
                OnPropertyChanged(nameof(PageTitle));
            }
            catch (Exception ex)
            {
                await _dialogService.ShowErrorAsync("Erro ao carregar formulário do colaborador.", ex);
            }
            finally
            {
                IsBusy = false;
                RefreshCommandStates();
            }
        }

        public async Task LoadForEditAsync(Guid id)
        {
            if (IsBusy) return;

            try
            {
                IsBusy = true;
                RefreshCommandStates();
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
                    HireDate = details.HireDate,
                    TerminationDate = details.TerminationDate,
                    EmploymentStatus = details.EmploymentStatus,
                    ContractType = details.ContractType,
                    JobRole = details.JobRole,
                    SalaryCurrency = details.SalaryCurrency ?? "BRL",
                    SalaryAmount = details.SalaryAmount,
                    DepartmentId = details.DepartmentId,
                    PositionId = details.PositionId
                };

                SalaryText = details.SalaryAmount.ToString(CultureInfo.InvariantCulture);
                await LoadLookupsAsync();

                _selectedDepartment = Departments.FirstOrDefault(x => x.Id == details.DepartmentId);
                _selectedPosition = Positions.FirstOrDefault(x => x.Id == details.PositionId);
                _selectedContractType = ContractTypes.FirstOrDefault(x => x.Value == details.ContractType);

                OnPropertyChanged(nameof(SelectedDepartment));
                OnPropertyChanged(nameof(SelectedPosition));
                OnPropertyChanged(nameof(SelectedContractType));
                OnPropertyChanged(nameof(PageTitle));
            }
            catch (Exception ex)
            {
                await _dialogService.ShowErrorAsync("Erro ao carregar colaborador para edição.", ex);
            }
            finally
            {
                IsBusy = false;
                RefreshCommandStates();
            }
        }

        private async Task LoadLookupsAsync()
        {
            var departments = await _departmentAppService.GetAllAsync();
            var positions = await _positionAppService.GetAllAsync();

            Departments.Clear();
            foreach (var department in departments.Where(x => x.IsActive).OrderBy(x => x.Name))
                Departments.Add(department);

            Positions.Clear();
            foreach (var position in positions.Where(x => x.IsActive).OrderBy(x => x.Name))
                Positions.Add(position);
        }

        private async Task SaveAsync()
        {
            if (IsBusy) return;

            try
            {
                IsBusy = true;
                RefreshCommandStates();

                if (SelectedContractType == null)
                {
                    await _dialogService.ShowWarningAsync(
                        "Contrato obrigatório",
                        "Selecione o tipo de contrato.");
                    return;
                }

                if (!decimal.TryParse(
                    SalaryText?.Replace(",", "."),
                    NumberStyles.Any,
                    CultureInfo.InvariantCulture,
                    out var salary))
                {
                    salary = 0;
                }

                Form.SalaryAmount = salary;
                if (string.IsNullOrWhiteSpace(Form.SalaryCurrency))
                    Form.SalaryCurrency = "BRL";

                Form.DepartmentId = SelectedDepartment?.Id;
                Form.PositionId = SelectedPosition?.Id;
                Form.ContractType = SelectedContractType.Value;

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
                RefreshCommandStates();
            }
        }

        private void RefreshCommandStates()
        {
            ((Command)SaveCommand).ChangeCanExecute();
            ((Command)CancelCommand).ChangeCanExecute();
        }
    }
}
