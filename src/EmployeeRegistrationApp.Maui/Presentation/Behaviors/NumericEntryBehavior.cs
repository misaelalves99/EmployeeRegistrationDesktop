// src/EmployeeRegistrationApp.Maui/Presentation/Behaviors/NumericEntryBehavior.cs
using System;
using System.Linq;
using Microsoft.Maui.Controls;

namespace EmployeeRegistrationApp.Maui.Presentation.Behaviors
{
    /// <summary>
    /// Behavior para forçar que o Entry aceite apenas números.
    /// Útil para campos de salário, quantidade, código, etc.
    ///
    /// Exemplo de uso:
    /// <Entry Placeholder="Salário">
    ///     <Entry.Behaviors>
    ///         <behaviors:NumericEntryBehavior AllowDecimal="True"
    ///                                        MaxIntegerDigits="10"
    ///                                        MaxDecimalPlaces="2" />
    ///     </Entry.Behaviors>
    /// </Entry>
    /// </summary>
    public class NumericEntryBehavior : Behavior<Entry>
    {
        private bool _isUpdating;

        public static readonly BindableProperty AllowDecimalProperty =
            BindableProperty.Create(
                nameof(AllowDecimal),
                typeof(bool),
                typeof(NumericEntryBehavior),
                true);

        /// <summary>
        /// Se true, permite separador decimal.
        /// </summary>
        public bool AllowDecimal
        {
            get => (bool)GetValue(AllowDecimalProperty);
            set => SetValue(AllowDecimalProperty, value);
        }

        public static readonly BindableProperty MaxIntegerDigitsProperty =
            BindableProperty.Create(
                nameof(MaxIntegerDigits),
                typeof(int),
                typeof(NumericEntryBehavior),
                10);

        /// <summary>
        /// Quantidade máxima de dígitos antes da vírgula/ponto.
        /// </summary>
        public int MaxIntegerDigits
        {
            get => (int)GetValue(MaxIntegerDigitsProperty);
            set => SetValue(MaxIntegerDigitsProperty, value);
        }

        public static readonly BindableProperty MaxDecimalPlacesProperty =
            BindableProperty.Create(
                nameof(MaxDecimalPlaces),
                typeof(int),
                typeof(NumericEntryBehavior),
                2);

        /// <summary>
        /// Quantidade máxima de casas decimais permitidas.
        /// Somente usado quando AllowDecimal = true.
        /// </summary>
        public int MaxDecimalPlaces
        {
            get => (int)GetValue(MaxDecimalPlacesProperty);
            set => SetValue(MaxDecimalPlacesProperty, value);
        }

        protected override void OnAttachedTo(Entry bindable)
        {
            base.OnAttachedTo(bindable);
            bindable.Keyboard = Keyboard.Numeric;
            bindable.TextChanged += OnEntryTextChanged;
        }

        protected override void OnDetachingFrom(Entry bindable)
        {
            base.OnDetachingFrom(bindable);
            bindable.TextChanged -= OnEntryTextChanged;
        }

        private void OnEntryTextChanged(object? sender, TextChangedEventArgs e)
        {
            if (_isUpdating)
                return;

            if (sender is not Entry entry)
                return;

            _isUpdating = true;

            try
            {
                string? text = e.NewTextValue ?? string.Empty;
                string normalized = FilterNumeric(text, AllowDecimal, MaxIntegerDigits, MaxDecimalPlaces);
                entry.Text = normalized;
            }
            finally
            {
                _isUpdating = false;
            }
        }

        private static string FilterNumeric(string input, bool allowDecimal, int maxInteger, int maxDecimal)
        {
            if (string.IsNullOrEmpty(input))
                return string.Empty;

            // Normaliza separador para ponto (cultura neutra para armazenar)
            char decimalSeparator = '.';

            // Remove tudo que não for dígito ou separador (se permitido)
            var allowedChars = input.Where(c =>
                char.IsDigit(c) ||
                (allowDecimal && (c == '.' || c == ',')));

            string cleaned = new string(allowedChars.ToArray());

            if (string.IsNullOrEmpty(cleaned))
                return string.Empty;

            if (!allowDecimal)
            {
                // Só dígitos
                string digitsOnly = new string(cleaned.Where(char.IsDigit).ToArray());
                if (maxInteger > 0 && digitsOnly.Length > maxInteger)
                    digitsOnly = digitsOnly[..maxInteger];

                return digitsOnly;
            }

            // Trata decimal: garante no máximo um separador e limita casas
            cleaned = cleaned.Replace(',', decimalSeparator);
            int separatorIndex = cleaned.IndexOf(decimalSeparator);

            if (separatorIndex < 0)
            {
                // Só parte inteira
                string integerPart = cleaned;
                integerPart = new string(integerPart.Where(char.IsDigit).ToArray());

                if (maxInteger > 0 && integerPart.Length > maxInteger)
                    integerPart = integerPart[..maxInteger];

                return integerPart;
            }

            string rawInteger = cleaned[..separatorIndex];
            string rawDecimal = cleaned[(separatorIndex + 1)..];

            rawInteger = new string(rawInteger.Where(char.IsDigit).ToArray());
            rawDecimal = new string(rawDecimal.Where(char.IsDigit).ToArray());

            if (maxInteger > 0 && rawInteger.Length > maxInteger)
                rawInteger = rawInteger[..maxInteger];

            if (maxDecimal >= 0 && rawDecimal.Length > maxDecimal)
                rawDecimal = rawDecimal[..maxDecimal];

            if (string.IsNullOrEmpty(rawDecimal))
                return rawInteger; // evita "123."

            return $"{rawInteger}{decimalSeparator}{rawDecimal}";
        }
    }
}
