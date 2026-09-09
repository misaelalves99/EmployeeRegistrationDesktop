using System;
using System.Collections.Generic;
using System.Linq;
using EmployeeRegistrationApp.Domain.Base;

namespace EmployeeRegistrationApp.Domain.ValueObjects
{
    /// <summary>
    /// Value Object para CPF (somente Brasil).
    /// Armazena sempre apenas os dígitos (11 caracteres).
    /// </summary>
    public sealed class Cpf : ValueObject
    {
        // ✅ Compatibilidade com EF config: x.Value
        public string Value { get; private set; } = string.Empty;

        // ✅ Compatibilidade com código legado: Number
        public string Number => Value;

        // Construtor privado para EF Core
        private Cpf() { }

        private Cpf(string digits)
        {
            Value = digits;
        }

        public static Cpf Create(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("CPF não pode ser vazio.", nameof(value));

            var digits = OnlyDigits(value);

            if (digits.Length != 11)
                throw new ArgumentException("CPF deve conter 11 dígitos.", nameof(value));

            if (AllDigitsSame(digits))
                throw new ArgumentException("CPF inválido (dígitos repetidos).", nameof(value));

            if (!IsValidCpf(digits))
                throw new ArgumentException("CPF inválido.", nameof(value));

            return new Cpf(digits);
        }

        public string ToFormattedString()
        {
            if (Value.Length != 11)
                return Value;

            return $"{Value[..3]}.{Value.Substring(3, 3)}.{Value.Substring(6, 3)}-{Value.Substring(9, 2)}";
        }

        public override string ToString() => ToFormattedString();

        protected override IEnumerable<object?> GetEqualityComponents()
        {
            yield return Value;
        }

        private static string OnlyDigits(string value)
            => new string(value.Where(char.IsDigit).ToArray());

        private static bool AllDigitsSame(string value)
            => value.All(d => d == value[0]);

        private static bool IsValidCpf(string cpf)
        {
            int[] multiplicador1 = { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            int[] multiplicador2 = { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };

            var tempCpf = cpf[..9];
            int soma = 0;

            for (int i = 0; i < 9; i++)
                soma += int.Parse(tempCpf[i].ToString()) * multiplicador1[i];

            int resto = soma % 11;
            resto = resto < 2 ? 0 : 11 - resto;
            var digito = resto.ToString();

            tempCpf += digito;
            soma = 0;

            for (int i = 0; i < 10; i++)
                soma += int.Parse(tempCpf[i].ToString()) * multiplicador2[i];

            resto = soma % 11;
            resto = resto < 2 ? 0 : 11 - resto;
            digito += resto;

            return cpf.EndsWith(digito);
        }
    }
}
