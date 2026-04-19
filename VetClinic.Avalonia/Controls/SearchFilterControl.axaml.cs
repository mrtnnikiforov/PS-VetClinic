using Avalonia.Controls;
using Avalonia.Data;
using VetClinic.Common;
using VetClinic.ViewModels;

namespace VetClinic.Avalonia.Controls
{
    public partial class SearchFilterControl : UserControl
    {
        public SearchFilterControl()
        {
            InitializeComponent();
            DataContextChanged += OnDataContextChanged;
        }

        private void OnDataContextChanged(object? sender, System.EventArgs e)
        {
            if (DataContext is SearchFilterViewModel vm)
                BuildColumns(vm.EntityType);
        }

        private void BuildColumns(Type entityType)
        {
            var grid = this.FindControl<DataGrid>("ResultsDataGrid");
            if (grid == null) return;

            grid.Columns.Clear();
            foreach (var col in ReflectionHelper.GetDisplayableColumns(entityType))
            {
                grid.Columns.Add(new DataGridTextColumn
                {
                    Header = col.Header,
                    Binding = new Binding(col.PropertyName)
                });
            }
        }
    }
}
