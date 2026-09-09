// src/EmployeeRegistrationApp.Domain/Enums/JobRole.cs
namespace EmployeeRegistrationApp.Domain.Enums
{
    /// <summary>
    /// Função / papel principal do colaborador na empresa.
    /// </summary>
    public enum JobRole
    {
        /// <summary>
        /// Desenvolvedor / Engenheiro de Software.
        /// </summary>
        Developer = 1,

        /// <summary>
        /// Gestor / Gerente de equipe ou área.
        /// </summary>
        Manager = 2,

        /// <summary>
        /// Profissional de Recursos Humanos.
        /// </summary>
        HR = 3,

        /// <summary>
        /// Profissional da área financeira (contas a pagar/receber, controladoria, etc.).
        /// </summary>
        Finance = 4,

        /// <summary>
        /// Vendas / Comercial.
        /// </summary>
        Sales = 5,

        /// <summary>
        /// Suporte / Atendimento.
        /// </summary>
        Support = 6,

        /// <summary>
        /// Operações / Administrativo.
        /// </summary>
        Operations = 7,

        /// <summary>
        /// Outros cargos não categorizados.
        /// </summary>
        Other = 99
    }
}
