using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Backend.Persistence;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using NUnit.Framework;

namespace Backend.Test;

public class IntegrationTests
{
    private const string SaveTaskUri = "/Project/SaveTask/";
    private const string GetTasksUri = "/Project/GetTasks/";

    [SetUp]
    public void SetUp()
    {
        using var context = new BackendContext();
        context.Database.EnsureCreated();
    }
    
    [TearDown]
    public void TearDown()
    {
        using var context = new BackendContext();
        context.Database.EnsureDeleted();
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
        return new WebApplicationFactory<DummyForAspNetTests>().CreateClient();
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