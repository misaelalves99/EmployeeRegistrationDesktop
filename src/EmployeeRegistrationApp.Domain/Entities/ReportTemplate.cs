// src/EmployeeRegistrationApp.Domain/Entities/ReportTemplate.cs
using System;
using EmployeeRegistrationApp.Domain.Base;

namespace EmployeeRegistrationApp.Domain.Entities
{
    /// <summary>
    /// Template de relatórios (ex: Headcount por Departamento, Sumário de Salários).
    /// Permite parametrizar relatórios na aplicação.
    /// </summary>
    public sealed class ReportTemplate : AggregateRoot
    {
        /// <summary>
        /// Código interno curto (ex: "HEADCOUNT_BY_DEPARTMENT").
        /// </summary>
        public string Code { get; private set; } = string.Empty;

        /// <summary>
        /// Nome amigável apresentado na UI.
        /// </summary>
        public string Name { get; private set; } = string.Empty;

        /// <summary>
        /// Descrição do relatório.
        /// </summary>
        public string? Description { get; private set; }

        /// <summary>
        /// Definição técnica (pode ser JSON, nome de stored procedure, etc.).
        /// </summary>
        public string Definition { get; private set; } = string.Empty;

        /// <summary>
        /// Configurações adicionais (layout, colunas, filtros).
        /// Pode ser JSON serializado.
        /// </summary>
        public string? Configuration { get; private set; }

        public bool IsActive { get; private set; }

        // Construtor privado para EF
        private ReportTemplate() { }

        public ReportTemplate(
            string code,
            string name,
            string definition,
            string? description = null,
            string? configuration = null,
            bool isActive = true)
        {
            UpdateInfo(code, name, definition, description, configuration);
            IsActive = isActive;
        }

        public void UpdateInfo(
            string code,
            string name,
            string definition,
            string? description,
            string? configuration)
        {
            if (string.IsNullOrWhiteSpace(code))
                throw new ArgumentException("Código do template de relatório é obrigatório.", nameof(code));

            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Nome do relatório é obrigatório.", nameof(name));

            if (string.IsNullOrWhiteSpace(definition))
                throw new ArgumentException("Definição do relatório é obrigatória.", nameof(definition));

            Code = code.Trim().ToUpperInvariant();
            Name = name.Trim();
            Definition = definition.Trim();
            Description = string.IsNullOrWhiteSpace(description) ? null : description.Trim();
            Configuration = string.IsNullOrWhiteSpace(configuration) ? null : configuration.Trim();
        }

        public void Activate() => IsActive = true;

        public void Deactivate() => IsActive = false;
    }
}
