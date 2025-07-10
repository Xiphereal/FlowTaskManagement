using System.Windows;

namespace Desktop.Common;

public class Popup : IMessageNotifier
{
    public void Notify(string message)
    {
        MessageBox.Show(
            message,
            "Internal Error",
            MessageBoxButton.OK,
            MessageBoxImage.Error);
    }
}