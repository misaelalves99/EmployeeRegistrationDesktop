// src/EmployeeRegistrationApp.Application/AutoMapper/SettingsMappingProfile.cs
using AutoMapper;
using EmployeeRegistrationApp.Application.DTOs.Settings;
using EmployeeRegistrationApp.Domain.Entities;
using EmployeeRegistrationApp.Domain.ValueObjects;

namespace EmployeeRegistrationApp.Application.AutoMapper
{
    /// <summary>
    /// Perfil de mapeamento entre CompanySettings (Domínio) e CompanySettingsDto (Application).
    /// </summary>
    public sealed class SettingsMappingProfile : Profile
    {
        public SettingsMappingProfile()
        {
            // Domain -> DTO
            CreateMap<CompanySettings, CompanySettingsDto>()
                .ForMember(d => d.PrimaryEmail, opt => opt.MapFrom(s => s.PrimaryEmail != null ? s.PrimaryEmail.Address : null))
                .ForMember(d => d.PrimaryPhone, opt => opt.MapFrom(s => s.PrimaryPhone != null ? s.PrimaryPhone.Digits : null))
                .ForMember(d => d.CreatedAt, opt => opt.MapFrom(s => s.CreatedAt))
                .ForMember(d => d.CreatedBy, opt => opt.MapFrom(s => s.CreatedBy))
                .ForMember(d => d.UpdatedAt, opt => opt.MapFrom(s => s.UpdatedAt))
                .ForMember(d => d.UpdatedBy, opt => opt.MapFrom(s => s.UpdatedBy));

            // DTO -> Domain
            CreateMap<CompanySettingsDto, CompanySettings>()
                // Id do AggregateRoot não deve ser setado por DTO
                .ForMember(d => d.Id, opt => opt.Ignore())
                .AfterMap((src, dest) =>
                {
                    // ✅ Aggregate tem regras e private set, então usamos os métodos
                    dest.UpdateBasicInfo(
                        companyName: src.CompanyName ?? string.Empty,
                        legalName: src.LegalName ?? string.Empty,
                        registrationNumber: src.RegistrationNumber
                    );

                    dest.UpdateContact(
                        primaryEmail: string.IsNullOrWhiteSpace(src.PrimaryEmail) ? null : Email.Create(src.PrimaryEmail),
                        primaryPhone: string.IsNullOrWhiteSpace(src.PrimaryPhone) ? null : PhoneNumber.Create(src.PrimaryPhone),
                        websiteUrl: src.WebsiteUrl
                    );

                    dest.UpdatePreferences(
                        defaultCurrency: string.IsNullOrWhiteSpace(src.DefaultCurrency) ? "BRL" : src.DefaultCurrency!,
                        defaultWorkHoursPerWeek: src.DefaultWorkHoursPerWeek ?? 40,
                        allowRemoteWork: src.AllowRemoteWork ?? true
                    );

                    // Auditoria (opcional — depende se sua UI preenche isso)
                    if (!string.IsNullOrWhiteSpace(src.UpdatedBy))
                    {
                        dest.TouchAudit(src.UpdatedBy!);
                    }
                });
        }
    }
}
