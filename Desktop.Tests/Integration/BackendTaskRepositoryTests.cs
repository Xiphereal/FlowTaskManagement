using System.Threading.Tasks;
using Backend;
using Desktop.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
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

    [Test]
    public async Task LearningTestAbout_ASP_NET_Tests()
    {
        var factory = new WebApplicationFactory<DummyForAspNetTests>();
        var client = factory.CreateClient();
        
        var result = await client.GetAsync("/Project/");

        result.IsSuccessStatusCode.Should().BeTrue();
        var content = await result.Content.ReadAsStringAsync();
        content.Should()
            .Contain("A task").And
            .Contain("Another task").And
            .Contain("Yet another task");
    }
}