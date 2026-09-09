// src/EmployeeRegistrationApp.Infrastructure/Repositories/InMemory/InMemoryUserAccountRepository.cs
using System;
using System.Linq;
using System.Threading.Tasks;
using EmployeeRegistrationApp.Application.Interfaces.Repositories;
using EmployeeRegistrationApp.Domain.Entities;

namespace EmployeeRegistrationApp.Infrastructure.Repositories.InMemory
{
    public sealed class InMemoryUserAccountRepository : IUserAccountRepository
    {
        private readonly InMemoryDatabase _database;

        public InMemoryUserAccountRepository(InMemoryDatabase database)
        {
            _database = database ?? throw new ArgumentNullException(nameof(database));
        }

        public Task<UserAccount?> GetByIdAsync(Guid id)
        {
            var user = _database.UserAccounts.FirstOrDefault(x => x.Id == id);
            return Task.FromResult(user);
        }

        public Task<UserAccount?> GetByEmailAsync(string email)
        {
            email = (email ?? string.Empty).Trim().ToLowerInvariant();

            var user = _database.UserAccounts.FirstOrDefault(x =>
                x.Email != null &&
                string.Equals(x.Email.Address, email, StringComparison.OrdinalIgnoreCase));

            return Task.FromResult(user);
        }

        public Task<UserAccount?> GetByUserNameAsync(string userName)
        {
            userName = (userName ?? string.Empty).Trim();

            var user = _database.UserAccounts.FirstOrDefault(x =>
                string.Equals(x.UserName, userName, StringComparison.OrdinalIgnoreCase));

            return Task.FromResult(user);
        }

        public Task AddAsync(UserAccount userAccount)
        {
            if (userAccount is null) throw new ArgumentNullException(nameof(userAccount));
            _database.UserAccounts.Add(userAccount);
            return Task.CompletedTask;
        }

        public Task UpdateAsync(UserAccount userAccount)
        {
            if (userAccount is null) throw new ArgumentNullException(nameof(userAccount));

            var index = _database.UserAccounts.FindIndex(x => x.Id == userAccount.Id);
            if (index >= 0) _database.UserAccounts[index] = userAccount;

            return Task.CompletedTask;
        }

        public Task<bool> ExistsByEmailAsync(string email, Guid? ignoreId = null)
        {
            email = (email ?? string.Empty).Trim().ToLowerInvariant();

            var exists = _database.UserAccounts.Any(x =>
                x.Email != null &&
                string.Equals(x.Email.Address, email, StringComparison.OrdinalIgnoreCase) &&
                (!ignoreId.HasValue || x.Id != ignoreId.Value));

            return Task.FromResult(exists);
        }

        public Task<bool> ExistsByUserNameAsync(string userName, Guid? ignoreId = null)
        {
            userName = (userName ?? string.Empty).Trim();

            var exists = _database.UserAccounts.Any(x =>
                string.Equals(x.UserName, userName, StringComparison.OrdinalIgnoreCase) &&
                (!ignoreId.HasValue || x.Id != ignoreId.Value));

            return Task.FromResult(exists);
        }
    }
}
