using System;
using System.Collections.Generic;
using System.Linq;
using EmployeeRegistrationApp.Domain.Base;
using EmployeeRegistrationApp.Domain.Interfaces;

namespace EmployeeRegistrationApp.Domain.Entities
{
    /// <summary>
    /// Departamento (RH, TI, Financeiro...), com vínculo a funcionários.
    /// AggregateRoot auditável.
    /// </summary>
    public sealed class Department : AggregateRoot, IAuditable
    {
        private readonly List<Employee> _employees = new();

        public string Name { get; private set; } = string.Empty;
        public string? Code { get; private set; }
        public string? Description { get; private set; }
        public bool IsActive { get; private set; }

        /// <summary>
        /// Id do gerente responsável (se houver).
        /// </summary>
        public Guid? ManagerId { get; private set; }

        /// <summary>
        /// Navegação opcional para o Manager (especialização de Employee).
        /// </summary>
        public Manager? Manager { get; private set; }

        /// <summary>
        /// Coleção de funcionários lotados no departamento.
        /// </summary>
        public IReadOnlyCollection<Employee> Employees => _employees.AsReadOnly();

        // Auditoria
        public DateTime CreatedAt { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string? UpdatedBy { get; set; }

        // Construtor privado para EF
        private Department() { }

        public Department(string name, string? code, string? description, bool isActive = true)
        {
            UpdateInfo(name, code, description);
            IsActive = isActive;

            CreatedAt = DateTime.UtcNow;
        }

        public void UpdateInfo(string name, string? code, string? description)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Nome do departamento é obrigatório.", nameof(name));

            Name = name.Trim();
            Code = string.IsNullOrWhiteSpace(code) ? null : code.Trim().ToUpperInvariant();
            Description = string.IsNullOrWhiteSpace(description) ? null : description.Trim();

            UpdatedAt = DateTime.UtcNow;
        }

        public void Activate()
        {
            IsActive = true;
            UpdatedAt = DateTime.UtcNow;
        }

        public void Deactivate()
        {
            IsActive = false;
            UpdatedAt = DateTime.UtcNow;
        }

        public void AssignManager(Manager manager)
        {
            Manager = manager ?? throw new ArgumentNullException(nameof(manager));
            ManagerId = manager.Id;
            UpdatedAt = DateTime.UtcNow;
        }

        public void ClearManager()
        {
            Manager = null;
            ManagerId = null;
            UpdatedAt = DateTime.UtcNow;
        }

        public void AddEmployee(Employee employee)
        {
            if (employee is null)
                throw new ArgumentNullException(nameof(employee));

            if (_employees.All(e => e.Id != employee.Id))
            {
                _employees.Add(employee);
                UpdatedAt = DateTime.UtcNow;
            }
        }

        public void RemoveEmployee(Guid employeeId)
        {
            var toRemove = _employees.FirstOrDefault(e => e.Id == employeeId);
            if (toRemove is not null)
            {
                _employees.Remove(toRemove);
                UpdatedAt = DateTime.UtcNow;
            }
        }
    }
}
