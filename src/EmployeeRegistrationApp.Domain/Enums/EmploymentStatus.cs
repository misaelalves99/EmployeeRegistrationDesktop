// src/EmployeeRegistrationApp.Domain/Enums/EmploymentStatus.cs
namespace EmployeeRegistrationApp.Domain.Enums
{
    /// <summary>
    /// Status atual do vínculo do colaborador com a empresa.
    /// </summary>
    public enum EmploymentStatus
    {
        /// <summary>
        /// Colaborador ativo, trabalhando normalmente.
        /// </summary>
        Active = 1,

        /// <summary>
        /// Colaborador inativo (desligado ou sem vínculo atual).
        /// </summary>
        Inactive = 2,

        /// <summary>
        /// Em período de experiência.
        /// </summary>
        Probation = 3,

        /// <summary>
        /// Em férias.
        /// </summary>
        Vacation = 4,

        /// <summary>
        /// Em licença (médica, maternidade, etc.).
        /// </summary>
        OnLeave = 5,

        /// <summary>
        /// Desligado da empresa.
        /// </summary>
        Terminated = 6
    }
}
