using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using NUnit.Framework;

namespace Backend.Test;

public class IntegrationTests
{
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