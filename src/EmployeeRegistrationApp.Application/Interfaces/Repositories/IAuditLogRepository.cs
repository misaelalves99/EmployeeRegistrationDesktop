// src/EmployeeRegistrationApp.Application/Interfaces/Repositories/IAuditLogRepository.cs
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EmployeeRegistrationApp.Domain.Entities;

namespace EmployeeRegistrationApp.Application.Interfaces.Repositories
{
    /// <summary>
    /// Repositório de logs de auditoria.
    /// Usado para registrar ações importantes e consultar histórico.
    /// </summary>
    public interface IAuditLogRepository
    {
        Task AddAsync(AuditLog auditLog);

        /// <summary>
        /// Retorna os registros mais recentes (para dashboard, por exemplo).
        /// </summary>
        Task<IReadOnlyList<AuditLog>> GetRecentAsync(int take = 50);

        /// <summary>
        /// Retorna logs filtrados por usuário.
        /// </summary>
        Task<IReadOnlyList<AuditLog>> GetByUserAsync(string userId, int take = 100);
    }
}
