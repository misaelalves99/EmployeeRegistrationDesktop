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
            services.AddScoped<IUnitOfWork, InMemoryUnitOfWork>();

            return services;
        }

        private sealed class NoOpUnitOfWork : IUnitOfWork, IDisposable
        {
            public Task<int> SaveChangesAsync()
                => Task.FromResult(0);

            public void Dispose()
            {
                // no-op
            }
        }

        /// <summary>
        /// Unit of work for the in-memory persistence mode.
        /// In-memory repositories apply changes immediately, so commit is a no-op.
        /// </summary>
        private sealed class InMemoryUnitOfWork : IUnitOfWork
        {
            public Task<int> SaveChangesAsync() => Task.FromResult(0);

            public void Dispose()
            {
                // No resources are owned by the in-memory unit of work.
            }
        }
    }
}
