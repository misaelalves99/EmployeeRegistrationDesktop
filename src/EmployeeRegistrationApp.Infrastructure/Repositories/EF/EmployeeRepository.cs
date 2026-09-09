// src/EmployeeRegistrationApp.Infrastructure/Repositories/EF/EmployeeRepository.cs
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
    public sealed class EmployeeRepository : IEmployeeRepository
    {
        private readonly AppDbContext _context;

        public EmployeeRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Employee?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _context.Employees
                .AsNoTracking()
                .Include(e => e.Department)
                .Include(e => e.Position)
                .FirstOrDefaultAsync(e => e.Id == id, cancellationToken);
        }

        public async Task<IReadOnlyList<Employee>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await _context.Employees
                .AsNoTracking()
                .Include(e => e.Department)
                .Include(e => e.Position)
                // ✅ não usar FullName (ignorado pelo EF). Ordena por First/Last.
                .OrderBy(e => e.FirstName)
                .ThenBy(e => e.LastName)
                .ToListAsync(cancellationToken);
        }

        public IQueryable<Employee> Query()
        {
            return _context.Employees
                .Include(e => e.Department)
                .Include(e => e.Position)
                .AsQueryable();
        }

        public async Task AddAsync(Employee employee, CancellationToken cancellationToken = default)
        {
            if (employee is null) throw new ArgumentNullException(nameof(employee));
            await _context.Employees.AddAsync(employee, cancellationToken);
        }

        public void Update(Employee employee)
        {
            if (employee is null) throw new ArgumentNullException(nameof(employee));
            _context.Employees.Update(employee);
        }

        public void Remove(Employee employee)
        {
            if (employee is null) throw new ArgumentNullException(nameof(employee));
            _context.Employees.Remove(employee);
        }
    }
}
