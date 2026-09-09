// src/EmployeeRegistrationApp.Maui/Presentation/Departments/ViewModels/DepartmentsListViewModel.cs
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
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
    /// ViewModel da lista de departamentos.
    /// </summary>
    public sealed class DepartmentsListViewModel : ViewModelBase
    {
        private readonly IDepartmentAppService _departmentAppService;
        private readonly INavigationService _navigationService;
        private readonly IDialogService _dialogService;

        private readonly ObservableCollection<DepartmentDto> _allDepartments =
            new ObservableCollection<DepartmentDto>();

        public ObservableCollection<DepartmentDto> Departments { get; } =
            new ObservableCollection<DepartmentDto>();

        private string _searchTerm = string.Empty;
        private bool _isRefreshing;

        public DepartmentsListViewModel(
            IDepartmentAppService departmentAppService,
            INavigationService navigationService,
            IDialogService dialogService)
        {
            _departmentAppService = departmentAppService;
            _navigationService = navigationService;
            _dialogService = dialogService;

            Title = "Departamentos";

            LoadCommand = new Command(async () => await LoadAsync(), () => IsNotBusy);
            RefreshCommand = new Command(async () => await LoadAsync(), () => IsNotBusy);
            NewDepartmentCommand = new Command(async () => await NavigateToCreateAsync(), () => IsNotBusy);
            OpenDetailsCommand = new Command<DepartmentDto>(
                async d => await OpenDetailsAsync(d),
                d => IsNotBusy && d is not null);
        }

        public string SearchTerm
        {
            get => _searchTerm;
            set
            {
                if (SetProperty(ref _searchTerm, value))
                {
                    ApplyFilter();
                }
            }
        }

        public bool IsRefreshing
        {
            get => _isRefreshing;
            set => SetProperty(ref _isRefreshing, value);
        }

        public ICommand LoadCommand { get; }
        public ICommand RefreshCommand { get; }
        public ICommand NewDepartmentCommand { get; }
        public ICommand OpenDetailsCommand { get; }

        public async Task LoadAsync()
        {
            if (IsBusy) return;

            try
            {
                IsBusy = true;
                IsRefreshing = true;

                _allDepartments.Clear();
                Departments.Clear();

                var items = await _departmentAppService.GetAllAsync();

                foreach (var dept in items)
                    _allDepartments.Add(dept);

                ApplyFilter();
            }
            catch (Exception)
            {
                await _dialogService.ShowErrorAsync(
                    "Erro",
                    "Erro ao carregar departamentos."
                );
            }
            finally
            {
                IsBusy = false;
                IsRefreshing = false;
            }
        }

        private void ApplyFilter()
        {
            Departments.Clear();

            var query = _allDepartments.AsEnumerable();

            if (!string.IsNullOrWhiteSpace(SearchTerm))
            {
                var term = SearchTerm.Trim().ToLowerInvariant();
                query = query.Where(d =>
                    (!string.IsNullOrEmpty(d.Name) &&
                     d.Name!.ToLowerInvariant().Contains(term)) ||
                    (!string.IsNullOrEmpty(d.Description) &&
                     d.Description!.ToLowerInvariant().Contains(term)));
            }

            foreach (var dept in query)
                Departments.Add(dept);
        }

        private Task NavigateToCreateAsync()
        {
            return _navigationService.GoToAsync(NavigationRoutes.DepartmentFormPage);
        }

        private Task OpenDetailsAsync(DepartmentDto? department)
        {
            if (department is null)
                return Task.CompletedTask;

            var parameters = new Dictionary<string, object>
            {
                ["id"] = department.Id
            };

            return _navigationService.GoToAsync(NavigationRoutes.DepartmentDetailsPage, parameters);
        }
    }
}
