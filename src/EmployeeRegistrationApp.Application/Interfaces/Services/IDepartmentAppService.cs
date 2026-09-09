// src/EmployeeRegistrationApp.Application/Interfaces/Services/IDepartmentAppService.cs
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EmployeeRegistrationApp.Application.DTOs.Departments;

namespace EmployeeRegistrationApp.Application.Interfaces.Services
{
    /// <summary>
    /// Aplicação de departamentos (RH, TI, Financeiro, etc.).
    /// </summary>
    public interface IDepartmentAppService
    {
        Task<IReadOnlyList<DepartmentDto>> GetAllAsync();

        Task<DepartmentDto?> GetByIdAsync(Guid id);

        Task<DepartmentDto> CreateAsync(DepartmentDto dto);

        Task<DepartmentDto> UpdateAsync(DepartmentDto dto);

        Task DeleteAsync(Guid id);
    }
}
