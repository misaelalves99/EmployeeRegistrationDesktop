// src/EmployeeRegistrationApp.Infrastructure/Persistence/Configurations/PositionConfiguration.cs
using EmployeeRegistrationApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EmployeeRegistrationApp.Infrastructure.Persistence.Configurations
{
    /// <summary>
    /// Configuração EF Core para o agregado Position (cargo).
    /// 
    /// Mantida mais genérica de propósito para evitar conflito com
    /// nomes específicos de propriedades do seu domínio.
    /// </summary>
    public sealed class PositionConfiguration : IEntityTypeConfiguration<Position>
    {
        public void Configure(EntityTypeBuilder<Position> builder)
        {
            builder.ToTable("Positions");

            builder.HasKey(p => p.Id);

            builder.Property(p => p.Id)
                .ValueGeneratedOnAdd();

            // Se no domínio você tiver propriedades como Name/Title/Code, pode ativar:
            //
            // builder.Property(p => p.Name)
            //     .HasMaxLength(150)
            //     .IsRequired();
            //
            // builder.Property(p => p.Code)
            //     .HasMaxLength(50)
            //     .IsRequired();
            //
            // builder.Property(p => p.Description)
            //     .HasMaxLength(500);
            //
            // builder.Property(p => p.PositionType)
            //     .HasConversion<int>();
            //
            // builder.Property(p => p.IsActive)
            //     .IsRequired();

            // Relacionamento com Department (se existir no domínio):
            // builder
            //     .HasOne(p => p.Department)
            //     .WithMany(d => d.Positions)
            //     .HasForeignKey(p => p.DepartmentId)
            //     .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
