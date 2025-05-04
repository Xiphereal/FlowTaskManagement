using System.Threading.Tasks;
using Desktop.Tasks;
using FluentAssertions;
using NUnit.Framework;

namespace Desktop.Tests.Integration;

using Backend = Backend.Tests.TestAPI.Backend;

public class BackendTaskRepositoryTests
{
    [Test]
    public async Task NoTaskArePersisted_ReturnsNothing()
    {
        var backend = Backend.Launch();
        var sut = new BackendTaskRepository(backend);

        var result = await sut.All();

        result.Should().BeEmpty();
    }
}