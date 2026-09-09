using System;
using System.Linq;
using AutoMapper;
using EmployeeRegistrationApp.Application.DTOs.Employees;
using EmployeeRegistrationApp.Domain.Entities;
using EmployeeRegistrationApp.Domain.Enums;
using EmployeeRegistrationApp.Domain.ValueObjects;

namespace EmployeeRegistrationApp.Application.AutoMapper.Employees
{
    public sealed class EmployeeApplicationProfile : Profile
    {
        public EmployeeApplicationProfile()
        {
            // Domain -> DTO (edição)
            CreateMap<Employee, EmployeeDto>()
                .ForMember(d => d.Cpf, opt => opt.MapFrom(s => s.Cpf.ToString()))
                .ForMember(d => d.Email, opt => opt.MapFrom(s => s.Email.Address))
                .ForMember(d => d.PhoneNumber, opt => opt.MapFrom(s => s.Phone != null ? s.Phone.ToString() : null))
                .ForMember(d => d.SalaryAmount, opt => opt.MapFrom(s => s.Salary.Amount))
                .ForMember(d => d.SalaryCurrency, opt => opt.MapFrom(s => s.Salary.Currency))
                .ForMember(d => d.DepartmentId, opt => opt.MapFrom(s => s.DepartmentId))
                .ForMember(d => d.PositionId, opt => opt.MapFrom(s => s.PositionId))
                .ForMember(d => d.FullName, opt => opt.MapFrom(s => s.FullName));

            // Domain -> DTO (detalhes)
            CreateMap<Employee, EmployeeDetailsDto>()
                .ForMember(d => d.Cpf, opt => opt.MapFrom(s => s.Cpf.ToString()))
                .ForMember(d => d.Email, opt => opt.MapFrom(s => s.Email.Address))
                .ForMember(d => d.PhoneNumber, opt => opt.MapFrom(s => s.Phone != null ? s.Phone.ToString() : null))
                .ForMember(d => d.SalaryAmount, opt => opt.MapFrom(s => s.Salary.Amount))
                .ForMember(d => d.SalaryCurrency, opt => opt.MapFrom(s => s.Salary.Currency))
                .ForMember(d => d.DepartmentName, opt => opt.MapFrom(s => s.Department != null ? s.Department.Name : string.Empty))
                .ForMember(d => d.PositionName, opt => opt.MapFrom(s => s.Position != null ? s.Position.Name : string.Empty))
                .ForMember(d => d.ContractTypeLabel, opt => opt.MapFrom(s => s.ContractType.ToString()))
                .ForMember(d => d.EmploymentStatusLabel, opt => opt.MapFrom(s => s.EmploymentStatus.ToString()))
                .ForMember(d => d.FullName, opt => opt.MapFrom(s => s.FullName))
                .ForMember(d => d.IsActive, opt => opt.MapFrom(s => s.IsActive));

            // Domain -> DTO (listagem)
            CreateMap<Employee, EmployeeListItemDto>()
                .ForMember(d => d.Cpf, opt => opt.MapFrom(s => s.Cpf.ToString()))
                .ForMember(d => d.Email, opt => opt.MapFrom(s => s.Email.Address))
                .ForMember(d => d.DepartmentName, opt => opt.MapFrom(s => s.Department != null ? s.Department.Name : null))
                .ForMember(d => d.PositionName, opt => opt.MapFrom(s => s.Position != null ? s.Position.Name : null))
                .ForMember(d => d.SalaryAmount, opt => opt.MapFrom(s => s.Salary.Amount))
                .ForMember(d => d.SalaryCurrency, opt => opt.MapFrom(s => s.Salary.Currency))
                .ForMember(d => d.IsActive, opt => opt.MapFrom(s => s.IsActive));

            // DTO -> Domain (criar/atualizar)
            CreateMap<EmployeeDto, Employee>()
                .ForAllMembers(opt => opt.Ignore())
                .AfterMap((src, dest) =>
                {
                    // ==========
                    // Nome (suporta UI que envia só FullName)
                    // ==========
                    var firstName = src.FirstName?.Trim();
                    var lastName = src.LastName?.Trim();

                    if (string.IsNullOrWhiteSpace(firstName) && string.IsNullOrWhiteSpace(lastName))
                    {
                        var full = (src.FullName ?? string.Empty).Trim();
                        if (!string.IsNullOrWhiteSpace(full))
                        {
                            var parts = full.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                            firstName = parts.Length > 0 ? parts[0] : null;
                            lastName = parts.Length > 1 ? string.Join(" ", parts.Skip(1)) : null;
                        }
                    }

                    if (string.IsNullOrWhiteSpace(firstName)) firstName = "Colaborador";
                    if (string.IsNullOrWhiteSpace(lastName)) lastName = "Sem Sobrenome";

                    dest.UpdatePersonalInfo(firstName, lastName);

                    // ==========
                    // Documentos (fallback para evitar crash na UI enquanto CPF não existir no formulário)
                    // ==========
                    var cpfText = string.IsNullOrWhiteSpace(src.Cpf) ? "11144477735" : src.Cpf!;
                    var emailText = string.IsNullOrWhiteSpace(src.Email) ? "colaborador@empresa.com" : src.Email!;

                    var cpf = Cpf.Create(cpfText);
                    var email = Email.Create(emailText);
                    dest.UpdateDocuments(cpf, email);

                    // ==========
                    // Contato
                    // ==========
                    var phone = string.IsNullOrWhiteSpace(src.PhoneNumber)
                        ? null
                        : PhoneNumber.Create(src.PhoneNumber);

                    dest.UpdateContact(phone);

                    // ==========
                    // Contrato
                    // ==========
                    dest.ChangeContract(src.ContractType, src.JobRole);

                    // ==========
                    // Salário
                    // ==========
                    var currency = string.IsNullOrWhiteSpace(src.SalaryCurrency) ? "BRL" : src.SalaryCurrency!;
                    var salary = Money.FromDecimal(src.SalaryAmount, currency);
                    dest.SetSalary(salary);

                    // ==========
                    // Datas / Status (evita default 0001)
                    // ==========
                    var hireDate = src.HireDate == default ? DateTime.Today : src.HireDate;

                    dest.ChangeEmploymentStatus(src.EmploymentStatus, src.TerminationDate);

                    var mustBeInactive =
                        src.EmploymentStatus == EmploymentStatus.Terminated ||
                        src.EmploymentStatus == EmploymentStatus.Inactive ||
                        src.TerminationDate.HasValue;

                    if (mustBeInactive)
                        dest.Deactivate(src.TerminationDate);
                    else
                        dest.Reactivate(hireDate);
                });
        }
    }
}
