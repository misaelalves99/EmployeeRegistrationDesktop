// src/EmployeeRegistrationApp.Maui/Presentation/Reports/ViewModels/SalarySummaryViewModel.cs
using System;
using System.Threading.Tasks;
using EmployeeRegistrationApp.Application.DTOs.Reports;
using EmployeeRegistrationApp.Application.Interfaces.Services;
using EmployeeRegistrationApp.Maui.Core.Base;

namespace EmployeeRegistrationApp.Maui.Presentation.Reports.ViewModels
{
    public sealed class SalarySummaryViewModel : ViewModelBase
    {
        private readonly IReportAppService _reportAppService;

        private decimal _totalSalary;
        private decimal _averageSalary;
        private decimal _minSalary;
        private decimal _maxSalary;
        private int _employeesCount;

        public decimal TotalSalary
        {
            get => _totalSalary;
            set => SetProperty(ref _totalSalary, value);
        }

        public decimal AverageSalary
        {
            get => _averageSalary;
            set => SetProperty(ref _averageSalary, value);
        }

        public decimal MinSalary
        {
            get => _minSalary;
            set => SetProperty(ref _minSalary, value);
        }

        public decimal MaxSalary
        {
            get => _maxSalary;
            set => SetProperty(ref _maxSalary, value);
        }

        public int EmployeesCount
        {
            get => _employeesCount;
            set => SetProperty(ref _employeesCount, value);
        }

        public AsyncCommand InitializeCommand { get; }
        public AsyncCommand RefreshCommand { get; }

        public SalarySummaryViewModel(IReportAppService reportAppService)
        {
            _reportAppService = reportAppService ?? throw new ArgumentNullException(nameof(reportAppService));

            InitializeCommand = new AsyncCommand(LoadDataAsync);
            RefreshCommand = new AsyncCommand(LoadDataAsync);
        }

        public Task InitializeAsync() => LoadDataAsync();

        private async Task LoadDataAsync()
        {
            if (IsBusy) return;

            try
            {
                IsBusy = true;

                SalarySummaryDto summary = await _reportAppService.GetSalarySummaryAsync();

                TotalSalary = summary.TotalPayroll;
                AverageSalary = summary.AverageSalary;
                MinSalary = summary.MinSalary;
                MaxSalary = summary.MaxSalary;
                EmployeesCount = summary.EmployeesCount;
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}
