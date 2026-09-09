// src/EmployeeRegistrationApp.Domain/Base/Entity.cs
using System;

namespace EmployeeRegistrationApp.Domain.Base
{
    /// <summary>
    /// Classe base para todas as entidades do domínio.
    /// Gerencia Id e igualdade baseada na identidade.
    /// </summary>
    public abstract class Entity
    {
        public Guid Id { get; protected set; }

        protected Entity()
        {
            if (Id == Guid.Empty)
            {
                Id = Guid.NewGuid();
            }
        }

        protected Entity(Guid id)
        {
            Id = id == Guid.Empty ? Guid.NewGuid() : id;
        }

        public override bool Equals(object? obj)
        {
            if (obj is null || obj is not Entity other)
                return false;

            if (ReferenceEquals(this, obj))
                return true;

            if (GetType() != other.GetType())
                return false;

            // Entidades sem Id ainda não são consideradas iguais
            if (Id == Guid.Empty || other.Id == Guid.Empty)
                return false;

            return Id == other.Id;
        }

        public static bool operator ==(Entity? a, Entity? b)
        {
            if (a is null && b is null)
                return true;

            if (a is null || b is null)
                return false;

            return a.Equals(b);
        }

        public static bool operator !=(Entity? a, Entity? b) => !(a == b);

        public override int GetHashCode()
        {
            // Se ainda não tem Id, usa hash da classe para evitar problemas
            return Id == Guid.Empty
                ? base.GetHashCode()
                : Id.GetHashCode();
        }

        public override string ToString()
        {
            return $"{GetType().Name} [Id={Id}]";
        }
    }
}
