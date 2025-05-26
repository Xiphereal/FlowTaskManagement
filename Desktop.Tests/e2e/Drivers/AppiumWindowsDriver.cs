using System;
using OpenQA.Selenium.Appium;

namespace Desktop.Tests.e2e.Drivers;

public abstract class AppiumWindowsDriver : IDisposable
{
    private const string DefaultServerUri = "http://127.0.0.1:4723/";

    protected readonly Uri ServerUri = new(
        Environment.GetEnvironmentVariable("APPIUM_HOST") ?? DefaultServerUri);

    protected readonly AppiumOptions Options = new()
    {
        DeviceName = "WindowsPC",
        AutomationName = "Windows",
        PlatformName = "Windows"
    };

    public abstract void Dispose();
}