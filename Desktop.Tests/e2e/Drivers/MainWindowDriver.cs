using System;
using System.IO;
using OpenQA.Selenium.Appium.Windows;

namespace Desktop.Tests.e2e.Drivers;

public class MainWindowDriver : AppiumWindowsDriver
{
    private readonly WindowsDriver windowsDriver;

    private CreationWindowDriver creationWindow;

    public MainWindowDriver()
    {
        Options.App = Path.GetFullPath(
            Path.Combine(
                Environment.CurrentDirectory,
                "..",
                "..",
                "..",
                "..",
                "Desktop",
                "bin",
                "Debug",
                "net9.0-windows",
                "Desktop.exe"));
        windowsDriver = new WindowsDriver(ServerUri, Options);
    }

    public void IsOpened()
    {
        windowsDriver.FindElementByAccessibilityId("TasksAppMainWindow");
    }

    public override void Dispose()
    {
        windowsDriver?.Dispose();
        creationWindow?.Dispose();
    }

    public void Launch()
    {
        windowsDriver.LaunchApp();
    }

    public void Close()
    {
        windowsDriver.CloseApp();
    }

    public void ClickOnNewTask()
    {
        windowsDriver.FindElementByAccessibilityId("AddTask").Click();
    }

    public void InputTaskName(string name)
    {
        creationWindow = new CreationWindowDriver(windowsDriver.WindowHandles[0]);
        creationWindow.InputTaskName(name);
    }

    public void ClickOnSaveTask()
    {
        creationWindow.ClickOnSaveTask();
    }

    public void ExistTaskNamed(string name)
    {
        windowsDriver.FindElement("name", name);
    }
}