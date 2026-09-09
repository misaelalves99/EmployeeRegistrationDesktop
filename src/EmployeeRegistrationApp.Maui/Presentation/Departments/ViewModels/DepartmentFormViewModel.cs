// src/EmployeeRegistrationApp.Maui/Presentation/Departments/ViewModels/DepartmentFormViewModel.cs
using System;
using System.Threading.Tasks;
using EmployeeRegistrationApp.Application.DTOs.Departments;
using EmployeeRegistrationApp.Application.Interfaces.Services;
using EmployeeRegistrationApp.Maui.Core.Base;
using EmployeeRegistrationApp.Maui.Core.Config;
using EmployeeRegistrationApp.Maui.Core.Services.Dialogs;
using EmployeeRegistrationApp.Maui.Core.Services.Navigation;
using Microsoft.Maui.Controls;

namespace EmployeeRegistrationApp.Maui.Presentation.Departments.ViewModels
{
    /// <summary>
    /// ViewModel do formulário de criação/edição de departamento.
    /// </summary>
    [QueryProperty(nameof(DepartmentId), "id")]
    public sealed class DepartmentFormViewModel : ViewModelBase
    {
        private readonly IDepartmentAppService _departmentAppService;
        private readonly INavigationService _navigationService;
        private readonly IDialogService _dialogService;

        public DepartmentFormViewModel(
            IDepartmentAppService departmentAppService,
            INavigationService navigationService,
            IDialogService dialogService)
        {
            _departmentAppService = departmentAppService;
            _navigationService = navigationService;
            _dialogService = dialogService;

            Title = "Novo departamento";

            SaveCommand = new Command(async () => await SaveAsync(), () => CanSave && !IsBusy);
            CancelCommand = new Command(async () => await CancelAsync());
        }

        public Guid? DepartmentId { get; private set; }
        public bool IsEditMode => DepartmentId.HasValue;

        private string _name = string.Empty;
        public string Name
        {
            get => _name;
            set
            {
                if (SetProperty(ref _name, value))
                {
                    Validate();
                    UpdateCanSave();
                }
            }
        }

        private string _description = string.Empty;
        public string Description
        {
            get => _description;
            set => SetProperty(ref _description, value);
        }

        private string _nameError = string.Empty;
        public string NameError
        {
            get => _nameError;
            private set
            {
                if (SetProperty(ref _nameError, value))
                    OnPropertyChanged(nameof(HasNameError));
            }
        }

        public bool HasNameError => !string.IsNullOrWhiteSpace(NameError);

        private bool _canSave;
        public bool CanSave
        {
            get => _canSave;
            private set
            {
                if (SetProperty(ref _canSave, value))
                    SaveCommand.ChangeCanExecute();
            }
        }

        public Command SaveCommand { get; }
        public Command CancelCommand { get; }

        public async Task InitializeAsync()
        {
            if (!IsEditMode && DepartmentId is null)
                return;

            if (DepartmentId is null)
                return;

            try
            {
                IsBusy = true;

                var dto = await _departmentAppService.GetByIdAsync(DepartmentId.Value);
                if (dto != null)
                {
                    Name = dto.Name ?? string.Empty;
                    Description = dto.Description ?? string.Empty;

                    Title = "Editar departamento";
                }
            }
            catch (Exception)
            {
                await _dialogService.ShowErrorAsync(
                    "Erro",
                    "Erro ao carregar departamento."
                );
            }
            finally
            {
                IsBusy = false;
            }
        }

        public void SetDepartmentId(Guid? id)
        {
            DepartmentId = id;
            OnPropertyChanged(nameof(IsEditMode));
        }

        private void Validate()
        {
            if (string.IsNullOrWhiteSpace(Name))
                NameError = "O nome do departamento é obrigatório.";
            else if (Name.Trim().Length < 3)
                NameError = "O nome deve ter pelo menos 3 caracteres.";
            else
                NameError = string.Empty;
        }

        private void UpdateCanSave()
        {
            CanSave = !HasNameError && !string.IsNullOrWhiteSpace(Name);
        }

        private async Task SaveAsync()
        {
            if (!CanSave) return;

            try
            {
                IsBusy = true;
                SaveCommand.ChangeCanExecute();

                var dto = new DepartmentDto
                {
                    Id = DepartmentId,
                    Name = Name.Trim(),
                    Description = string.IsNullOrWhiteSpace(Description)
                        ? null
                        : Description.Trim()
                };

                if (IsEditMode)
                    await _departmentAppService.UpdateAsync(dto);
                else
                    await _departmentAppService.CreateAsync(dto);

                await _dialogService.ShowInfoAsync(
                    "Sucesso",
                    "Departamento salvo com sucesso."
                );

                await _navigationService.GoBackAsync();
            }
            catch (Exception)
            {
                await _dialogService.ShowErrorAsync(
                    "Erro",
                    "Erro ao salvar departamento."
                );
            }
            finally
            {
                IsBusy = false;
                SaveCommand.ChangeCanExecute();
            }
        }

        private Task CancelAsync()
        {
            return _navigationService.GoBackAsync();
        }
    }
}
