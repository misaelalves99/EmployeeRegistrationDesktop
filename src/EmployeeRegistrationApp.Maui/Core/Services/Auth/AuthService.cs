// src/EmployeeRegistrationApp.Maui/Core/Services/Auth/AuthService.cs
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using EmployeeRegistrationApp.Application.Interfaces.Repositories;
using EmployeeRegistrationApp.Domain.Entities;
using EmployeeRegistrationApp.Domain.ValueObjects;
using Microsoft.Maui.Storage;

namespace EmployeeRegistrationApp.Maui.Core.Services.Auth;

public sealed class AuthService : IAuthService
{
    private const string PrefIsAuthenticated = "EmployeeApp.Auth.IsAuthenticated";
    private const string PrefUserName = "EmployeeApp.Auth.UserName";
    private const string PrefUserEmail = "EmployeeApp.Auth.UserEmail";

    private readonly IUserAccountRepository _userAccountRepository;

    private bool _isAuthenticated;
    private string? _currentUserName;
    private string? _currentUserEmail;

    public AuthService(IUserAccountRepository userAccountRepository)
    {
        _userAccountRepository = userAccountRepository ?? throw new ArgumentNullException(nameof(userAccountRepository));
    }

    public bool IsAuthenticated => _isAuthenticated;
    public string? CurrentUserName => _currentUserName;
    public string? CurrentUserEmail => _currentUserEmail;

    public event EventHandler<bool>? AuthenticationStateChanged;

    public void Initialize()
    {
        try
        {
            _isAuthenticated = Preferences.Get(PrefIsAuthenticated, false);

            var userName = Preferences.Get(PrefUserName, string.Empty);
            _currentUserName = string.IsNullOrWhiteSpace(userName) ? null : userName;

            var userEmail = Preferences.Get(PrefUserEmail, string.Empty);
            _currentUserEmail = string.IsNullOrWhiteSpace(userEmail) ? null : userEmail;
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"[AUTH][ERROR] Initialize: {ex}");
            _isAuthenticated = false;
            _currentUserName = null;
            _currentUserEmail = null;
        }
    }

    public async Task<bool> SignInAsync(string userNameOrEmail, string password)
    {
        await Task.Delay(120);

        if (string.IsNullOrWhiteSpace(userNameOrEmail) || string.IsNullOrWhiteSpace(password))
            return false;

        if (password.Length < 4) // demo
            return false;

        // procura por email ou username
        UserAccount? account =
            userNameOrEmail.Contains("@", StringComparison.Ordinal)
                ? await _userAccountRepository.GetByEmailAsync(userNameOrEmail.Trim())
                : await _userAccountRepository.GetByUserNameAsync(userNameOrEmail.Trim());

        // demo: se não existir, cria um usuário básico
        if (account is null)
        {
            var email = userNameOrEmail.Contains("@", StringComparison.Ordinal)
                ? Email.Create(userNameOrEmail)
                : Email.Create($"{userNameOrEmail.Trim().ToLowerInvariant()}@employeeapp.local");

            account = new UserAccount(userNameOrEmail.Trim(), email, isActive: true);
            await _userAccountRepository.AddAsync(account);
        }

        if (!account.IsActive)
            return false;

        _isAuthenticated = true;
        _currentUserName = account.UserName;
        _currentUserEmail = account.Email.Address;

        PersistAuthState();
        RaiseAuthChanged();
        return true;
    }

    public async Task<bool> RegisterAsync(string fullName, string email, string password)
    {
        await Task.Delay(160);

        if (string.IsNullOrWhiteSpace(fullName) ||
            string.IsNullOrWhiteSpace(email) ||
            string.IsNullOrWhiteSpace(password))
            return false;

        if (!email.Contains("@", StringComparison.Ordinal))
            return false;

        if (password.Length < 4)
            return false;

        // evita duplicidade
        if (await _userAccountRepository.ExistsByEmailAsync(email.Trim()))
            return false;

        var account = new UserAccount(fullName.Trim(), Email.Create(email.Trim()), isActive: true);
        await _userAccountRepository.AddAsync(account);

        _isAuthenticated = true;
        _currentUserName = account.UserName;
        _currentUserEmail = account.Email.Address;

        PersistAuthState();
        RaiseAuthChanged();
        return true;
    }

    public Task SignOutAsync()
    {
        _isAuthenticated = false;
        _currentUserName = null;
        _currentUserEmail = null;

        PersistAuthState();
        RaiseAuthChanged();
        return Task.CompletedTask;
    }

    private void PersistAuthState()
    {
        try
        {
            Preferences.Set(PrefIsAuthenticated, _isAuthenticated);
            Preferences.Set(PrefUserName, _currentUserName ?? string.Empty);
            Preferences.Set(PrefUserEmail, _currentUserEmail ?? string.Empty);
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"[AUTH][ERROR] PersistAuthState: {ex}");
        }
    }

    private void RaiseAuthChanged()
    {
        try
        {
            AuthenticationStateChanged?.Invoke(this, _isAuthenticated);
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"[AUTH][ERROR] RaiseAuthChanged: {ex}");
        }
    }
}
