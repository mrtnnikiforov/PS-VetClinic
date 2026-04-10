using Avalonia.Controls;
using Avalonia.Controls.Templates;
using VetClinic.Common;

namespace VetClinic.Avalonia.Converters
{
    public class FilterFieldTemplateSelector : IDataTemplate
    {
        public IDataTemplate? StringTemplate { get; set; }
        public IDataTemplate? NumericTemplate { get; set; }
        public IDataTemplate? DateTemplate { get; set; }
        public IDataTemplate? EnumTemplate { get; set; }

        public Control? Build(object? param)
        {
            if (param is not FilterFieldInfo field)
            {
                return StringTemplate?.Build(param);
            }

            var selectedTemplate = SelectTemplate(field) ?? StringTemplate;
            return selectedTemplate?.Build(param);
        }

        public bool Match(object? data)
        {
            return data is FilterFieldInfo;
        }

        private IDataTemplate? SelectTemplate(FilterFieldInfo field)
        {
            if (field.PropertyType == typeof(string))
            {
                return StringTemplate;
            }

            if (field.PropertyType == typeof(DateTime))
            {
                return DateTemplate;
            }

            if (field.PropertyType.IsEnum)
            {
                return EnumTemplate;
            }

            if (field.PropertyType == typeof(int) ||
                field.PropertyType == typeof(double) ||
                field.PropertyType == typeof(decimal))
            {
                return NumericTemplate;
            }

            return StringTemplate;
        }
    }
}

