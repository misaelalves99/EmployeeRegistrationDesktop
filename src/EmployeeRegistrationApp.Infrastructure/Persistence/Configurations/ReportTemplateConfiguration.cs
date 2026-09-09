// src/EmployeeRegistrationApp.Infrastructure/Persistence/Configurations/ReportTemplateConfiguration.cs
using EmployeeRegistrationApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EmployeeRegistrationApp.Infrastructure.Persistence.Configurations
{
    /// <summary>
    /// Configuração EF Core para a entidade ReportTemplate.
    /// 
    /// Mantida intencionalmente simples para evitar erros de compilação
    /// caso as propriedades no domínio mudem. Você pode enriquecer com
    /// mais detalhes (Name, Description, TemplateJson, etc.) depois.
    /// </summary>
    public sealed class ReportTemplateConfiguration : IEntityTypeConfiguration<ReportTemplate>
    {
        public void Configure(EntityTypeBuilder<ReportTemplate> builder)
        {
            builder.ToTable("ReportTemplates");

            builder.HasKey(r => r.Id);

            builder.Property(r => r.Id)
                .ValueGeneratedOnAdd();

            // Exemplos (descomente / ajuste se estas propriedades existirem no domínio):

            // builder.Property(r => r.Name)
            //     .HasMaxLength(200)
            //     .IsRequired();
            //
            // builder.Property(r => r.Code)
            //     .HasMaxLength(100)
            //     .IsRequired();
            //
            // builder.Property(r => r.Description)
            //     .HasMaxLength(1000);
            //
            // builder.Property(r => r.TemplateJson)
            //     .HasColumnType("nvarchar(max)");

            // Campos de auditoria opcionais (se existirem em ReportTemplate)
            // builder.Property(r => r.CreatedAtUtc).IsRequired();
            // builder.Property(r => r.UpdatedAtUtc);
        }
    }
}
