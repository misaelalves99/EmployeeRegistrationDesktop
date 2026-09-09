// src/EmployeeRegistrationApp.Maui/Core/Helpers/TaskExtensions.cs
using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace EmployeeRegistrationApp.Maui.Core.Helpers;

/// <summary>
/// Extensões utilitárias para Task, pensadas para uso em ViewModels (MVVM).
/// </summary>
public static class TaskExtensions
{
    /// <summary>
    /// Executa uma Task "fire-and-forget" com tratamento de exceção.
    /// Útil para comandos que não precisam aguardar o término, mas
    /// não devem derrubar a aplicação em caso de erro.
    /// </summary>
    public static async void FireAndForgetSafeAsync(
        this Task task,
        Action<Exception>? onException = null)
    {
        try
        {
            await task.ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"[TASK][ERROR] {ex}");

            if (onException is not null)
                onException(ex);
        }
    }

    /// <summary>
    /// Versão genérica de FireAndForgetSafeAsync.
    /// </summary>
    public static async void FireAndForgetSafeAsync<TResult>(
        this Task<TResult> task,
        Action<Exception>? onException = null)
    {
        try
        {
            _ = await task.ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"[TASK][ERROR] {ex}");

            if (onException is not null)
                onException(ex);
        }
    }

    /// <summary>
    /// Aplica timeout em uma Task. Retorna default(T) em caso de timeout.
    /// </summary>
    public static async Task<TResult?> WithTimeout<TResult>(
        this Task<TResult> task,
        TimeSpan timeout,
        CancellationToken cancellationToken = default)
    {
        using var cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
        var timeoutTask = Task.Delay(timeout, cts.Token);

        var completed = await Task.WhenAny(task, timeoutTask).ConfigureAwait(false);

        if (completed == timeoutTask)
        {
            // Timeout
            return default;
        }

        cts.Cancel(); // cancela o delay
        return await task.ConfigureAwait(false);
    }

    /// <summary>
    /// Versão sem retorno do WithTimeout.
    /// </summary>
    public static async Task<bool> WithTimeout(
        this Task task,
        TimeSpan timeout,
        CancellationToken cancellationToken = default)
    {
        using var cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
        var timeoutTask = Task.Delay(timeout, cts.Token);

        var completed = await Task.WhenAny(task, timeoutTask).ConfigureAwait(false);

        if (completed == timeoutTask)
        {
            // Timeout
            return false;
        }

        cts.Cancel();
        await task.ConfigureAwait(false);
        return true;
    }
}
