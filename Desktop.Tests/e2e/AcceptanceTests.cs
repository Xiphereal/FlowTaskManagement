using NUnit.Framework;

namespace Desktop.Tests.e2e;

public class AcceptanceTests
{
    private AppActions app;

    [SetUp]
    public void BeforeEach()
    {
        app = new AppActions();
    }

    [TearDown]
    public void AfterEach()
    {
        app.Dispose();
    }

    [Test]
    public void CanBeLaunched()
    {
        app.IsOpened();
    }

    [Test]
    public void CreatedTasks_ArePersisted()
    {
        app.CreateAnyTask();

        app.Close();

        app.Launch();
        app.HasAnyTaskBeenCreated();
    }
}