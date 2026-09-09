// src/EmployeeRegistrationApp.Application/Interfaces/Repositories/IReportTemplateRepository.cs
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EmployeeRegistrationApp.Domain.Entities;

namespace EmployeeRegistrationApp.Application.Interfaces.Repositories
{
    public interface IReportTemplateRepository
    {
        Task<IReadOnlyList<ReportTemplate>> GetAllAsync();
        Task<ReportTemplate?> GetByIdAsync(Guid id);
        Task<ReportTemplate?> GetByCodeAsync(string code);

        Task AddAsync(ReportTemplate template);
        void Update(ReportTemplate template);
        void Remove(ReportTemplate template);
    }
}
