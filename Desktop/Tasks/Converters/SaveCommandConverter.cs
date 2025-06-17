using System.Globalization;
using System.Windows.Data;
using Desktop.Common;

namespace Desktop.Tasks.Converters;

public class SaveCommandConverter : IMultiValueConverter
{
    public object Convert(
        object[] values,
        Type targetType,
        object parameter,
        CultureInfo culture)
    {
        return (values[0] as ICloseable, values[1] as string, values[2] as string);
    }

    public object[] ConvertBack(
        object value,
        Type[] targetTypes,
        object parameter,
        CultureInfo culture)
    {
        throw new NotSupportedException();
    }
}