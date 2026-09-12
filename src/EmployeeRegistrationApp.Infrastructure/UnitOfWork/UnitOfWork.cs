// src/EmployeeRegistrationApp.Infrastructure/UnitOfWork/UnitOfWork.cs
using System;
using System.Threading;
using System.Threading.Tasks;
using EmployeeRegistrationApp.Application.Interfaces.Services;
using EmployeeRegistrationApp.Infrastructure.Persistence;

namespace EmployeeRegistrationApp.Infrastructure.UnitOfWork
{
    /// <summary>
    /// Implementação padrão de Unit of Work usando o AppDbContext (EF Core).
    /// </summary>
    public sealed class UnitOfWork : IUnitOfWork, IDisposable
    {
        private readonly AppDbContext _context;

        public UnitOfWork(AppDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public Task<int> SaveChangesAsync()
        {
            return _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
