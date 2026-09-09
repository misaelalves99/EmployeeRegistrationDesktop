// src/EmployeeRegistrationApp.Infrastructure/Repositories/InMemory/InMemoryDepartmentRepository.cs
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EmployeeRegistrationApp.Application.Interfaces.Repositories;
using EmployeeRegistrationApp.Domain.Entities;

namespace EmployeeRegistrationApp.Infrastructure.Repositories.InMemory
{
    public sealed class InMemoryDepartmentRepository : IDepartmentRepository
    {
        private readonly InMemoryDatabase _database;

        public InMemoryDepartmentRepository(InMemoryDatabase database)
        {
            _database = database;
        }

        public Task<Department?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var department = _database.Departments.FirstOrDefault(d => d.Id == id);
            return Task.FromResult(department);
        }

        public Task<IReadOnlyList<Department>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            IReadOnlyList<Department> result = _database.Departments
                .OrderBy(d => d.Name)
                .ToList();

            return Task.FromResult(result);
        }

        public Task<IReadOnlyList<Department>> SearchByNameAsync(string search, CancellationToken cancellationToken = default)
        {
            search = (search ?? string.Empty).Trim();

            IReadOnlyList<Department> result = _database.Departments
                .Where(d => d.Name.Contains(search, StringComparison.OrdinalIgnoreCase))
                .OrderBy(d => d.Name)
                .ToList();

            return Task.FromResult(result);
        }

        public Task<bool> ExistsByNameAsync(string name, Guid? ignoreId = null, CancellationToken cancellationToken = default)
        {
            name = (name ?? string.Empty).Trim();

            var exists = _database.Departments.Any(d =>
                string.Equals(d.Name, name, StringComparison.OrdinalIgnoreCase) &&
                (!ignoreId.HasValue || d.Id != ignoreId.Value));

            return Task.FromResult(exists);
        }

        public IQueryable<Department> Query() => _database.Departments.AsQueryable();

        public Task AddAsync(Department department, CancellationToken cancellationToken = default)
        {
            if (department is null) throw new ArgumentNullException(nameof(department));
            _database.Departments.Add(department);
            return Task.CompletedTask;
        }

        public Task UpdateAsync(Department department, CancellationToken cancellationToken = default)
        {
            if (department is null) throw new ArgumentNullException(nameof(department));

            var index = _database.Departments.FindIndex(d => d.Id == department.Id);
            if (index >= 0)
                _database.Departments[index] = department;

            return Task.CompletedTask;
        }

        public Task DeleteAsync(Department department, CancellationToken cancellationToken = default)
        {
            if (department is null) throw new ArgumentNullException(nameof(department));
            _database.Departments.RemoveAll(d => d.Id == department.Id);
            return Task.CompletedTask;
        }
    }
}
