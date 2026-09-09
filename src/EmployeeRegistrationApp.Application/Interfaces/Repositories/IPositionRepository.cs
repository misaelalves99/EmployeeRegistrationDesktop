// src/EmployeeRegistrationApp.Application/Interfaces/Repositories/IPositionRepository.cs
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EmployeeRegistrationApp.Domain.Entities;

namespace EmployeeRegistrationApp.Application.Interfaces.Repositories
{
    /// <summary>
    /// Repositório de cargos/posições (Position).
    /// Responsável apenas por acesso a dados.
    /// </summary>
    public interface IPositionRepository
    {
        Task<Position?> GetByIdAsync(Guid id);

        Task<IReadOnlyList<Position>> GetAllAsync();

        /// <summary>
        /// Retorna os cargos vinculados a um departamento específico.
        /// </summary>
        Task<IReadOnlyList<Position>> GetByDepartmentAsync(Guid departmentId);

        /// <summary>
        /// Verifica se já existe um cargo com determinado título.
        /// ignoreId permite ignorar um registro ao editar.
        /// </summary>
        Task<bool> ExistsByTitleAsync(string title, Guid? ignoreId = null);

        Task AddAsync(Position position);

        Task UpdateAsync(Position position);

        Task DeleteAsync(Position position);

        Task<int> CountAsync();
    }
}
