using System;
using EmployeeRegistrationApp.Domain.Enums;
using EmployeeRegistrationApp.Domain.ValueObjects;

namespace EmployeeRegistrationApp.Domain.Entities
{
    /// <summary>
    /// Especialização de Employee para perfil de Desenvolvedor.
    /// Herdando todas as regras de Employee e adicionando detalhes de tecnologia.
    /// </summary>
    public sealed class Developer : Employee
    {
        /// <summary>
        /// Principal stack ou linguagem do desenvolvedor (ex: ".NET", "React", "Fullstack").
        /// </summary>
        public string PrimaryStack { get; private set; } = string.Empty;

        /// <summary>
        /// Nível de senioridade (Junior, Pleno, Senior, etc.).
        /// Pode ser livre em texto ou mapeado via JobRole, dependendo do uso.
        /// </summary>
        public string Seniority { get; private set; } = "Junior";

        // Construtor privado para EF
        private Developer() { }

        public Developer(
            string firstName,
            string lastName,
            Cpf cpf,
            Email email,
            PhoneNumber? phone,
            DateTime hireDate,
            JobRole jobRole,
            EmploymentStatus status,
            ContractType contractType,
            Money salary,
            string primaryStack,
            string seniority = "Junior")
            : base(firstName, lastName, cpf, email, phone, hireDate, jobRole, status, contractType, salary)
        {
            UpdateDeveloperProfile(primaryStack, seniority);
        }

        public void UpdateDeveloperProfile(string primaryStack, string seniority)
        {
            if (string.IsNullOrWhiteSpace(primaryStack))
                throw new ArgumentException("Stack principal do desenvolvedor é obrigatória.", nameof(primaryStack));

            PrimaryStack = primaryStack.Trim();
            Seniority = string.IsNullOrWhiteSpace(seniority)
                ? Seniority
                : seniority.Trim();
        }
    }
}
