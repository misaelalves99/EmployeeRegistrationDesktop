// src/EmployeeRegistrationApp.Maui/Presentation/Reports/ViewModels/HeadcountByDepartmentViewModel.cs
using EmployeeRegistrationApp.Application.DTOs.Reports;
using EmployeeRegistrationApp.Application.Interfaces.Services;
using EmployeeRegistrationApp.Maui.Core.Base;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace EmployeeRegistrationApp.Maui.Presentation.Reports.ViewModels;

public sealed class HeadcountByDepartmentViewModel : ViewModelBase
{
    private readonly IReportAppService _reportAppService;

    public ObservableCollection<HeadcountItemViewModel> HeadcountItems { get; } = new();

    public List<string> Periods { get; } = new()
    {
        "Últimos 30 dias",
        "Últimos 3 meses",
        "Últimos 12 meses",
        "Todo o período"
    };

    private string _selectedPeriod;
    public string SelectedPeriod
    {
        get => _selectedPeriod;
        set
        {
            if (SetProperty(ref _selectedPeriod, value))
            {
                // Você pode chamar LoadDataAsync aqui se quiser atualizar ao trocar o período
            }
        }
    }

    public AsyncCommand InitializeCommand { get; }
    public AsyncCommand RefreshCommand { get; }

    public HeadcountByDepartmentViewModel(IReportAppService reportAppService)
    {
        _reportAppService = reportAppService;

        _selectedPeriod = Periods.First();

        InitializeCommand = new AsyncCommand(InitializeAsync);
        RefreshCommand = new AsyncCommand(LoadDataAsync);
    }

    public async Task InitializeAsync()
    {
        if (IsBusy) return;

        await LoadDataAsync();
    }

    private async Task LoadDataAsync()
    {
        if (IsBusy) return;

        try
        {
            IsBusy = true;

            HeadcountItems.Clear();

            // Busca lista da camada Application
            IEnumerable<HeadcountByDepartmentDto> result =
                await _reportAppService.GetHeadcountByDepartmentAsync();

            int totalEmployees = result.Sum(x => x.TotalEmployees);

            foreach (var dto in result)
            {
                double percentage = totalEmployees > 0
                    ? (double)dto.TotalEmployees / totalEmployees
                    : 0;

                HeadcountItems.Add(new HeadcountItemViewModel
                {
                    DepartmentName = dto.DepartmentName,
                    TotalEmployees = dto.TotalEmployees,
                    ActiveEmployees = dto.ActiveEmployees,
                    InactiveEmployees = dto.InactiveEmployees,
                    PercentageInWorkforce = percentage
                });
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine(ex);
        }
        finally
        {
            IsBusy = false;
        }
    }
}

public sealed class HeadcountItemViewModel
{
    public string DepartmentName { get; set; } = string.Empty;
    public int TotalEmployees { get; set; }
    public int ActiveEmployees { get; set; }
    public int InactiveEmployees { get; set; }
    public double PercentageInWorkforce { get; set; }
}
