using System.Threading.Tasks;
using Backend.Tests.TestAPI;
using Desktop.Tasks;
using FluentAssertions;
using NUnit.Framework;

namespace Desktop.Tests.Integration;

public class BackendTaskRepositoryTests
{
    [TearDown]
    public void TearDown()
    {
        BackendBuilder.CleanUp();
    }

    [Test]
    public async Task NoTaskArePersisted_ReturnsNothing()
    {
        var backend = BackendBuilder.Backend().Launch();
        var sut = new BackendTaskRepository(backend);

        var result = await sut.All();

        result.Should().BeEmpty();
    }

    [Test]
    public async Task AnyPersistedTask_CanBeRetrieved()
    {
        var backend = BackendBuilder.Backend()
            .With(new Backend.Domain.Task("Any", "Any"))
            .Launch();
        var sut = new BackendTaskRepository(backend);

        var result = await sut.All();

        result.Should().NotBeEmpty();
    }
}