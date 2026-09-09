// src/EmployeeRegistrationApp.Maui/Presentation/Converters/EmploymentStatusToColorConverter.cs
using System;
using System.Collections.Generic;
using System.Globalization;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;

namespace EmployeeRegistrationApp.Maui.Presentation.Converters
{
    /// <summary>
    /// Converte um EmploymentStatus (enum) em uma cor.
    /// Não depende diretamente dos membros do enum: usa o nome da enum (ToString)
    /// para mapear, então funciona mesmo se você alterar o enum depois.
    /// </summary>
    public sealed class EmploymentStatusToColorConverter : IValueConverter
    {
        private readonly IDictionary<string, Color> _statusColors =
            new Dictionary<string, Color>(StringComparer.OrdinalIgnoreCase)
            {
                // Ajuste esses nomes de chave para combinarem com os nomes reais do seu enum EmploymentStatus.
                // Ex.: EmploymentStatus.Active.ToString() → "Active"
                ["Active"] = Colors.LimeGreen,
                ["Inactive"] = Colors.Red,
                ["OnLeave"] = Colors.Orange,
                ["Suspended"] = Colors.OrangeRed,
                ["Terminated"] = Colors.DarkRed
            };

        public Color DefaultColor { get; set; } = Colors.SlateGray;

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is null)
                return DefaultColor;

            var key = value.ToString() ?? string.Empty;

            if (_statusColors.TryGetValue(key, out var color))
            {
                return color;
            }

            return DefaultColor;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // Em geral não há conversão de volta de cor para status
            return Binding.DoNothing;
        }
    }
}
