// src/EmployeeRegistrationApp.Infrastructure/Repositories/InMemory/InMemoryAuditLogRepository.cs
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EmployeeRegistrationApp.Application.Interfaces.Repositories;
using EmployeeRegistrationApp.Domain.Entities;

namespace EmployeeRegistrationApp.Infrastructure.Repositories.InMemory
{
    public sealed class InMemoryAuditLogRepository : IAuditLogRepository
    {
        private readonly InMemoryDatabase _database;

        public InMemoryAuditLogRepository(InMemoryDatabase database)
        {
            _database = database ?? throw new ArgumentNullException(nameof(database));
        }

        public Task AddAsync(AuditLog auditLog)
        {
            if (auditLog is null) throw new ArgumentNullException(nameof(auditLog));
            _database.AuditLogs.Add(auditLog);
            return Task.CompletedTask;
        }

        public Task<IReadOnlyList<AuditLog>> GetRecentAsync(int take = 50)
        {
            if (take <= 0) take = 50;

            IReadOnlyList<AuditLog> result = _database.AuditLogs
                .OrderByDescending(x => x.Timestamp)
                .Take(take)
                .ToList();

            return Task.FromResult(result);
        }

        public Task<IReadOnlyList<AuditLog>> GetByUserAsync(string userId, int take = 100)
        {
            if (take <= 0) take = 100;

            userId = (userId ?? string.Empty).Trim();

            // Suporta tanto Guid (string) quanto username em cenários simples
            IReadOnlyList<AuditLog> result = _database.AuditLogs
                .Where(x =>
                    (x.UserId.HasValue && string.Equals(x.UserId.Value.ToString(), userId, StringComparison.OrdinalIgnoreCase)) ||
                    (!string.IsNullOrWhiteSpace(x.UserName) && string.Equals(x.UserName, userId, StringComparison.OrdinalIgnoreCase)))
                .OrderByDescending(x => x.Timestamp)
                .Take(take)
                .ToList();

            return Task.FromResult(result);
        }
    }
}
