using System;
using System.Collections.Generic;
using System.Net;
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
    public async Task SeveralTasksCanHaveTheSameName()
    {
        var backend = BackendBuilder.Backend().Launch();
        const string sameName = "Same name";
        var aTaskToBeSaved = BackendTask(sameName);
        var anotherTaskToBeSaved = BackendTask(sameName);

        await backend.PostAsJsonAsync(TasksUri, aTaskToBeSaved);
        var postResult = await backend.PostAsJsonAsync(TasksUri, anotherTaskToBeSaved);

        postResult.IsSuccessStatusCode.Should().BeTrue();
        var existingTasks = await GetExistingTasks(backend);
        existingTasks.Should().AllSatisfy(x => x.Name.Should().Be(sameName));
    }

    [Test]
    public async Task TaskToBeCreatedAlreadyExists_NotifiesCallerAndDoesNotDuplicatedIt()
    {
        var alreadyExistingTask = AnyBackendTask();
        var sut = BackendBuilder.Backend()
            .With(alreadyExistingTask)
            .Launch();

        var result = await sut.PostAsJsonAsync(TasksUri, alreadyExistingTask);

        result.StatusCode.Should().Be(HttpStatusCode.Conflict);
        var existingTasks = await GetExistingTasks(sut);
        existingTasks.Should().ContainSingle();
    }

    [Test]
    public async Task TasksCanBeModified()
    {
        var originalTask = BackendTask("Old name", "Old description");
        var backend = BackendBuilder.Backend()
            .With(originalTask)
            .Launch();

        var modifiedTask = BackendTask(
            originalTask.Id,
            "New name",
            "New description");
        var result = await backend.PutAsJsonAsync(TasksUri, modifiedTask);

        result.IsSuccessStatusCode.Should().BeTrue();
        var existingTasks = await GetExistingTasks(backend);
        existingTasks.Should().BeEquivalentTo([modifiedTask]);
    }

    [Test]
    public async Task TaskToBeModifiedDoesNotExist_NotifiesCaller()
    {
        var sut = BackendBuilder.Backend().Launch();
        var toBeModified = BackendTask("Any");

        var result = await sut.PutAsJsonAsync(TasksUri, toBeModified);

        result.StatusCode.Should().Be(HttpStatusCode.NotFound);
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

    [Test]
    public async Task TaskToBeDeletedDoesNotExist_NotifiesCaller()
    {
        var sut = BackendBuilder.Backend().Launch();

        var result = await sut.DeleteAsync($"{TasksUri}/nonExisting");

        result.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    private static async Task<IEnumerable<Domain.Task>> GetExistingTasks(HttpClient client)
    {
        var getResult = await client.GetAsync(TasksUri);

        return await getResult.Content.ReadAsAsync<IEnumerable<Domain.Task>>();
    }
}