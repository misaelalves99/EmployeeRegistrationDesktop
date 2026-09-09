// src/EmployeeRegistrationApp.Maui/Core/Validation/ValidationResult.cs
using System.Collections.Generic;
using System.Linq;

namespace EmployeeRegistrationApp.Maui.Core.Validation;

/// <summary>
/// Representa o resultado de uma validação de ViewModel ou formulário.
/// </summary>
public sealed class ValidationResult
{
    private readonly List<ValidationError> _errors = [];

    public bool IsValid => !_errors.Any();

    public IReadOnlyList<ValidationError> Errors => _errors;

    public void AddError(string propertyName, string errorMessage)
    {
        if (string.IsNullOrWhiteSpace(errorMessage))
            return;

        _errors.Add(new ValidationError(propertyName, errorMessage));
    }

    public string? GetFirstError(string propertyName)
    {
        return _errors
            .FirstOrDefault(e => e.PropertyName == propertyName)?
            .ErrorMessage;
    }

    public override string ToString()
    {
        if (IsValid)
            return "Validação bem sucedida (sem erros).";

        return string.Join("; ", _errors.Select(e => $"{e.PropertyName}: {e.ErrorMessage}"));
    }
}

/// <summary>
/// Erro individual de validação (campo + mensagem).
/// </summary>
public sealed class ValidationError
{
    public string PropertyName { get; }
    public string ErrorMessage { get; }

    public ValidationError(string propertyName, string errorMessage)
    {
        PropertyName = propertyName;
        ErrorMessage = errorMessage;
    }
}
