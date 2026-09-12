// src/EmployeeRegistrationApp.Infrastructure/Repositories/InMemory/InMemoryDatabase.cs
using System;
using System.Collections.Generic;
using EmployeeRegistrationApp.Domain.Entities;
using EmployeeRegistrationApp.Domain.Users;
using EmployeeRegistrationApp.Domain.Enums;
using EmployeeRegistrationApp.Domain.ValueObjects;

namespace EmployeeRegistrationApp.Infrastructure.Repositories.InMemory
{
    /// <summary>
    /// "Banco" em memória para cenários de teste, protótipo e/ou execução sem banco físico.
    /// </summary>
    public sealed class InMemoryDatabase
    {
        public List<Department> Departments { get; } = new();
        public List<Employee> Employees { get; } = new();
        public List<Position> Positions { get; } = new();
        public List<UserAccount> UserAccounts { get; } = new();

        public List<AuditLog> AuditLogs { get; } = new();
        public List<CompanySettings> CompanySettings { get; } = new();

        private readonly object _lock = new();

        public void SeedIfEmpty()
        {
            lock (_lock)
            {
                if (Departments.Count > 0 || Employees.Count > 0 || Positions.Count > 0 || CompanySettings.Count > 0)
                    return;

                // ======================
                // Departments
                // assinatura: Department(string name, string? code, string? description, bool isActive = true)
                // ======================
                var hr = new Department("Recursos Humanos", "RH", "Gestão de pessoas, cultura e rotinas.", true);
                var it = new Department("Tecnologia da Informação", "TI", "Engenharia, suporte e infraestrutura.", true);
                var finance = new Department("Financeiro", "FIN", "Pagamentos, faturamento e orçamento.", true);

                Departments.Add(hr);
                Departments.Add(it);
                Departments.Add(finance);

                // ======================
                // Positions
                // assinatura: Position(string name, PositionType positionType, bool isLeadership, Money? baseSalary, string? code=null, string? description=null, bool isActive=true)
                // ======================
                var devJunior = new Position(
                    name: "Desenvolvedor Júnior",
                    positionType: PositionType.Junior,
                    isLeadership: false,
                    baseSalary: Money.FromDecimal(4500m, "BRL"),
                    code: "DEV-JR",
                    description: "Desenvolvimento de features e correções com supervisão.",
                    isActive: true);

                var devSenior = new Position(
                    name: "Desenvolvedor Sênior",
                    positionType: PositionType.Senior,
                    isLeadership: true,
                    baseSalary: Money.FromDecimal(12000m, "BRL"),
                    code: "DEV-SR",
                    description: "Liderança técnica e entregas críticas.",
                    isActive: true);

                var hrAnalyst = new Position(
                    name: "Analista de RH",
                    positionType: PositionType.MidLevel,
                    isLeadership: false,
                    baseSalary: Money.FromDecimal(6000m, "BRL"),
                    code: "RH-AN",
                    description: "Processos seletivos, rotinas e indicadores.",
                    isActive: true);

                Positions.Add(devJunior);
                Positions.Add(devSenior);
                Positions.Add(hrAnalyst);

                // (Opcional) default department em cargos
                devJunior.AssignDefaultDepartment(it);
                devSenior.AssignDefaultDepartment(it);
                hrAnalyst.AssignDefaultDepartment(hr);

                // ======================
                // CompanySettings
                // assinatura: CompanySettings(companyName, legalName, registrationNumber, primaryEmail, primaryPhone, websiteUrl, defaultCurrency, defaultWorkHoursPerWeek, allowRemoteWork)
                // ======================
                var settings = new CompanySettings(
                    companyName: "EmployeeApp LTDA",
                    legalName: "EmployeeApp LTDA",
                    registrationNumber: null,
                    primaryEmail: null,
                    primaryPhone: null,
                    websiteUrl: null,
                    defaultCurrency: "BRL",
                    defaultWorkHoursPerWeek: 40,
                    allowRemoteWork: true
                );

                CompanySettings.Add(settings);
            }
        }
    }
}
