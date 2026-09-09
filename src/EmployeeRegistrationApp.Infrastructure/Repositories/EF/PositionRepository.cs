// src/EmployeeRegistrationApp.Infrastructure/Repositories/EF/PositionRepository.cs
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EmployeeRegistrationApp.Application.Interfaces.Repositories;
using EmployeeRegistrationApp.Domain.Entities;
using EmployeeRegistrationApp.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace EmployeeRegistrationApp.Infrastructure.Repositories.EF
{
    /// <summary>
    /// Repositório EF Core para cargos/posições (Position).
    /// </summary>
    public sealed class PositionRepository : IPositionRepository
    {
        private readonly AppDbContext _context;

        public PositionRepository(AppDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<Position?> GetByIdAsync(Guid id)
        {
            return await _context.Positions
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<IReadOnlyList<Position>> GetAllAsync()
        {
            return await _context.Positions
                .AsNoTracking()
                .OrderBy(p => p.Name)
                .ToListAsync();
        }

        public async Task<IReadOnlyList<Position>> GetByDepartmentAsync(Guid departmentId)
        {
            return await _context.Positions
                .AsNoTracking()
                .Where(p => p.DefaultDepartmentId == departmentId)
                .OrderBy(p => p.Name)
                .ToListAsync();
        }

        public async Task<bool> ExistsByTitleAsync(string title, Guid? ignoreId = null)
        {
            if (string.IsNullOrWhiteSpace(title))
                return false;

            title = title.Trim();

            var query = _context.Positions.AsQueryable();

            if (ignoreId.HasValue)
            {
                query = query.Where(p => p.Id != ignoreId.Value);
            }

            return await query.AnyAsync(p => p.Name == title);
        }

        public async Task AddAsync(Position position)
        {
            if (position is null) throw new ArgumentNullException(nameof(position));
            await _context.Positions.AddAsync(position);
        }

        public Task UpdateAsync(Position position)
        {
            if (position is null) throw new ArgumentNullException(nameof(position));
            _context.Positions.Update(position);
            return Task.CompletedTask;
        }

        public Task DeleteAsync(Position position)
        {
            if (position is null) throw new ArgumentNullException(nameof(position));
            _context.Positions.Remove(position);
            return Task.CompletedTask;
        }

        public Task<int> CountAsync()
        {
            return _context.Positions.CountAsync();
        }
    }
}
