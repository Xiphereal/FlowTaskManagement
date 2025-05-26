using NUnit.Framework;

namespace Desktop.Tests.e2e;

public class AcceptanceTests
{
    private AppiumMainWindowsDriver driver;

    [SetUp]
    public void BeforeEach()
    {
        driver = new AppiumMainWindowsDriver();
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

    [Test]
    public void CreatedTasks_ArePersisted()
    {
        driver.ClickOnNewTask();
        driver.InputTaskName(name: "Any");
        driver.ClickOnSaveTask();

        driver.Close();

        driver.Launch();
        driver.ExistTaskNamed(name: "Any");
    }
}