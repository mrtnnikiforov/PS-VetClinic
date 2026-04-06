using System.Collections.ObjectModel;
using System.Collections.Specialized;
using Avalonia.Controls;
using Avalonia.Data;
using CommonColumnDefinition = VetClinic.Common.ColumnDefinition;
using VetClinic.ViewModels;

namespace VetClinic.Avalonia.Controls
{
    public partial class GenericListControl : UserControl
    {
        public GenericListControl()
        {
            InitializeComponent();
            DataContextChanged += OnDataContextChanged;
        }

        private void OnDataContextChanged(object? sender, EventArgs e)
        {
            if (DataContext is GenericListViewModel vm)
            {
                vm.Columns.CollectionChanged += OnColumnsChanged;
                RebuildColumns(vm.Columns);
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
