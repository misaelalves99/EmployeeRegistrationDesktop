// src/EmployeeRegistrationApp.Infrastructure/Identity/IdentityConfig.cs
using System;
using EmployeeRegistrationApp.Infrastructure.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace EmployeeRegistrationApp.Infrastructure.Identity
{
    /// <summary>
    /// Configuração do .NET Identity para o cenário Desktop (local).
    /// 
    /// Aqui registramos:
    /// - ApplicationUser (usuário local)
    /// - IdentityRole&lt;Guid&gt;
    /// - EntityFramework stores usando AppDbContext (SQLite)
    /// 
    /// Não há configuração de cookies HTTP nem middleware web,
    /// pois o host é uma aplicação .NET MAUI Desktop.
    /// </summary>
    public static class IdentityConfig
    {
        public static IServiceCollection AddIdentityConfig(this IServiceCollection services)
        {
            // Identity Core para usuários locais (sem pipeline HTTP)
            services
                .AddIdentityCore<ApplicationUser>(options =>
                {
                    // Regras de senha e usuário — pode ajustar como quiser
                    options.User.RequireUniqueEmail = true;

                    options.Password.RequiredLength = 8;
                    options.Password.RequireDigit = true;
                    options.Password.RequireLowercase = true;
                    options.Password.RequireUppercase = false;
                    options.Password.RequireNonAlphanumeric = false;

                    options.Lockout.MaxFailedAccessAttempts = 5;
                    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                    options.Lockout.AllowedForNewUsers = true;
                })
                .AddRoles<IdentityRole<Guid>>()          // Suporte a perfis (Admin, HR, Manager, etc.)
                .AddEntityFrameworkStores<AppDbContext>() // Usa o DbContext de Infraestrutura (SQLite)
                .AddSignInManager()                       // SignInManager para validar login/senha
                .AddDefaultTokenProviders();              // Tokens (reset de senha, etc.)

            return services;
        }
    }
}
