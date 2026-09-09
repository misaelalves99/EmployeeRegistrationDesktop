// src/EmployeeRegistrationApp.Maui/Core/Base/ViewModelBase.cs
using System.Threading;
using System.Threading.Tasks;

namespace EmployeeRegistrationApp.Maui.Core.Base;

/// <summary>
/// Base para todos os ViewModels da aplicação.
/// Inclui propriedades comuns como Title e IsBusy.
/// </summary>
public abstract class ViewModelBase : ObservableObject
{
    private string _title = string.Empty;
    private bool _isBusy;
    private CancellationTokenSource? _cancellationTokenSource;

    /// <summary>
    /// Título da tela (bindado no Shell / Header / Page).
    /// </summary>
    public string Title
    {
        get => _title;
        set => SetProperty(ref _title, value);
    }

    /// <summary>
    /// Indica se o ViewModel está executando alguma operação longa (loading).
    /// </summary>
    public bool IsBusy
    {
        get => _isBusy;
        protected set
        {
            if (SetProperty(ref _isBusy, value))
            {
                // Notifica também IsNotBusy
                OnPropertyChanged(nameof(IsNotBusy));
            }
        }
    }

    /// <summary>
    /// Conveniência para binding inverso de IsBusy.
    /// </summary>
    public bool IsNotBusy => !IsBusy;

    /// <summary>
    /// Token de cancelamento para operações assíncronas longas (ex.: carregamento).
    /// </summary>
    protected CancellationTokenSource CancellationTokenSource
        => _cancellationTokenSource ??= new CancellationTokenSource();

    /// <summary>
    /// Chamado quando a página correspondente é exibida.
    /// Ideal para carregar dados iniciais.
    /// </summary>
    public virtual Task OnAppearingAsync()
    {
        return Task.CompletedTask;
    }

    /// <summary>
    /// Chamado quando a página correspondente é ocultada/removida.
    /// Ideal para limpar recursos ou cancelar operações.
    /// </summary>
    public virtual Task OnDisappearingAsync()
    {
        _cancellationTokenSource?.Cancel();
        _cancellationTokenSource?.Dispose();
        _cancellationTokenSource = null;

        return Task.CompletedTask;
    }

    /// <summary>
    /// Helper para executar uma operação assíncrona controlando IsBusy.
    /// </summary>
    protected async Task RunBusyAsync(Func<Task> operation)
    {
        if (IsBusy)
            return;

        try
        {
            IsBusy = true;
            await operation();
        }
        finally
        {
            IsBusy = false;
        }
    }
}
