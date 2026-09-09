using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using AutoMapper;
using EmployeeRegistrationApp.Application.DTOs.Common;
using EmployeeRegistrationApp.Application.DTOs.Employees;
using EmployeeRegistrationApp.Application.Interfaces.Repositories;
using EmployeeRegistrationApp.Application.Interfaces.Services;
using EmployeeRegistrationApp.Domain.Entities;
using EmployeeRegistrationApp.Domain.Enums;

namespace EmployeeRegistrationApp.Application.Services.Employees
{
    public sealed class EmployeeAppService : IEmployeeAppService
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IDepartmentRepository _departmentRepository;
        private readonly IPositionRepository _positionRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public EmployeeAppService(
            IEmployeeRepository employeeRepository,
            IDepartmentRepository departmentRepository,
            IPositionRepository positionRepository,
            IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _employeeRepository = employeeRepository;
            _departmentRepository = departmentRepository;
            _positionRepository = positionRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<PagedResultDto<EmployeeListItemDto>> GetPagedAsync(
            int page,
            int pageSize,
            string? search = null,
            Guid? departmentId = null,
            EmploymentStatus? status = null)
        {
            if (page <= 0) page = 1;
            if (pageSize <= 0) pageSize = 10;

            var allEmployees = await _employeeRepository.GetAllAsync();

            if (!string.IsNullOrWhiteSpace(search))
            {
                var term = search.Trim().ToLowerInvariant();

                allEmployees = allEmployees
                    .Where(e =>
                        (e.FullName ?? string.Empty).ToLowerInvariant().Contains(term) ||
                        (e.Email?.Address ?? string.Empty).ToLowerInvariant().Contains(term) ||
                        (e.Department != null && (e.Department.Name ?? string.Empty).ToLowerInvariant().Contains(term)))
                    .ToList();
            }

            if (departmentId.HasValue)
            {
                allEmployees = allEmployees
                    .Where(e => e.DepartmentId.HasValue && e.DepartmentId.Value == departmentId.Value)
                    .ToList();
            }

            if (status.HasValue)
            {
                allEmployees = allEmployees
                    .Where(e => e.EmploymentStatus == status.Value)
                    .ToList();
            }

            var totalCount = allEmployees.Count;

            var pageItems = allEmployees
                .OrderBy(e => e.FullName)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            var itemsDto = _mapper.Map<IReadOnlyList<EmployeeListItemDto>>(pageItems);

            return new PagedResultDto<EmployeeListItemDto>
            {
                PageNumber = page,
                PageSize = pageSize,
                TotalCount = totalCount,
                Items = itemsDto
            };
        }

        public async Task<EmployeeDetailsDto?> GetByIdAsync(Guid id)
        {
            var employee = await _employeeRepository.GetByIdAsync(id);
            return employee is null ? null : _mapper.Map<EmployeeDetailsDto>(employee);
        }

        public async Task<EmployeeDetailsDto> CreateAsync(EmployeeDto dto)
        {
            var department = dto.DepartmentId.HasValue
                ? await _departmentRepository.GetByIdAsync(dto.DepartmentId.Value)
                : null;

            var position = dto.PositionId.HasValue
                ? await _positionRepository.GetByIdAsync(dto.PositionId.Value)
                : null;

            var employee = _mapper.Map<Employee>(dto);

            if (department != null) employee.AssignDepartment(department);
            if (position != null) employee.AssignPosition(position);

            await _employeeRepository.AddAsync(employee);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<EmployeeDetailsDto>(employee);
        }

        public async Task<EmployeeDetailsDto?> UpdateAsync(Guid id, EmployeeDto dto)
        {
            var employee = await _employeeRepository.GetByIdAsync(id);
            if (employee is null) return null;

            _mapper.Map(dto, employee);

            if (dto.DepartmentId.HasValue)
            {
                var department = await _departmentRepository.GetByIdAsync(dto.DepartmentId.Value);
                if (department != null) employee.AssignDepartment(department);
            }

            if (dto.PositionId.HasValue)
            {
                var position = await _positionRepository.GetByIdAsync(dto.PositionId.Value);
                if (position != null) employee.AssignPosition(position);
            }

            _employeeRepository.Update(employee);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<EmployeeDetailsDto>(employee);
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var employee = await _employeeRepository.GetByIdAsync(id);
            if (employee is null) return false;

            _employeeRepository.Remove(employee);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeactivateAsync(Guid id)
        {
            var employee = await _employeeRepository.GetByIdAsync(id);
            if (employee is null) return false;

            employee.Deactivate();
            _employeeRepository.Update(employee);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ReactivateAsync(Guid id)
        {
            var employee = await _employeeRepository.GetByIdAsync(id);
            if (employee is null) return false;

            employee.Reactivate();
            _employeeRepository.Update(employee);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }
    }
}
