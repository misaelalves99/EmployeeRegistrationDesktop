// src/EmployeeRegistrationApp.Maui/Presentation/Converters/ContractTypeToStringConverter.cs
using System;
using System.Collections.Generic;
using System.Globalization;
using Microsoft.Maui.Controls;

namespace EmployeeRegistrationApp.Maui.Presentation.Converters
{
    /// <summary>
    /// Converte um ContractType (enum) para uma descrição amigável em português.
    /// O mapeamento é feito por string (enum.ToString()), ou seja, funciona mesmo
    /// que você ajuste o enum no domínio.
    /// </summary>
    public sealed class ContractTypeToStringConverter : IValueConverter
    {
        private readonly IDictionary<string, string> _labels =
            new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                // Ajuste as chaves para os nomes reais do seu enum ContractType
                // e os valores para o texto exibido na UI.
                ["CLT"] = "CLT (Regime CLT)",
                ["PJ"] = "PJ (Pessoa Jurídica)",
                ["Intern"] = "Estágio",
                ["Temporary"] = "Temporário",
                ["Freelancer"] = "Freelancer"
            };

        /// <summary>
        /// Texto padrão caso não haja mapeamento.
        /// </summary>
        public string DefaultLabel { get; set; } = "Não informado";

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is null)
                return DefaultLabel;

            var key = value.ToString() ?? string.Empty;

            if (_labels.TryGetValue(key, out var label))
            {
                return label;
            }

            // Se não achar no dicionário, devolve o próprio nome do enum ou o padrão
            return string.IsNullOrWhiteSpace(key) ? DefaultLabel : key;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // Geralmente a UI manda o enum de volta direto, então não fazemos o caminho inverso aqui.
            return Binding.DoNothing;
        }
    }
}
