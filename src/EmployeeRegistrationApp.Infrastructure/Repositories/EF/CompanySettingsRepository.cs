// src/EmployeeRegistrationApp.Infrastructure/Repositories/EF/CompanySettingsRepository.cs
using System;
using System.Threading;
using System.Threading.Tasks;
using EmployeeRegistrationApp.Application.Interfaces.Repositories;
using EmployeeRegistrationApp.Domain.Entities;
using EmployeeRegistrationApp.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace EmployeeRegistrationApp.Infrastructure.Repositories.EF
{
    public sealed class CompanySettingsRepository : ICompanySettingsRepository
    {
        private readonly AppDbContext _context;

        public CompanySettingsRepository(AppDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<CompanySettings?> GetCurrentAsync(CancellationToken cancellationToken = default)
        {
            // geralmente só existe um registro (pega o primeiro)
            return await _context.CompanySettings
                .AsNoTracking()
                .FirstOrDefaultAsync(cancellationToken);
        }

        public async Task AddAsync(CompanySettings settings, CancellationToken cancellationToken = default)
        {
            if (settings is null) throw new ArgumentNullException(nameof(settings));
            await _context.CompanySettings.AddAsync(settings, cancellationToken);
        }

        public Task UpdateAsync(CompanySettings settings, CancellationToken cancellationToken = default)
        {
            if (settings is null) throw new ArgumentNullException(nameof(settings));
            _context.CompanySettings.Update(settings);
            return Task.CompletedTask;
        }
    }
}
