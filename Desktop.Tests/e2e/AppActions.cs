using System;
using Desktop.Tests.e2e.Drivers;

namespace Desktop.Tests.e2e;

public class AppActions : IDisposable
{
    private readonly AppiumMainWindowsDriver mainWindow = new();

    public void Dispose()
    {
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