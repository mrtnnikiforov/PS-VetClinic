using System.Collections;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Reflection;
using Avalonia.Collections;
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
                ListDataGrid.ItemsSource = new DataGridCollectionView(vm.Items);
            }
        }

        private void OnVmPropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(GenericListViewModel.Items) && _currentVm != null)
            {
                ListDataGrid.ItemsSource = new DataGridCollectionView(_currentVm.Items);
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
                    Binding = new Binding(col.PropertyName),
                    Width = GetColumnWidth(col.Header),
                    CanUserSort = true,
                    CustomSortComparer = new PropertyComparer(col.PropertyName)
                });
            }
        }

        private static DataGridLength GetColumnWidth(string header)
        {
            var h = header.ToUpperInvariant();
            return h switch
            {
                "ID" => new DataGridLength(60, DataGridLengthUnitType.Pixel),
                "COST" or "WEIGHT" or "WEIGHTKG" or "STATUS" =>
                    new DataGridLength(80, DataGridLengthUnitType.Pixel),
                "ADDRESS" or "EMAIL" or "NOTES" or "TREATMENT" or "REASON" or "MEDICATIONS" or "DIAGNOSIS" =>
                    new DataGridLength(2, DataGridLengthUnitType.Star),
                _ => new DataGridLength(1, DataGridLengthUnitType.Star),
            };
        }

        private sealed class PropertyComparer(string propertyName) : IComparer
        {
            public int Compare(object? x, object? y)
            {
                var valX = GetValue(x);
                var valY = GetValue(y);

                if (valX is IComparable cx)
                    return cx.CompareTo(valY);
                if (valY is IComparable cy)
                    return -cy.CompareTo(valX);

                return string.Compare(valX?.ToString(), valY?.ToString(), StringComparison.Ordinal);
            }

            private object? GetValue(object? obj)
            {
                return obj?.GetType().GetProperty(propertyName, BindingFlags.Public | BindingFlags.Instance)?.GetValue(obj);
            }
        }
    }
}
