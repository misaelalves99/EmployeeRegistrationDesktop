// src/EmployeeRegistrationApp.Infrastructure/Repositories/InMemory/InMemoryPositionRepository.cs
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EmployeeRegistrationApp.Application.Interfaces.Repositories;
using EmployeeRegistrationApp.Domain.Entities;

namespace EmployeeRegistrationApp.Infrastructure.Repositories.InMemory
{
    /// <summary>
    /// Implementação in-memory de IPositionRepository.
    /// Usada para testes / demo sem banco real.
    /// </summary>
    public sealed class InMemoryPositionRepository : IPositionRepository
    {
        private readonly InMemoryDatabase _database;

        public InMemoryPositionRepository(InMemoryDatabase database)
        {
            _database = database ?? throw new ArgumentNullException(nameof(database));
        }

        public Task<Position?> GetByIdAsync(Guid id)
        {
            var position = _database.Positions.FirstOrDefault(p => p.Id == id);
            return Task.FromResult(position);
        }

        public Task<IReadOnlyList<Position>> GetAllAsync()
        {
            IReadOnlyList<Position> result = _database.Positions
                .OrderBy(p => p.Name)
                .ToList();

            return Task.FromResult(result);
        }

        public Task<IReadOnlyList<Position>> GetByDepartmentAsync(Guid departmentId)
        {
            IReadOnlyList<Position> result = _database.Positions
                .Where(p => p.DefaultDepartmentId == departmentId)
                .OrderBy(p => p.Name)
                .ToList();

            return Task.FromResult(result);
        }

        public Task<bool> ExistsByTitleAsync(string title, Guid? ignoreId = null)
        {
            if (string.IsNullOrWhiteSpace(title))
                return Task.FromResult(false);

            var term = title.Trim();
            var query = _database.Positions.AsEnumerable();

            if (ignoreId.HasValue)
            {
                query = query.Where(p => p.Id != ignoreId.Value);
            }

            var exists = query.Any(p => p.Name == term);
            return Task.FromResult(exists);
        }

        public Task AddAsync(Position position)
        {
            if (position is null) throw new ArgumentNullException(nameof(position));

            _database.Positions.Add(position);
            return Task.CompletedTask;
        }

        public Task UpdateAsync(Position position)
        {
            if (position is null) throw new ArgumentNullException(nameof(position));

            var index = _database.Positions.FindIndex(p => p.Id == position.Id);
            if (index >= 0)
            {
                _database.Positions[index] = position;
            }

            return Task.CompletedTask;
        }

        public Task DeleteAsync(Position position)
        {
            if (position is null) throw new ArgumentNullException(nameof(position));

            _database.Positions.RemoveAll(p => p.Id == position.Id);
            return Task.CompletedTask;
        }

        public Task<int> CountAsync()
        {
            return Task.FromResult(_database.Positions.Count);
        }
    }
}
