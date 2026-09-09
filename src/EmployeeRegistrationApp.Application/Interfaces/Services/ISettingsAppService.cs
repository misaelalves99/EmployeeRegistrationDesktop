// src/EmployeeRegistrationApp.Application/Interfaces/Services/ISettingsAppService.cs
using System.Threading.Tasks;
using EmployeeRegistrationApp.Application.DTOs.Settings;

namespace EmployeeRegistrationApp.Application.Interfaces.Services
{
    /// <summary>
    /// Serviço de aplicação para gerenciar as configurações da empresa
    /// (nome fantasia, razão social, CNPJ, tema, etc.).
    /// </summary>
    public interface ISettingsAppService
    {
        /// <summary>
        /// Obtém as configurações atuais da empresa, ou null se ainda não houver registro.
        /// </summary>
        Task<CompanySettingsDto?> GetCompanySettingsAsync();

        /// <summary>
        /// Cria ou atualiza as configurações da empresa.
        /// </summary>
        Task<CompanySettingsDto> SaveCompanySettingsAsync(CompanySettingsDto dto);
    }
}
