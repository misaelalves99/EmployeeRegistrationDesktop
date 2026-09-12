// src/EmployeeRegistrationApp.Maui/Presentation/Common/Controls/MetricCard.xaml.cs
using Microsoft.Maui.Controls;

namespace EmployeeRegistrationApp.Maui.Presentation.Common.Controls
{
    public partial class MetricCard : ContentView
    {
        public MetricCard()
        {
            InitializeComponent();
            UpdateTrendVisual();
        }

        // ========== Bindable Properties ==========

        public static readonly BindableProperty TitleProperty =
            BindableProperty.Create(
                nameof(Title),
                typeof(string),
                typeof(MetricCard),
                default(string));

        public static readonly BindableProperty SubtitleProperty =
            BindableProperty.Create(
                nameof(Subtitle),
                typeof(string),
                typeof(MetricCard),
                default(string));

        public static readonly BindableProperty ValueProperty =
            BindableProperty.Create(
                nameof(Value),
                typeof(string),
                typeof(MetricCard),
                default(string));

        public static readonly BindableProperty IconProperty =
            BindableProperty.Create(
                nameof(Icon),
                typeof(string),
                typeof(MetricCard),
                "??");


        public static readonly BindableProperty AccentColorProperty =
            BindableProperty.Create(
                nameof(AccentColor),
                typeof(Microsoft.Maui.Graphics.Color),
                typeof(MetricCard),
                Microsoft.Maui.Graphics.Color.FromArgb("#6b7280"));
public static readonly BindableProperty TrendTextProperty =
            BindableProperty.Create(
                nameof(TrendText),
                typeof(string),
                typeof(MetricCard),
                default(string),
                propertyChanged: OnTrendChanged);

        public static readonly BindableProperty IsTrendPositiveProperty =
            BindableProperty.Create(
                nameof(IsTrendPositive),
                typeof(bool?),
                typeof(MetricCard),
                null,
                propertyChanged: OnTrendChanged);

        // ========== Propriedades p�blicas ==========

        public string Title
        {
            get => (string)GetValue(TitleProperty);
            set => SetValue(TitleProperty, value);
        }

        public string Subtitle
        {
            get => (string)GetValue(SubtitleProperty);
            set => SetValue(SubtitleProperty, value);
        }

        public string Value
        {
            get => (string)GetValue(ValueProperty);
            set => SetValue(ValueProperty, value);
        }

        public string Icon
        {
            get => (string)GetValue(IconProperty);
            set => SetValue(IconProperty, value);
        }


        public Microsoft.Maui.Graphics.Color AccentColor
        {
            get => (Microsoft.Maui.Graphics.Color)GetValue(AccentColorProperty);
            set => SetValue(AccentColorProperty, value);
        }
/// <summary>
        /// Texto de tend�ncia (ex.: "+12% vs m�s anterior").
        /// </summary>
        public string TrendText
        {
            get => (string)GetValue(TrendTextProperty);
            set => SetValue(TrendTextProperty, value);
        }

        /// <summary>
        /// true = positivo (verde), false = negativo (vermelho), null = neutro.
        /// </summary>
        public bool? IsTrendPositive
        {
            get => (bool?)GetValue(IsTrendPositiveProperty);
            set => SetValue(IsTrendPositiveProperty, value);
        }

        // ========== Internals ==========

        private static void OnTrendChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is MetricCard card)
            {
                card.UpdateTrendVisual();
            }
        }

        private void UpdateTrendVisual()
        {
            if (string.IsNullOrWhiteSpace(TrendText))
            {
                TrendLabel.IsVisible = false;
                return;
            }

            TrendLabel.IsVisible = true;
            TrendLabel.Text = TrendText;

            if (IsTrendPositive == true)
            {
                TrendLabel.TextColor = Color.FromArgb("#22c55e"); // verde
            }
            else if (IsTrendPositive == false)
            {
                TrendLabel.TextColor = Color.FromArgb("#ef4444"); // vermelho
            }
            else
            {
                TrendLabel.TextColor = Color.FromArgb("#9ca3af"); // neutro
            }
        }
    }
}
