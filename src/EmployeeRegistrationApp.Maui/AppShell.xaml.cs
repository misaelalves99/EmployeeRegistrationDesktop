// src/EmployeeRegistrationApp.Maui/AppShell.xaml.cs
using EmployeeRegistrationApp.Maui.Presentation.Dashboard.Views;
using EmployeeRegistrationApp.Maui.Presentation.Employees.Views;
using EmployeeRegistrationApp.Maui.Presentation.Departments.Views;
using EmployeeRegistrationApp.Maui.Presentation.Positions.Views;
using EmployeeRegistrationApp.Maui.Presentation.Reports.Views;
using EmployeeRegistrationApp.Maui.Presentation.Settings.Views;
using Microsoft.Maui.Controls;

namespace EmployeeRegistrationApp.Maui;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();

        RegisterRoutes();
    }

    private static void RegisterRoutes()
    {
        // Dashboard
        Routing.RegisterRoute("dashboard", typeof(DashboardPage));

        // Funcionários
        Routing.RegisterRoute("employees/list", typeof(EmployeesListPage));
        Routing.RegisterRoute("employees/details", typeof(EmployeeDetailsPage));
        Routing.RegisterRoute("employees/form", typeof(EmployeeFormPage));
        Routing.RegisterRoute("employees/reactivate", typeof(EmployeeReactivatePage));

        // Departamentos
        Routing.RegisterRoute("departments/list", typeof(DepartmentsListPage));
        Routing.RegisterRoute("departments/details", typeof(DepartmentDetailsPage));
        Routing.RegisterRoute("departments/form", typeof(DepartmentFormPage));

        // Posições
        Routing.RegisterRoute("positions/list", typeof(PositionsListPage));
        Routing.RegisterRoute("positions/details", typeof(PositionDetailsPage));
        Routing.RegisterRoute("positions/form", typeof(PositionFormPage));

        // Relatórios
        Routing.RegisterRoute("reports/headcount", typeof(HeadcountByDepartmentPage));
        Routing.RegisterRoute("reports/salary-summary", typeof(SalarySummaryPage));

        // Configurações
        Routing.RegisterRoute("settings/company", typeof(CompanySettingsPage));
    }
}
