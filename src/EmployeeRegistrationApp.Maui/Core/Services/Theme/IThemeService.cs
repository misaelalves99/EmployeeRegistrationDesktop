// src/EmployeeRegistrationApp.Maui/Core/Services/Theme/IThemeService.cs
using System;
using System.Threading.Tasks;
using Microsoft.Maui;

namespace EmployeeRegistrationApp.Maui.Core.Services.Theme;

/// <summary>
/// Informações de tema retornadas pelo serviço.
/// </summary>
public sealed class ThemeInfo
{
    public AppTheme AppTheme { get; init; }
    public string DisplayName { get; init; } = string.Empty;
    public bool IsDark { get; init; }
}

/// <summary>
/// Serviço responsável por controlar o tema da aplicação (Light/Dark).
/// Centraliza a lógica de troca de tema + persistência.
/// </summary>
public interface IThemeService
{
    /// <summary>
    /// Tema atual aplicado (UserAppTheme).
    /// </summary>
    AppTheme CurrentTheme { get; }

    /// <summary>
    /// Evento disparado quando o tema é alterado.
    /// </summary>
    event EventHandler<AppTheme>? ThemeChanged;

    /// <summary>
    /// Inicializa o tema (ex.: aplica preferências salvas ou default).
    /// </summary>
    Task InitializeAsync();

    /// <summary>
    /// Aplica o tema inicial na aplicação (normalmente chamada na inicialização).
    /// </summary>
    Task ApplyInitialThemeAsync();

    /// <summary>
    /// Retorna informações amigáveis sobre o tema atual (nome, se é dark, etc).
    /// </summary>
    Task<ThemeInfo> GetCurrentThemeAsync();

    /// <summary>
    /// Define explicitamente o tema a ser usado.
    /// </summary>
    Task SetThemeAsync(AppTheme theme);

    /// <summary>
    /// Alterna entre Light e Dark.
    /// </summary>
    Task ToggleThemeAsync();
}
