// src/EmployeeRegistrationApp.Maui/Core/Base/CommandBase.cs
using System;
using System.Windows.Input;

namespace EmployeeRegistrationApp.Maui.Core.Base;

/// <summary>
/// Base abstrata para comandos (ICommand) com suporte a RaiseCanExecuteChanged.
/// </summary>
public abstract class CommandBase : ICommand
{
    public event EventHandler? CanExecuteChanged;

    public bool CanExecute(object? parameter)
    {
        return CanExecuteCore(parameter);
    }

    public void Execute(object? parameter)
    {
        ExecuteCore(parameter);
    }

    /// <summary>
    /// Implementação concreta do CanExecute nas subclasses.
    /// </summary>
    protected abstract bool CanExecuteCore(object? parameter);

    /// <summary>
    /// Implementação concreta do Execute nas subclasses.
    /// </summary>
    protected abstract void ExecuteCore(object? parameter);

    /// <summary>
    /// Notifica a UI de que o estado de execução do comando mudou.
    /// </summary>
    public void RaiseCanExecuteChanged()
    {
        CanExecuteChanged?.Invoke(this, EventArgs.Empty);
    }
}
