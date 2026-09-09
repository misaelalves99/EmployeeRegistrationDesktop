// src/EmployeeRegistrationApp.Application/Interfaces/Services/IUnitOfWork.cs
using System;
using System.Threading.Tasks;

namespace EmployeeRegistrationApp.Application.Interfaces.Services
{
    /// <summary>
    /// Abstração de unidade de trabalho.
    /// Centraliza o commit das alterações de repositórios.
    /// </summary>
    public interface IUnitOfWork : IDisposable
    {
        /// <summary>
        /// Persiste todas as alterações pendentes no contexto atual.
        /// Retorna o número de entidades afetadas.
        /// </summary>
        Task<int> SaveChangesAsync();
    }
}
