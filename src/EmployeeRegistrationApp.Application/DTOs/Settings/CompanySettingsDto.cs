// src/EmployeeRegistrationApp.Application/DTOs/Settings/CompanySettingsDto.cs
using System;

namespace EmployeeRegistrationApp.Application.DTOs.Settings
{
    /// <summary>
    /// DTO usado para transportar as configurações da empresa
    /// entre Application e camadas Web/Maui.
    /// 
    /// ⚠️ Alinhado ao entity Domain: CompanySettings
    /// (CompanyName, LegalName, RegistrationNumber, PrimaryEmail/Phone, WebsiteUrl,
    /// DefaultCurrency, DefaultWorkHoursPerWeek, AllowRemoteWork + auditoria).
    /// </summary>
    public sealed class CompanySettingsDto
    {
        public Guid? Id { get; set; }

        // Dados básicos
        public string CompanyName { get; set; } = string.Empty;
        public string LegalName { get; set; } = string.Empty;
        public string? RegistrationNumber { get; set; }

        // Contato (Value Objects no domínio)
        public string? PrimaryEmail { get; set; }
        public string? PrimaryPhone { get; set; }
        public string? WebsiteUrl { get; set; }

        // Preferências
        public string? DefaultCurrency { get; set; } = "BRL";
        public int? DefaultWorkHoursPerWeek { get; set; } = 40;
        public bool? AllowRemoteWork { get; set; } = true;

        // Auditoria
        public DateTime CreatedAt { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string? UpdatedBy { get; set; }
    }
}
