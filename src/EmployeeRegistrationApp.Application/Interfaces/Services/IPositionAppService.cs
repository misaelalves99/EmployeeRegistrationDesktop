// src/EmployeeRegistrationApp.Application/Interfaces/Services/IPositionAppService.cs
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EmployeeRegistrationApp.Application.DTOs.Positions;

namespace EmployeeRegistrationApp.Application.Interfaces.Services
{
    /// <summary>
    /// Serviço de aplicação para cargos/posições.
    /// Orquestra regras de negócio + repositórios.
    /// </summary>
    public interface IPositionAppService
    {
        /// <summary>
        /// Lista cargos, opcionalmente filtrando por termo de busca.
        /// </summary>
        Task<IReadOnlyList<PositionDto>> GetAllAsync(string? searchTerm = null);

        Task<PositionDto?> GetByIdAsync(Guid id);

        Task<IReadOnlyList<PositionDto>> GetByDepartmentAsync(Guid departmentId);

        Task<PositionDto> CreateAsync(PositionDto dto);

        Task<PositionDto> UpdateAsync(PositionDto dto);

        Task DeleteAsync(Guid id);
    }
}
