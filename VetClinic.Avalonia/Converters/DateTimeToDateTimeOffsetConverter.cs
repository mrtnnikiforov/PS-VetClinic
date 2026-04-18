using System.Globalization;
using Avalonia.Data.Converters;

namespace VetClinic.Avalonia.Converters
{
    public class DateTimeToDateTimeOffsetConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is DateTime dateTime)
            {
                if (dateTime.Kind == DateTimeKind.Unspecified)
                    dateTime = DateTime.SpecifyKind(dateTime, DateTimeKind.Utc);

                return new DateTimeOffset(dateTime);
            }

            return new DateTimeOffset(DateTime.Today);
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is DateTimeOffset dateTimeOffset)
            {
                return dateTimeOffset.DateTime;
            }

            if (value is DateTime dateTime)
            {
                return dateTime;
            }

            return DateTime.Today;
        }
    }
}

