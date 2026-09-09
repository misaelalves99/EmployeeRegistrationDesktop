// src/EmployeeRegistrationApp.Maui/Core/Helpers/FormattingHelpers.cs
using System;
using System.Globalization;
using EmployeeRegistrationApp.Domain.Enums;

namespace EmployeeRegistrationApp.Maui.Core.Helpers;

/// <summary>
/// Funções de formatação reutilizáveis para datas, valores monetários
/// e labels amigáveis ligados ao domínio (contrato, status, etc.).
/// </summary>
public static class FormattingHelpers
{
    private static readonly CultureInfo DefaultCulture = CultureInfo.CurrentCulture;

    public static string FormatCurrency(decimal value, string? cultureName = null)
    {
        var culture = !string.IsNullOrWhiteSpace(cultureName)
            ? new CultureInfo(cultureName)
            : DefaultCulture;

        return string.Format(culture, "{0:C}", value);
    }

    public static string FormatDate(DateTime? date, string format = "dd/MM/yyyy")
    {
        if (date is null)
            return string.Empty;

        return date.Value.ToString(format, DefaultCulture);
    }

    public static string FormatDateTime(DateTime? dateTime, string format = "dd/MM/yyyy HH:mm")
    {
        if (dateTime is null)
            return string.Empty;

        return dateTime.Value.ToString(format, DefaultCulture);
    }

    public static string FormatEmploymentStatusLabel(EmploymentStatus status)
    {
        // Este switch pode ser refinado se você quiser labels específicas,
        // mas aqui deixo só os que com certeza existem.
        return status switch
        {
            EmploymentStatus.Active => "Ativo",
            EmploymentStatus.Inactive => "Inativo",
            _ => status.ToString()
        };
    }

    public static string FormatContractTypeLabel(ContractType type)
    {
        // Caso você queira algo mais elaborado depois, é só ajustar aqui.
        return type.ToString();
    }

    public static string FormatPositionTypeLabel(PositionType type)
    {
        return type.ToString();
    }

    public static string Truncate(string? text, int maxLength, string suffix = "…")
    {
        if (string.IsNullOrWhiteSpace(text) || maxLength <= 0)
            return string.Empty;

        if (text.Length <= maxLength)
            return text;

        return text.Substring(0, maxLength) + suffix;
    }
}
