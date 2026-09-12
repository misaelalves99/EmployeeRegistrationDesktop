// src/EmployeeRegistrationApp.Infrastructure/Persistence/Configurations/UserAccountConfiguration.cs
using EmployeeRegistrationApp.Domain.Entities;
using EmployeeRegistrationApp.Domain.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EmployeeRegistrationApp.Infrastructure.Persistence.Configurations
{
    /// <summary>
    /// Configuração EF Core para a entidade UserAccount.
    /// 
    /// Esta entidade representa um usuário de negócio (RH/Aplicação),
    /// podendo ter vínculo com o usuário do Identity.
    /// </summary>
    public sealed class UserAccountConfiguration : IEntityTypeConfiguration<UserAccount>
    {
        public void Configure(EntityTypeBuilder<UserAccount> builder)
        {
            builder.ToTable("UserAccounts");

            builder.HasKey(u => u.Id);

            builder.Property(u => u.Id)
                .ValueGeneratedOnAdd();

            // Propriedades básicas (ajuste conforme a entidade real):
            // Exemplo: se existir UserName, Email, IsActive etc.

            // if (builder.Metadata.FindProperty(nameof(UserAccount.UserName)) is not null)
            // {
            //     builder.Property(u => u.UserName)
            //         .HasMaxLength(200)
            //         .IsRequired();
            // }

            // if (builder.Metadata.FindProperty(nameof(UserAccount.IsActive)) is not null)
            // {
            //     builder.Property(u => u.IsActive)
            //         .IsRequired();
            // }

            // Se tiver relacionamento com ApplicationUser (Identity), configure aqui.
            // Comentei para não quebrar compilação caso a propriedade não exista.
        }
    }
}
