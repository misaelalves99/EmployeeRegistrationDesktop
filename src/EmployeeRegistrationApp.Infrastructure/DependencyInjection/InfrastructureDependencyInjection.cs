// src/EmployeeRegistrationApp.Infrastructure/DependencyInjection/InfrastructureDependencyInjection.cs
using System;
using System.Threading;
using System.Threading.Tasks;
using EmployeeRegistrationApp.Application.Interfaces.Repositories;
using EmployeeRegistrationApp.Application.Interfaces.Services;
using EmployeeRegistrationApp.Infrastructure.Repositories.InMemory;
using Microsoft.Extensions.DependencyInjection;

namespace EmployeeRegistrationApp.Infrastructure.DependencyInjection
{
    public static class InfrastructureDependencyInjection
    {
        public static IServiceCollection AddInMemoryPersistence(this IServiceCollection services)
        {
            services.AddSingleton<InMemoryDatabase>();

            // ✅ Repositórios InMemory
            services.AddScoped<IDepartmentRepository, InMemoryDepartmentRepository>();
            services.AddScoped<IEmployeeRepository, InMemoryEmployeeRepository>();
            services.AddScoped<IPositionRepository, InMemoryPositionRepository>();
            services.AddScoped<ICompanySettingsRepository, InMemoryCompanySettingsRepository>();
            services.AddScoped<IAuditLogRepository, InMemoryAuditLogRepository>();
            services.AddScoped<IUserAccountRepository, InMemoryUserAccountRepository>();

            // ✅ UnitOfWork no-op (sem EF)
            services.AddScoped<IUnitOfWork, NoOpUnitOfWork>();

            return services;
        }

        private sealed class NoOpUnitOfWork : IUnitOfWork, IDisposable
        {
            public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
                => Task.FromResult(0);

            public void Dispose()
            {
                // no-op
            }
        }
    }
}
