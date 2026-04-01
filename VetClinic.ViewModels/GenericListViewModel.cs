using System.Collections.ObjectModel;
using System.Windows.Input;
using VetClinic.Common;

namespace VetClinic.ViewModels
{
    public class GenericListViewModel : ViewModelBase
    {
        private readonly Type _entityType;

        private ObservableCollection<ColumnDefinition> _columns = new();
        public ObservableCollection<ColumnDefinition> Columns
        {
            get => _columns;
            set => SetProperty(ref _columns, value);
        }

        private ObservableCollection<object> _items = new();
        public ObservableCollection<object> Items
        {
            get => _items;
            set => SetProperty(ref _items, value);
        }

        private object? _selectedItem;
        public object? SelectedItem
        {
            get => _selectedItem;
            set
            {
                if (SetProperty(ref _selectedItem, value) && value != null)
                {
                    ItemSelected?.Invoke(value);
                }
            }
        }

        public event Action<object>? ItemSelected;

        public ICommand ClearCommand { get; }

        public GenericListViewModel(Type entityType)
        {
            _entityType = entityType;
            Columns = new ObservableCollection<ColumnDefinition>(
                ReflectionHelper.GetDisplayableColumns(entityType));
            ClearCommand = new RelayCommand(_ => Clear());
        }

        public void SetColumns(IEnumerable<ColumnDefinition> columns)
        {
            Columns = new ObservableCollection<ColumnDefinition>(columns);
        }

        public void SetItems(System.Collections.IEnumerable items)
        {
            Items = new ObservableCollection<object>(items.Cast<object>());
        }

        public void Clear()
        {
            Items.Clear();
            SelectedItem = null;
        }
    }
}
