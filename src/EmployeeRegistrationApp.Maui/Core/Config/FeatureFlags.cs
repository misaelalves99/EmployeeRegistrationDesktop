// src/EmployeeRegistrationApp.Maui/Core/Config/FeatureFlags.cs
namespace EmployeeRegistrationApp.Maui.Core.Config;

/// <summary>
/// Flags de funcionalidades para habilitar/desabilitar módulos
/// sem precisar remover código. Útil em demos, versões trial, etc.
/// </summary>
public static class FeatureFlags
{
    /// <summary>
    /// Controle de acesso ao módulo de Dashboard.
    /// </summary>
    public static bool DashboardEnabled { get; set; } = true;

    /// <summary>
    /// Controle de acesso ao módulo de Funcionários.
    /// </summary>
    public static bool EmployeesModuleEnabled { get; set; } = true;

    /// <summary>
    /// Controle de acesso ao módulo de Departamentos.
    /// </summary>
    public static bool DepartmentsModuleEnabled { get; set; } = true;

    /// <summary>
    /// Controle de acesso ao módulo de Cargos (Positions).
    /// </summary>
    public static bool PositionsModuleEnabled { get; set; } = true;

    /// <summary>
    /// Controle de acesso ao módulo de Relatórios.
    /// </summary>
    public static bool ReportsModuleEnabled { get; set; } = true;

    /// <summary>
    /// Controle de acesso ao módulo de Configurações.
    /// </summary>
    public static bool SettingsModuleEnabled { get; set; } = true;

    /// <summary>
    /// Se habilitado, exibe labels de "DEMO" em algumas telas.
    /// </summary>
    public static bool ShowDemoBadges { get; set; } = true;
}
