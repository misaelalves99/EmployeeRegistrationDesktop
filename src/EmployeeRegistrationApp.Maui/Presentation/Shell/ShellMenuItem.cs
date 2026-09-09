// src/EmployeeRegistrationApp.Maui/Presentation/Shell/ShellMenuItem.cs
using System;

namespace EmployeeRegistrationApp.Maui.Presentation.Shell
{
    /// <summary>
    /// Representa um item de menu do Shell principal (Dashboard, Employees, etc.).
    /// É usado para montar o menu lateral / flyout com MVVM.
    /// </summary>
    public sealed class ShellMenuItem
    {
        /// <summary>
        /// Título exibido no menu (ex: "Dashboard", "Funcionários").
        /// </summary>
        public string Title { get; init; } = string.Empty;

        /// <summary>
        /// Rota de navegação registrada no AppShell (ex: "//dashboard", "//employees").
        /// </summary>
        public string Route { get; init; } = string.Empty;

        /// <summary>
        /// Ícone textual ou glyph (pode ser emoji, ícone de fonte ou resource key).
        /// Ex: "🏠", "👥", "📊".
        /// </summary>
        public string Icon { get; init; } = string.Empty;

        /// <summary>
        /// Badge opcional (ex: contagem de notificações, pendências, etc.).
        /// </summary>
        public string? Badge { get; set; }

        /// <summary>
        /// Indica se este item está selecionado no momento.
        /// </summary>
        public bool IsSelected { get; set; }

        /// <summary>
        /// Qualquer payload adicional que você queira associar ao item
        /// (ex: tipo de filtro, ID de módulo, etc.).
        /// </summary>
        public object? Tag { get; init; }
    }
}
