using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace Desktop.BackendAvailability.Converters;

public class BoolToImageConverter : IValueConverter
{
    public ImageSource BackendAvailable { get; set; }
    public ImageSource BackendUnavailable { get; set; }

    public object? Convert(
        object? value,
        Type targetType,
        object? parameter,
        CultureInfo culture)
    {
        return (bool)value ? BackendAvailable : BackendUnavailable;
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