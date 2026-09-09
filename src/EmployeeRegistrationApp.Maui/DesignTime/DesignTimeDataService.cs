// src/EmployeeRegistrationApp.Maui/DesignTime/DesignTimeDataService.cs
using System.Collections.ObjectModel;

namespace EmployeeRegistrationApp.Maui.DesignTime;

/// <summary>
/// Serviço usado apenas em tempo de design (preview do XAML),
/// com dados estáticos para popular listas, cards, etc.
/// </summary>
public static class DesignTimeDataService
{
    public static IReadOnlyList<DesignTimeEmployeeItem> GetSampleEmployees()
    {
        return new ObservableCollection<DesignTimeEmployeeItem>
        {
            new()
            {
                Id = 1,
                FullName = "Ana Souza",
                Department = "Tecnologia",
                Position = "Desenvolvedora Plena",
                EmploymentStatus = "Ativa",
                ContractType = "CLT",
                Salary = 8500.00m
            },
            new()
            {
                Id = 2,
                FullName = "Carlos Pereira",
                Department = "Recursos Humanos",
                Position = "Analista de RH",
                EmploymentStatus = "Ativa",
                ContractType = "CLT",
                Salary = 6200.00m
            },
            new()
            {
                Id = 3,
                FullName = "Mariana Lima",
                Department = "Financeiro",
                Position = "Coordenadora Financeira",
                EmploymentStatus = "Inativa",
                ContractType = "PJ",
                Salary = 12000.00m
            },
            new()
            {
                Id = 4,
                FullName = "João Santos",
                Department = "Tecnologia",
                Position = "Desenvolvedor Júnior",
                EmploymentStatus = "Ativa",
                ContractType = "CLT",
                Salary = 4500.00m
            }
        };
    }

    public static DesignTimeDashboardSummary GetSampleDashboard()
    {
        return new DesignTimeDashboardSummary
        {
            TotalEmployees = 42,
            ActiveEmployees = 36,
            InactiveEmployees = 6,
            NewAdmissionsLast30Days = 5,
            DepartmentsHeadcount = new List<DesignTimeDepartmentHeadcount>
            {
                new() { DepartmentName = "Tecnologia", Headcount = 18 },
                new() { DepartmentName = "Recursos Humanos", Headcount = 6 },
                new() { DepartmentName = "Financeiro", Headcount = 8 },
                new() { DepartmentName = "Comercial", Headcount = 10 }
            }
        };
    }
}

/// <summary>
/// Modelo simples apenas para design-time, representando um colaborador na lista.
/// </summary>
public sealed class DesignTimeEmployeeItem
{
    public int Id { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string Department { get; set; } = string.Empty;
    public string Position { get; set; } = string.Empty;
    public string EmploymentStatus { get; set; } = string.Empty;
    public string ContractType { get; set; } = string.Empty;
    public decimal Salary { get; set; }
}

/// <summary>
/// Modelo simples de resumo de dashboard em tempo de design.
/// </summary>
public sealed class DesignTimeDashboardSummary
{
    public int TotalEmployees { get; set; }
    public int ActiveEmployees { get; set; }
    public int InactiveEmployees { get; set; }
    public int NewAdmissionsLast30Days { get; set; }

    public IList<DesignTimeDepartmentHeadcount> DepartmentsHeadcount { get; set; }
        = new List<DesignTimeDepartmentHeadcount>();
}

public sealed class DesignTimeDepartmentHeadcount
{
    public string DepartmentName { get; set; } = string.Empty;
    public int Headcount { get; set; }
}
