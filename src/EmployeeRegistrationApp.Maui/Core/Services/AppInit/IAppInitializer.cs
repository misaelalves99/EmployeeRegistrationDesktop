// src/EmployeeRegistrationApp.Maui/Core/Services/AppInit/IAppInitializer.cs
using System.Threading.Tasks;

namespace EmployeeRegistrationApp.Maui.Core.Services.AppInit;

public interface IAppInitializer
{
    Task InitializeAsync();
}
