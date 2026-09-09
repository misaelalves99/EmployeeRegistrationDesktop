// src/EmployeeRegistrationApp.Infrastructure/Persistence/AppDbContext.cs
using EmployeeRegistrationApp.Domain.Entities;
using EmployeeRegistrationApp.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace EmployeeRegistrationApp.Infrastructure.Persistence
{
    /// <summary>
    /// DbContext principal da aplicação.
    /// 
    /// - Integra Identity (ApplicationUser)
    /// - Expõe DbSets para as entidades de domínio
    /// - Aplica as configurações de mapeamento (Configurations/*)
    /// 
    /// Este contexto é reutilizável tanto em ASP.NET Core MVC quanto
    /// no projeto .NET MAUI Desktop (normalmente configurado com SQLite).
    /// </summary>
    public class AppDbContext
        : IdentityDbContext<ApplicationUser, IdentityRole<Guid>, Guid>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        // ==========================
        // DbSets de domínio
        // ==========================

        public DbSet<AuditLog> AuditLogs => Set<AuditLog>();
        public DbSet<CompanySettings> CompanySettings => Set<CompanySettings>();
        public DbSet<Department> Departments => Set<Department>();
        public DbSet<Developer> Developers => Set<Developer>();
        public DbSet<Employee> Employees => Set<Employee>();
        public DbSet<Manager> Managers => Set<Manager>();
        public DbSet<Position> Positions => Set<Position>();
        public DbSet<ReportTemplate> ReportTemplates => Set<ReportTemplate>();
        public DbSet<UserAccount> UserAccounts => Set<UserAccount>();

        /// <summary>
        /// Configuração global do modelo — aplica as configurações
        /// específicas de cada entidade (Fluent API).
        /// </summary>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Aplicar configurações de cada entidade
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);

            // Se quiser ser explícito em vez de usar ApplyConfigurationsFromAssembly, pode usar:
            //
            // modelBuilder.ApplyConfiguration(new AuditLogConfiguration());
            // modelBuilder.ApplyConfiguration(new CompanySettingsConfiguration());
            // modelBuilder.ApplyConfiguration(new DepartmentConfiguration());
            // modelBuilder.ApplyConfiguration(new DeveloperConfiguration());
            // modelBuilder.ApplyConfiguration(new EmployeeConfiguration());
            // modelBuilder.ApplyConfiguration(new ManagerConfiguration());
            // modelBuilder.ApplyConfiguration(new PositionConfiguration());
            // modelBuilder.ApplyConfiguration(new ReportTemplateConfiguration());
            // modelBuilder.ApplyConfiguration(new UserAccountConfiguration());
        }
    }
}
