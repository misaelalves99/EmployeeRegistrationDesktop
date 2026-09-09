// src/EmployeeRegistrationApp.Maui/Presentation/Behaviors/MaskedEntryBehavior.cs
using System;
using System.Linq;
using Microsoft.Maui.Controls;

namespace EmployeeRegistrationApp.Maui.Presentation.Behaviors
{
    /// <summary>
    /// Behavior genérico de máscara para Entry.
    /// Pensado para CPF, CNPJ, telefone, CEP etc.
    ///
    /// Exemplo de uso em XAML:
    /// <Entry Placeholder="CPF">
    ///     <Entry.Behaviors>
    ///         <behaviors:MaskedEntryBehavior Mask="000.000.000-00" />
    ///     </Entry.Behaviors>
    /// </Entry>
    /// </summary>
    public class MaskedEntryBehavior : Behavior<Entry>
    {
        private bool _isUpdating;

        public static readonly BindableProperty MaskProperty =
            BindableProperty.Create(
                nameof(Mask),
                typeof(string),
                typeof(MaskedEntryBehavior),
                default(string));

        /// <summary>
        /// Máscara no formato de padrão, onde '0' representa dígitos.
        /// Exemplos:
        /// - CPF: 000.000.000-00
        /// - CNPJ: 00.000.000/0000-00
        /// - Telefone: (00) 00000-0000
        /// </summary>
        public string? Mask
        {
            get => (string?)GetValue(MaskProperty);
            set => SetValue(MaskProperty, value);
        }

        public static readonly BindableProperty AllowOnlyDigitsProperty =
            BindableProperty.Create(
                nameof(AllowOnlyDigits),
                typeof(bool),
                typeof(MaskedEntryBehavior),
                true);

        /// <summary>
        /// Se true, remove tudo que não for dígito antes de aplicar a máscara.
        /// </summary>
        public bool AllowOnlyDigits
        {
            get => (bool)GetValue(AllowOnlyDigitsProperty);
            set => SetValue(AllowOnlyDigitsProperty, value);
        }

        protected override void OnAttachedTo(Entry bindable)
        {
            base.OnAttachedTo(bindable);
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

            if (string.IsNullOrWhiteSpace(Mask))
                return;

            _isUpdating = true;

            try
            {
                string? newText = e.NewTextValue ?? string.Empty;
                string masked = ApplyMask(newText, Mask!, AllowOnlyDigits);

                // Mantém o cursor no fim do texto
                entry.Text = masked;
            }
            finally
            {
                _isUpdating = false;
            }
        }

        private static string ApplyMask(string value, string mask, bool allowOnlyDigits)
        {
            if (string.IsNullOrEmpty(value))
                return string.Empty;

            // Remove caracteres não desejados (se configurado)
            if (allowOnlyDigits)
            {
                value = new string(value.Where(char.IsDigit).ToArray());
            }

            if (string.IsNullOrEmpty(value))
                return string.Empty;

            var result = string.Empty;
            var valueIndex = 0;

            for (int i = 0; i < mask.Length; i++)
            {
                if (valueIndex >= value.Length)
                    break;

                char maskChar = mask[i];

                if (maskChar == '0')
                {
                    // Pega o próximo dígito disponível
                    char c = value[valueIndex];

                    // Garante que é dígito
                    if (!char.IsDigit(c))
                    {
                        valueIndex++;
                        i--;
                        continue;
                    }

                    result += c;
                    valueIndex++;
                }
                else
                {
                    // Caracter fixo da máscara
                    result += maskChar;
                }
            }

            return result;
        }
    }
}
