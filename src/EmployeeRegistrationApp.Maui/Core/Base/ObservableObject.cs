// src/EmployeeRegistrationApp.Maui/Core/Base/ObservableObject.cs
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace EmployeeRegistrationApp.Maui.Core.Base;

/// <summary>
/// Implementação base de INotifyPropertyChanged para ViewModels e modelos observáveis.
/// </summary>
public abstract class ObservableObject : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;

    /// <summary>
    /// Dispara o evento PropertyChanged.
    /// </summary>
    /// <param name="propertyName">Nome da propriedade alterada.</param>
    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        if (string.IsNullOrEmpty(propertyName))
            return;

        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    /// <summary>
    /// Atualiza o backing field e dispara PropertyChanged se o valor realmente mudou.
    /// </summary>
    protected bool SetProperty<T>(
        ref T backingField,
        T value,
        [CallerMemberName] string? propertyName = null)
    {
        if (Equals(backingField, value))
            return false;

        backingField = value;
        OnPropertyChanged(propertyName);
        return true;
    }
}
