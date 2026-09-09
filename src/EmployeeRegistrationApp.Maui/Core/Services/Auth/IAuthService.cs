// src/EmployeeRegistrationApp.Maui/Core/Services/Auth/IAuthService.cs
using System;
using System.Threading.Tasks;

namespace EmployeeRegistrationApp.Maui.Core.Services.Auth;

/// <summary>
/// Serviço de autenticação usado pelos ViewModels.
/// Abstrai o mecanismo real de login/logout (Identity, API, etc.).
/// </summary>
public interface IAuthService
{
    /// <summary>
    /// Indica se há um usuário autenticado no momento.
    /// </summary>
    bool IsAuthenticated { get; }

    /// <summary>
    /// Nome de exibição do usuário autenticado (se houver).
    /// </summary>
    string? CurrentUserName { get; }

    /// <summary>
    /// Email do usuário autenticado (se houver).
    /// </summary>
    string? CurrentUserEmail { get; }

    /// <summary>
    /// Evento disparado sempre que o estado de autenticação mudar.
    /// </summary>
    event EventHandler<bool>? AuthenticationStateChanged;

    /// <summary>
    /// Tenta autenticar o usuário com email/usuário e senha.
    /// Retorna true em caso de sucesso.
    /// </summary>
    Task<bool> SignInAsync(string userNameOrEmail, string password);

    /// <summary>
    /// Registra um novo usuário localmente.
    /// Em um cenário real, integraria com Identity / API.
    /// </summary>
    Task<bool> RegisterAsync(string fullName, string email, string password);

    /// <summary>
    /// Faz logout do usuário atual.
    /// </summary>
    Task SignOutAsync();

    /// <summary>
    /// Inicializa o estado de autenticação (por exemplo, lendo de Preferences).
    /// </summary>
    void Initialize();
}
