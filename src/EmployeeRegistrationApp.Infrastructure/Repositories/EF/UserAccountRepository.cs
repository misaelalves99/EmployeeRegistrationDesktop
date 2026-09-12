// src/EmployeeRegistrationApp.Infrastructure/Repositories/EF/UserAccountRepository.cs
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

using EmployeeRegistrationApp.Application.Interfaces.Repositories;
using EmployeeRegistrationApp.Domain.Users;
using EmployeeRegistrationApp.Infrastructure.Persistence;

namespace EmployeeRegistrationApp.Infrastructure.Repositories.EF
{
    /// <summary>
    /// Repositório EF Core para contas de usuário (UserAccount) do domínio.
    /// Note que isto é diferente de ApplicationUser (Identity).
    /// </summary>
    public sealed class UserAccountRepository : IUserAccountRepository
    {
        private readonly AppDbContext _context;

        public UserAccountRepository(AppDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        // =====================================================================
        // GETTERS
        // =====================================================================

        public async Task<UserAccount?> GetByIdAsync(Guid id)
        {
            return await _context.UserAccounts
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<UserAccount?> GetByEmailAsync(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return null;

            email = email.Trim();

            return await _context.UserAccounts
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Email.Value == email);
        }

        public async Task<UserAccount?> GetByUserNameAsync(string userName)
        {
            if (string.IsNullOrWhiteSpace(userName))
                return null;

            userName = userName.Trim();

            return await _context.UserAccounts
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.UserName == userName);
        }

        // =====================================================================
        // EXISTS CHECKERS
        // =====================================================================

        public async Task<bool> ExistsByEmailAsync(string email, Guid? ignoreId = null)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            email = email.Trim();

            var query = _context.UserAccounts.AsQueryable();

            if (ignoreId.HasValue)
                query = query.Where(u => u.Id != ignoreId.Value);

            return await query.AnyAsync(u => u.Email.Value == email);
        }

        public async Task<bool> ExistsByUserNameAsync(string userName, Guid? ignoreId = null)
        {
            if (string.IsNullOrWhiteSpace(userName))
                return false;

            userName = userName.Trim();

            var query = _context.UserAccounts.AsQueryable();

            if (ignoreId.HasValue)
                query = query.Where(u => u.Id != ignoreId.Value);

            return await query.AnyAsync(u => u.UserName == userName);
        }

        // =====================================================================
        // QUERY
        // =====================================================================

        public IQueryable<UserAccount> Query()
        {
            return _context.UserAccounts.AsQueryable();
        }

        // =====================================================================
        // COMMANDS (ADD / UPDATE)
        // =====================================================================

        public async Task AddAsync(UserAccount userAccount)
        {
            if (userAccount is null)
                throw new ArgumentNullException(nameof(userAccount));

            await _context.UserAccounts.AddAsync(userAccount);
        }

        public Task UpdateAsync(UserAccount userAccount)
        {
            if (userAccount is null)
                throw new ArgumentNullException(nameof(userAccount));

            _context.UserAccounts.Update(userAccount);
            return Task.CompletedTask;
        }
    }
}
