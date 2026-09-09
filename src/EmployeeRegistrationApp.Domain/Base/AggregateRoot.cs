// src/EmployeeRegistrationApp.Domain/Base/AggregateRoot.cs
using System;

namespace EmployeeRegistrationApp.Domain.Base
{
    /// <summary>
    /// Classe base para raízes de agregados.
    /// Combina Entity + IAggregateRoot.
    /// </summary>
    public abstract class AggregateRoot : Entity, IAggregateRoot
    {
        protected AggregateRoot()
        {
        }

        protected AggregateRoot(Guid id) : base(id)
        {
        }
    }
}
