// src/EmployeeRegistrationApp.Maui/Presentation/Converters/BoolToColorConverter.cs
using System;
using System.Globalization;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;

namespace EmployeeRegistrationApp.Maui.Presentation.Converters
{
    /// <summary>
    /// Converte um bool em uma cor.
    /// Exemplo:
    ///  true  → verde (sucesso)
    ///  false → cinza (neutro)
    /// Pode ser configurado via XAML definindo as propriedades TrueColor/FalseColor.
    /// </summary>
    public sealed class BoolToColorConverter : IValueConverter
    {
        public Color TrueColor { get; set; } = Colors.LimeGreen;
        public Color FalseColor { get; set; } = Colors.DimGray;
        public Color NullColor { get; set; } = Colors.Transparent;

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool b)
            {
                return b ? TrueColor : FalseColor;
            }

            return NullColor;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // Normalmente não usamos ConvertBack em cores, então retornamos Binding.DoNothing
            return Binding.DoNothing;
        }
    }
}
