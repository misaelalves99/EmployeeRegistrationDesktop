// src/EmployeeRegistrationApp.Infrastructure/Persistence/Configurations/DepartmentConfiguration.cs
using EmployeeRegistrationApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EmployeeRegistrationApp.Infrastructure.Persistence.Configurations
{
    public sealed class DepartmentConfiguration : IEntityTypeConfiguration<Department>
    {
        public void Configure(EntityTypeBuilder<Department> builder)
        {
            builder.ToTable("Departments");

            builder.HasKey(d => d.Id);

            builder.Property(d => d.Id)
                .ValueGeneratedOnAdd();

            builder.Property(d => d.Name)
                .HasMaxLength(150)
                .IsRequired();

            builder.Property(d => d.Code)
                .HasMaxLength(20);

            builder.Property(d => d.Description)
                .HasMaxLength(500);

            builder.Property(d => d.IsActive)
                .IsRequired();

            builder.Property(d => d.CreatedAt)
                .IsRequired();

            builder.Property(d => d.UpdatedAt);

            builder.Property(d => d.CreatedBy)
                .HasMaxLength(120);

            builder.Property(d => d.UpdatedBy)
                .HasMaxLength(120);

            builder
                .HasOne(d => d.Manager)
                .WithMany()
                .HasForeignKey(d => d.ManagerId)
                .OnDelete(DeleteBehavior.SetNull);

            builder
                .HasMany(d => d.Employees)
                .WithOne(e => e.Department)
                .HasForeignKey(e => e.DepartmentId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(d => d.Code);
            builder.HasIndex(d => d.Name);
            builder.HasIndex(d => d.IsActive);
        }
    }
}
