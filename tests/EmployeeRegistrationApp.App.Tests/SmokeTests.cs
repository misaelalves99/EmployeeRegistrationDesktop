using EmployeeRegistrationApp.Application.DTOs.Common;
using Xunit;

namespace EmployeeRegistrationApp.App.Tests;

public sealed class SmokeTests
{
    [Fact]
    public void PagedResult_Zero_page_size_has_zero_pages_and_no_next_page()
    {
        var result = new PagedResultDto<string>(1, 0, 12, new[] { "employee" });

        Assert.Equal(0, result.TotalPages);
        Assert.False(result.HasPreviousPage);
        Assert.False(result.HasNextPage);
    }
}