// src/EmployeeRegistrationApp.Infrastructure/Persistence/Configurations/ManagerConfiguration.cs
using EmployeeRegistrationApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EmployeeRegistrationApp.Infrastructure.Persistence.Configurations
{
    /// <summary>
    /// Configuração EF Core para a entidade Manager.
    /// Manager também herda de Employee no domínio.
    /// </summary>
    public sealed class ManagerConfiguration : IEntityTypeConfiguration<Manager>
    {
        public void Configure(EntityTypeBuilder<Manager> builder)
        {
            // Herdando o mapeamento base de Employee
            builder.HasBaseType<Employee>();

            // Se quiser TPT:
            // builder.ToTable("Managers");

            // Propriedades específicas de Manager podem ser configuradas aqui, por exemplo:
            // builder.Property(m => m.TeamSize);
            // builder.Property(m => m.Level)
            //     .HasMaxLength(50);
        }
    }
}
