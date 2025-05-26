using System;
using System.Linq;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Windows;

namespace Desktop.Tests.e2e;

public class CreationWindowAppiumDriver : IDisposable
{
    private readonly WindowsDriver windowsDriver;

    public CreationWindowAppiumDriver(string creationWindowWindowHandle)
    {
        var serverUri = new Uri(
            Environment.GetEnvironmentVariable("APPIUM_HOST") ?? "http://127.0.0.1:4723/");
        var options = new AppiumOptions()
        {
            DeviceName = "WindowsPC",
            AutomationName = "Windows",
            PlatformName = "Windows"
        };
        options.AddAdditionalAppiumOption("appTopLevelWindow", creationWindowWindowHandle);
        windowsDriver = new WindowsDriver(serverUri, options);
    }

    public void InputTaskName(string name)
    {
        var textBoxes = windowsDriver.FindElements("class name", "TextBox");

        textBoxes.First().SendKeys(name);
    }

    public void ClickOnSaveTask()
    {
        windowsDriver.FindElement("name", "Add").Click();
    }

    public void Dispose()
    {
        windowsDriver?.Dispose();
    }
}