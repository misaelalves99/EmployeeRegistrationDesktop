// src/EmployeeRegistrationApp.Infrastructure/Persistence/Configurations/DeveloperConfiguration.cs
using EmployeeRegistrationApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EmployeeRegistrationApp.Infrastructure.Persistence.Configurations
{
    /// <summary>
    /// Configuração EF Core para a entidade Developer.
    /// 
    /// Aqui assumimos que Developer herda de Employee e usamos
    /// mapeamento por herança (TPH ou TPT, conforme o AppDbContext).
    /// </summary>
    public sealed class DeveloperConfiguration : IEntityTypeConfiguration<Developer>
    {
        public void Configure(EntityTypeBuilder<Developer> builder)
        {
            // Herdando o mapeamento base de Employee
            builder.HasBaseType<Employee>();

            // Se quiser TPT (tabela própria), descomente:
            // builder.ToTable("Developers");

            // Se houver propriedades específicas em Developer (exemplos):
            // builder.Property(d => d.MainTechnology)
            //     .HasMaxLength(100);
            //
            // builder.Property(d => d.SeniorityLevel)
            //     .HasMaxLength(50);
        }
    }
}
