using NUnit.Framework;

namespace Desktop.Tests.e2e;

public class AcceptanceTests
{
    private AppiumWindowsDriver driver;

    [SetUp]
    public void BeforeEach()
    {
        driver = new AppiumWindowsDriver();
    }

    [TearDown]
    public void AfterEach()
    {
        driver.Dispose();
    }

    [Test]
    public void CanBeLaunched()
    {
        driver.IsOpened();
    }
}