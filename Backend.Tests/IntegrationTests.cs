using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;

namespace Backend.Tests;

public class IntegrationTests
{
    private const string SaveTaskUri = "/project/tasks/";
    private const string GetTasksUri = "/project/tasks/";

    [TearDown]
    public void TearDown()
    {
        TestAPI.Backend.CleanUp();
    }

    [Test]
    public async Task NoTaskExistByDefault()
    {
        var client = CreateHttpClient();

        var existingTasks = await GetExistingTasks(client);

        existingTasks.Should().BeEmpty();
    }

    [Test]
    public async Task TasksCanBeSaved()
    {
        var client = CreateHttpClient();
        var taskToBeSaved = AnyTask();

        var postResult =
            await client.PostAsJsonAsync(SaveTaskUri, taskToBeSaved);

        postResult.IsSuccessStatusCode.Should().BeTrue();
        var existingTasks = await GetExistingTasks(client);
        existingTasks.Should().Contain(taskToBeSaved);
    }

    [Test]
    public async Task TasksCanBeSaved_AcrossDifferentServiceInstances()
    {
        var aClient = CreateHttpClient();
        var taskToBeSaved = AnyTask();

        var postResult =
            await aClient.PostAsJsonAsync(SaveTaskUri, taskToBeSaved);

        postResult.IsSuccessStatusCode.Should().BeTrue();
        var anotherClient = CreateHttpClient();
        var existingTasks = await GetExistingTasks(anotherClient);
        existingTasks.Should().Contain(taskToBeSaved);
    }

    private static HttpClient CreateHttpClient()
    {
        return TestAPI.Backend.Launch();
    }

    private static async Task<IEnumerable<Domain.Task>> GetExistingTasks(HttpClient client)
    {
        var getResult = await client.GetAsync(GetTasksUri);

        return await getResult.Content.ReadAsAsync<IEnumerable<Domain.Task>>();
    }

    private static Domain.Task AnyTask()
    {
        return new Domain.Task(Name: "Any", Description: "Any");
    }
}