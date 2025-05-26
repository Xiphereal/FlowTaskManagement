using System;
using System.IO;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Service;

namespace Desktop.Tests.e2e.Drivers;

public abstract class AppiumWindowsDriver : IDisposable
{
    private const string AppiumServerLogPath = "AppiumServerLog.txt";

    private const string AppiumServerIp = "127.0.0.1";
    private const int AppiumServerPort = 4723;

    private static readonly string DefaultServerUri =
        $"http://{AppiumServerIp}:{AppiumServerPort}/";

    protected readonly Uri ServerUri = new(
        Environment.GetEnvironmentVariable("APPIUM_HOST") ?? DefaultServerUri);

    protected readonly AppiumOptions Options = new()
    {
        DeviceName = "WindowsPC",
        AutomationName = "Windows",
        PlatformName = "Windows"
    };

    protected readonly AppiumLocalService AppiumLocalService = new AppiumServiceBuilder()
        .WithIPAddress(AppiumServerIp).UsingPort(AppiumServerPort)
        .WithLogFile(new FileInfo(AppiumServerLogPath))
        .Build();

    public abstract void Dispose();
}