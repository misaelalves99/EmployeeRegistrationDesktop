using EmployeeRegistrationApp.Maui.Presentation.Employees.ViewModels;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Xaml;
using System;
using System.Threading.Tasks;

namespace EmployeeRegistrationApp.Maui.Presentation.Employees.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    [QueryProperty(nameof(EmployeeId), "id")]
    public partial class EmployeeDetailsPage : ContentPage
    {
        private readonly EmployeeDetailsViewModel _viewModel;

        private string _employeeId = string.Empty;

        public string EmployeeId
        {
            get => _employeeId;
            set
            {
                _employeeId = value;
                _ = SafeLoadAsync(_employeeId);
            }
        }

        public EmployeeDetailsPage(EmployeeDetailsViewModel viewModel)
        {
            InitializeComponent();
            _viewModel = viewModel;
            BindingContext = _viewModel;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await SafeLoadAsync(_employeeId);
        }

        private async Task SafeLoadAsync(string idText)
        {
            if (Guid.TryParse(idText, out var id))
            {
                await _viewModel.LoadAsync(id);
            }
        }
    }
}
