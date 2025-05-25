using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Backend.Tests.TestAPI;
using FluentAssertions;
using NUnit.Framework;
using static Backend.Tests.TestAPI.TaskFactory;

namespace Backend.Tests;

public class IntegrationTests
{
    private readonly Guid aDatabaseId = Guid.NewGuid();
    private const string TasksUri = "/project/tasks";

    [Test]
    public async Task NoTaskExistByDefault()
    {
        var client = BackendBuilder.Backend().Launch();

        var existingTasks = await GetExistingTasks(client);

        existingTasks.Should().BeEmpty();
    }

    [Test]
    public async Task TasksCanBeSaved()
    {
        var client = BackendBuilder.Backend().Launch();
        var taskToBeSaved = AnyBackendTask();

        var postResult =
            await client.PostAsJsonAsync(TasksUri, taskToBeSaved);

        postResult.IsSuccessStatusCode.Should().BeTrue();
        var existingTasks = await GetExistingTasks(client);
        existingTasks.Should().Contain(taskToBeSaved);
    }

    [Test]
    public async Task TasksCanBeSaved_AcrossDifferentServiceInstances()
    {
        var aClient = BackendBuilder.Backend().With(aDatabaseId).Launch();
        var taskToBeSaved = AnyBackendTask();

        var postResult =
            await aClient.PostAsJsonAsync(TasksUri, taskToBeSaved);

        postResult.IsSuccessStatusCode.Should().BeTrue();
        var anotherClient = BackendBuilder.Backend().With(aDatabaseId).Launch();
        var existingTasks = await GetExistingTasks(anotherClient);
        existingTasks.Should().Contain(taskToBeSaved);
    }

    [Test]
    public async Task TasksCanBeDeleted()
    {
        var sut = BackendBuilder.Backend().Launch();
        var taskToBeDeleted = AnyBackendTask();
        await sut.PostAsJsonAsync(TasksUri, taskToBeDeleted);

        var result = await sut.DeleteAsync($"{TasksUri}/{taskToBeDeleted.Name}");

        result.IsSuccessStatusCode.Should().BeTrue();
        var existingTasks = await GetExistingTasks(sut);
        existingTasks.Should().BeEmpty();
    }

    private static async Task<IEnumerable<Domain.Task>> GetExistingTasks(HttpClient client)
    {
        var getResult = await client.GetAsync(TasksUri);

        return await getResult.Content.ReadAsAsync<IEnumerable<Domain.Task>>();
    }
}