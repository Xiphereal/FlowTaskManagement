using Desktop.Tasks;
using NUnit.Framework;

namespace Desktop.Tests.Integration;

public class BackendTaskRepositoryTests
{
    private const string FakeUrl = "http://fakeUrl";

    [Test]
    public void CanBeInstantiated()
    {
        var sut = new BackendTaskRepository(FakeUrl);
    }
}