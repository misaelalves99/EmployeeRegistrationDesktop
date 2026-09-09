// src/EmployeeRegistrationApp.Application/DTOs/Employees/EmployeeListItemDto.cs
using System;
using EmployeeRegistrationApp.Domain.Enums;
using EmployeeRegistrationApp.Domain.ValueObjects;

namespace EmployeeRegistrationApp.Application.DTOs.Employees
{
    public sealed class EmployeeListItemDto
    {
        public Guid Id { get; set; }

        public string FullName { get; set; } = string.Empty;

        public EmploymentStatus EmploymentStatus { get; set; }

        public JobRole JobRole { get; set; }

        public decimal SalaryAmount { get; set; }
        public string SalaryCurrency { get; set; } = "BRL";

        public bool IsActive { get; set; }

        public string DepartmentName { get; set; } = string.Empty;
        public string PositionName { get; set; } = string.Empty;

        // ---- Helpers p/ UI (sem converter complexo) ----

        public string EmploymentStatusLabel => EmploymentStatus.ToString();

        public Money Salary => Money.FromDecimal(SalaryAmount, SalaryCurrency);

        public string DepartmentAndPosition =>
            string.IsNullOrWhiteSpace(DepartmentName) && string.IsNullOrWhiteSpace(PositionName)
                ? "-"
                : $"{DepartmentName} • {PositionName}";
    }
}
