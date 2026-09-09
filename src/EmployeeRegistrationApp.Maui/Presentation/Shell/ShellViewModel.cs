// src/EmployeeRegistrationApp.Maui/Presentation/Shell/ShellViewModel.cs
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using EmployeeRegistrationApp.Maui.Core.Base;
using EmployeeRegistrationApp.Maui.Core.Config;
using EmployeeRegistrationApp.Maui.Core.Services.Auth;
using EmployeeRegistrationApp.Maui.Core.Services.Navigation;
using Microsoft.Maui.Controls;

namespace EmployeeRegistrationApp.Maui.Presentation.Shell
{
    /// <summary>
    /// ViewModel do Shell principal (AppShell).
    /// Gerencia o menu lateral, seleção de módulo e ações globais (logout, tema, etc.).
    /// </summary>
    public sealed class ShellViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private readonly IAuthService _authService;

        private ShellMenuItem? _selectedMenuItem;

        public ShellViewModel(
            INavigationService navigationService,
            IAuthService authService)
        {
            _navigationService = navigationService;
            _authService = authService;

            MainMenuItems = new ObservableCollection<ShellMenuItem>();
            FooterMenuItems = new ObservableCollection<ShellMenuItem>();

            NavigateCommand = new Command<ShellMenuItem?>(async item => await NavigateAsync(item));
            LogoutCommand = new Command(async () => await LogoutAsync());

            BuildMenu();
        }

        /// <summary>
        /// Itens principais do menu (Dashboard, Employees, Departments, etc.).
        /// </summary>
        public ObservableCollection<ShellMenuItem> MainMenuItems { get; }

        /// <summary>
        /// Itens do rodapé do menu (Configurações, Logout, Sobre, etc.).
        /// </summary>
        public ObservableCollection<ShellMenuItem> FooterMenuItems { get; }

        /// <summary>
        /// Item atualmente selecionado.
        /// </summary>
        public ShellMenuItem? SelectedMenuItem
        {
            get => _selectedMenuItem;
            set
            {
                if (SetProperty(ref _selectedMenuItem, value))
                {
                    if (value != null)
                    {
                        _ = NavigateAsync(value);
                    }
                }
            }
        }

        /// <summary>
        /// Command para navegação quando um item do menu é clicado.
        /// </summary>
        public ICommand NavigateCommand { get; }

        /// <summary>
        /// Command para logout.
        /// </summary>
        public ICommand LogoutCommand { get; }

        private void BuildMenu()
        {
            MainMenuItems.Clear();
            FooterMenuItems.Clear();

            MainMenuItems.Add(new ShellMenuItem
            {
                Title = "Dashboard",
                Route = NavigationRoutes.Dashboard,
                Icon = "🏠",
                IsSelected = true
            });

            MainMenuItems.Add(new ShellMenuItem
            {
                Title = "Funcionários",
                Route = NavigationRoutes.EmployeesList,
                Icon = "👥"
            });

            MainMenuItems.Add(new ShellMenuItem
            {
                Title = "Departamentos",
                Route = NavigationRoutes.DepartmentsList,
                Icon = "🏢"
            });

            MainMenuItems.Add(new ShellMenuItem
            {
                Title = "Cargos",
                Route = NavigationRoutes.PositionsList,
                Icon = "💼"
            });

            MainMenuItems.Add(new ShellMenuItem
            {
                Title = "Relatórios",
                Route = NavigationRoutes.ReportsHome,
                Icon = "📊"
            });

            FooterMenuItems.Add(new ShellMenuItem
            {
                Title = "Configurações",
                Route = NavigationRoutes.Settings,
                Icon = "⚙️"
            });

            FooterMenuItems.Add(new ShellMenuItem
            {
                Title = "Sair",
                Route = NavigationRoutes.AuthLogin,
                Icon = "🚪"
            });

            if (MainMenuItems.Count > 0)
            {
                SetSelectedItem(MainMenuItems[0]);
            }
        }

        private async Task NavigateAsync(ShellMenuItem? item)
        {
            if (item == null || string.IsNullOrWhiteSpace(item.Route))
                return;

            SetSelectedItem(item);

            IsBusy = true;
            try
            {
                await _navigationService.GoToAsync(item.Route);
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async Task LogoutAsync()
        {
            IsBusy = true;
            try
            {
                await _authService.SignOutAsync();
                await _navigationService.NavigateToLoginAsync();
            }
            finally
            {
                IsBusy = false;
            }
        }

        private void SetSelectedItem(ShellMenuItem item)
        {
            foreach (var menuItem in MainMenuItems)
            {
                menuItem.IsSelected = ReferenceEquals(menuItem, item);
            }

            foreach (var footerItem in FooterMenuItems)
            {
                footerItem.IsSelected = ReferenceEquals(footerItem, item);
            }

            _selectedMenuItem = item;
            OnPropertyChanged(nameof(SelectedMenuItem));
        }
    }
}
