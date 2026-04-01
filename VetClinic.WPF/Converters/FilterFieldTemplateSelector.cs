using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using VetClinic.Common;

namespace VetClinic.WPF.Converters
{
    public class FilterFieldTemplateSelector : DataTemplateSelector
    {
        public DataTemplate? StringTemplate { get; set; }
        public DataTemplate? NumericTemplate { get; set; }
        public DataTemplate? DateTemplate { get; set; }
        public DataTemplate? EnumTemplate { get; set; }

        public override DataTemplate? SelectTemplate(object item, DependencyObject container)
        {
            if (item is FilterFieldInfo field)
            {
                if (field.PropertyType == typeof(string))
                    return StringTemplate;
                if (field.PropertyType == typeof(DateTime))
                    return DateTemplate;
                if (field.PropertyType.IsEnum)
                    return EnumTemplate;
                if (field.PropertyType == typeof(int) || field.PropertyType == typeof(double) || field.PropertyType == typeof(decimal))
                    return NumericTemplate;
            }
            return StringTemplate;
        }
    }
}
