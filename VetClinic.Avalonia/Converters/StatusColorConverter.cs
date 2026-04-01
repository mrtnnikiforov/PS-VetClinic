using System.Globalization;
using Avalonia.Data.Converters;
using Avalonia.Media;
using VetClinic.Model.Enums;

namespace VetClinic.Avalonia.Converters
{
    public class StatusColorConverter : IValueConverter
    {
        public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is AppointmentStatus status)
            {
                return status switch
                {
                    AppointmentStatus.Scheduled => Brushes.DodgerBlue,
                    AppointmentStatus.Completed => Brushes.Green,
                    AppointmentStatus.Cancelled => Brushes.Red,
                    _ => Brushes.Black
                };
            }
            return Brushes.Black;
        }

        public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
