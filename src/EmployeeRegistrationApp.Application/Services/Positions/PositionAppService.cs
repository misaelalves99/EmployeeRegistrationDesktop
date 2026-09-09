// src/EmployeeRegistrationApp.Application/Services/Positions/PositionAppService.cs
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using EmployeeRegistrationApp.Application.DTOs.Positions;
using EmployeeRegistrationApp.Application.Interfaces.Repositories;
using EmployeeRegistrationApp.Application.Interfaces.Services;
using EmployeeRegistrationApp.Domain.Entities;
using EmployeeRegistrationApp.Domain.ValueObjects;

namespace EmployeeRegistrationApp.Application.Services.Positions
{
    public sealed class PositionAppService : IPositionAppService
    {
        private readonly IPositionRepository _positionRepository;
        private readonly IDepartmentRepository _departmentRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public PositionAppService(
            IPositionRepository positionRepository,
            IDepartmentRepository departmentRepository,
            IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _positionRepository = positionRepository ?? throw new ArgumentNullException(nameof(positionRepository));
            _departmentRepository = departmentRepository ?? throw new ArgumentNullException(nameof(departmentRepository));
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<IReadOnlyList<PositionDto>> GetAllAsync(string? searchTerm = null)
        {
            var positions = await _positionRepository.GetAllAsync();

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                var term = searchTerm.Trim().ToLowerInvariant();
                positions = positions
                    .Where(p =>
                        p.Name.ToLower().Contains(term) ||
                        (!string.IsNullOrWhiteSpace(p.Code) && p.Code!.ToLower().Contains(term)))
                    .ToList();
            }

            var ordered = positions
                .OrderBy(p => p.PositionType)
                .ThenBy(p => p.Name)
                .ToList();

            return _mapper.Map<IReadOnlyList<PositionDto>>(ordered);
        }

        public async Task<PositionDto?> GetByIdAsync(Guid id)
        {
            var position = await _positionRepository.GetByIdAsync(id);
            return position is null ? null : _mapper.Map<PositionDto>(position);
        }

        public async Task<IReadOnlyList<PositionDto>> GetByDepartmentAsync(Guid departmentId)
        {
            var positions = await _positionRepository.GetByDepartmentAsync(departmentId);
            var ordered = positions.OrderBy(p => p.Name).ToList();
            return _mapper.Map<IReadOnlyList<PositionDto>>(ordered);
        }

        public async Task<PositionDto> CreateAsync(PositionDto dto)
        {
            if (dto is null) throw new ArgumentNullException(nameof(dto));

            if (await _positionRepository.ExistsByTitleAsync(dto.Name))
                throw new InvalidOperationException("Já existe um cargo com esse nome.");

            var baseSalary = dto.BaseSalary > 0
                ? Money.FromDecimal(dto.BaseSalary, dto.BaseSalaryCurrency)
                : null;

            var entity = new Position(
                dto.Name,
                dto.Type,
                dto.IsLeadership,
                baseSalary,
                dto.Code,
                dto.Description,
                isActive: true);

            if (dto.IsActive) entity.Activate();
            else entity.Deactivate();

            if (dto.DepartmentId.HasValue)
            {
                var dept = await _departmentRepository.GetByIdAsync(dto.DepartmentId.Value);
                entity.AssignDefaultDepartment(dept);
            }
            else
            {
                entity.AssignDefaultDepartment(null);
            }

            await _positionRepository.AddAsync(entity);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<PositionDto>(entity);
        }

        public async Task<PositionDto> UpdateAsync(PositionDto dto)
        {
            if (dto is null) throw new ArgumentNullException(nameof(dto));
            if (!dto.Id.HasValue || dto.Id == Guid.Empty)
                throw new ArgumentException("Id do cargo é obrigatório para atualização.", nameof(dto));

            var entity = await _positionRepository.GetByIdAsync(dto.Id.Value);
            if (entity is null)
                throw new InvalidOperationException("Cargo não encontrado.");

            if (await _positionRepository.ExistsByTitleAsync(dto.Name, dto.Id))
                throw new InvalidOperationException("Já existe outro cargo com esse nome.");

            var baseSalary = dto.BaseSalary > 0
                ? Money.FromDecimal(dto.BaseSalary, dto.BaseSalaryCurrency)
                : null;

            entity.UpdateInfo(
                dto.Name,
                dto.Type,
                dto.IsLeadership,
                baseSalary,
                dto.Code,
                dto.Description);

            if (dto.IsActive) entity.Activate();
            else entity.Deactivate();

            if (dto.DepartmentId.HasValue)
            {
                var dept = await _departmentRepository.GetByIdAsync(dto.DepartmentId.Value);
                entity.AssignDefaultDepartment(dept);
            }
            else
            {
                entity.AssignDefaultDepartment(null);
            }

            await _positionRepository.UpdateAsync(entity);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<PositionDto>(entity);
        }

        public async Task DeleteAsync(Guid id)
        {
            var entity = await _positionRepository.GetByIdAsync(id);
            if (entity is null) return;

            await _positionRepository.DeleteAsync(entity);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
