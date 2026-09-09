// src/EmployeeRegistrationApp.Domain/Entities/AuditLog.cs
using System;
using EmployeeRegistrationApp.Domain.Base;

namespace EmployeeRegistrationApp.Domain.Entities
{
    /// <summary>
    /// Registro de auditoria de ações importantes no sistema.
    /// É um AggregateRoot próprio, usado para rastreio e relatórios.
    /// </summary>
    public sealed class AuditLog : AggregateRoot
    {
        /// <summary>
        /// Momento em que a ação ocorreu.
        /// </summary>
        public DateTime Timestamp { get; private set; }

        /// <summary>
        /// Id do usuário que realizou a ação (se conhecido).
        /// </summary>
        public Guid? UserId { get; private set; }

        /// <summary>
        /// Nome do usuário na época da ação (snapshot).
        /// </summary>
        public string? UserName { get; private set; }

        /// <summary>
        /// Ação realizada (Create, Update, Delete, Login, etc.).
        /// </summary>
        public string Action { get; private set; } = string.Empty;

        /// <summary>
        /// Nome do tipo de entidade afetada (Employee, Department, Position...).
        /// </summary>
        public string EntityName { get; private set; } = string.Empty;

        /// <summary>
        /// Identificador da entidade afetada, em texto (para suportar Guid, string, etc.).
        /// </summary>
        public string EntityId { get; private set; } = string.Empty;

        /// <summary>
        /// Detalhes da mudança, geralmente em JSON ou texto estruturado.
        /// </summary>
        public string? Changes { get; private set; }

        /// <summary>
        /// Informações adicionais (IP, máquina, contexto).
        /// </summary>
        public string? Metadata { get; private set; }

        // Construtor privado para EF Core
        private AuditLog() { }

        public AuditLog(
            Guid? userId,
            string? userName,
            string action,
            string entityName,
            string entityId,
            string? changes = null,
            string? metadata = null)
        {
            Timestamp = DateTime.UtcNow;
            UserId = userId;
            UserName = userName?.Trim();
            Action = action?.Trim() ?? throw new ArgumentNullException(nameof(action));
            EntityName = entityName?.Trim() ?? throw new ArgumentNullException(nameof(entityName));
            EntityId = entityId?.Trim() ?? throw new ArgumentNullException(nameof(entityId));
            Changes = string.IsNullOrWhiteSpace(changes) ? null : changes.Trim();
            Metadata = string.IsNullOrWhiteSpace(metadata) ? null : metadata.Trim();
        }
    }
}
