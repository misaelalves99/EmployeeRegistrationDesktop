// src/EmployeeRegistrationApp.Application/Interfaces/Services/IReportAppService.cs
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EmployeeRegistrationApp.Application.DTOs.Dashboard;
using EmployeeRegistrationApp.Application.DTOs.Reports;

namespace EmployeeRegistrationApp.Application.Interfaces.Services
{
    /// <summary>
    /// Serviço de aplicação responsável pelos relatórios e visão de dashboard.
    /// Agrupa métricas como headcount por departamento e resumo de salários.
    /// </summary>
    public interface IReportAppService
    {
        /// <summary>
        /// Retorna o resumo principal usado no dashboard:
        /// total de colaboradores, ativos/inativos, novas admissões, etc.
        /// </summary>
        Task<DashboardSummaryDto> GetDashboardSummaryAsync(DateTime? referenceDate = null);

        /// <summary>
        /// Retorna o headcount agrupado por departamento em um intervalo de datas.
        /// Se datas forem nulas, considera o período completo.
        /// </summary>
        Task<IReadOnlyList<HeadcountByDepartmentDto>> GetHeadcountByDepartmentAsync(
            DateTime? startDate = null,
            DateTime? endDate = null);

        /// <summary>
        /// Retorna o resumo de salários no período (folha total, média, mínimo, máximo).
        /// </summary>
        Task<SalarySummaryDto> GetSalarySummaryAsync(
            DateTime? startDate = null,
            DateTime? endDate = null);
    }
}
