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

        public async Task AddAsync(AuditLog auditLog)
        {
            if (auditLog is null) throw new ArgumentNullException(nameof(auditLog));

            await _context.AuditLogs.AddAsync(auditLog);
        }

        /// <summary>
        /// Retorna os logs mais recentes (por padrão 100).
        /// </summary>
        public async Task<IReadOnlyList<AuditLog>> GetRecentAsync(int take = 50)
        {
            if (take <= 0) take = 50;

            return await _context.AuditLogs
                .AsNoTracking()
                .OrderByDescending(l => l.Timestamp)
                .Take(take)
                .ToListAsync();
        }

        public async Task<IReadOnlyList<AuditLog>> GetByUserAsync(string userId, int take = 100)
        {
            if (take <= 0) take = 100;

            userId = (userId ?? string.Empty).Trim();

            IQueryable<AuditLog> query = _context.AuditLogs.AsNoTracking();

            if (Guid.TryParse(userId, out var parsedUserId))
            {
                query = query.Where(x => x.UserId == parsedUserId || x.UserName == userId);
            }
            else
            {
                query = query.Where(x => x.UserName == userId);
            }

            return await query
                .OrderByDescending(x => x.Timestamp)
                .Take(take)
                .ToListAsync();
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
