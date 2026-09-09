// src/EmployeeRegistrationApp.Maui/Presentation/Positions/ViewModels/PositionsListViewModel.cs
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using EmployeeRegistrationApp.Application.DTOs.Positions;
using EmployeeRegistrationApp.Application.Interfaces.Services;
using EmployeeRegistrationApp.Maui.Core.Base;
using EmployeeRegistrationApp.Maui.Core.Config;
using EmployeeRegistrationApp.Maui.Core.Services.Dialogs;
using EmployeeRegistrationApp.Maui.Core.Services.Navigation;

namespace EmployeeRegistrationApp.Maui.Presentation.Positions.ViewModels
{
    public sealed class PositionsListViewModel : ViewModelBase
    {
        private readonly IPositionAppService _positionAppService;
        private readonly INavigationService _navigationService;
        private readonly IDialogService _dialogService;

        private string _searchTerm = string.Empty;

        public ObservableCollection<PositionDto> Positions { get; } = new();

        public string SearchTerm
        {
            get => _searchTerm;
            set
            {
                if (SetProperty(ref _searchTerm, value))
                {
                    _ = LoadAsync();
                }
            }
        }

        public AsyncCommand LoadCommand { get; }
        public AsyncCommand RefreshCommand { get; }
        public AsyncCommand NewPositionCommand { get; }

        public PositionsListViewModel(
            IPositionAppService positionAppService,
            INavigationService navigationService,
            IDialogService dialogService)
        {
            _positionAppService = positionAppService;
            _navigationService = navigationService;
            _dialogService = dialogService;

            LoadCommand = new AsyncCommand(LoadAsync);
            RefreshCommand = new AsyncCommand(LoadAsync);
            NewPositionCommand = new AsyncCommand(ExecuteNewPositionAsync);
        }

        public async Task LoadAsync()
        {
            if (IsBusy)
                return;

            try
            {
                IsBusy = true;

                Positions.Clear();

                var result = await _positionAppService
                    .GetAllAsync(_searchTerm);

                if (result != null && result.Any())
                {
                    foreach (var dto in result)
                    {
                        Positions.Add(dto);
                    }
                }
            }
            catch (Exception ex)
            {
                await _dialogService.ShowErrorAsync(
                    "Erro ao carregar cargos",
                    ex.Message);
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async Task ExecuteNewPositionAsync()
        {
            await _navigationService.GoToAsync(NavigationRoutes.PositionForm);
        }

        public async Task OpenDetailsAsync(PositionDto dto)
        {
            if (dto == null)
                return;

            var route = $"{NavigationRoutes.PositionDetails}?id={dto.Id}";
            await _navigationService.GoToAsync(route);
        }
    }
}
