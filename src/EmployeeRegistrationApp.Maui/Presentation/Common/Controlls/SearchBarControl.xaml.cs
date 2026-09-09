// src/EmployeeRegistrationApp.Maui/Presentation/Common/Controls/SearchBarControl.xaml.cs
using AndroidX.ConstraintLayout.Utils.Widget;
using Microsoft.Maui.Controls;
using System.Windows.Input;

namespace EmployeeRegistrationApp.Maui.Presentation.Common.Controls
{
    /// <summary>
    /// Barra de busca reutilizável para listas (Employees, Departments, etc.).
    /// Exibe ícone de busca, Entry e botão "Buscar".
    /// </summary>
    public partial class SearchBarControl : ContentView
    {
        public SearchBarControl()
        {
            InitializeComponent();
            UpdateActionVisual();
        }

        // ============================
        // Bindable Properties
        // ============================

        public static readonly BindableProperty TextProperty =
            BindableProperty.Create(
                nameof(Text),
                typeof(string),
                typeof(SearchBarControl),
                default(string),
                BindingMode.TwoWay,
                propertyChanged: OnTextChanged);

        public static readonly BindableProperty PlaceholderProperty =
            BindableProperty.Create(
                nameof(Placeholder),
                typeof(string),
                typeof(SearchBarControl),
                "Buscar...");

        public static readonly BindableProperty SearchCommandProperty =
            BindableProperty.Create(
                nameof(SearchCommand),
                typeof(ICommand),
                typeof(SearchBarControl),
                default(ICommand));

        public static readonly BindableProperty SearchCommandParameterProperty =
            BindableProperty.Create(
                nameof(SearchCommandParameter),
                typeof(object),
                typeof(SearchBarControl),
                default(object));

        // ============================
        // Propriedades públicas
        // ============================

        /// <summary>
        /// Texto atual digitado na barra de busca.
        /// </summary>
        public string Text
        {
            get => (string)GetValue(TextProperty);
            set => SetValue(TextProperty, value);
        }

        /// <summary>
        /// Placeholder exibido no Entry.
        /// </summary>
        public string Placeholder
        {
            get => (string)GetValue(PlaceholderProperty);
            set => SetValue(PlaceholderProperty, value);
        }

        /// <summary>
        /// Command executado quando o usuário toca em "Buscar" ou aperta Enter.
        /// </summary>
        public ICommand SearchCommand
        {
            get => (ICommand)GetValue(SearchCommandProperty);
            set => SetValue(SearchCommandProperty, value);
        }

        /// <summary>
        /// Parâmetro opcional do SearchCommand.
        /// Se não for definido, o próprio Text será usado como parâmetro.
        /// </summary>
        public object SearchCommandParameter
        {
            get => GetValue(SearchCommandParameterProperty);
            set => SetValue(SearchCommandParameterProperty, value);
        }

        // ============================
        // Eventos internos
        // ============================

        private static void OnTextChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is SearchBarControl control)
            {
                control.UpdateActionVisual();
            }
        }

        private void OnEntryCompleted(object? sender, System.EventArgs e)
        {
            ExecuteSearch();
        }

        private void OnActionTapped(object? sender, TappedEventArgs e)
        {
            ExecuteSearch();
        }

        private void ExecuteSearch()
        {
            var parameter = SearchCommandParameter ?? Text;

            if (SearchCommand?.CanExecute(parameter) == true)
            {
                SearchCommand.Execute(parameter);
            }
        }

        private void UpdateActionVisual()
        {
            // Aqui você pode evoluir para "Limpar" se tiver texto,
            // por enquanto mantemos sempre "Buscar" para simplificar.
            ActionLabel.Text = "Buscar";
        }
    }
}
