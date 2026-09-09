// src/EmployeeRegistrationApp.Infrastructure/Persistence/Configurations/CompanySettingsConfiguration.cs
using EmployeeRegistrationApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EmployeeRegistrationApp.Infrastructure.Persistence.Configurations
{
    /// <summary>
    /// Configuração EF Core para CompanySettings (de acordo com o entity real).
    /// </summary>
    public sealed class CompanySettingsConfiguration : IEntityTypeConfiguration<CompanySettings>
    {
        public void Configure(EntityTypeBuilder<CompanySettings> builder)
        {
            builder.ToTable("CompanySettings");

            builder.HasKey(c => c.Id);

            builder.Property(c => c.Id)
                .ValueGeneratedOnAdd();

            // ===== Basic Info =====
            builder.Property(c => c.CompanyName)
                .HasMaxLength(200)
                .IsRequired();

            builder.Property(c => c.LegalName)
                .HasMaxLength(200)
                .IsRequired();

            builder.Property(c => c.RegistrationNumber)
                .HasMaxLength(50);

            // ===== Contact (Value Objects) =====
            builder.OwnsOne(c => c.PrimaryEmail, email =>
            {
                email.Property(e => e.Value)
                    .HasColumnName("PrimaryEmail")
                    .HasMaxLength(200);
            });

            builder.OwnsOne(c => c.PrimaryPhone, phone =>
            {
                phone.Property(p => p.Value)
                    .HasColumnName("PrimaryPhone")
                    .HasMaxLength(50);
            });

            builder.Property(c => c.WebsiteUrl)
                .HasMaxLength(250);

            // ===== Preferences =====
            builder.Property(c => c.DefaultCurrency)
                .HasMaxLength(10)
                .IsRequired();

            builder.Property(c => c.DefaultWorkHoursPerWeek)
                .IsRequired();

            builder.Property(c => c.AllowRemoteWork)
                .IsRequired();

            // ===== Audit =====
            builder.Property(c => c.CreatedAt)
                .IsRequired();

            builder.Property(c => c.CreatedBy)
                .HasMaxLength(120);

            builder.Property(c => c.UpdatedAt);

            builder.Property(c => c.UpdatedBy)
                .HasMaxLength(120);
        }
    }
}
