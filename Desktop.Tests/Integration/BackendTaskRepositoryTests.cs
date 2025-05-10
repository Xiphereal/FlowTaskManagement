using System.Threading.Tasks;
using Backend.Tests.TestAPI;
using Desktop.Tasks;
using FluentAssertions;
using NUnit.Framework;

namespace Desktop.Tests.Integration;

public class BackendTaskRepositoryTests
{
    [Test]
    public async Task NoTaskArePersisted_ReturnsNothing()
    {
        var backend = BackendBuilder.Backend().Launch();
        var sut = new BackendTaskRepository(backend);

        var result = await sut.All();

        result.Should().BeEmpty();
    }
}