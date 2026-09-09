// src/EmployeeRegistrationApp.Application/DTOs/Positions/PositionDto.cs
using System;
using EmployeeRegistrationApp.Domain.Enums;

namespace EmployeeRegistrationApp.Application.DTOs.Positions
{
    public sealed class PositionDto
    {
        public Guid? Id { get; set; }

        public string Name { get; set; } = string.Empty;
        public string? Code { get; set; }
        public string? Description { get; set; }

        public PositionType Type { get; set; } = PositionType.Junior;
        public bool IsLeadership { get; set; }

        public decimal BaseSalary { get; set; }
        public string BaseSalaryCurrency { get; set; } = "BRL";

        public bool IsActive { get; set; } = true;

        public Guid? DepartmentId { get; set; }
        public string? DepartmentName { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime? LastModifiedAt { get; set; }
    }
}
