using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Windows;

namespace Desktop.Tests.e2e.Drivers;

public static class AppiumExtensions
{
    public static AppiumElement FindElementByAccessibilityId(
        this WindowsDriver driver,
        string elementId)
    {
        return driver.FindElement("accessibility id", elementId);
    }
}