// src/EmployeeRegistrationApp.Infrastructure/Repositories/EF/ReportTemplateRepository.cs
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EmployeeRegistrationApp.Application.Interfaces.Repositories;
using EmployeeRegistrationApp.Domain.Entities;
using EmployeeRegistrationApp.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace EmployeeRegistrationApp.Infrastructure.Repositories.EF
{
    public sealed class ReportTemplateRepository : IReportTemplateRepository
    {
        private readonly AppDbContext _context;

        public ReportTemplateRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IReadOnlyList<ReportTemplate>> GetAllAsync()
            => await _context.ReportTemplates.AsNoTracking().ToListAsync();

        public async Task<ReportTemplate?> GetByIdAsync(Guid id)
            => await _context.ReportTemplates.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);

        public async Task<ReportTemplate?> GetByCodeAsync(string code)
        {
            if (string.IsNullOrWhiteSpace(code)) return null;
            var normalized = code.Trim().ToUpperInvariant();

            return await _context.ReportTemplates
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Code == normalized);
        }

        public async Task AddAsync(ReportTemplate template)
            => await _context.ReportTemplates.AddAsync(template);

        public void Update(ReportTemplate template)
            => _context.ReportTemplates.Update(template);

        public void Remove(ReportTemplate template)
            => _context.ReportTemplates.Remove(template);
    }
}
