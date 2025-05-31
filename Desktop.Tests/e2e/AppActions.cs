using System;
using Desktop.Tests.e2e.Drivers;
using Desktop.Tests.e2e.Utils;

namespace Desktop.Tests.e2e;

public class AppActions : IDisposable
{
    private readonly MainWindowDriver mainWindow;
    private readonly BackendLauncher backendLauncher;

    public AppActions()
    {
        backendLauncher = new BackendLauncher();
        backendLauncher.Launch();

        mainWindow = new MainWindowDriver();
    }

    public void Dispose()
    {
        backendLauncher.Dispose();
        mainWindow.Dispose();
    }

    public void IsOpened()
    {
        mainWindow.IsOpened();
    }

    public void CreateAnyTask()
    {
        mainWindow.ClickOnNewTask();
        mainWindow.InputTaskName(name: "Any");
        mainWindow.ClickOnSaveTask();
    }

    public void Close()
    {
        mainWindow.Close();
    }

    public void Launch()
    {
        mainWindow.Launch();
    }

    public void HasAnyTaskBeenCreated()
    {
        mainWindow.ExistTaskNamed(name: "Any");
    }
}