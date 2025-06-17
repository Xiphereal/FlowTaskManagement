using System.Windows;
using System.Windows.Threading;
using Desktop.Tasks;
using Desktop.Tasks.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Desktop;

/// <summary>
///    Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    private IConfigurationRoot Config { get; } =
        new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .Build();

    protected override void OnStartup(StartupEventArgs e)
    {
        var services = new ServiceCollection();

        services.AddSingleton<MainWindow>();
        services.AddSingleton<TaskCreation>();
        services.AddTransient<TaskCreationViewModel>();
        services.AddBackendTaskRepository(Config);

        var serviceProvider = services.BuildServiceProvider();
        serviceProvider.GetRequiredService<MainWindow>().Show();
    }

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