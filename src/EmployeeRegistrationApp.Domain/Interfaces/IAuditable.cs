// src/EmployeeRegistrationApp.Domain/Interfaces/IAuditable.cs
using System;

namespace EmployeeRegistrationApp.Domain.Interfaces
{
    /// <summary>
    /// Contrato para entidades auditáveis (criação e atualização).
    /// </summary>
    public interface IAuditable
    {
        DateTime CreatedAt { get; set; }
        string? CreatedBy { get; set; }

        DateTime? UpdatedAt { get; set; }
        string? UpdatedBy { get; set; }
    }
}
