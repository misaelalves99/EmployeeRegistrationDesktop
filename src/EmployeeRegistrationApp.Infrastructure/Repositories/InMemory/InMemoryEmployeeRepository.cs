// src/EmployeeRegistrationApp.Infrastructure/Repositories/InMemory/InMemoryEmployeeRepository.cs
using EmployeeRegistrationApp.Application.Interfaces.Repositories;
using EmployeeRegistrationApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace EmployeeRegistrationApp.Infrastructure.Repositories.InMemory
{
    public sealed class InMemoryEmployeeRepository : IEmployeeRepository
    {
        private readonly InMemoryDatabase _database;

        public InMemoryEmployeeRepository(InMemoryDatabase database)
        {
            _database = database;
        }

        public Task<Employee?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var employee = _database.Employees.FirstOrDefault(e => e.Id == id);
            return Task.FromResult(employee);
        }

        public Task<IReadOnlyList<Employee>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            IReadOnlyList<Employee> result = _database.Employees
                .OrderBy(e => e.FullName)
                .ToList();

            return Task.FromResult(result);
        }

        public IQueryable<Employee> Query()
        {
            return _database.Employees.AsQueryable();
        }

        public Task AddAsync(Employee employee, CancellationToken cancellationToken = default)
        {
            if (employee is null) throw new ArgumentNullException(nameof(employee));
            _database.Employees.Add(employee);
            return Task.CompletedTask;
        }

        public void Update(Employee employee)
        {
            if (employee is null) throw new ArgumentNullException(nameof(employee));

            var index = _database.Employees.FindIndex(e => e.Id == employee.Id);
            if (index >= 0) _database.Employees[index] = employee;
        }

        public void Remove(Employee employee)
        {
            if (employee is null) throw new ArgumentNullException(nameof(employee));
            _database.Employees.RemoveAll(e => e.Id == employee.Id);
        }
    }
}
