using EmployeeRegistrationApp.Domain.Entities;
using Xunit;

namespace EmployeeRegistrationApp.Domain.Tests;

public sealed class DomainSmokeTests
{
    [Fact]
    public void Department_Normalizes_identity_fields_and_changes_active_state()
    {
        var department = new Department("  Tecnologia  ", " ti ", "  Plataforma  ");

        Assert.Equal("Tecnologia", department.Name);
        Assert.Equal("TI", department.Code);
        Assert.Equal("Plataforma", department.Description);
        Assert.True(department.IsActive);

        department.Deactivate();
        Assert.False(department.IsActive);

        department.Activate();
        Assert.True(department.IsActive);
        Assert.NotNull(department.UpdatedAt);
    }
}