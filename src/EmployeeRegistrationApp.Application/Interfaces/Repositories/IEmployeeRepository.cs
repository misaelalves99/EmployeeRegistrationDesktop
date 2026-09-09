// src/EmployeeRegistrationApp.Application/Interfaces/Repositories/IEmployeeRepository.cs
using System;
using System.Threading;
using System.Threading.Tasks;
using EmployeeRegistrationApp.Application.DTOs.Common;
using EmployeeRegistrationApp.Application.DTOs.Employees;

namespace EmployeeRegistrationApp.Application.Interfaces.Repositories
{
    public interface IEmployeeRepository
    {
        Task<EmployeeDetailsDto?> GetByIdAsync(Guid id, CancellationToken ct = default);

        Task<PagedResultDto<EmployeeListItemDto>> GetPagedAsync(
            int page,
            int pageSize,
            string? search,
            bool? isActive,
            Guid? departmentId,
            CancellationToken ct = default
        );

        Task AddAsync(EmployeeDetailsDto employee, CancellationToken ct = default);
        Task UpdateAsync(EmployeeDetailsDto employee, CancellationToken ct = default);
        Task DeleteAsync(Guid id, CancellationToken ct = default);
    }
}
