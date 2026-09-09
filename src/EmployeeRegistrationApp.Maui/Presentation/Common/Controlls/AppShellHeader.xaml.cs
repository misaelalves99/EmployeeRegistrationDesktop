// src/EmployeeRegistrationApp.Maui/Presentation/Common/Controls/AppShellHeader.xaml.cs
using System;
using Microsoft.Maui.Controls;

namespace EmployeeRegistrationApp.Maui.Presentation.Common.Controls
{
    public partial class AppShellHeader : ContentView
    {
        public AppShellHeader()
        {
            InitializeComponent();
            UpdateUserInitials();
        }

        // ========== Bindable Properties ==========

        public static readonly BindableProperty TitleProperty =
            BindableProperty.Create(
                nameof(Title),
                typeof(string),
                typeof(AppShellHeader),
                default(string));

        public static readonly BindableProperty SubtitleProperty =
            BindableProperty.Create(
                nameof(Subtitle),
                typeof(string),
                typeof(AppShellHeader),
                default(string));

        public static readonly BindableProperty IconProperty =
            BindableProperty.Create(
                nameof(Icon),
                typeof(string),
                typeof(AppShellHeader),
                "🏢");

        public static readonly BindableProperty UserNameProperty =
            BindableProperty.Create(
                nameof(UserName),
                typeof(string),
                typeof(AppShellHeader),
                default(string),
                propertyChanged: OnUserNameChanged);

        // ========== Propriedades públicas ==========

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

        /// <summary>
        /// Ícone/emoji exibido na bolinha à esquerda.
        /// </summary>
        public string Icon
        {
            get => (string)GetValue(IconProperty);
            set => SetValue(IconProperty, value);
        }

        /// <summary>
        /// Nome do usuário logado (exibe texto + iniciais).
        /// </summary>
        public string UserName
        {
            get => (string)GetValue(UserNameProperty);
            set => SetValue(UserNameProperty, value);
        }

        // ========== Internals ==========

        private static void OnUserNameChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is AppShellHeader header)
            {
                header.UpdateUserInitials();
            }
        }

        private void UpdateUserInitials()
        {
            var name = UserName ?? string.Empty;
            var initials = string.Empty;

            var parts = name.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length > 0)
            {
                initials += char.ToUpperInvariant(parts[0][0]);
            }

            if (parts.Length > 1)
            {
                initials += char.ToUpperInvariant(parts[^1][0]);
            }

            UserInitialsLabel.Text = string.IsNullOrWhiteSpace(initials) ? "?" : initials;
        }
    }
}
