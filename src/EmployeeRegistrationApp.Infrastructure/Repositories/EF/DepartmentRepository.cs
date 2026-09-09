// src/EmployeeRegistrationApp.Infrastructure/Repositories/EF/DepartmentRepository.cs
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
    public sealed class DepartmentRepository : IDepartmentRepository
    {
        private readonly AppDbContext _context;

        public DepartmentRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Department?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _context.Departments
                .AsNoTracking()
                .FirstOrDefaultAsync(d => d.Id == id, cancellationToken);
        }

        public async Task<IReadOnlyList<Department>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await _context.Departments
                .AsNoTracking()
                .OrderBy(d => d.Name)
                .ToListAsync(cancellationToken);
        }

        public async Task<IReadOnlyList<Department>> SearchByNameAsync(string search, CancellationToken cancellationToken = default)
        {
            search = (search ?? string.Empty).Trim();

            return await _context.Departments
                .AsNoTracking()
                .Where(d => d.Name.Contains(search))
                .OrderBy(d => d.Name)
                .ToListAsync(cancellationToken);
        }

        public async Task<bool> ExistsByNameAsync(string name, Guid? ignoreId = null, CancellationToken cancellationToken = default)
        {
            name = (name ?? string.Empty).Trim();

            return await _context.Departments
                .AsNoTracking()
                .AnyAsync(d => d.Name == name && (!ignoreId.HasValue || d.Id != ignoreId.Value), cancellationToken);
        }

        public IQueryable<Department> Query()
        {
            return _context.Departments.AsQueryable();
        }

        public async Task AddAsync(Department department, CancellationToken cancellationToken = default)
        {
            if (department is null) throw new ArgumentNullException(nameof(department));
            await _context.Departments.AddAsync(department, cancellationToken);
        }

        public Task UpdateAsync(Department department, CancellationToken cancellationToken = default)
        {
            if (department is null) throw new ArgumentNullException(nameof(department));
            _context.Departments.Update(department);
            return Task.CompletedTask;
        }

        public Task DeleteAsync(Department department, CancellationToken cancellationToken = default)
        {
            if (department is null) throw new ArgumentNullException(nameof(department));
            _context.Departments.Remove(department);
            return Task.CompletedTask;
        }
    }
}
