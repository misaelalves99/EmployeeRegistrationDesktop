// src/EmployeeRegistrationApp.Maui/Presentation/Converters/BoolToVisibilityConverter.cs
using System;
using System.Globalization;
using Microsoft.Maui.Controls;

namespace EmployeeRegistrationApp.Maui.Presentation.Converters
{
    /// <summary>
    /// Converte um bool em visibilidade (true = visível, false = oculto).
    /// Pode ser usado diretamente em <ContentView IsVisible="{Binding AlgumaFlag, Converter={StaticResource BoolToVisibilityConverter}}" />
    /// </summary>
    public sealed class BoolToVisibilityConverter : IValueConverter
    {
        /// <summary>
        /// Se true, inverte o valor (true → false, false → true).
        /// </summary>
        public bool Invert { get; set; }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var boolValue = false;

            if (value is bool b)
            {
                boolValue = b;
            }

            if (Invert)
            {
                boolValue = !boolValue;
            }

            // Para propriedades IsVisible / IsEnabled, o destino também é bool
            if (targetType == typeof(bool) || targetType == typeof(bool?))
            {
                return boolValue;
            }

            // Se, por algum motivo, for usado em uma propriedade de visibilidade customizada,
            // podemos devolver o próprio bool e deixar o XAML decidir.
            return boolValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool b)
            {
                return Invert ? !b : b;
            }

            return false;
        }
    }
}
