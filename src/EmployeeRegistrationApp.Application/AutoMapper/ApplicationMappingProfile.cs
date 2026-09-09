// src/EmployeeRegistrationApp.Application/Mapping/EmployeeApplicationProfile.cs
using AutoMapper;
using EmployeeRegistrationApp.Application.DTOs.Employees;
using EmployeeRegistrationApp.Domain.Entities;
using EmployeeRegistrationApp.Domain.Enums;
using EmployeeRegistrationApp.Domain.ValueObjects;

namespace EmployeeRegistrationApp.Application.Mapping
{
    public sealed class EmployeeApplicationProfile : Profile
    {
        public EmployeeApplicationProfile()
        {
            // Domain -> DTO (list)
            CreateMap<Employee, EmployeeListItemDto>()
                .ForMember(d => d.FullName, opt => opt.MapFrom(s => s.FullName))
                .ForMember(d => d.EmploymentStatus, opt => opt.MapFrom(s => s.EmploymentStatus))
                .ForMember(d => d.JobRole, opt => opt.MapFrom(s => s.JobRole))
                .ForMember(d => d.IsActive, opt => opt.MapFrom(s => s.IsActive))
                .ForMember(d => d.SalaryAmount, opt => opt.MapFrom(s => s.Salary.Amount))
                .ForMember(d => d.SalaryCurrency, opt => opt.MapFrom(s => s.Salary.Currency))
                .ForMember(d => d.DepartmentName, opt => opt.MapFrom(s => s.Department != null ? s.Department.Name : string.Empty))
                .ForMember(d => d.PositionName, opt => opt.MapFrom(s => s.Position != null ? s.Position.Title : string.Empty));

            // Domain -> DTO (details)
            CreateMap<Employee, EmployeeDetailsDto>()
                .ForMember(d => d.FullName, opt => opt.MapFrom(s => s.FullName))
                .ForMember(d => d.Email, opt => opt.MapFrom(s => s.Email.Address))
                .ForMember(d => d.PhoneNumber, opt => opt.MapFrom(s => s.PhoneNumber.ToString()))
                .ForMember(d => d.IsActive, opt => opt.MapFrom(s => s.IsActive))
                .ForMember(d => d.SalaryAmount, opt => opt.MapFrom(s => s.Salary.Amount))
                .ForMember(d => d.SalaryCurrency, opt => opt.MapFrom(s => s.Salary.Currency))
                .ForMember(d => d.DepartmentName, opt => opt.MapFrom(s => s.Department != null ? s.Department.Name : string.Empty))
                .ForMember(d => d.PositionName, opt => opt.MapFrom(s => s.Position != null ? s.Position.Title : string.Empty))
                .ForMember(d => d.ContractTypeLabel, opt => opt.MapFrom(s => s.ContractType.ToString()))
                .ForMember(d => d.EmploymentStatusLabel, opt => opt.MapFrom(s => s.EmploymentStatus.ToString()));

            // DTO -> Domain (update existente)
            // IMPORTANT: essa map é usada no Update, não no Create (vamos criar manualmente no AppService).
            CreateMap<EmployeeDetailsDto, Employee>()
                .AfterMap((src, dest) =>
                {
                    // nome completo -> first/last (fallback simples)
                    var (firstName, lastName) = SplitName(src.FullName);

                    // Update de dados pessoais
                    dest.UpdatePersonalInfo(firstName, lastName);

                    // Docs e contato
                    dest.UpdateDocuments(Cpf.Create("00000000000")); // placeholder seguro p/ não quebrar caso DTO não tenha cpf
                    dest.UpdateContact(Email.Create(src.Email), PhoneNumber.Create(src.PhoneNumber));

                    // Contrato / função / status
                    dest.ChangeContract(src.ContractType, src.JobRole);
                    dest.ChangeEmploymentStatus(src.EmploymentStatus);

                    // salário
                    dest.SetSalary(Money.FromDecimal(src.SalaryAmount, src.SalaryCurrency));

                    // ativo/inativo (se tiver data)
                    if (src.IsActive && !dest.IsActive)
                        dest.Reactivate(src.HireDate == default ? System.DateTime.UtcNow : src.HireDate);

                    if (!src.IsActive && dest.IsActive)
                        dest.Deactivate(src.TerminationDate);
                });
        }

        private static (string firstName, string lastName) SplitName(string fullName)
        {
            fullName ??= string.Empty;
            var parts = fullName.Trim().Split(' ', System.StringSplitOptions.RemoveEmptyEntries);

            if (parts.Length == 0) return ("Sem", "Nome");
            if (parts.Length == 1) return (parts[0], "—");

            var first = parts[0];
            var last = string.Join(" ", parts, 1, parts.Length - 1);
            return (first, last);
        }
    }
}
