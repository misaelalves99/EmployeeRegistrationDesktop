// src/EmployeeRegistrationApp.Application/DTOs/Employees/EmployeeDto.cs
using System;
using EmployeeRegistrationApp.Domain.Enums;

namespace EmployeeRegistrationApp.Application.DTOs.Employees
{
    /// <summary>
    /// DTO base de funcionário, usado para criação/edição.
    /// Compatível com as UIs Web e MAUI.
    /// </summary>
    public class EmployeeDto
    {
        public Guid? Id { get; set; }

        // Dados pessoais
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;

        /// <summary>
        /// Nome completo (conveniência para UI).
        /// </summary>
        public string FullName { get; set; } = string.Empty;

        /// <summary>
        /// Documento CPF em formato texto.
        /// </summary>
        public string Cpf { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;
        public string? PhoneNumber { get; set; }

        // Contrato e status (modelo "canônico")
        public DateTime HireDate { get; set; }
        public DateTime? TerminationDate { get; set; }

        public EmploymentStatus EmploymentStatus { get; set; }
        public ContractType ContractType { get; set; }
        public JobRole JobRole { get; set; }

        /// <summary>
        /// Valor numérico do salário (sem a lógica de Money).
        /// </summary>
        public decimal SalaryAmount { get; set; }

        /// <summary>
        /// Código da moeda (ex: "BRL", "USD").
        /// </summary>
        public string SalaryCurrency { get; set; } = "BRL";

        // Relacionamentos
        public Guid? DepartmentId { get; set; }
        public Guid? PositionId { get; set; }

        // --------------------------------------------------------------------
        // Aliases usados pela UI MAUI (mantêm compatibilidade com o modelo antigo)
        // --------------------------------------------------------------------

        /// <summary>
        /// Data de admissão (alias para HireDate).
        /// </summary>
        public DateTime AdmissionDate
        {
            get => HireDate;
            set => HireDate = value;
        }

        /// <summary>
        /// Salário base (alias para SalaryAmount).
        /// </summary>
        public decimal BaseSalary
        {
            get => SalaryAmount;
            set => SalaryAmount = value;
        }
    }
}
