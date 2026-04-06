using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using Avalonia.Controls;
using Avalonia.Data;
using CommonColumnDefinition = VetClinic.Common.ColumnDefinition;
using VetClinic.ViewModels;

namespace VetClinic.Avalonia.Controls
{
    public partial class GenericListControl : UserControl
    {
        private GenericListViewModel? _currentVm;

        public GenericListControl()
        {
            InitializeComponent();
            DataContextChanged += OnDataContextChanged;
        }

        private void OnDataContextChanged(object? sender, EventArgs e)
        {
            if (_currentVm != null)
            {
                _currentVm.Columns.CollectionChanged -= OnColumnsChanged;
                _currentVm.PropertyChanged -= OnVmPropertyChanged;
            }

            if (DataContext is GenericListViewModel vm)
            {
                _currentVm = vm;
                vm.Columns.CollectionChanged += OnColumnsChanged;
                vm.PropertyChanged += OnVmPropertyChanged;
                RebuildColumns(vm.Columns);
                ListDataGrid.ItemsSource = vm.Items;
            }
        }

        private void OnVmPropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(GenericListViewModel.Items) && _currentVm != null)
            {
                ListDataGrid.ItemsSource = _currentVm.Items;
            }
        }

        private void OnColumnsChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            if (_currentVm != null)
            {
                RebuildColumns(_currentVm.Columns);
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
