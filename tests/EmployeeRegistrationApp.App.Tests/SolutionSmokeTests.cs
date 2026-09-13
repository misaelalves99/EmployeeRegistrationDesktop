using EmployeeRegistrationApp.Application.DTOs.Departments;
using Xunit;

namespace EmployeeRegistrationApp.Tests;

public sealed class SolutionSmokeTests
{
    [Fact]
    public void DepartmentDto_Defaults_match_application_contract()
    {
        var dto = new DepartmentDto();

        Assert.Null(dto.Id);
        Assert.Equal(string.Empty, dto.Name);
        Assert.True(dto.IsActive);
        Assert.Equal(0, dto.Headcount);
        Assert.Null(dto.Code);
        Assert.Null(dto.Description);
    }
}