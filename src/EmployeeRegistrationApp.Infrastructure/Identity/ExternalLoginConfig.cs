// src/EmployeeRegistrationApp.Infrastructure/Identity/ExternalLoginConfig.cs
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EmployeeRegistrationApp.Infrastructure.Identity
{
    /// <summary>
    /// Ponto central para configurar provedores de login externo
    /// (Google, Microsoft, etc.) caso você decida conectar o app
    /// desktop a um backend de autenticação no futuro.
    /// 
    /// No cenário atual (.NET MAUI Desktop offline / local),
    /// essa configuração é opcional e está vazia de propósito.
    /// </summary>
    public static class ExternalLoginConfig
    {
        /// <summary>
        /// Método de extensão preparado para uso futuro.
        /// No momento, não registra nada (no-op).
        /// </summary>
        public static IServiceCollection AddExternalLoginConfig(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            // Exemplo para futuro (se criar um backend Web API para autenticação):
            //
            // - Ler URLs de autoridade (IdentityServer, Azure AD B2C, etc.)
            // - Registrar clientes HTTP para fluxo OAuth/OpenID Connect
            // - Configurar serviços que conversam com o backend de identidade
            //
            // Por enquanto, mantemos tudo local (Identity + SQLite),
            // então esse método intencionalmente não faz nada.

            return services;
        }
    }
}
