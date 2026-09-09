// src/EmployeeRegistrationApp.Maui/Core/Config/AppConfig.cs
namespace EmployeeRegistrationApp.Maui.Core.Config;

/// <summary>
/// Configurações gerais da aplicação (nível UI/Maui).
/// Aqui você centraliza valores que podem ser alterados por ambiente
/// (ex: dev, homolog, produção desktop).
/// </summary>
public static class AppConfig
{
    /// <summary>
    /// Nome amigável da aplicação.
    /// </summary>
    public const string ApplicationName = "Employee Registration Desktop";

    /// <summary>
    /// Versão de exibição (pode ser sincronizada com AssemblyVersion se quiser).
    /// </summary>
    public const string ApplicationVersion = "1.0.0";

    /// <summary>
    /// Indica se o aplicativo está rodando em modo demonstração (dados fake).
    /// Isso pode ser usado para mostrar avisos, desabilitar operações críticas, etc.
    /// </summary>
    public static bool IsDemoMode { get; set; } = true;

    /// <summary>
    /// Nome do arquivo de banco de dados local (SQLite).
    /// </summary>
    public const string DatabaseFileName = "employee_registration.db3";

    /// <summary>
    /// Nome lógico do schema, caso você queira padronizar.
    /// </summary>
    public const string DatabaseSchemaName = "employee_app";

    /// <summary>
    /// Timeout padrão para chamadas assíncronas simulando back-end (em ms).
    /// Útil para fazer loading spinners aparecerem de forma realista.
    /// </summary>
    public const int DefaultOperationTimeoutMs = 1500;

    /// <summary>
    /// Indica se logs de navegação devem ser escritos em Debug.
    /// </summary>
    public static bool EnableNavigationLogging { get; set; } = true;

    /// <summary>
    /// Indica se o tema dark deve ser aplicado como padrão.
    /// Pode ser combinado com ThemeService.
    /// </summary>
    public static bool UseDarkThemeByDefault { get; set; } = true;
}
