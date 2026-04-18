using System.Windows.Controls;
using System.Windows.Data;
using VetClinic.Common;
using VetClinic.ViewModels;

namespace VetClinic.WPF.Controls
{
    public partial class SearchFilterControl : UserControl
    {
        public SearchFilterControl()
        {
            InitializeComponent();
            DataContextChanged += OnDataContextChanged;
        }

        private void OnDataContextChanged(object sender, System.Windows.DependencyPropertyChangedEventArgs e)
        {
            if (DataContext is SearchFilterViewModel vm)
                BuildColumns(vm.EntityType);
        }

        private void BuildColumns(Type entityType)
        {
            ResultsDataGrid.Columns.Clear();
            foreach (var col in ReflectionHelper.GetDisplayableColumns(entityType))
            {
                ResultsDataGrid.Columns.Add(new DataGridTextColumn
                {
                    Header = col.Header,
                    Binding = new Binding(col.PropertyName)
                });
            }
        }
    }
}
