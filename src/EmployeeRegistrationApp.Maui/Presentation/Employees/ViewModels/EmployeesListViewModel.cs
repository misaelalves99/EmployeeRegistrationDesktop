using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;
using EmployeeRegistrationApp.Application.DTOs.Employees;
using EmployeeRegistrationApp.Application.Interfaces.Services;
using EmployeeRegistrationApp.Maui.Core.Base;
using EmployeeRegistrationApp.Maui.Core.Config;
using EmployeeRegistrationApp.Maui.Core.Services.Dialogs;
using EmployeeRegistrationApp.Maui.Core.Services.Navigation;
using Microsoft.Maui.Controls;

namespace EmployeeRegistrationApp.Maui.Presentation.Employees.ViewModels;

public sealed class EmployeesListViewModel : ViewModelBase
{
    private readonly IEmployeeAppService _employeeAppService;
    private readonly INavigationService _navigationService;
    private readonly IDialogService _dialogService;

    public EmployeesListViewModel(
        IEmployeeAppService employeeAppService,
        INavigationService navigationService,
        IDialogService dialogService)
    {
        _employeeAppService = employeeAppService;
        _navigationService = navigationService;
        _dialogService = dialogService;

        Title = "Colaboradores";

        Employees = new ObservableCollection<EmployeeListItemDto>();

        RefreshCommand = new Command(async () => await LoadEmployeesAsync(), () => IsNotBusy);
        NewEmployeeCommand = new Command(async () => await NavigateToCreateAsync(), () => IsNotBusy);
        OpenDetailsCommand = new Command<EmployeeListItemDto>(
            async e => await NavigateToDetailsAsync(e),
            e => IsNotBusy && e != null);
        ReactivateCommand = new Command<EmployeeListItemDto>(
            async e => await NavigateToReactivateAsync(e),
            e => IsNotBusy && e != null);
    }

    private string _searchText = string.Empty;
    private bool _isRefreshing;

    public ObservableCollection<EmployeeListItemDto> Employees { get; }

    public string SearchText
    {
        get => _searchText;
        set
        {
            if (SetProperty(ref _searchText, value))
            {
                _ = LoadEmployeesAsync();
            }
        }
    }

    public bool IsRefreshing
    {
        get => _isRefreshing;
        set => SetProperty(ref _isRefreshing, value);
    }

    public ICommand RefreshCommand { get; }
    public ICommand NewEmployeeCommand { get; }
    public ICommand OpenDetailsCommand { get; }
    public ICommand ReactivateCommand { get; }

    public async Task InitializeAsync()
    {
        await LoadEmployeesAsync();
    }

    public Task LoadAsync() => InitializeAsync();

    private async Task LoadEmployeesAsync()
    {
        if (IsBusy) return;

        try
        {
            IsBusy = true;
            IsRefreshing = true;
            RefreshCommandStates();

            Employees.Clear();

            var result = await _employeeAppService.GetPagedAsync(
                page: 1,
                pageSize: 100,
                search: string.IsNullOrWhiteSpace(SearchText) ? null : SearchText.Trim(),
                departmentId: null,
                status: null);

            foreach (var item in result.Items)
            {
                Employees.Add(item);
            }
        }
        catch (Exception ex)
        {
            await _dialogService.ShowErrorAsync("Erro ao carregar colaboradores.", ex);
        }
        finally
        {
            IsBusy = false;
            IsRefreshing = false;
            RefreshCommandStates();
        }
    }

    private Task NavigateToCreateAsync()
        => _navigationService.NavigateToAsync(NavigationRoutes.EmployeeFormPage);

    private Task NavigateToDetailsAsync(EmployeeListItemDto? employee)
    {
        if (employee is null)
            return Task.CompletedTask;

        var route = $"{NavigationRoutes.EmployeeDetailsPage}?id={employee.Id:D}";
        return _navigationService.NavigateToAsync(route);
    }

    private Task NavigateToReactivateAsync(EmployeeListItemDto? employee)
    {
        if (employee is null)
            return Task.CompletedTask;

        var route = $"{NavigationRoutes.EmployeeReactivatePage}?id={employee.Id:D}";
        return _navigationService.NavigateToAsync(route);
    }

    private void RefreshCommandStates()
    {
        ((Command)RefreshCommand).ChangeCanExecute();
        ((Command)NewEmployeeCommand).ChangeCanExecute();
        ((Command<EmployeeListItemDto>)OpenDetailsCommand).ChangeCanExecute();
        ((Command<EmployeeListItemDto>)ReactivateCommand).ChangeCanExecute();
    }
}
