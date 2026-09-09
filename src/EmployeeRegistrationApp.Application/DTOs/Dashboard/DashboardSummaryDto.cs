// src/EmployeeRegistrationApp.Application/DTOs/Dashboard/DashboardSummaryDto.cs
namespace EmployeeRegistrationApp.Application.DTOs.Dashboard
{
    /// <summary>
    /// DTO com os números principais do dashboard.
    /// </summary>
    public sealed class DashboardSummaryDto
    {
        public int TotalEmployees { get; set; }
        public int ActiveEmployees { get; set; }
        public int InactiveEmployees { get; set; }

        // ✅ usado no MAUI DashboardViewModel
        public int DepartmentsCount { get; set; }
        public int PositionsCount { get; set; }

        public int NewAdmissionsLast30Days { get; set; }

        // ✅ compat: ReportsViewModel antigo usa TotalDepartments
        public int TotalDepartments
        {
            get => DepartmentsCount;
            set => DepartmentsCount = value;
        }
    }
}
