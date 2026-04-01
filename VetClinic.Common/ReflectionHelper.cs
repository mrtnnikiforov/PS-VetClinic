using System.Reflection;
using VetClinic.Model.Attributes;

namespace VetClinic.Common
{
    public static class ReflectionHelper
    {
        public static List<FilterFieldInfo> GetSearchableFields(Type entityType)
        {
            var fields = new List<FilterFieldInfo>();

            foreach (var prop in entityType.GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                var attr = prop.GetCustomAttribute<SearchableAttribute>();
                if (attr != null)
                {
                    fields.Add(new FilterFieldInfo
                    {
                        PropertyName = prop.Name,
                        DisplayLabel = attr.DisplayName,
                        PropertyType = prop.PropertyType
                    });
                }
            }

            return fields;
        }

        public static List<ColumnDefinition> GetDisplayableColumns(Type entityType)
        {
            var columns = new List<ColumnDefinition>();

            foreach (var prop in entityType.GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                var attr = prop.GetCustomAttribute<DisplayableAttribute>();
                if (attr != null)
                {
                    columns.Add(new ColumnDefinition
                    {
                        PropertyName = prop.Name,
                        Header = attr.Header,
                        Order = attr.Order
                    });
                }
            }

            return columns.OrderBy(c => c.Order).ToList();
        }
    }
}
