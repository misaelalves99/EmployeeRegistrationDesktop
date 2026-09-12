using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using EmployeeRegistrationApp.Application.DTOs.Departments;
using EmployeeRegistrationApp.Application.Interfaces.Repositories;
using EmployeeRegistrationApp.Application.Interfaces.Services;
using EmployeeRegistrationApp.Domain.Entities;

namespace EmployeeRegistrationApp.Application.Services.Departments
{
    public sealed class DepartmentAppService : IDepartmentAppService
    {
        private readonly IDepartmentRepository _departmentRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public DepartmentAppService(
            IDepartmentRepository departmentRepository,
            IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _departmentRepository = departmentRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IReadOnlyList<DepartmentDto>> GetAllAsync()
        {
            var departments = await _departmentRepository.GetAllAsync();
            var ordered = departments.OrderBy(d => d.Name).ToList();
            return _mapper.Map<IReadOnlyList<DepartmentDto>>(ordered);
        }

        public async Task<DepartmentDto?> GetByIdAsync(Guid id)
        {
            var department = await _departmentRepository.GetByIdAsync(id);
            return department is null ? null : _mapper.Map<DepartmentDto>(department);
        }

        public async Task<DepartmentDto> CreateAsync(DepartmentDto dto)
        {
            var entity = _mapper.Map<Department>(dto);

            await _departmentRepository.AddAsync(entity);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<DepartmentDto>(entity);
        }

        public async Task<DepartmentDto> UpdateAsync(DepartmentDto dto)
        {
            if (!dto.Id.HasValue || dto.Id.Value == Guid.Empty)
                throw new ArgumentException(
                    "Department Id is required for update.",
                    nameof(dto));

            var entity = await _departmentRepository.GetByIdAsync(dto.Id.Value);

            if (entity is null)
                throw new InvalidOperationException(
                    $"Department '{dto.Id.Value}' was not found.");

            _mapper.Map(dto, entity);

            await _departmentRepository.UpdateAsync(entity);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<DepartmentDto>(entity);
        }

        public async Task DeleteAsync(Guid id)
        {
            var entity = await _departmentRepository.GetByIdAsync(id);

            if (entity is null)
                return;

            await _departmentRepository.DeleteAsync(entity);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}