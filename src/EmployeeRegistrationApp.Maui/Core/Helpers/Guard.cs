// src/EmployeeRegistrationApp.Maui/Core/Helpers/Guard.cs
using System;

namespace EmployeeRegistrationApp.Maui.Core.Helpers;

/// <summary>
/// Utilitário simples para validações defensivas (guard clauses).
/// Útil para garantir pré-condições em construtores, services e viewmodels.
/// </summary>
public static class Guard
{
    public static T AgainstNull<T>(T? value, string paramName) where T : class
    {
        if (value is null)
            throw new ArgumentNullException(paramName);

        return value;
    }

    public static string AgainstNullOrEmpty(string? value, string paramName)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException($"'{paramName}' não pode ser nulo ou vazio.", paramName);

        return value;
    }

    public static int AgainstOutOfRange(int value, int min, int max, string paramName)
    {
        if (value < min || value > max)
        {
            throw new ArgumentOutOfRangeException(
                paramName,
                value,
                $"'{paramName}' deve estar entre {min} e {max}.");
        }

        return value;
    }

    public static decimal AgainstNegative(decimal value, string paramName)
    {
        if (value < 0)
            throw new ArgumentOutOfRangeException(paramName, value, $"'{paramName}' não pode ser negativo.");

        return value;
    }

    public static void Against(bool condition, string message, string paramName = "")
    {
        if (condition)
        {
            if (string.IsNullOrWhiteSpace(paramName))
                throw new ArgumentException(message);

            throw new ArgumentException(message, paramName);
        }
    }
}
