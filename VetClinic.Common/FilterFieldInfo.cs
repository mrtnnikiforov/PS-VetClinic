using System.ComponentModel;

namespace VetClinic.Common
{
    public class FilterFieldInfo : INotifyPropertyChanged
    {
        public string PropertyName { get; set; } = string.Empty;
        public string DisplayLabel { get; set; } = string.Empty;
        public Type PropertyType { get; set; } = typeof(string);

        private object? _value;
        public object? Value
        {
            get => _value;
            set
            {
                _value = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Value)));
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
