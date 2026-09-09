// src/EmployeeRegistrationApp.Domain/Enums/ContractType.cs
namespace EmployeeRegistrationApp.Domain.Enums
{
    /// <summary>
    /// Tipo de contrato de trabalho do colaborador.
    /// </summary>
    public enum ContractType
    {
        /// <summary>
        /// Contrato CLT (regime trabalhista padrão).
        /// </summary>
        CLT = 1,

        /// <summary>
        /// Contrato PJ (pessoa jurídica / prestador de serviços).
        /// </summary>
        PJ = 2,

        /// <summary>
        /// Estágio (contrato de estágio).
        /// </summary>
        Intern = 3,

        /// <summary>
        /// Temporário (contrato com prazo determinado / sazonal).
        /// </summary>
        Temporary = 4
    }
}
