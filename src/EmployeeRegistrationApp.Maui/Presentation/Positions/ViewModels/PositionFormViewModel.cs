// src/EmployeeRegistrationApp.Maui/Presentation/Positions/ViewModels/PositionFormViewModel.cs
using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using EmployeeRegistrationApp.Application.DTOs.Departments;
using EmployeeRegistrationApp.Application.DTOs.Positions;
using EmployeeRegistrationApp.Application.Interfaces.Services;
using EmployeeRegistrationApp.Domain.Enums;
using EmployeeRegistrationApp.Maui.Core.Base;
using EmployeeRegistrationApp.Maui.Core.Services.Dialogs;
using EmployeeRegistrationApp.Maui.Core.Services.Navigation;

namespace EmployeeRegistrationApp.Maui.Presentation.Positions.ViewModels
{
    public sealed class PositionFormViewModel : ViewModelBase
    {
        private readonly IPositionAppService _positionAppService;
        private readonly IDepartmentAppService _departmentAppService;
        private readonly INavigationService _navigationService;
        private readonly IDialogService _dialogService;

        private Guid? _positionId;
        private string _name = string.Empty;
        private string _description = string.Empty;
        private string _baseSalaryText = string.Empty;
        private string _nameError = string.Empty;
        private DepartmentDto? _selectedDepartment;
        private PositionType? _selectedPositionType;

        private bool _initialized;

        public string PageTitle => _positionId.HasValue ? "Editar cargo" : "Novo cargo";
        public string HeaderTitle => _positionId.HasValue ? "Editar cargo" : "Cadastrar novo cargo";
        public string HeaderSubtitle =>
            _positionId.HasValue
                ? "Atualize as informações do cargo."
                : "Preencha os dados para cadastrar um novo cargo na empresa.";

        public ObservableCollection<DepartmentDto> Departments { get; } = new();
        public ObservableCollection<PositionType> PositionTypes { get; } =
            new(Enum.GetValues(typeof(PositionType)).Cast<PositionType>());

        public string Name
        {
            get => _name;
            set
            {
                if (SetProperty(ref _name, value))
                {
                    ValidateName();
                    OnPropertyChanged(nameof(CanSave));
                    SaveCommand.RaiseCanExecuteChanged();
                }
            }
        }

        public string Description
        {
            get => _description;
            set => SetProperty(ref _description, value);
        }

        public string BaseSalaryText
        {
            get => _baseSalaryText;
            set => SetProperty(ref _baseSalaryText, value);
        }

        public DepartmentDto? SelectedDepartment
        {
            get => _selectedDepartment;
            set
            {
                if (SetProperty(ref _selectedDepartment, value))
                {
                    OnPropertyChanged(nameof(CanSave));
                    SaveCommand.RaiseCanExecuteChanged();
                }
            }
        }

        public PositionType? SelectedPositionType
        {
            get => _selectedPositionType;
            set => SetProperty(ref _selectedPositionType, value);
        }

        public string NameError
        {
            get => _nameError;
            private set
            {
                if (SetProperty(ref _nameError, value))
                {
                    OnPropertyChanged(nameof(HasNameError));
                }
            }
        }

        public bool HasNameError => !string.IsNullOrWhiteSpace(NameError);

        public bool CanSave =>
            !string.IsNullOrWhiteSpace(Name) &&
            SelectedDepartment != null &&
            !IsBusy &&
            !HasNameError;

        public AsyncCommand SaveCommand { get; }
        public AsyncCommand CancelCommand { get; }

        public PositionFormViewModel(
            IPositionAppService positionAppService,
            IDepartmentAppService departmentAppService,
            INavigationService navigationService,
            IDialogService dialogService)
        {
            _positionAppService = positionAppService;
            _departmentAppService = departmentAppService;
            _navigationService = navigationService;
            _dialogService = dialogService;

            SaveCommand = new AsyncCommand(ExecuteSaveAsync, () => CanSave);
            CancelCommand = new AsyncCommand(ExecuteCancelAsync);
        }

        /// <summary>
        /// Usado pelo Shell via query string: ?id={guid}
        /// </summary>
        public Guid? PositionId
        {
            get => _positionId;
            set
            {
                if (SetProperty(ref _positionId, value))
                {
                    OnPropertyChanged(nameof(PageTitle));
                    OnPropertyChanged(nameof(HeaderTitle));
                    OnPropertyChanged(nameof(HeaderSubtitle));
                }
            }
        }

        public async Task InitializeAsync()
        {
            if (_initialized)
                return;

            _initialized = true;

            await LoadDepartmentsAsync();

            if (PositionId.HasValue)
            {
                await LoadExistingPositionAsync(PositionId.Value);
            }
        }

        private async Task LoadDepartmentsAsync()
        {
            try
            {
                Departments.Clear();

                var departments = await _departmentAppService.GetAllAsync();

                foreach (var d in departments)
                {
                    Departments.Add(d);
                }
            }
            catch (Exception ex)
            {
                await _dialogService.ShowErrorAsync(
                    "Erro ao carregar departamentos",
                    ex.Message);
            }
        }

        private async Task LoadExistingPositionAsync(Guid id)
        {
            try
            {
                IsBusy = true;

                var dto = await _positionAppService.GetByIdAsync(id);
                if (dto == null)
                {
                    await _dialogService.ShowErrorAsync(
                        "Cargo não encontrado",
                        "O cargo solicitado não pôde ser carregado.");
                    await _navigationService.GoBackAsync();
                    return;
                }

                Name = dto.Name;
                Description = dto.Description ?? string.Empty;
                BaseSalaryText = dto.BaseSalary.ToString("F2", new CultureInfo("pt-BR"));

                SelectedDepartment = Departments.FirstOrDefault(d => d.Id == dto.DepartmentId);
                SelectedPositionType = dto.Type;
            }
            catch (Exception ex)
            {
                await _dialogService.ShowErrorAsync(
                    "Erro ao carregar cargo",
                    ex.Message);
            }
            finally
            {
                IsBusy = false;
            }
        }

        private void ValidateName()
        {
            if (string.IsNullOrWhiteSpace(Name))
            {
                NameError = "O nome do cargo é obrigatório.";
            }
            else if (Name.Length < 3)
            {
                NameError = "O nome deve ter pelo menos 3 caracteres.";
            }
            else
            {
                NameError = string.Empty;
            }
        }

        private async Task ExecuteSaveAsync()
        {
            if (!CanSave)
                return;

            try
            {
                IsBusy = true;

                var dto = new PositionDto
                {
                    Id = PositionId ?? Guid.Empty,
                    Name = Name.Trim(),
                    Description = string.IsNullOrWhiteSpace(Description)
                        ? null
                        : Description.Trim(),
                    DepartmentId = SelectedDepartment!.Id,
                    Type = SelectedPositionType ?? PositionType.Junior, // <- corrigido
                    BaseSalary = ParseSalary(BaseSalaryText)
                };

                if (PositionId.HasValue && PositionId.Value != Guid.Empty)
                {
                    await _positionAppService.UpdateAsync(dto);
                    await _dialogService.ShowInfoAsync(
                        "Cargo atualizado",
                        "As informações do cargo foram atualizadas com sucesso.");
                }
                else
                {
                    await _positionAppService.CreateAsync(dto);
                    await _dialogService.ShowInfoAsync(
                        "Cargo criado",
                        "O novo cargo foi cadastrado com sucesso.");
                }

                await _navigationService.GoBackAsync();
            }
            catch (Exception ex)
            {
                await _dialogService.ShowErrorAsync(
                    "Erro ao salvar cargo",
                    ex.Message);
            }
            finally
            {
                IsBusy = false;
            }
        }

        private static decimal ParseSalary(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return 0m;

            if (decimal.TryParse(text, NumberStyles.Any, new CultureInfo("pt-BR"), out var valueBr))
                return valueBr;

            if (decimal.TryParse(text, NumberStyles.Any, CultureInfo.InvariantCulture, out var valueInv))
                return valueInv;

            return 0m;
        }

        private async Task ExecuteCancelAsync()
        {
            await _navigationService.GoBackAsync();
        }
    }
}
