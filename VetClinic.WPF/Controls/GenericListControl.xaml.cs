using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Windows.Controls;
using System.Windows.Data;
using CommonColumnDefinition = VetClinic.Common.ColumnDefinition;
using VetClinic.ViewModels;

namespace VetClinic.WPF.Controls
{
    public partial class GenericListControl : UserControl
    {
        public GenericListControl()
        {
            InitializeComponent();
            DataContextChanged += OnDataContextChanged;
        }

        private void OnDataContextChanged(object sender, System.Windows.DependencyPropertyChangedEventArgs e)
        {
            if (e.OldValue is GenericListViewModel oldVm)
            {
                oldVm.Columns.CollectionChanged -= OnColumnsChanged;
            }

            if (e.NewValue is GenericListViewModel newVm)
            {
                newVm.Columns.CollectionChanged += OnColumnsChanged;
                RebuildColumns(newVm.Columns);
            }
        }

        private void OnColumnsChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            if (DataContext is GenericListViewModel vm)
            {
                RebuildColumns(vm.Columns);
            }
        }

        private void RebuildColumns(ObservableCollection<CommonColumnDefinition> columns)
        {
            ListDataGrid.Columns.Clear();
            foreach (var col in columns)
            {
                ListDataGrid.Columns.Add(new DataGridTextColumn
                {
                    Header = col.Header,
                    Binding = new Binding(col.PropertyName)
                });
            }
        }
    }
}
