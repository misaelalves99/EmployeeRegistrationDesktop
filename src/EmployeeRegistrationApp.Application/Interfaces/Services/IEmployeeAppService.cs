using System;
using System.Threading.Tasks;
using EmployeeRegistrationApp.Application.DTOs.Common;
using EmployeeRegistrationApp.Application.DTOs.Employees;
using EmployeeRegistrationApp.Domain.Enums;

namespace EmployeeRegistrationApp.Application.Interfaces.Services
{
    public interface IEmployeeAppService
    {
        Task<PagedResultDto<EmployeeListItemDto>> GetPagedAsync(
            int page,
            int pageSize,
            string? search = null,
            Guid? departmentId = null,
            EmploymentStatus? status = null);

        Task<EmployeeDetailsDto?> GetByIdAsync(Guid id);

        Task<EmployeeDetailsDto> CreateAsync(EmployeeDto dto);

        Task<EmployeeDetailsDto?> UpdateAsync(Guid id, EmployeeDto dto);

        Task<bool> DeleteAsync(Guid id);

        Task<bool> DeactivateAsync(Guid id);

        Task<bool> ReactivateAsync(Guid id);
    }
}
