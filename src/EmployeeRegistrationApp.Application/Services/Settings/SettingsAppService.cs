// src/EmployeeRegistrationApp.Application/Services/Settings/SettingsAppService.cs
using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using EmployeeRegistrationApp.Application.DTOs.Settings;
using EmployeeRegistrationApp.Application.Interfaces.Repositories;
using EmployeeRegistrationApp.Application.Interfaces.Services;
using EmployeeRegistrationApp.Domain.Entities;

namespace EmployeeRegistrationApp.Application.Services.Settings
{
    public sealed class SettingsAppService : ISettingsAppService
    {
        private readonly ICompanySettingsRepository _repository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public SettingsAppService(
            ICompanySettingsRepository repository,
            IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<CompanySettingsDto?> GetCompanySettingsAsync()
        {
            var entity = await _repository.GetCurrentAsync(CancellationToken.None);

            // ✅ Em modo demo/in-memory, evitar "null" ajuda MUITO a UI (Maui) a não quebrar.
            if (entity is null)
            {
                return new CompanySettingsDto
                {
                    CompanyName = string.Empty,
                    LegalName = string.Empty
                };
            }

            return _mapper.Map<CompanySettingsDto>(entity);
        }

        public async Task<CompanySettingsDto> SaveCompanySettingsAsync(CompanySettingsDto dto)
        {
            if (dto is null) throw new ArgumentNullException(nameof(dto));

            var entity = await _repository.GetCurrentAsync(CancellationToken.None);

            if (entity is null)
            {
                // ✅ cria o aggregate com defaults seguros
                entity = new CompanySettings(
                    companyName: dto.CompanyName ?? string.Empty,
                    legalName: dto.LegalName ?? string.Empty,
                    registrationNumber: dto.RegistrationNumber,
                    primaryEmail: null,
                    primaryPhone: null,
                    websiteUrl: dto.WebsiteUrl,
                    defaultCurrency: string.IsNullOrWhiteSpace(dto.DefaultCurrency) ? "BRL" : dto.DefaultCurrency!,
                    defaultWorkHoursPerWeek: dto.DefaultWorkHoursPerWeek ?? 40,
                    allowRemoteWork: dto.AllowRemoteWork ?? true
                );

                // ✅ aplica via mapping (AfterMap usa Update* do aggregate)
                _mapper.Map(dto, entity);

                await _repository.AddAsync(entity, CancellationToken.None);
            }
            else
            {
                _mapper.Map(dto, entity);
                await _repository.UpdateAsync(entity, CancellationToken.None);
            }

            await _unitOfWork.SaveChangesAsync(CancellationToken.None);

            return _mapper.Map<CompanySettingsDto>(entity);
        }
    }
}
