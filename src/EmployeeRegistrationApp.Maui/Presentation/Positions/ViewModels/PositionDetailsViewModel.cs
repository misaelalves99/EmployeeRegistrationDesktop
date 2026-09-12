// src/EmployeeRegistrationApp.Maui/Presentation/Positions/ViewModels/PositionDetailsViewModel.cs
using System;
using System.Globalization;
using System.Threading.Tasks;
using EmployeeRegistrationApp.Application.DTOs.Positions;
using EmployeeRegistrationApp.Application.Interfaces.Services;
using EmployeeRegistrationApp.Maui.Core.Base;
using EmployeeRegistrationApp.Maui.Core.Config;
using EmployeeRegistrationApp.Maui.Core.Services.Dialogs;
using EmployeeRegistrationApp.Maui.Core.Services.Navigation;
using Microsoft.Maui.Graphics;

namespace EmployeeRegistrationApp.Maui.Presentation.Positions.ViewModels
{
    public sealed class PositionDetailsViewModel : ViewModelBase
    {
        private readonly IPositionAppService _positionAppService;
        private readonly INavigationService _navigationService;
        private readonly IDialogService _dialogService;

        private Guid _positionId;

        private string _name = string.Empty;
        private string _departmentName = string.Empty;
        private string _typeDisplay = string.Empty;
        private string _salaryRangeDisplay = string.Empty;
        private string _statusDisplay = string.Empty;
        private Color _statusColor = Colors.Green;
        private string _description = string.Empty;
        private string _createdAtDisplay = string.Empty;
        private string _updatedAtDisplay = string.Empty;

        public string Name
        {
            get => _name;
            private set => SetProperty(ref _name, value);
        }

        public string DepartmentName
        {
            get => _departmentName;
            private set => SetProperty(ref _departmentName, value);
        }

        public string TypeDisplay
        {
            get => _typeDisplay;
            private set => SetProperty(ref _typeDisplay, value);
        }

        public string SalaryRangeDisplay
        {
            get => _salaryRangeDisplay;
            private set => SetProperty(ref _salaryRangeDisplay, value);
        }

        public string StatusDisplay
        {
            get => _statusDisplay;
            private set => SetProperty(ref _statusDisplay, value);
        }

        public Color StatusColor
        {
            get => _statusColor;
            private set => SetProperty(ref _statusColor, value);
        }

        public string Description
        {
            get => _description;
            private set => SetProperty(ref _description, value);
        }

        public string CreatedAtDisplay
        {
            get => _createdAtDisplay;
            private set => SetProperty(ref _createdAtDisplay, value);
        }

        public string UpdatedAtDisplay
        {
            get => _updatedAtDisplay;
            private set => SetProperty(ref _updatedAtDisplay, value);
        }

        public AsyncCommand EditCommand { get; }
        public AsyncCommand DeleteCommand { get; }
        public AsyncCommand BackCommand { get; }

        /// <summary>
        /// Propriedade que deve ser preenchida via rota Shell: ?id={guid}
        /// </summary>
        public Guid PositionId
        {
            get => _positionId;
            set => SetProperty(ref _positionId, value);
        }

        public PositionDetailsViewModel(
            IPositionAppService positionAppService,
            INavigationService navigationService,
            IDialogService dialogService)
        {
            _positionAppService = positionAppService;
            _navigationService = navigationService;
            _dialogService = dialogService;

            EditCommand = new AsyncCommand(ExecuteEditAsync);
            DeleteCommand = new AsyncCommand(ExecuteDeleteAsync);
            BackCommand = new AsyncCommand(ExecuteBackAsync);
        }

        public async Task LoadAsync()
        {
            if (IsBusy || PositionId == Guid.Empty)
                return;

            try
            {
                IsBusy = true;

                var dto = await _positionAppService.GetByIdAsync(PositionId);
                if (dto == null)
                {
                    await _dialogService.ShowErrorAsync(
                        "Cargo não encontrado",
                        "Não foi possível carregar as informações deste cargo.");
                    await _navigationService.GoBackAsync();
                    return;
                }

                FillFromDto(dto);
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

        private void FillFromDto(PositionDto dto)
        {
            Name = dto.Name;
            DepartmentName = dto.DepartmentName ?? string.Empty;
            TypeDisplay = dto.Type.ToString();

            SalaryRangeDisplay = dto.BaseSalary > 0
                ? $"{dto.BaseSalaryCurrency} {dto.BaseSalary:N2}"
                : "Não informado";

            StatusDisplay = dto.IsActive ? "Ativo" : "Inativo";
            StatusColor = dto.IsActive ? Colors.Green : Colors.Red;
            Description = dto.Description ?? string.Empty;

            var culture = new CultureInfo("pt-BR");

            CreatedAtDisplay = dto.CreatedAt != default
                ? $"Criado em {dto.CreatedAt.ToString("g", culture)}"
                : string.Empty;

            UpdatedAtDisplay = dto.LastModifiedAt.HasValue
                ? $"Atualizado em {dto.LastModifiedAt.Value.ToString("g", culture)}"
                : string.Empty;
        }

        private async Task ExecuteEditAsync()
        {
            if (PositionId == Guid.Empty)
                return;

            var route = $"{NavigationRoutes.PositionForm}?id={PositionId}";
            await _navigationService.GoToAsync(route);
        }

        private async Task ExecuteDeleteAsync()
        {
            if (PositionId == Guid.Empty)
                return;

            var confirm = await _dialogService.ShowConfirmationAsync(
                "Excluir cargo",
                "Tem certeza que deseja excluir este cargo? Essa ação não poderá ser desfeita.");

            if (!confirm)
                return;

            try
            {
                IsBusy = true;

                await _positionAppService.DeleteAsync(PositionId);

                await _dialogService.ShowInfoAsync(
                    "Cargo excluído",
                    "O cargo foi excluído com sucesso.");

                await _navigationService.GoToRootAsync(NavigationRoutes.PositionsList);
            }
            catch (Exception ex)
            {
                await _dialogService.ShowErrorAsync(
                    "Erro ao excluir cargo",
                    ex.Message);
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async Task ExecuteBackAsync()
        {
            await _navigationService.GoBackAsync();
        }
    }
}
