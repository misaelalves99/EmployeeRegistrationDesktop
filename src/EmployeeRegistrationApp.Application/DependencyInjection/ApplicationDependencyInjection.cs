// src/EmployeeRegistrationApp.Application/DependencyInjection/ApplicationDependencyInjection.cs
using Microsoft.Extensions.DependencyInjection;
using AutoMapper;

using EmployeeRegistrationApp.Application.AutoMapper.Employees;
using EmployeeRegistrationApp.Application.Interfaces.Services;
using EmployeeRegistrationApp.Application.Services.Departments;
using EmployeeRegistrationApp.Application.Services.Employees;
using EmployeeRegistrationApp.Application.Services.Positions;
using EmployeeRegistrationApp.Application.Services.Reports;
using EmployeeRegistrationApp.Application.Services.Settings;

namespace EmployeeRegistrationApp.Application.DependencyInjection
{
    /// <summary>
    /// Configuração de DI da camada Application.
    /// 
    /// Essa extensão é chamada a partir do projeto de UI (no seu caso, o .Maui),
    /// para registrar serviços de aplicação + AutoMapper.
    /// </summary>
    public static class ApplicationDependencyInjection
    {
        /// <summary>
        /// Registra todos os serviços de aplicação e perfis do AutoMapper
        /// deste assembly.
        /// </summary>
        public static IServiceCollection AddApplicationServices(
            this IServiceCollection services)
        {
            // Serviços de aplicação (App Services)
            services.AddScoped<IEmployeeAppService, EmployeeAppService>();
            services.AddScoped<IDepartmentAppService, DepartmentAppService>();
            services.AddScoped<IPositionAppService, PositionAppService>();
            services.AddScoped<IReportAppService, ReportAppService>();
            services.AddScoped<ISettingsAppService, SettingsAppService>();

            // AutoMapper: varre todo o assembly da Application
            // a partir do EmployeeApplicationProfile
            services.AddAutoMapper(typeof(EmployeeApplicationProfile).Assembly);

            return services;
        }
    }
}
