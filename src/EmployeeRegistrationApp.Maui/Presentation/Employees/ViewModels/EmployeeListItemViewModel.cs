// // src/EmployeeRegistrationApp.Maui/Presentation/Employees/ViewModels/EmployeeListItemViewModel.cs
using System;

namespace EmployeeRegistrationApp.Maui.Presentation.Employees.ViewModels
{
    public sealed class EmployeeListItemViewModel
    {
        public Guid Id { get; init; }

        public string FullName { get; init; } = string.Empty;
        public string Email { get; init; } = string.Empty;

        public string DepartmentName { get; init; } = "—";
        public string PositionName { get; init; } = "—";

        public bool IsActive { get; init; }

        public string StatusLabel => IsActive ? "ATIVO" : "INATIVO";

        // Cor simples sem converter (evita dores de XAML)
        public Microsoft.Maui.Graphics.Color StatusBadgeColor =>
            IsActive ? Microsoft.Maui.Graphics.Color.FromArgb("#03CEA4") : Microsoft.Maui.Graphics.Color.FromArgb("#EAC435");
    }
}
