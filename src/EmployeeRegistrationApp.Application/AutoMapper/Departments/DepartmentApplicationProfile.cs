// src/EmployeeRegistrationApp.Application/AutoMapper/Departments/DepartmentApplicationProfile.cs
using AutoMapper;
using EmployeeRegistrationApp.Application.DTOs.Departments;
using EmployeeRegistrationApp.Domain.Entities;

namespace EmployeeRegistrationApp.Application.AutoMapper.Departments
{
    public sealed class DepartmentApplicationProfile : Profile
    {
        public DepartmentApplicationProfile()
        {
            CreateMap<Department, DepartmentDto>()
                .ForMember(d => d.LastModifiedAt, opt => opt.MapFrom(s => s.UpdatedAt))
                .ForMember(d => d.LastModifiedBy, opt => opt.MapFrom(s => s.UpdatedBy))
                .ForMember(d => d.Headcount, opt => opt.MapFrom(s => s.Employees.Count));

            CreateMap<DepartmentDto, Department>()
                .ForAllMembers(opt => opt.Ignore())
                .AfterMap((src, dest) =>
                {
                    dest.UpdateInfo(src.Name ?? string.Empty, src.Code, src.Description);

                    if (src.IsActive) dest.Activate();
                    else dest.Deactivate();
                });
        }
    }
}
