using EmployeeRegistrationApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EmployeeRegistrationApp.Infrastructure.Persistence.Configurations
{
    public sealed class EmployeeConfiguration : IEntityTypeConfiguration<Employee>
    {
        public void Configure(EntityTypeBuilder<Employee> builder)
        {
            builder.ToTable("Employees");

            builder.HasKey(e => e.Id);

            builder.Property(e => e.Id)
                .ValueGeneratedOnAdd();

            builder.Property(e => e.FirstName)
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(e => e.LastName)
                .HasMaxLength(100)
                .IsRequired();

            // Calculados -> não mapear como coluna
            builder.Ignore(e => e.DisplayName);
            builder.Ignore(e => e.FullName);

            builder.Property(e => e.DateOfBirth)
                .HasColumnType("date");

            builder.Property(e => e.HireDate)
                .HasColumnType("date")
                .IsRequired();

            builder.Property(e => e.TerminationDate)
                .HasColumnType("date");

            builder.Property(e => e.EmploymentStatus)
                .HasConversion<int>()
                .IsRequired();

            builder.Property(e => e.ContractType)
                .HasConversion<int>()
                .IsRequired();

            builder.Property(e => e.JobRole)
                .HasConversion<int>()
                .IsRequired();

            builder.Property(e => e.IsActive)
                .IsRequired();

            builder.Property(e => e.CreatedAt)
                .IsRequired();

            builder.Property(e => e.UpdatedAt);

            builder.Property(e => e.CreatedBy)
                .HasMaxLength(120);

            builder.Property(e => e.UpdatedBy)
                .HasMaxLength(120);

            // --------------------------
            // Relacionamentos
            // --------------------------
            builder
                .HasOne(e => e.Department)
                .WithMany(d => d.Employees)
                .HasForeignKey(e => e.DepartmentId)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .HasOne(e => e.Position)
                .WithMany(p => p.Employees)
                .HasForeignKey(e => e.PositionId)
                .OnDelete(DeleteBehavior.Restrict);

            // --------------------------
            // Value Objects (Owned)
            // --------------------------
            builder.OwnsOne(e => e.Cpf, cpf =>
            {
                cpf.Property(x => x.Value)
                   .HasColumnName("Cpf")
                   .HasMaxLength(20)
                   .IsRequired();
            });

            builder.OwnsOne(e => e.Email, email =>
            {
                email.Property(x => x.Value)
                     .HasColumnName("Email")
                     .HasMaxLength(200)
                     .IsRequired();
            });

            builder.OwnsOne(e => e.Phone, phone =>
            {
                phone.Property(x => x.Value)
                     .HasColumnName("PhoneNumber")
                     .HasMaxLength(50);
            });

            builder.OwnsOne(e => e.Salary, salary =>
            {
                salary.Property(x => x.Amount)
                      .HasColumnName("SalaryAmount")
                      .HasColumnType("decimal(18,2)")
                      .IsRequired();

                salary.Property(x => x.Currency)
                      .HasColumnName("SalaryCurrency")
                      .HasMaxLength(10)
                      .IsRequired();
            });

            builder.Navigation(e => e.Cpf).IsRequired();
            builder.Navigation(e => e.Email).IsRequired();
            builder.Navigation(e => e.Salary).IsRequired();

            // --------------------------
            // Índices
            // --------------------------
            builder.HasIndex(e => e.IsActive);
            builder.HasIndex(e => e.DepartmentId);
            builder.HasIndex(e => e.PositionId);
            builder.HasIndex(e => e.EmploymentStatus);
            builder.HasIndex(e => e.ContractType);
            builder.HasIndex(e => e.JobRole);
        }
    }
}
