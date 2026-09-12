// src/EmployeeRegistrationApp.Infrastructure/Persistence/Configurations/AuditLogConfiguration.cs
using EmployeeRegistrationApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EmployeeRegistrationApp.Infrastructure.Persistence.Configurations
{
    /// <summary>
    /// Configuração EF Core para a entidade de auditoria (AuditLog).
    /// Registra ações realizadas no sistema (CRUD, login, etc.).
    /// </summary>
    public sealed class AuditLogConfiguration : IEntityTypeConfiguration<AuditLog>
    {
        public void Configure(EntityTypeBuilder<AuditLog> builder)
        {
            builder.ToTable("AuditLogs");

            builder.HasKey(a => a.Id);

            builder.Property(a => a.Id)
                .ValueGeneratedOnAdd();

            builder.Property(a => a.Timestamp)
                .IsRequired();

            builder.Property(a => a.UserName)
                .HasMaxLength(200);

            builder.Property(a => a.Action)
                .HasMaxLength(200)
                .IsRequired();

            builder.Property(a => a.EntityName)
                .HasMaxLength(200);

            builder.Property(a => a.EntityId)
                .HasMaxLength(100);

            builder.Property(a => a.Changes)
                .HasColumnType("TEXT"); // JSON / texto grande (SQLite / SQL Server)

            builder.Property(a => a.Metadata)
                .HasColumnType("TEXT");

            // Índices para facilitar consultas por data e usuário
            builder.HasIndex(a => a.Timestamp);
            builder.HasIndex(a => a.UserName);
            builder.HasIndex(a => a.EntityName);
        }
    }
}
