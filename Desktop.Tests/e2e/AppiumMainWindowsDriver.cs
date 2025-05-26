using System;
using System.IO;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Windows;

namespace Desktop.Tests.e2e;

public class AppiumMainWindowsDriver : IDisposable
{
    private readonly WindowsDriver windowsDriver;

    private readonly string desktopAppPath = Path.GetFullPath(
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

    private CreationWindowAppiumDriver creationWindow;

    public AppiumMainWindowsDriver()
    {
        var serverUri = new Uri(
            Environment.GetEnvironmentVariable("APPIUM_HOST") ?? "http://127.0.0.1:4723/");
        var options = new AppiumOptions()
        {
            App = desktopAppPath,
            DeviceName = "WindowsPC",
            AutomationName = "Windows",
            PlatformName = "Windows"
        };

        windowsDriver = new WindowsDriver(serverUri, options);
    }

    public void IsOpened()
    {
        windowsDriver.FindElement("name", "MainWindow");
    }

    public void Dispose()
    {
        windowsDriver?.Dispose();
        creationWindow.Dispose();
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
        windowsDriver.FindElement("name", "_Add task").Click();
    }

    public void InputTaskName(string name)
    {
        creationWindow = new CreationWindowAppiumDriver(windowsDriver.WindowHandles[0]);
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