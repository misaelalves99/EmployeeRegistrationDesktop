// src/EmployeeRegistrationApp.Application/DTOs/Reports/HeadcountByDepartmentDto.cs
using System;

namespace EmployeeRegistrationApp.Application.DTOs.Reports
{
    public sealed class HeadcountByDepartmentDto
    {
        public Guid? DepartmentId { get; set; }
        public string DepartmentName { get; set; } = string.Empty;
        public string? DepartmentCode { get; set; }

        public int ActiveEmployees { get; set; }
        public int InactiveEmployees { get; set; }

        public int TotalEmployees { get; set; }
    }
}
