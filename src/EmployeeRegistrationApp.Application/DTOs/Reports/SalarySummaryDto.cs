// src/EmployeeRegistrationApp.Application/DTOs/Reports/SalarySummaryDto.cs
using System.Collections.Generic;

namespace EmployeeRegistrationApp.Application.DTOs.Reports
{
    public sealed class SalarySummaryDto
    {
        public int EmployeesCount { get; set; }

        // Totais globais
        public decimal TotalPayroll { get; set; }
        public decimal AverageSalary { get; set; }
        public decimal MinSalary { get; set; }
        public decimal MaxSalary { get; set; }

        // Itens por departamento
        public IReadOnlyList<SalarySummaryDepartmentItemDto> Items { get; set; }
            = System.Array.Empty<SalarySummaryDepartmentItemDto>();
    }

    public sealed class SalarySummaryDepartmentItemDto
    {
        public string DepartmentName { get; set; } = string.Empty;

        public int EmployeesCount { get; set; }

        public decimal TotalDepartmentPayroll { get; set; }
        public decimal AverageSalary { get; set; }
        public decimal MinSalary { get; set; }
        public decimal MaxSalary { get; set; }
    }
}
