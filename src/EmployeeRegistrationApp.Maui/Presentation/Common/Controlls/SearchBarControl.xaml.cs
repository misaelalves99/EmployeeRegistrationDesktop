// src/EmployeeRegistrationApp.Maui/Presentation/Common/Controls/SearchBarControl.xaml.cs
using Microsoft.Maui.Controls;
using System.Windows.Input;

namespace EmployeeRegistrationApp.Maui.Presentation.Common.Controls
{
    /// <summary>
    /// Barra de busca reutiliz�vel para listas (Employees, Departments, etc.).
    /// Exibe �cone de busca, Entry e bot�o "Buscar".
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
        // Propriedades p�blicas
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
        /// Command executado quando o usu�rio toca em "Buscar" ou aperta Enter.
        /// </summary>
        public ICommand SearchCommand
        {
            get => (ICommand)GetValue(SearchCommandProperty);
            set => SetValue(SearchCommandProperty, value);
        }

        /// <summary>
        /// Par�metro opcional do SearchCommand.
        /// Se n�o for definido, o pr�prio Text ser� usado como par�metro.
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
            // Aqui voc� pode evoluir para "Limpar" se tiver texto,
            // por enquanto mantemos sempre "Buscar" para simplificar.
            ActionLabel.Text = "Buscar";
        }
    }
}
