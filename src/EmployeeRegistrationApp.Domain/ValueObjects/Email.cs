using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using EmployeeRegistrationApp.Domain.Base;

namespace EmployeeRegistrationApp.Domain.ValueObjects
{
    /// <summary>
    /// Value Object para e-mail corporativo/pessoal.
    /// Armazena sempre em minúsculas e trimado.
    /// </summary>
    public sealed class Email : ValueObject
    {
        private static readonly Regex EmailRegex = new(
            @"^[^@\s]+@[^@\s]+\.[^@\s]+$",
            RegexOptions.Compiled | RegexOptions.IgnoreCase);

        // ✅ Compatibilidade com EF config: x.Value
        public string Value { get; private set; } = string.Empty;

        // ✅ Compatibilidade com código legado: Address
        public string Address => Value;

        // Construtor privado para EF
        private Email() { }

        private Email(string address)
        {
            Value = address;
        }

        public static Email Create(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("E-mail não pode ser vazio.", nameof(value));

            var normalized = value.Trim().ToLowerInvariant();

            if (!EmailRegex.IsMatch(normalized))
                throw new ArgumentException("E-mail em formato inválido.", nameof(value));

            return new Email(normalized);
        }

        public override string ToString() => Address;

        protected override IEnumerable<object?> GetEqualityComponents()
        {
            yield return Value;
        }
    }
}
