// src/EmployeeRegistrationApp.Application/Profiles/PositionApplicationProfile.cs
using AutoMapper;
using EmployeeRegistrationApp.Application.DTOs.Positions;
using EmployeeRegistrationApp.Domain.Entities;
using EmployeeRegistrationApp.Domain.ValueObjects;

namespace EmployeeRegistrationApp.Application.Profiles
{
    public sealed class PositionApplicationProfile : Profile
    {
        public PositionApplicationProfile()
        {
            // Domain -> DTO
            CreateMap<Position, PositionDto>()
                .ForMember(d => d.Type, opt => opt.MapFrom(s => s.PositionType))
                .ForMember(d => d.BaseSalary, opt => opt.MapFrom(s => s.BaseSalary != null ? s.BaseSalary.Amount : 0m))
                .ForMember(d => d.BaseSalaryCurrency, opt => opt.MapFrom(s => s.BaseSalary != null ? s.BaseSalary.Currency : "BRL"))
                .ForMember(d => d.DepartmentId, opt => opt.MapFrom(s => s.DefaultDepartmentId))
                .ForMember(d => d.DepartmentName, opt => opt.MapFrom(s => s.DefaultDepartment != null ? s.DefaultDepartment.Name : null))
                .ForMember(d => d.CreatedAt, opt => opt.MapFrom(s => s.CreatedAt))
                .ForMember(d => d.LastModifiedAt, opt => opt.MapFrom(s => s.UpdatedAt));

            // DTO -> Domain
            CreateMap<PositionDto, Position>()
                .ConstructUsing(dto =>
                    new Position(
                        dto.Name,
                        dto.Type,
                        dto.IsLeadership,
                        dto.BaseSalary > 0
                            ? Money.FromDecimal(dto.BaseSalary, dto.BaseSalaryCurrency)
                            : null,
                        dto.Code,
                        dto.Description,
                        dto.IsActive))
                .ForMember(d => d.PositionType, opt => opt.Ignore())
                .ForMember(d => d.BaseSalary, opt => opt.Ignore())
                .ForMember(d => d.DefaultDepartmentId, opt => opt.Ignore())
                .ForMember(d => d.DefaultDepartment, opt => opt.Ignore())
                .ForMember(d => d.Employees, opt => opt.Ignore())
                .ForMember(d => d.CreatedBy, opt => opt.Ignore())
                .ForMember(d => d.UpdatedAt, opt => opt.Ignore())
                .ForMember(d => d.UpdatedBy, opt => opt.Ignore());
        }
    }
}
