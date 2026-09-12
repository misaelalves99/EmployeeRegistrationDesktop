// src/EmployeeRegistrationApp.Maui/Core/Config/NavigationRoutes.cs
namespace EmployeeRegistrationApp.Maui.Core.Config;

/// <summary>
/// Centraliza as rotas usadas no Shell.
/// Mantém tudo tipado por constante, evitando strings soltas espalhadas.
/// </summary>
public static class NavigationRoutes
{
    // =========================================================
    // Rotas base (como você já tinha)
    // =========================================================

    // Auth
    public const string LoginPage = "login";
    public const string RegisterPage = "register";

    // Dashboard
    public const string DashboardPage = "dashboard";

    // Employees
    public const string EmployeesListPage = "employees/list";
    public const string EmployeeDetailsPage = "employees/details";
    public const string EmployeeFormPage = "employees/form";
    public const string EmployeeReactivatePage = "employees/reactivate";

    // Departments
    public const string DepartmentsListPage = "departments/list";
    public const string DepartmentDetailsPage = "departments/details";
    public const string DepartmentFormPage = "departments/form";

    // Positions
    public const string PositionsListPage = "positions/list";
    public const string PositionDetailsPage = "positions/details";
    public const string PositionFormPage = "positions/form";

    // Reports
    public const string ReportsPage = "reports";
    public const string HeadcountByDepartmentPage = "reports/headcount";
    public const string SalarySummaryPage = "reports/salary-summary";

    // Settings
    public const string SettingsPage = "settings";
    public const string CompanySettingsPage = "settings/company";

    /// <summary>
    /// Rotas absolutas (Shell) que podem ser usadas para resetar stack.
    /// Ex: //login, //dashboard
    /// </summary>
    public static class Root
    {
        // Mantive o seu padrão com "//" aqui
        public const string Login = $"//{LoginPage}";
        public const string Dashboard = $"//{DashboardPage}";
    }

    // =========================================================
    // Aliases usados pelos ViewModels (para não dar CS0117)
    // =========================================================

    // Atalhos para Auth usados em LoginViewModel / ShellViewModel
    public const string AuthLogin = LoginPage;
    public const string AuthRegister = RegisterPage;
    public const string AuthForgotPassword = "forgot-password"; // ajuste se sua rota for diferente

    // Dashboard (usado em ShellViewModel como NavigationRoutes.Dashboard)
    public const string Dashboard = DashboardPage;

    // Employees
    public const string EmployeesList = EmployeesListPage;
    public const string EmployeeDetails = EmployeeDetailsPage;
    public const string EmployeeForm = EmployeeFormPage;
    public const string EmployeeReactivate = EmployeeReactivatePage;

    // Departments
    public const string DepartmentsList = DepartmentsListPage;
    public const string DepartmentDetails = DepartmentDetailsPage;
    public const string DepartmentForm = DepartmentFormPage;

    // Positions
    public const string PositionsList = PositionsListPage;
    public const string PositionDetails = PositionDetailsPage;
    public const string PositionForm = PositionFormPage;

    // Reports (ShellViewModel usa ReportsHome)
    public const string ReportsHome = ReportsPage;

    // Settings
    public const string Settings = SettingsPage;
}
