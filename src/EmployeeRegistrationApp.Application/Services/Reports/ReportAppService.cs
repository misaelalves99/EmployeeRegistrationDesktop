// src/EmployeeRegistrationApp.Application/Services/Reports/ReportAppService.cs
using AutoMapper;
using EmployeeRegistrationApp.Application.DTOs.Dashboard;
using EmployeeRegistrationApp.Application.DTOs.Reports;
using EmployeeRegistrationApp.Application.Interfaces.Repositories;
using EmployeeRegistrationApp.Application.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeRegistrationApp.Application.Services.Reports
{
    public sealed class ReportAppService : IReportAppService
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IReportTemplateRepository? _reportTemplateRepository;
        private readonly IMapper _mapper;

        public ReportAppService(
            IEmployeeRepository employeeRepository,
            IMapper mapper,
            IReportTemplateRepository? reportTemplateRepository = null)
        {
            _employeeRepository = employeeRepository ?? throw new ArgumentNullException(nameof(employeeRepository));
            _reportTemplateRepository = reportTemplateRepository;
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<DashboardSummaryDto> GetDashboardSummaryAsync(DateTime? referenceDate = null)
        {
            var employees = await _employeeRepository.GetAllAsync();

            var total = employees.Count;
            var active = employees.Count(e => e.IsActive);
            var inactive = total - active;

            var endDate = referenceDate ?? DateTime.UtcNow;
            var startDate = endDate.AddDays(-30);

            var newAdmissions = employees.Count(e => e.HireDate >= startDate && e.HireDate <= endDate);

            var departmentsCount = employees
                .Where(e => e.Department != null)
                .Select(e => e.Department!.Id)
                .Distinct()
                .Count();

            var positionsCount = employees
                .Where(e => e.Position != null)
                .Select(e => e.Position!.Id)
                .Distinct()
                .Count();

            return new DashboardSummaryDto
            {
                TotalEmployees = total,
                ActiveEmployees = active,
                InactiveEmployees = inactive,
                NewAdmissionsLast30Days = newAdmissions,
                DepartmentsCount = departmentsCount,
                PositionsCount = positionsCount
            };
        }

        public async Task<IReadOnlyList<HeadcountByDepartmentDto>> GetHeadcountByDepartmentAsync(
            DateTime? startDate = null,
            DateTime? endDate = null)
        {
            // ✅ Sem banco: GetAllAsync já devolve employees com Department preenchido (via seed / assign)
            var employees = await _employeeRepository.GetAllAsync();

            if (startDate.HasValue || endDate.HasValue)
            {
                var from = startDate ?? DateTime.MinValue;
                var to = endDate ?? DateTime.MaxValue;

                employees = employees
                    .Where(e => e.HireDate >= from && e.HireDate <= to)
                    .ToList();
            }

            var grouped = employees
                .GroupBy(e => e.Department) // pode ser null
                .Select(g =>
                {
                    var dept = g.Key;
                    var activeCount = g.Count(x => x.IsActive);
                    var inactiveCount = g.Count - activeCount;

                    return new HeadcountByDepartmentDto
                    {
                        DepartmentId = dept?.Id ?? Guid.Empty,
                        DepartmentName = dept?.Name ?? "Sem departamento",
                        DepartmentCode = dept?.Code,
                        ActiveEmployees = activeCount,
                        InactiveEmployees = inactiveCount
                    };
                })
                .OrderBy(x => x.DepartmentName)
                .ToList();

            return grouped;
        }

        public async Task<SalarySummaryDto> GetSalarySummaryAsync(
            DateTime? startDate = null,
            DateTime? endDate = null)
        {
            var employees = await _employeeRepository.GetAllAsync();

            if (startDate.HasValue || endDate.HasValue)
            {
                var from = startDate ?? DateTime.MinValue;
                var to = endDate ?? DateTime.MaxValue;

                employees = employees
                    .Where(e => e.HireDate >= from && e.HireDate <= to)
                    .ToList();
            }

            if (employees.Count == 0)
            {
                return new SalarySummaryDto
                {
                    TotalSalary = 0m,
                    AverageSalary = 0m,
                    MinSalary = 0m,
                    MaxSalary = 0m,
                    EmployeesCount = 0,
                    DepartmentName = "—"
                };
            }

            var salaries = employees.Select(e => e.Salary.Amount).ToList();

            return new SalarySummaryDto
            {
                DepartmentName = "Geral",
                EmployeesCount = employees.Count,
                TotalSalary = salaries.Sum(),
                AverageSalary = salaries.Average(),
                MinSalary = salaries.Min(),
                MaxSalary = salaries.Max()
            };
        }
    }
}
