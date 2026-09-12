// src/EmployeeRegistrationApp.Maui/Presentation/Departments/ViewModels/DepartmentDetailsViewModel.cs
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EmployeeRegistrationApp.Application.Interfaces.Services;
using EmployeeRegistrationApp.Maui.Core.Base;
using EmployeeRegistrationApp.Maui.Core.Config;
using EmployeeRegistrationApp.Maui.Core.Services.Dialogs;
using EmployeeRegistrationApp.Maui.Core.Services.Navigation;
using Microsoft.Maui.Controls;

namespace EmployeeRegistrationApp.Maui.Presentation.Departments.ViewModels
{
    [QueryProperty(nameof(DepartmentId), "id")]
    public sealed class DepartmentDetailsViewModel : ViewModelBase
    {
        private readonly IDepartmentAppService _departmentAppService;
        private readonly INavigationService _navigationService;
        private readonly IDialogService _dialogService;

        public DepartmentDetailsViewModel(
            IDepartmentAppService departmentAppService,
            INavigationService navigationService,
            IDialogService dialogService)
        {
            _departmentAppService = departmentAppService;
            _navigationService = navigationService;
            _dialogService = dialogService;

            EditCommand = new Command(async () => await EditAsync(), () => !IsBusy);
            DeleteCommand = new Command(async () => await DeleteAsync(), () => !IsBusy);
            BackCommand = new Command(async () => await BackAsync(), () => !IsBusy);
        }

        public Guid DepartmentId { get; set; }

        private string _name = string.Empty;
        public string Name { get => _name; private set => SetProperty(ref _name, value); }

        private string _description = string.Empty;
        public string Description { get => _description; private set => SetProperty(ref _description, value); }

        // Se você não tiver headcount no DTO, mantém 0 sem quebrar build
        private int _activeEmployeesCount;
        public int ActiveEmployeesCount { get => _activeEmployeesCount; private set => SetProperty(ref _activeEmployeesCount, value); }

        private int _totalEmployeesCount;
        public int TotalEmployeesCount { get => _totalEmployeesCount; private set => SetProperty(ref _totalEmployeesCount, value); }

        public string CodeDisplay => $"ID: {DepartmentId}";

        public Command EditCommand { get; }
        public Command DeleteCommand { get; }
        public Command BackCommand { get; }

        public async Task LoadAsync()
        {
            if (DepartmentId == Guid.Empty) return;

            try
            {
                IsBusy = true;

                var dto = await _departmentAppService.GetByIdAsync(DepartmentId);
                if (dto is null) return;

                Name = dto.Name ?? string.Empty;
                Description = dto.Description ?? string.Empty;

                // se não existir no seu DTO, comente essas linhas
                ActiveEmployeesCount = dto.Headcount;
                TotalEmployeesCount = dto.Headcount;

                Title = $"Departamento - {Name}";
            }
            catch
            {
                await _dialogService.ShowErrorAsync("Erro", "Erro ao carregar detalhes do departamento.");
            }
            finally
            {
                IsBusy = false;
            }
        }

        private Task EditAsync()
        {
            var parameters = new Dictionary<string, object> { ["id"] = DepartmentId };
            return _navigationService.GoToAsync(NavigationRoutes.DepartmentFormPage, parameters);
        }

        private async Task DeleteAsync()
        {
            var confirm = await _dialogService.ShowDangerConfirmationAsync(
                "Excluir departamento",
                "Tem certeza que deseja excluir este departamento?");

            if (!confirm) return;

            try
            {
                IsBusy = true;
                await _departmentAppService.DeleteAsync(DepartmentId);

                await _dialogService.ShowInfoAsync("Sucesso", "Departamento excluído com sucesso.");
                await _navigationService.GoBackAsync();
            }
            catch
            {
                await _dialogService.ShowErrorAsync("Erro", "Erro ao excluir departamento.");
            }
            finally
            {
                IsBusy = false;
            }
        }

        private Task BackAsync() => _navigationService.GoBackAsync();
    }
}
