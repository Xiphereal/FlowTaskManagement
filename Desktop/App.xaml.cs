using System.Configuration;
using System.Data;
using System.Windows;
using System.Windows.Threading;

namespace Desktop;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    private void App_OnDispatcherUnhandledException(
        object sender,
        DispatcherUnhandledExceptionEventArgs e)
    {
        MessageBox.Show(
            "An unknown internal failure has occurred. The app will be closed.",
            "Internal failure",
            MessageBoxButton.OK,
            MessageBoxImage.Error);
    }
}