// src/EmployeeRegistrationApp.Domain/Entities/Position.cs
using System;
using System.Collections.Generic;
using EmployeeRegistrationApp.Domain.Base;
using EmployeeRegistrationApp.Domain.Enums;
using EmployeeRegistrationApp.Domain.Interfaces;
using EmployeeRegistrationApp.Domain.ValueObjects;

namespace EmployeeRegistrationApp.Domain.Entities
{
    public sealed class Position : AggregateRoot, IAuditable
    {
        private readonly List<Employee> _employees = new();

        public string Name { get; private set; } = string.Empty;
        public string? Code { get; private set; }
        public string? Description { get; private set; }

        public PositionType PositionType { get; private set; }

        public Money? BaseSalary { get; private set; }

        public bool IsLeadership { get; private set; }
        public bool IsActive { get; private set; }

        public Guid? DefaultDepartmentId { get; private set; }
        public Department? DefaultDepartment { get; private set; }

        /// <summary>
        /// ✅ Para suportar EmployeeConfiguration/relacionamentos: WithMany(p => p.Employees)
        /// </summary>
        public IReadOnlyCollection<Employee> Employees => _employees.AsReadOnly();

        // Auditoria (IAuditable)
        public DateTime CreatedAt { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string? UpdatedBy { get; set; }

        public string Title => Name;

        private Position() { } // EF

        public Position(
            string name,
            PositionType positionType,
            bool isLeadership,
            Money? baseSalary,
            string? code = null,
            string? description = null,
            bool isActive = true)
        {
            UpdateInfo(name, positionType, isLeadership, baseSalary, code, description);
            IsActive = isActive;
            CreatedAt = DateTime.UtcNow;
        }

        public void UpdateInfo(
            string name,
            PositionType positionType,
            bool isLeadership,
            Money? baseSalary,
            string? code,
            string? description)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Nome do cargo é obrigatório.", nameof(name));

            Name = name.Trim();
            PositionType = positionType;
            IsLeadership = isLeadership;
            BaseSalary = baseSalary;
            Code = string.IsNullOrWhiteSpace(code) ? null : code.Trim().ToUpperInvariant();
            Description = string.IsNullOrWhiteSpace(description) ? null : description.Trim();
            Touch();
        }

        public void Activate()
        {
            IsActive = true;
            Touch();
        }

        public void Deactivate()
        {
            IsActive = false;
            Touch();
        }

        public void AssignDefaultDepartment(Department? department)
        {
            DefaultDepartment = department;
            DefaultDepartmentId = department?.Id;
            Touch();
        }

        private void Touch()
        {
            UpdatedAt = DateTime.UtcNow;
        }

        public void TouchBy(string userName)
        {
            Touch();
            UpdatedBy = userName;
        }
    }
}
