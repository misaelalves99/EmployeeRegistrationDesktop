// src/EmployeeRegistrationApp.Application/DTOs/Departments/DepartmentDto.cs
using System;

namespace EmployeeRegistrationApp.Application.DTOs.Departments
{
    /// <summary>
    /// DTO de Departamento, usado tanto para listagem quanto para formulário.
    /// </summary>
    public sealed class DepartmentDto
    {
        public Guid? Id { get; set; }

        /// <summary>
        /// Nome do departamento (ex: "Tecnologia", "RH", "Financeiro").
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Código curto do departamento (ex: "TI", "HR", "FIN").
        /// </summary>
        public string? Code { get; set; }

        /// <summary>
        /// Descrição/resumo da área.
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// Indica se o departamento está ativo para novas alocações.
        /// </summary>
        public bool IsActive { get; set; } = true;

        /// <summary>
        /// Headcount atual (apenas leitura, alimentado por relatórios / joins).
        /// </summary>
        public int Headcount { get; set; }

        public DateTime CreatedAt { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? LastModifiedAt { get; set; }
        public string? LastModifiedBy { get; set; }
    }
}
