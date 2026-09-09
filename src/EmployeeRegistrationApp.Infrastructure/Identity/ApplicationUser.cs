// src/EmployeeRegistrationApp.Infrastructure/Identity/ApplicationUser.cs
using System;
using Microsoft.AspNetCore.Identity;

namespace EmployeeRegistrationApp.Infrastructure.Identity
{
    /// <summary>
    /// Usuário de identidade usado pelo .NET Identity na camada de Infraestrutura.
    /// É o "usuário técnico" usado para autenticação, tokens e segurança.
    /// </summary>
    public sealed class ApplicationUser : IdentityUser<Guid>
    {
        /// <summary>
        /// Nome completo exibido na UI (dashboard, topbar, etc.).
        /// </summary>
        public string FullName { get; set; } = string.Empty;

        /// <summary>
        /// Indica se a conta está ativa (para bloquear login de usuários desativados).
        /// </summary>
        public bool IsActive { get; set; } = true;

        /// <summary>
        /// Chave da entidade de domínio UserAccount (camada Domain),
        /// caso você queira manter uma separação clara entre
        /// "conta de login" e "usuário de negócio".
        /// </summary>
        public Guid? UserAccountId { get; set; }
    }
}
