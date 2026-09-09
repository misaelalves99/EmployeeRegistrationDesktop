// src/EmployeeRegistrationApp.Domain/Entities/Manager.cs
using System;
using EmployeeRegistrationApp.Domain.Enums;
using EmployeeRegistrationApp.Domain.ValueObjects;

namespace EmployeeRegistrationApp.Domain.Entities
{
    /// <summary>
    /// Especialização de Employee para perfis de gestão/liderança.
    /// </summary>
    public sealed class Manager : Employee
    {
        /// <summary>
        /// Área principal de responsabilidade (ex: "Tecnologia", "RH", "Operações").
        /// </summary>
        public string? ManagementArea { get; private set; }

        /// <summary>
        /// Tamanho máximo esperado de time sob gestão (apenas referência gerencial).
        /// </summary>
        public int? MaxTeamSize { get; private set; }

        // Construtor privado para EF
        private Manager() { }

        public Manager(
            string firstName,
            string lastName,
            Cpf cpf,
            Email email,
            PhoneNumber? phone,
            DateTime hireDate,
            JobRole jobRole,
            EmploymentStatus employmentStatus,
            ContractType contractType,
            Money salary,
            string? managementArea = null,
            int? maxTeamSize = null)
            : base(firstName, lastName, cpf, email, phone, hireDate, jobRole, employmentStatus, contractType, salary)
        {
            UpdateManagementProfile(managementArea, maxTeamSize);
        }

        public void UpdateManagementProfile(string? managementArea, int? maxTeamSize)
        {
            ManagementArea = string.IsNullOrWhiteSpace(managementArea)
                ? null
                : managementArea.Trim();

            if (maxTeamSize.HasValue && maxTeamSize.Value < 0)
                throw new ArgumentException("Tamanho máximo de time não pode ser negativo.", nameof(maxTeamSize));

            MaxTeamSize = maxTeamSize;
        }
    }
}
