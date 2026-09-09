// src/EmployeeRegistrationApp.Application/DTOs/Employees/EmployeeDetailsDto.cs
using System;
using EmployeeRegistrationApp.Domain.Enums;
using EmployeeRegistrationApp.Domain.ValueObjects;

namespace EmployeeRegistrationApp.Application.DTOs.Employees
{
    public sealed class EmployeeDetailsDto
    {
        public Guid Id { get; set; }

        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;

        public DateTime HireDate { get; set; }
        public DateTime? TerminationDate { get; set; }

        public ContractType ContractType { get; set; }
        public JobRole JobRole { get; set; }
        public EmploymentStatus EmploymentStatus { get; set; }

        public decimal SalaryAmount { get; set; }
        public string SalaryCurrency { get; set; } = "BRL";

        public bool IsActive { get; set; }

        public string DepartmentName { get; set; } = string.Empty;
        public string PositionName { get; set; } = string.Empty;

        public string ContractTypeLabel { get; set; } = string.Empty;
        public string EmploymentStatusLabel { get; set; } = string.Empty;

        // ---- Helpers p/ UI ----
        public Money Salary => Money.FromDecimal(SalaryAmount, SalaryCurrency);

        // Alias p/ telas que esperam "PositionTitle"
        public string PositionTitle => PositionName;
    }
}
