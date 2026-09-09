// src/EmployeeRegistrationApp.Maui/Core/Validation/ViewModelValidator.cs
using System;
using System.Text.RegularExpressions;

namespace EmployeeRegistrationApp.Maui.Core.Validation;

/// <summary>
/// Helper estático para construir regras de validação em ViewModels.
/// 
/// Exemplo de uso em um ViewModel:
/// 
/// var result = new ValidationResult();
/// ViewModelValidator.Required(result, Name, nameof(Name), "Nome é obrigatório");
/// ViewModelValidator.Email(result, Email, nameof(Email));
/// ViewModelValidator.MinLength(result, Password, 6, nameof(Password));
/// 
/// if (!result.IsValid) { ... }
/// </summary>
public static class ViewModelValidator
{
    public static void Required(
        ValidationResult result,
        string? value,
        string propertyName,
        string? message = null)
    {
        if (!string.IsNullOrWhiteSpace(value))
            return;

        var errorMessage = message ?? "Campo obrigatório.";
        result.AddError(propertyName, errorMessage);
    }

    public static void MinLength(
        ValidationResult result,
        string? value,
        int minLength,
        string propertyName,
        string? message = null)
    {
        if (string.IsNullOrEmpty(value))
            return; // Required trata vazio

        if (value.Length >= minLength)
            return;

        var errorMessage = message ?? $"Deve conter pelo menos {minLength} caracteres.";
        result.AddError(propertyName, errorMessage);
    }

    public static void MaxLength(
        ValidationResult result,
        string? value,
        int maxLength,
        string propertyName,
        string? message = null)
    {
        if (string.IsNullOrEmpty(value))
            return;

        if (value.Length <= maxLength)
            return;

        var errorMessage = message ?? $"Não pode ultrapassar {maxLength} caracteres.";
        result.AddError(propertyName, errorMessage);
    }

    public static void Email(
        ValidationResult result,
        string? value,
        string propertyName,
        string? message = null)
    {
        if (string.IsNullOrWhiteSpace(value))
            return; // Required trata vazio

        // Regex simples apenas para validação básica de email.
        const string pattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";

        if (Regex.IsMatch(value, pattern, RegexOptions.IgnoreCase))
            return;

        var errorMessage = message ?? "E-mail inválido.";
        result.AddError(propertyName, errorMessage);
    }

    public static void Range(
        ValidationResult result,
        decimal value,
        decimal min,
        decimal max,
        string propertyName,
        string? message = null)
    {
        if (value >= min && value <= max)
            return;

        var errorMessage = message ?? $"O valor deve estar entre {min} e {max}.";
        result.AddError(propertyName, errorMessage);
    }

    public static void GreaterThanZero(
        ValidationResult result,
        decimal value,
        string propertyName,
        string? message = null)
    {
        if (value > 0)
            return;

        var errorMessage = message ?? "O valor deve ser maior que zero.";
        result.AddError(propertyName, errorMessage);
    }

    public static void Custom(
        ValidationResult result,
        bool conditionIsValid,
        string propertyName,
        string errorMessage)
    {
        if (conditionIsValid)
            return;

        result.AddError(propertyName, errorMessage);
    }
}
