using System;
using EmployeeRegistrationApp.Domain.Base;
using EmployeeRegistrationApp.Domain.Entities;
using EmployeeRegistrationApp.Domain.Interfaces;
using EmployeeRegistrationApp.Domain.ValueObjects;

namespace EmployeeRegistrationApp.Domain.Users
{
    /// <summary>
    /// Conta de usuário vinculada ao Identity e ao funcionário.
    /// Representa o usuário de aplicação em nível de domínio.
    /// </summary>
    public sealed class UserAccount : AggregateRoot, IAuditable
    {
        /// <summary>
        /// Nome de usuário (login) exibido na aplicação.
        /// </summary>
        public string UserName { get; private set; } = string.Empty;

        /// <summary>
        /// Email principal para login/recuperação.
        /// </summary>
        public Email Email { get; private set; } = null!;

        /// <summary>
        /// Id do usuário no ASP.NET Identity (ApplicationUser).
        /// </summary>
        public string IdentityUserId { get; private set; } = string.Empty;

        /// <summary>
        /// Relacionamento com funcionário (opcional).
        /// </summary>
        public Guid? EmployeeId { get; private set; }
        public Employee? Employee { get; private set; }

        /// <summary>
        /// Papel principal (Admin, Manager, HR, Employee, etc.).
        /// Você pode alinhar isso com roles do Identity.
        /// </summary>
        public string Role { get; private set; } = "Employee";

        public bool IsActive { get; private set; } = true;

        // Auditoria
        public DateTime CreatedAt { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string? UpdatedBy { get; set; }

        public DateTime? LastLoginAt { get; private set; }

        // Construtor privado para EF
        private UserAccount() { }

        public UserAccount(
            string userName,
            Email email,
            string identityUserId,
            string role,
            Guid? employeeId = null)
        {
            UpdateBasicInfo(userName, email, role);
            SetIdentityUser(identityUserId);
            LinkToEmployee(employeeId);

            CreatedAt = DateTime.UtcNow;
        }

        public void UpdateBasicInfo(string userName, Email email, string role)
        {
            if (string.IsNullOrWhiteSpace(userName))
                throw new ArgumentException("UserName é obrigatório.", nameof(userName));

            if (string.IsNullOrWhiteSpace(role))
                throw new ArgumentException("Role é obrigatória.", nameof(role));

            UserName = userName.Trim();
            Email = email ?? throw new ArgumentNullException(nameof(email));
            Role = role.Trim();
            Touch();
        }

        public void SetIdentityUser(string identityUserId)
        {
            if (string.IsNullOrWhiteSpace(identityUserId))
                throw new ArgumentException("IdentityUserId é obrigatório.", nameof(identityUserId));

            IdentityUserId = identityUserId.Trim();
            Touch();
        }

        public void LinkToEmployee(Guid? employeeId)
        {
            EmployeeId = employeeId;
            Touch();
        }

        public void SetActive(bool isActive)
        {
            IsActive = isActive;
            Touch();
        }

        public void RegisterLogin()
        {
            LastLoginAt = DateTime.UtcNow;
            Touch();
        }

        private void Touch()
        {
            UpdatedAt = DateTime.UtcNow;
        }

        public void TouchBy(string userName)
        {
            Touch();
            UpdatedBy = userName;
        }
    }
}
