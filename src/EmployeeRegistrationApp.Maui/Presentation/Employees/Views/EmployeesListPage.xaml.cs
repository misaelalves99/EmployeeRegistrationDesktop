using EmployeeRegistrationApp.Maui.Presentation.Employees.ViewModels;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Xaml;
using System.Threading.Tasks;

namespace EmployeeRegistrationApp.Maui.Presentation.Employees.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EmployeesListPage : ContentPage
    {
        private readonly EmployeesListViewModel _viewModel;

        public EmployeesListPage(EmployeesListViewModel viewModel)
        {
            InitializeComponent();
            _viewModel = viewModel;
            BindingContext = _viewModel;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            // Evita recarregar repetidamente se já estiver carregando
            await SafeLoadAsync();
        }

        private Task SafeLoadAsync()
        {
            // Se seu ViewModel já controla IsBusy, aqui só chama o InitializeAsync.
            return _viewModel.InitializeAsync();
        }
    }
}
