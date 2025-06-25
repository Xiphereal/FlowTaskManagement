using System.Globalization;
using System.Windows.Data;

namespace Desktop.BackendAvailability.Converters;

public class BoolToAvailabilityTextConverter : IValueConverter
{
    public object? Convert(
        object? value,
        Type targetType,
        object? parameter,
        CultureInfo culture)
    {
        return (bool)value
            ? "The cloud storage is available"
            : "The cloud storage is currently unavailable";
    }

    public object? ConvertBack(
        object? value,
        Type targetType,
        object? parameter,
        CultureInfo culture)
    {
        throw new NotSupportedException();
    }
}