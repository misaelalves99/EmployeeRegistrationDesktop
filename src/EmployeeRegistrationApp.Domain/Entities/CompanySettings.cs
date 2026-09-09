using System;
using EmployeeRegistrationApp.Domain.Base;
using EmployeeRegistrationApp.Domain.Interfaces;
using EmployeeRegistrationApp.Domain.ValueObjects;

namespace EmployeeRegistrationApp.Domain.Entities
{
    /// <summary>
    /// Configurações globais da empresa.
    /// É um AggregateRoot único (geralmente apenas um registro).
    /// </summary>
    public sealed class CompanySettings : AggregateRoot, IAuditable
    {
        // Dados básicos
        public string CompanyName { get; private set; } = string.Empty;  // Nome fantasia
        public string LegalName { get; private set; } = string.Empty;    // Razão social
        public string? RegistrationNumber { get; private set; } = null;  // CNPJ ou equivalente

        // Contato principal
        public Email? PrimaryEmail { get; private set; }
        public PhoneNumber? PrimaryPhone { get; private set; }
        public string? WebsiteUrl { get; private set; }

        // Preferências
        public string DefaultCurrency { get; private set; } = "BRL";
        public int DefaultWorkHoursPerWeek { get; private set; } = 40;
        public bool AllowRemoteWork { get; private set; }

        // Auditoria
        public DateTime CreatedAt { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string? UpdatedBy { get; set; }

        // Construtor privado para EF
        private CompanySettings() { }

        public CompanySettings(
            string companyName,
            string legalName,
            string? registrationNumber,
            Email? primaryEmail,
            PhoneNumber? primaryPhone,
            string? websiteUrl,
            string defaultCurrency = "BRL",
            int defaultWorkHoursPerWeek = 40,
            bool allowRemoteWork = true)
        {
            UpdateBasicInfo(companyName, legalName, registrationNumber);
            UpdateContact(primaryEmail, primaryPhone, websiteUrl);
            UpdatePreferences(defaultCurrency, defaultWorkHoursPerWeek, allowRemoteWork);

            CreatedAt = DateTime.UtcNow;
        }

        public void UpdateBasicInfo(string companyName, string legalName, string? registrationNumber)
        {
            if (string.IsNullOrWhiteSpace(companyName))
                throw new ArgumentException("Nome fantasia da empresa é obrigatório.", nameof(companyName));

            if (string.IsNullOrWhiteSpace(legalName))
                throw new ArgumentException("Razão social da empresa é obrigatória.", nameof(legalName));

            CompanyName = companyName.Trim();
            LegalName = legalName.Trim();
            RegistrationNumber = string.IsNullOrWhiteSpace(registrationNumber)
                ? null
                : registrationNumber.Trim();

            UpdatedAt = DateTime.UtcNow;
        }

        public void UpdateContact(Email? primaryEmail, PhoneNumber? primaryPhone, string? websiteUrl)
        {
            PrimaryEmail = primaryEmail;
            PrimaryPhone = primaryPhone;
            WebsiteUrl = string.IsNullOrWhiteSpace(websiteUrl) ? null : websiteUrl.Trim();

            UpdatedAt = DateTime.UtcNow;
        }

        public void UpdatePreferences(string defaultCurrency, int defaultWorkHoursPerWeek, bool allowRemoteWork)
        {
            if (string.IsNullOrWhiteSpace(defaultCurrency))
                throw new ArgumentException("Moeda padrão é obrigatória.", nameof(defaultCurrency));

            if (defaultWorkHoursPerWeek <= 0)
                throw new ArgumentException("Horas semanais devem ser maiores que zero.", nameof(defaultWorkHoursPerWeek));

            DefaultCurrency = defaultCurrency.Trim().ToUpperInvariant();
            DefaultWorkHoursPerWeek = defaultWorkHoursPerWeek;
            AllowRemoteWork = allowRemoteWork;

            UpdatedAt = DateTime.UtcNow;
        }

        public void TouchAudit(string userName)
        {
            UpdatedAt = DateTime.UtcNow;
            UpdatedBy = userName;
        }
    }
}
