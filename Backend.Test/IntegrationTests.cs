using System.Collections.Generic;
using System.Net.Http;
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
        var client = CreateHttpClient();

        var result = await client.GetAsync("/Project/GetTasks/");

        result.IsSuccessStatusCode.Should().BeTrue();
        var content = await result.Content.ReadAsStringAsync();
        content.Should()
            .Contain("A task").And
            .Contain("Another task").And
            .Contain("Yet another task");

        var tasks =
            await result.Content.ReadAsAsync<IEnumerable<Domain.Task>>();

        tasks.Should().HaveCount(3);
        tasks.Should().AllBeOfType<Domain.Task>();
    }

    private static HttpClient CreateHttpClient()
    {
        return new WebApplicationFactory<DummyForAspNetTests>().CreateClient();
    }

    [Test]
    public async Task TasksCanBeSaved()
    {
        var client = CreateHttpClient();
        var taskToBeSaved = AnyTask();

        var postResult =
            await client.PostAsJsonAsync("/Project/SaveTask/", taskToBeSaved);

        postResult.IsSuccessStatusCode.Should().BeTrue();
        var existingTasks = await GetExistingTasks(client);
        existingTasks.Should().Contain(taskToBeSaved);
    }

    private static async Task<IEnumerable<Domain.Task>> GetExistingTasks(HttpClient client)
    {
        var getResult = await client.GetAsync("/Project/GetTasks/");

        return await getResult.Content.ReadAsAsync<IEnumerable<Domain.Task>>();
    }

    private static Domain.Task AnyTask()
    {
        return new Domain.Task(Name: "Any", Description: "Any");
    }
}