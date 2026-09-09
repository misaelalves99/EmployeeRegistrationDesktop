// src/EmployeeRegistrationApp.Domain/ValueObjects/Money.cs
using System;
using System.Collections.Generic;
using EmployeeRegistrationApp.Domain.Base;

namespace EmployeeRegistrationApp.Domain.ValueObjects
{
    /// <summary>
    /// Value Object monetário, com valor e moeda.
    /// Por padrão considera BRL, mas permite outras moedas se necessário.
    /// Compatível com o converter de UI (MoneyToCurrencyStringConverter),
    /// expondo também a propriedade CurrencyCode.
    /// </summary>
    public sealed class Money : ValueObject
    {
        /// <summary>
        /// Valor numérico do dinheiro.
        /// </summary>
        public decimal Amount { get; }

        /// <summary>
        /// Código da moeda (ex.: BRL, USD).
        /// </summary>
        public string Currency { get; }

        /// <summary>
        /// Alias usado pela camada de apresentação (ex.: CurrencyCode).
        /// </summary>
        public string CurrencyCode => Currency;

        // Construtor privado para EF / serialização
        private Money()
        {
            Amount = 0m;
            Currency = "BRL";
        }

        private Money(decimal amount, string currency)
        {
            if (string.IsNullOrWhiteSpace(currency))
                throw new ArgumentException("A moeda não pode ser vazia.", nameof(currency));

            Amount = amount;
            Currency = currency.Trim().ToUpperInvariant();
        }

        /// <summary>
        /// Fábrica estática principal.
        /// </summary>
        public static Money FromDecimal(decimal amount, string currency = "BRL")
            => new(amount, currency);

        /// <summary>
        /// Soma dois valores monetários da mesma moeda.
        /// </summary>
        public Money Add(Money other)
        {
            EnsureSameCurrency(other);
            return new Money(Amount + other.Amount, Currency);
        }

        /// <summary>
        /// Subtrai dois valores monetários da mesma moeda.
        /// </summary>
        public Money Subtract(Money other)
        {
            EnsureSameCurrency(other);
            return new Money(Amount - other.Amount, Currency);
        }

        /// <summary>
        /// Multiplica o valor por um fator (ex.: reajuste).
        /// </summary>
        public Money Multiply(decimal factor)
        {
            return new Money(Amount * factor, Currency);
        }

        public bool IsNegative() => Amount < 0;
        public bool IsZero() => Amount == 0;
        public bool IsPositive() => Amount > 0;

        private void EnsureSameCurrency(Money other)
        {
            if (other is null)
                throw new ArgumentNullException(nameof(other));

            if (!string.Equals(Currency, other.Currency, StringComparison.OrdinalIgnoreCase))
                throw new InvalidOperationException("Não é possível operar valores com moedas diferentes.");
        }

        public override string ToString()
        {
            // Simples: você pode customizar com CultureInfo se quiser
            return $"{Currency} {Amount:N2}";
        }

        protected override IEnumerable<object?> GetEqualityComponents()
        {
            yield return Amount;
            yield return Currency;
        }
    }
}
