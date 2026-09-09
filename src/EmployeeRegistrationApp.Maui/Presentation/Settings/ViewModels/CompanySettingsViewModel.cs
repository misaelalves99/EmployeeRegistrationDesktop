// src/EmployeeRegistrationApp.Maui/Presentation/Settings/ViewModels/CompanySettingsViewModel.cs
using EmployeeRegistrationApp.Application.DTOs.Settings;
using EmployeeRegistrationApp.Application.Interfaces.Services;
using EmployeeRegistrationApp.Maui.Core.Base;
using EmployeeRegistrationApp.Maui.Core.Services.Navigation;
using EmployeeRegistrationApp.Maui.Core.Services.Notifications;
using System;
using System.Threading.Tasks;

namespace EmployeeRegistrationApp.Maui.Presentation.Settings.ViewModels;

public sealed class CompanySettingsViewModel : ViewModelBase
{
    private readonly ISettingsAppService _settingsAppService;
    private readonly INavigationService _navigationService;
    private readonly IToastService _toastService;

    private Guid? _id;
    public Guid? Id
    {
        get => _id;
        set => SetProperty(ref _id, value);
    }

    private string _companyName = string.Empty;
    public string CompanyName
    {
        get => _companyName;
        set
        {
            if (SetProperty(ref _companyName, value))
            {
                OnPropertyChanged(nameof(CanSave));
            }
        }
    }

    private string _legalName = string.Empty;
    public string LegalName
    {
        get => _legalName;
        set
        {
            if (SetProperty(ref _legalName, value))
            {
                OnPropertyChanged(nameof(CanSave));
            }
        }
    }

    private string _internalNotes = string.Empty;
    public string InternalNotes
    {
        get => _internalNotes;
        set => SetProperty(ref _internalNotes, value);
    }

    public bool CanSave =>
        !IsBusy &&
        !string.IsNullOrWhiteSpace(CompanyName) &&
        !string.IsNullOrWhiteSpace(LegalName);

    public AsyncCommand InitializeCommand { get; }
    public AsyncCommand SaveCommand { get; }
    public AsyncCommand CancelCommand { get; }

    public CompanySettingsViewModel(
        ISettingsAppService settingsAppService,
        INavigationService navigationService,
        IToastService toastService)
    {
        _settingsAppService = settingsAppService;
        _navigationService = navigationService;
        _toastService = toastService;

        InitializeCommand = new AsyncCommand(InitializeAsync);
        SaveCommand = new AsyncCommand(SaveAsync, () => CanSave);
        CancelCommand = new AsyncCommand(CancelAsync);
    }

    public async Task InitializeAsync()
    {
        if (IsBusy) return;

        try
        {
            IsBusy = true;

            var dto = await _settingsAppService.GetCompanySettingsAsync();

            Id = dto.Id;
            CompanyName = dto.CompanyName ?? string.Empty;
            LegalName = dto.LegalName ?? string.Empty;
            // InternalNotes = dto.Notes ?? string.Empty; // se existir no DTO
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine(ex);
            await _toastService.ShowError("Não foi possível carregar os dados da empresa.");
        }
        finally
        {
            IsBusy = false;
            RaiseCanExecuteChanged();
        }
    }

    private async Task SaveAsync()
    {
        if (!CanSave) return;

        try
        {
            IsBusy = true;
            RaiseCanExecuteChanged();

            var dto = new CompanySettingsDto
            {
                Id = Id,
                CompanyName = CompanyName.Trim(),
                LegalName = LegalName.Trim(),
                // Mapear outros campos se existirem (ex: Notes = InternalNotes)
            };

            await _settingsAppService.UpdateCompanySettingsAsync(dto);

            await _toastService.ShowSuccess("Configurações da empresa salvas com sucesso.");
            await _navigationService.GoBackAsync();
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine(ex);
            await _toastService.ShowError("Erro ao salvar configurações da empresa.");
        }
        finally
        {
            IsBusy = false;
            RaiseCanExecuteChanged();
        }
    }

    private Task CancelAsync()
    {
        return _navigationService.GoBackAsync();
    }

    private void RaiseCanExecuteChanged()
    {
        SaveCommand.RaiseCanExecuteChanged();
        OnPropertyChanged(nameof(CanSave));
    }
}
