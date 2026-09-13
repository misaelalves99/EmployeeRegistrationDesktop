using EmployeeRegistrationApp.Domain.ValueObjects;
using Xunit;

namespace EmployeeRegistrationApp.Domain.Tests;

public sealed class SampleDomainTests
{
    [Fact]
    public void Email_Normalizes_whitespace_and_case()
    {
        var email = Email.Create("  USER@Example.COM  ");

        Assert.Equal("user@example.com", email.Value);
        Assert.Equal(email.Value, email.Address);
        Assert.Equal("user@example.com", email.ToString());
    }

    [Fact]
    public void Money_Requires_same_currency_for_addition()
    {
        var brl = Money.FromDecimal(10m, "brl");
        var usd = Money.FromDecimal(5m, "usd");

        Assert.Equal("BRL", brl.Currency);
        Assert.Throws<InvalidOperationException>(() => brl.Add(usd));
    }

    [Fact]
    public void Cpf_Normalizes_and_formats_valid_value()
    {
        var cpf = Cpf.Create("529.982.247-25");

        Assert.Equal("52998224725", cpf.Value);
        Assert.Equal("529.982.247-25", cpf.ToFormattedString());
    }
}