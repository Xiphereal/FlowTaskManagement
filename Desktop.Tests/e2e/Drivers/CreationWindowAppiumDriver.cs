using System;
using System.Linq;
using OpenQA.Selenium.Appium.Windows;

namespace Desktop.Tests.e2e.Drivers;

public class CreationWindowAppiumDriver : AppiumDriver, IDisposable
{
    private readonly WindowsDriver windowsDriver;

    public CreationWindowAppiumDriver(string creationWindowWindowHandle)
    {
        Options.AddAdditionalAppiumOption("appTopLevelWindow", creationWindowWindowHandle);
        windowsDriver = new WindowsDriver(ServerUri, Options);
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

    public override void Dispose()
    {
        windowsDriver?.Dispose();
    }
}