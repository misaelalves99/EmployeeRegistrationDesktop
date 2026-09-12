// src/EmployeeRegistrationApp.Application/Interfaces/Repositories/IUserAccountRepository.cs
using System;
using System.Threading.Tasks;
using EmployeeRegistrationApp.Domain.Users;

namespace EmployeeRegistrationApp.Application.Interfaces.Repositories
{
    /// <summary>
    /// Repositório de contas de usuário (login, credenciais, vinculação ao colaborador).
    /// </summary>
    public interface IUserAccountRepository
    {
        Task<UserAccount?> GetByIdAsync(Guid id);

        Task<UserAccount?> GetByEmailAsync(string email);

        Task<UserAccount?> GetByUserNameAsync(string userName);

        Task AddAsync(UserAccount userAccount);

        Task UpdateAsync(UserAccount userAccount);

        Task<bool> ExistsByEmailAsync(string email, Guid? ignoreId = null);

        Task<bool> ExistsByUserNameAsync(string userName, Guid? ignoreId = null);
    }
}
