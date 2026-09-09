// src/EmployeeRegistrationApp.Maui/Presentation/Reports/ViewModels/ReportsViewModel.cs
using System;
using System.Threading.Tasks;
using EmployeeRegistrationApp.Application.DTOs.Dashboard;
using EmployeeRegistrationApp.Application.Interfaces.Services;
using EmployeeRegistrationApp.Maui.Core.Base;
using EmployeeRegistrationApp.Maui.Core.Services.Navigation;

namespace EmployeeRegistrationApp.Maui.Presentation.Reports.ViewModels
{
    public sealed class ReportsViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private readonly IReportAppService _reportAppService;

        private string _headcountSummary = "-";
        private string _departmentsSummary = "-";

        public string HeadcountSummary
        {
            get => _headcountSummary;
            set => SetProperty(ref _headcountSummary, value);
        }

        public string DepartmentsSummary
        {
            get => _departmentsSummary;
            set => SetProperty(ref _departmentsSummary, value);
        }

        public AsyncCommand InitializeCommand { get; }
        public AsyncCommand OpenHeadcountReportCommand { get; }
        public AsyncCommand OpenSalarySummaryReportCommand { get; }

        public ReportsViewModel(
            INavigationService navigationService,
            IReportAppService reportAppService)
        {
            _navigationService = navigationService ?? throw new ArgumentNullException(nameof(navigationService));
            _reportAppService = reportAppService ?? throw new ArgumentNullException(nameof(reportAppService));

            InitializeCommand = new AsyncCommand(InitializeAsync);
            OpenHeadcountReportCommand = new AsyncCommand(OpenHeadcountReportAsync);
            OpenSalarySummaryReportCommand = new AsyncCommand(OpenSalarySummaryReportAsync);
        }

        public async Task InitializeAsync()
        {
            if (IsBusy) return;

            try
            {
                IsBusy = true;

                DashboardSummaryDto summary = await _reportAppService.GetDashboardSummaryAsync();

                HeadcountSummary = $"{summary.TotalEmployees} colaboradores";
                DepartmentsSummary = $"{summary.DepartmentsCount} departamentos";
            }
            finally
            {
                IsBusy = false;
            }
        }

        private Task OpenHeadcountReportAsync()
            => _navigationService.NavigateToAsync("//Reports/HeadcountByDepartmentPage");

        private Task OpenSalarySummaryReportAsync()
            => _navigationService.NavigateToAsync("//Reports/SalarySummaryPage");
    }
}
