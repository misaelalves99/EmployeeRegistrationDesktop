// src/EmployeeRegistrationApp.Application/Interfaces/Repositories/ICompanySettingsRepository.cs
using System.Threading;
using System.Threading.Tasks;
using EmployeeRegistrationApp.Domain.Entities;

namespace EmployeeRegistrationApp.Application.Interfaces.Repositories
{
    public interface ICompanySettingsRepository
    {
        Task<CompanySettings?> GetCurrentAsync(CancellationToken cancellationToken = default);
        Task AddAsync(CompanySettings settings, CancellationToken cancellationToken = default);
        Task UpdateAsync(CompanySettings settings, CancellationToken cancellationToken = default);
    }
}
