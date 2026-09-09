// src/EmployeeRegistrationApp.Application/Interfaces/Repositories/IDepartmentRepository.cs
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EmployeeRegistrationApp.Domain.Entities;

namespace EmployeeRegistrationApp.Application.Interfaces.Repositories
{
    public interface IDepartmentRepository
    {
        Task<Department?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<IReadOnlyList<Department>> GetAllAsync(CancellationToken cancellationToken = default);

        Task<IReadOnlyList<Department>> SearchByNameAsync(string search, CancellationToken cancellationToken = default);
        Task<bool> ExistsByNameAsync(string name, Guid? ignoreId = null, CancellationToken cancellationToken = default);

        IQueryable<Department> Query();

        Task AddAsync(Department department, CancellationToken cancellationToken = default);
        Task UpdateAsync(Department department, CancellationToken cancellationToken = default);
        Task DeleteAsync(Department department, CancellationToken cancellationToken = default);
    }
}
