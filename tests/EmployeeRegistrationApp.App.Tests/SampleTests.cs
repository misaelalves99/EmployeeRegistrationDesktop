using EmployeeRegistrationApp.Application.DTOs.Common;
using Xunit;

namespace EmployeeRegistrationApp.App.Tests;

public sealed class SampleTests
{
    [Fact]
    public void PagedResult_Computes_navigation_from_page_contract()
    {
        var result = new PagedResultDto<int>(2, 10, 25, new[] { 11, 12 });

        Assert.Equal(3, result.TotalPages);
        Assert.True(result.HasPreviousPage);
        Assert.True(result.HasNextPage);
        Assert.Equal(2, result.Page);
        Assert.Equal(new[] { 11, 12 }, result.Items);
    }
}