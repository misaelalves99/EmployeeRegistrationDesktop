// src/EmployeeRegistrationApp.Maui/Presentation/Converters/MoneyToCurrencyStringConverter.cs
using System;
using System.Globalization;
using Microsoft.Maui.Controls;
using EmployeeRegistrationApp.Domain.ValueObjects;

namespace EmployeeRegistrationApp.Maui.Presentation.Converters
{
    /// <summary>
    /// Converte o Value Object Money ou um decimal em string de moeda.
    /// Padrão pensado para BRL (R$) em pt-BR, mas configurável via propriedades.
    /// </summary>
    public sealed class MoneyToCurrencyStringConverter : IValueConverter
    {
        /// <summary>
        /// Cultura usada para formatar o valor. Padrão: "pt-BR".
        /// </summary>
        public string CultureName { get; set; } = "pt-BR";

        /// <summary>
        /// Código de moeda padrão (usado quando não vier do VO).
        /// Ex.: "BRL".
        /// </summary>
        public string DefaultCurrencyCode { get; set; } = "BRL";

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is null)
                return string.Empty;

            var targetCulture = new CultureInfo(CultureName);

            // Caso 1: Value Object Money
            if (value is Money money)
            {
                var currencyCode = !string.IsNullOrWhiteSpace(money.CurrencyCode)
                    ? money.CurrencyCode
                    : DefaultCurrencyCode;

                // Se for BRL, usa formatação padrão de moeda da cultura pt-BR
                if (string.Equals(currencyCode, "BRL", StringComparison.OrdinalIgnoreCase))
                {
                    return string.Format(targetCulture, "{0:C}", money.Amount);
                }

                // Outros códigos de moeda podem ser prefixados manualmente
                return $"{currencyCode} {money.Amount.ToString("N2", targetCulture)}";
            }

            // Caso 2: decimal / double simples
            if (value is decimal d)
            {
                return string.Format(targetCulture, "{0:C}", d);
            }

            if (value is double dbl)
            {
                return string.Format(targetCulture, "{0:C}", (decimal)dbl);
            }

            // Outro tipo: tenta converter para decimal
            if (decimal.TryParse(Convert.ToString(value, CultureInfo.InvariantCulture), NumberStyles.Number, CultureInfo.InvariantCulture, out var parsed))
            {
                return string.Format(targetCulture, "{0:C}", parsed);
            }

            return string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // Se você precisar converter o texto de volta em Money/decimal,
            // pode implementar aqui. Por enquanto deixamos como DoNothing.
            return Binding.DoNothing;
        }
    }
}
