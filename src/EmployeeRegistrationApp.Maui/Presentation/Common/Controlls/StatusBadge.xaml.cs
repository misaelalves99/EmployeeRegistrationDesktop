// src/EmployeeRegistrationApp.Maui/Presentation/Common/Controls/StatusBadge.xaml.cs
using System;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;

namespace EmployeeRegistrationApp.Maui.Presentation.Common.Controls
{
    /// <summary>
    /// Badge de status para funcionários, departamentos, etc.
    /// Ajusta cor automaticamente com base no texto do status
    /// (ex.: Ativo, Inativo, Férias, Desligado...).
    /// </summary>
    public partial class StatusBadge : ContentView
    {
        public StatusBadge()
        {
            InitializeComponent();
            UpdateVisual();
        }

        // ============================
        // Bindable Properties
        // ============================

        public static readonly BindableProperty TextProperty =
            BindableProperty.Create(
                nameof(Text),
                typeof(string),
                typeof(StatusBadge),
                default(string),
                propertyChanged: OnStatusPropertyChanged);

        public static readonly BindableProperty CustomBackgroundColorProperty =
            BindableProperty.Create(
                nameof(CustomBackgroundColor),
                typeof(Color),
                typeof(StatusBadge),
                default(Color),
                propertyChanged: OnStatusPropertyChanged);

        public static readonly BindableProperty CustomTextColorProperty =
            BindableProperty.Create(
                nameof(CustomTextColor),
                typeof(Color),
                typeof(StatusBadge),
                default(Color),
                propertyChanged: OnStatusPropertyChanged);

        /// <summary>
        /// Se true, ignora o mapeamento automático e usa as cores customizadas.
        /// </summary>
        public static readonly BindableProperty UseCustomColorsProperty =
            BindableProperty.Create(
                nameof(UseCustomColors),
                typeof(bool),
                typeof(StatusBadge),
                false,
                propertyChanged: OnStatusPropertyChanged);

        // ============================
        // Propriedades públicas
        // ============================

        /// <summary>
        /// Texto do status (ex.: "Ativo", "Inativo", "Em teste").
        /// </summary>
        public string Text
        {
            get => (string)GetValue(TextProperty);
            set => SetValue(TextProperty, value);
        }

        /// <summary>
        /// Cor de fundo customizada (usada quando UseCustomColors = true).
        /// </summary>
        public Color CustomBackgroundColor
        {
            get => (Color)GetValue(CustomBackgroundColorProperty);
            set => SetValue(CustomBackgroundColorProperty, value);
        }

        /// <summary>
        /// Cor de texto customizada (usada quando UseCustomColors = true).
        /// </summary>
        public Color CustomTextColor
        {
            get => (Color)GetValue(CustomTextColorProperty);
            set => SetValue(CustomTextColorProperty, value);
        }

        /// <summary>
        /// Se true, o componente usa CustomBackgroundColor / CustomTextColor.
        /// Se false, calcula as cores automaticamente pelo texto.
        /// </summary>
        public bool UseCustomColors
        {
            get => (bool)GetValue(UseCustomColorsProperty);
            set => SetValue(UseCustomColorsProperty, value);
        }

        // ============================
        // Internals
        // ============================

        private static void OnStatusPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is StatusBadge badge)
            {
                badge.UpdateVisual();
            }
        }

        private void UpdateVisual()
        {
            // Label definido no XAML: x:Name="StatusLabel"
            StatusLabel.Text = Text ?? string.Empty;

            if (UseCustomColors && CustomBackgroundColor != default && CustomTextColor != default)
            {
                RootBorder.BackgroundColor = CustomBackgroundColor;
                StatusLabel.TextColor = CustomTextColor;
                return;
            }

            // Mapeamento simples de status -> cores
            var status = (Text ?? string.Empty).Trim().ToLowerInvariant();

            // Default
            Color bg = Color.FromArgb("#111827");
            Color fg = Color.FromArgb("#e5e7eb");

            if (status.Contains("ativo") || status.Contains("active"))
            {
                bg = Color.FromArgb("#064e3b");   // verde escuro
                fg = Color.FromArgb("#6ee7b7");
            }
            else if (status.Contains("inativo") || status.Contains("deslig") || status.Contains("inactive"))
            {
                bg = Color.FromArgb("#7f1d1d");   // vermelho escuro
                fg = Color.FromArgb("#fecaca");
            }
            else if (status.Contains("férias") || status.Contains("ferias") || status.Contains("leave"))
            {
                bg = Color.FromArgb("#78350f");   // amarelo/laranja
                fg = Color.FromArgb("#fed7aa");
            }
            else if (status.Contains("teste") || status.Contains("trial") || status.Contains("prob"))
            {
                bg = Color.FromArgb("#1e3a8a");   // azul
                fg = Color.FromArgb("#bfdbfe");
            }

            // Border definido no XAML: x:Name="RootBorder"
            RootBorder.BackgroundColor = bg;
            StatusLabel.TextColor = fg;
        }
    }
}
