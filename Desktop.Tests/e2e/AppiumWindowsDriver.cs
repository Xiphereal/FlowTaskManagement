using System;
using System.IO;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Windows;

namespace Desktop.Tests.e2e;

public class AppiumWindowsDriver : IDisposable
{
    private readonly WindowsDriver windowsDriver;

    public AppiumWindowsDriver()
    {
        var serverUri = new Uri(
            Environment.GetEnvironmentVariable("APPIUM_HOST") ?? "http://127.0.0.1:4723/");
        var options = new AppiumOptions()
        {
            AutomationName = "Windows",
            PlatformName = "Windows",
            App = Path.GetFullPath(
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
                    "Desktop.exe")),
        };

        windowsDriver = new WindowsDriver(serverUri, options);
    }

    public void IsOpened()
    {
        windowsDriver.FindElement("name", "MainWindow");
    }

    public void Dispose()
    {
        windowsDriver?.CloseApp();
        windowsDriver?.Dispose();
    }
}