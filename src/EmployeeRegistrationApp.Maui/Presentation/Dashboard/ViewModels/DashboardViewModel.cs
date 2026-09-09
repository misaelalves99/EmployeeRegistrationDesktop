// src/EmployeeRegistrationApp.Maui/Presentation/Dashboard/ViewModels/DashboardViewModel.cs
using System;
using System.Globalization;
using System.Threading.Tasks;
using System.Windows.Input;
using EmployeeRegistrationApp.Application.DTOs.Dashboard;
using EmployeeRegistrationApp.Application.Interfaces.Services;
using EmployeeRegistrationApp.Maui.Core.Base;
using EmployeeRegistrationApp.Maui.Core.Services.Notifications;

namespace EmployeeRegistrationApp.Maui.Presentation.Dashboard.ViewModels;

/// <summary>
/// ViewModel do dashboard principal: headcount, departamentos, cargos e média salarial.
/// </summary>
public sealed class DashboardViewModel : ViewModelBase
{
    private readonly IReportAppService _reportAppService;
    private readonly IEmployeeAppService _employeeAppService;
    private readonly IToastService _toastService;

    private int _totalEmployees;
    private int _activeEmployees;
    private int _inactiveEmployees;
    private int _departmentsCount;
    private int _positionsCount;
    private decimal _averageSalary;
    private DateTime _lastUpdated;
    private string _headcountSummary = string.Empty;

    public DashboardViewModel(
        IReportAppService reportAppService,
        IEmployeeAppService employeeAppService,
        IToastService toastService)
    {
        _reportAppService = reportAppService ?? throw new ArgumentNullException(nameof(reportAppService));
        _employeeAppService = employeeAppService ?? throw new ArgumentNullException(nameof(employeeAppService));
        _toastService = toastService ?? throw new ArgumentNullException(nameof(toastService));

        RefreshCommand = new AsyncCommand(LoadAsync);
    }

    public int TotalEmployees
    {
        get => _totalEmployees;
        private set => SetProperty(ref _totalEmployees, value);
    }

    public int ActiveEmployees
    {
        get => _activeEmployees;
        private set => SetProperty(ref _activeEmployees, value);
    }

    public int InactiveEmployees
    {
        get => _inactiveEmployees;
        private set => SetProperty(ref _inactiveEmployees, value);
    }

    public int DepartmentsCount
    {
        get => _departmentsCount;
        private set => SetProperty(ref _departmentsCount, value);
    }

    public int PositionsCount
    {
        get => _positionsCount;
        private set => SetProperty(ref _positionsCount, value);
    }

    public decimal AverageSalary
    {
        get => _averageSalary;
        private set
        {
            if (SetProperty(ref _averageSalary, value))
                OnPropertyChanged(nameof(AverageSalaryDisplay));
        }
    }

    public DateTime LastUpdated
    {
        get => _lastUpdated;
        private set
        {
            if (SetProperty(ref _lastUpdated, value))
                OnPropertyChanged(nameof(LastUpdatedDisplay));
        }
    }

    public string AverageSalaryDisplay =>
        AverageSalary <= 0
            ? "—"
            : AverageSalary.ToString("C0", CultureInfo.CurrentCulture);

    public string HeadcountSummary
    {
        get => _headcountSummary;
        private set => SetProperty(ref _headcountSummary, value);
    }

    public string LastUpdatedDisplay =>
        LastUpdated == default
            ? "Dados ainda não carregados"
            : $"Atualizado em {LastUpdated:g}";

    public ICommand RefreshCommand { get; }

    public async Task LoadAsync()
    {
        if (IsBusy)
            return;

        try
        {
            IsBusy = true;

            DashboardSummaryDto summary = await _reportAppService.GetDashboardSummaryAsync();

            TotalEmployees = summary.TotalEmployees;
            ActiveEmployees = summary.ActiveEmployees;
            InactiveEmployees = summary.InactiveEmployees;
            DepartmentsCount = summary.DepartmentsCount;
            PositionsCount = summary.PositionsCount;

            // Se o DTO tiver a média salarial, você pode ajustar aqui depois.
            // Por enquanto, apenas zera para evitar erro de compilação.
            AverageSalary = 0;

            LastUpdated = DateTime.Now;

            HeadcountSummary =
                $"Atualmente sua organização possui {TotalEmployees} funcionários, " +
                $"{ActiveEmployees} ativos e {InactiveEmployees} inativos, " +
                $"distribuídos em {DepartmentsCount} departamentos e {PositionsCount} cargos diferentes.";
        }
        catch (Exception ex)
        {
            await _toastService.ShowError($"Erro ao carregar dashboard: {ex.Message}");
        }
        finally
        {
            IsBusy = false;
        }
    }
}
