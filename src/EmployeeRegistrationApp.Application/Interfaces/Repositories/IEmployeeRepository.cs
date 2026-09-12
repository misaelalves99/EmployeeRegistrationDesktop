using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EmployeeRegistrationApp.Domain.Entities;

namespace EmployeeRegistrationApp.Application.Interfaces.Repositories
{
    public interface IEmployeeRepository
    {
        Task<Employee?> GetByIdAsync(
            Guid id,
            CancellationToken cancellationToken = default);

        Task<IReadOnlyList<Employee>> GetAllAsync(
            CancellationToken cancellationToken = default);

        IQueryable<Employee> Query();

        Task AddAsync(
            Employee employee,
            CancellationToken cancellationToken = default);

        void Update(Employee employee);

        void Remove(Employee employee);
    }
}