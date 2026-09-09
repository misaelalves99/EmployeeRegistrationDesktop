// src/EmployeeRegistrationApp.Infrastructure/Repositories/InMemory/InMemoryCompanySettingsRepository.cs
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EmployeeRegistrationApp.Application.Interfaces.Repositories;
using EmployeeRegistrationApp.Domain.Entities;

namespace EmployeeRegistrationApp.Infrastructure.Repositories.InMemory
{
    public sealed class InMemoryCompanySettingsRepository : ICompanySettingsRepository
    {
        private readonly InMemoryDatabase _database;

        public InMemoryCompanySettingsRepository(InMemoryDatabase database)
        {
            _database = database ?? throw new ArgumentNullException(nameof(database));
        }

        public Task<CompanySettings?> GetCurrentAsync(CancellationToken cancellationToken = default)
        {
            var current = _database.CompanySettings.FirstOrDefault();
            return Task.FromResult(current);
        }

        public Task AddAsync(CompanySettings settings, CancellationToken cancellationToken = default)
        {
            if (settings is null) throw new ArgumentNullException(nameof(settings));

            // Mantém 1 registro “ativo” (modo simples)
            _database.CompanySettings.Clear();
            _database.CompanySettings.Add(settings);

            return Task.CompletedTask;
        }

        public Task UpdateAsync(CompanySettings settings, CancellationToken cancellationToken = default)
        {
            if (settings is null) throw new ArgumentNullException(nameof(settings));

            var existing = _database.CompanySettings.FirstOrDefault();
            if (existing is null)
            {
                _database.CompanySettings.Add(settings);
                return Task.CompletedTask;
            }

            // substitui a referência (simples e eficaz em memória)
            _database.CompanySettings[0] = settings;
            return Task.CompletedTask;
        }
    }
}
