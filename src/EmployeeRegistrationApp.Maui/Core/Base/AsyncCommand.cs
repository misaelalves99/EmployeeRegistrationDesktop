// src/EmployeeRegistrationApp.Maui/Core/Base/AsyncCommand.cs
using System;
using System.Threading.Tasks;
using System.Windows.Input;
using EmployeeRegistrationApp.Maui.Core.Helpers;

namespace EmployeeRegistrationApp.Maui.Core.Base;

/// <summary>
/// Interface para comandos assíncronos, além de ICommand.
/// </summary>
public interface IAsyncCommand : ICommand
{
    Task ExecuteAsync(object? parameter);
    bool CanExecute(object? parameter);
}

/// <summary>
/// Implementação de ICommand para operações assíncronas (Task).
/// Ideal para botões que disparam chamadas async em ViewModels.
/// </summary>
public sealed class AsyncCommand : CommandBase, IAsyncCommand
{
    private readonly Func<object?, Task> _execute;
    private readonly Predicate<object?>? _canExecute;
    private bool _isExecuting;

    public AsyncCommand(
        Func<object?, Task> execute,
        Predicate<object?>? canExecute = null)
    {
        _execute = execute ?? throw new ArgumentNullException(nameof(execute));
        _canExecute = canExecute;
    }

    public AsyncCommand(
        Func<Task> execute,
        Func<bool>? canExecute = null)
    {
        if (execute is null)
            throw new ArgumentNullException(nameof(execute));

        _execute = _ => execute();

        if (canExecute is not null)
        {
            _canExecute = _ => canExecute();
        }
    }

    protected override bool CanExecuteCore(object? parameter)
    {
        if (_isExecuting)
            return false;

        if (_canExecute is null)
            return true;

        return _canExecute(parameter);
    }

    protected override void ExecuteCore(object? parameter)
    {
        // Usa helper "fire and forget" com tratamento de exceção.
        ExecuteAsync(parameter).FireAndForgetSafeAsync(ex =>
        {
            // Aqui você pode integrar com algum serviço global de log/toast.
            System.Diagnostics.Debug.WriteLine($"[AsyncCommand][ERROR] {ex}");
        });
    }

    public async Task ExecuteAsync(object? parameter)
    {
        if (!CanExecute(parameter))
            return;

        try
        {
            _isExecuting = true;
            RaiseCanExecuteChanged();

            await _execute(parameter).ConfigureAwait(false);
        }
        finally
        {
            _isExecuting = false;
            RaiseCanExecuteChanged();
        }
    }
}
