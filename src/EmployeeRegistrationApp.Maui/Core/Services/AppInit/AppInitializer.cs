// src/EmployeeRegistrationApp.Maui/Core/Services/AppInit/AppInitializer.cs
using System.Threading.Tasks;
using EmployeeRegistrationApp.Infrastructure.Repositories.InMemory;

namespace EmployeeRegistrationApp.Maui.Core.Services.AppInit;

public sealed class AppInitializer : IAppInitializer
{
    private readonly InMemoryDatabase _db;

    public AppInitializer(InMemoryDatabase db)
    {
        _db = db;
    }

    public Task InitializeAsync()
    {
        _db.SeedIfEmpty();
        return Task.CompletedTask;
    }
}
