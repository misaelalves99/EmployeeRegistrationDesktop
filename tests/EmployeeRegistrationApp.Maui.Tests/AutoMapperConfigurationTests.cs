using AutoMapper;
using EmployeeRegistrationApp.Application.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

namespace EmployeeRegistrationApp.Maui.Tests;

public sealed class AutoMapperConfigurationTests
{
    [Fact]
    public void ApplicationMappings_AreValid_AfterSecurityUpgrade()
    {
        var services = new ServiceCollection();
        services.AddLogging();
        services.AddApplicationServices();

        using var provider = services.BuildServiceProvider();
        var mapper = provider.GetRequiredService<IMapper>();

        mapper.ConfigurationProvider.AssertConfigurationIsValid();
    }
}