// src/EmployeeRegistrationApp.Infrastructure/Repositories/EF/AuditLogRepository.cs
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EmployeeRegistrationApp.Application.Interfaces.Repositories;
using EmployeeRegistrationApp.Domain.Entities;
using EmployeeRegistrationApp.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace EmployeeRegistrationApp.Infrastructure.Repositories.EF
{
    /// <summary>
    /// Repositório EF Core para logs de auditoria.
    /// Normalmente apenas escrita + consultas para relatórios.
    /// </summary>
    public sealed class AuditLogRepository : IAuditLogRepository
    {
        private readonly AppDbContext _context;

        public AuditLogRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(AuditLog auditLog, CancellationToken cancellationToken = default)
        {
            if (auditLog is null) throw new ArgumentNullException(nameof(auditLog));

            await _context.AuditLogs.AddAsync(auditLog, cancellationToken);
        }

        /// <summary>
        /// Retorna os logs mais recentes (por padrão 100).
        /// </summary>
        public async Task<IReadOnlyList<AuditLog>> GetRecentAsync(
            int take = 100,
            CancellationToken cancellationToken = default)
        {
            if (take <= 0) take = 100;

            return await _context.AuditLogs
                .AsNoTracking()
                .OrderByDescending(l => l.TimestampUtc)
                .Take(take)
                .ToListAsync(cancellationToken);
        }

        /// <summary>
        /// Exposição de query para cenários de relatórios customizados.
        /// </summary>
        public IQueryable<AuditLog> Query()
        {
            return _context.AuditLogs.AsNoTracking();
        }
    }
}
