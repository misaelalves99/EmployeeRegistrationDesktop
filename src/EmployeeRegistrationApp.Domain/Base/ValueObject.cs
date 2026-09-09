// src/EmployeeRegistrationApp.Domain/Base/ValueObject.cs
using System;
using System.Collections.Generic;
using System.Linq;

namespace EmployeeRegistrationApp.Domain.Base
{
    /// <summary>
    /// Classe base para Value Objects (DDD).
    /// Igualdade baseada nos valores dos componentes.
    /// </summary>
    public abstract class ValueObject
    {
        /// <summary>
        /// Retorna os componentes que definem a igualdade do VO.
        /// Cada ValueObject concreto deve implementar isso.
        /// </summary>
        /// <returns>Sequência de componentes usados na igualdade.</returns>
        protected abstract IEnumerable<object?> GetEqualityComponents();

        public override bool Equals(object? obj)
        {
            if (obj is null || obj.GetType() != GetType())
                return false;

            var other = (ValueObject)obj;

            return GetEqualityComponents()
                .SequenceEqual(other.GetEqualityComponents());
        }

        public override int GetHashCode()
        {
            return GetEqualityComponents()
                .Where(x => x is not null)
                .Aggregate(17, (current, obj) =>
                {
                    unchecked
                    {
                        return current * 23 + obj!.GetHashCode();
                    }
                });
        }

        public static bool operator ==(ValueObject? a, ValueObject? b)
        {
            if (a is null && b is null)
                return true;

            if (a is null || b is null)
                return false;

            return a.Equals(b);
        }

        public static bool operator !=(ValueObject? a, ValueObject? b) => !(a == b);

        /// <summary>
        /// Faz uma cópia rasa do ValueObject.
        /// Útil para cenários de imutabilidade controlada.
        /// </summary>
        public ValueObject GetCopy() => (ValueObject)MemberwiseClone();
    }
}
