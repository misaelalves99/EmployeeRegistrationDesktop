using System;
using System.Collections.Generic;
using System.Linq;
using EmployeeRegistrationApp.Domain.Base;

namespace EmployeeRegistrationApp.Domain.ValueObjects
{
    /// <summary>
    /// Value Object para telefone.
    /// Armazena apenas dígitos normalizados e oferece saída formatada.
    /// </summary>
    public sealed class PhoneNumber : ValueObject
    {
        // ✅ Compatibilidade com EF config: x.Value
        public string Value { get; private set; } = string.Empty;

        // ✅ Compatibilidade com código legado: Digits
        public string Digits => Value;

        // Construtor privado para EF
        private PhoneNumber() { }

        private PhoneNumber(string digits)
        {
            Value = digits;
        }

        public static PhoneNumber Create(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Telefone não pode ser vazio.", nameof(value));

            var digits = OnlyDigits(value);

            if (digits.Length < 8 || digits.Length > 13)
                throw new ArgumentException("Telefone deve ter entre 8 e 13 dígitos.", nameof(value));

            return new PhoneNumber(digits);
        }

        public string ToFormattedString()
        {
            if (Value.Length == 10)
            {
                var ddd = Value[..2];
                var parte1 = Value.Substring(2, 4);
                var parte2 = Value.Substring(6, 4);
                return $"({ddd}) {parte1}-{parte2}";
            }

            if (Value.Length == 11)
            {
                var ddd = Value[..2];
                var first = Value[2];
                var parte1 = Value.Substring(3, 4);
                var parte2 = Value.Substring(7, 4);
                return $"({ddd}) {first} {parte1}-{parte2}";
            }

            return Value;
        }

        public override string ToString() => ToFormattedString();

        protected override IEnumerable<object?> GetEqualityComponents()
        {
            yield return Value;
        }

        private static string OnlyDigits(string value)
            => new string(value.Where(char.IsDigit).ToArray());
    }
}
