using System;
using System.Threading.Tasks;
using Backend.Tests.TestAPI;
using Desktop.Tasks;
using FluentAssertions;
using NUnit.Framework;
using static Backend.Tests.TestAPI.TaskFactory;
using static Desktop.Tests.TestAPI.TaskFactory;

namespace Desktop.Tests.Integration;

public class BackendTaskRepositoryTests : ITaskRepositoryTests
{
    [Test]
    public async Task NoTaskArePersisted_ReturnsNothing()
    {
        var backend = BackendBuilder.Backend().Launch();
        var sut = new BackendTaskRepository(backend);

        var result = await sut.All();

        result.Tasks.Should().BeEmpty();
        result.Succeeded.Should().BeTrue();
    }

    [Test]
    public async Task CanLoadPersistedTasks()
    {
        var backend = BackendBuilder.Backend()
            .With(AnyBackendTask())
            .Launch();
        var sut = new BackendTaskRepository(backend);

        var result = await sut.All();

        result.Tasks.Should().NotBeEmpty();
        result.Succeeded.Should().BeTrue();
    }

    [Test]
    public async Task CanLoadSeveralPersistedTasks()
    {
        var aTask = BackendTask("aTask", "Any");
        var anotherTask = BackendTask("anotherTask", "Any");
        var backend = BackendBuilder.Backend()
            .With(aTask)
            .With(anotherTask)
            .Launch();
        var sut = new BackendTaskRepository(backend);

        var result = await sut.All();

        result.Tasks.Should().BeEquivalentTo(
            [
                aTask,
                anotherTask
            ],
            options => options.ComparingByMembers<Backend.Domain.Task>());
        result.Succeeded.Should().BeTrue();
    }

    [Test]
    public async Task CanPersistTasksWhenNoOtherExistedBefore()
    {
        var backend = BackendBuilder.Backend()
            .Launch();
        var sut = new BackendTaskRepository(backend);

        await sut.Save(DesktopTask(named: "A name", description: "A description"));

        var result = await sut.All();
        result.Tasks.Should().BeEquivalentTo(
            [
                BackendTask("A name", "A description"),
            ],
            options => options
                .Excluding(x => x.Id)
                .ComparingByMembers<Backend.Domain.Task>());
        result.Succeeded.Should().BeTrue();
    }

    [Test]
    public async Task PersistNewTaskDoesNotOverrideExistingOnes()
    {
        var existingTask = BackendTask(named: "existing");
        var backend = BackendBuilder.Backend()
            .With(existingTask)
            .Launch();
        var sut = new BackendTaskRepository(backend);

        var taskToPersist = DesktopTask(named: "Another task");
        await sut.Save(taskToPersist);

        var result = await sut.All();
        result.Tasks.Should().BeEquivalentTo(
        [
            DesktopTask(id: existingTask.Id),
            taskToPersist
        ]);
        result.Succeeded.Should().BeTrue();
    }

    [Test]
    public async Task TasksCanBeModified()
    {
        // Arrange.
        var existingTask = BackendTask(
            id: Guid.NewGuid(),
            named: "Old name",
            description: "Old description");
        var backend = BackendBuilder.Backend()
            .With(existingTask)
            .Launch();
        var sut = new BackendTaskRepository(backend);

        // Act.
        var modifiedTask = DesktopTask(
            existingTask.Id,
            named: "New name",
            description: "New description");
        var saveResult = await sut.Save(modifiedTask);

        // Assert.
        saveResult.Succeeded.Should().BeTrue();

        var queryResult = await sut.All();
        queryResult.Tasks.Should().BeEquivalentTo(
            [modifiedTask],
            options => options.ComparingByMembers<Domain.Task>());
        queryResult.Succeeded.Should().BeTrue();
    }

    [Test]
    public async Task CanDeleteExistingTasks()
    {
        var backend = BackendBuilder.Backend().Launch();
        var doc = new BackendTaskRepository(backend);
        var task = DesktopTask();
        await doc.Save(task);
        var sut = new BackendTaskRepository(backend);

        await sut.Delete(task);

        var result = await sut.All();
        result.Tasks.Should().BeEmpty();
        result.Succeeded.Should().BeTrue();
    }

    [Test]
    public async Task DeletesJustTheRequestedTask_KeepingTheOtherOnes()
    {
        // Arrange.
        var backend = BackendBuilder.Backend().Launch();
        var doc = new BackendTaskRepository(backend);

        var oneThatMustRemain = DesktopTask();
        await doc.Save(oneThatMustRemain);

        var toBeDeleted = DesktopTask("toBeDeleted");
        await doc.Save(toBeDeleted);

        var anotherThatMustRemain = DesktopTask("toBeDeletedStartingWithSameName");
        await doc.Save(anotherThatMustRemain);

        var sut = new BackendTaskRepository(backend);

        // Act.
        await sut.Delete(toBeDeleted);

        // Assert.
        var result = await sut.All();
        result.Tasks.Should().BeEquivalentTo(
        [
            oneThatMustRemain,
            anotherThatMustRemain,
        ]);
        result.Succeeded.Should().BeTrue();
    }

    [Test]
    public async Task AnyIssueOccursWhileLoadingPersistedTasks_ReturnsNothing()
    {
        var backend = BackendBuilder.Backend()
            .With(AnyBackendTask())
            .AlwaysThrowingAnyException()
            .Launch();
        var sut = new BackendTaskRepository(backend);

        var result = await sut.All();

        result.Tasks.Should().BeEmpty();
        result.Succeeded.Should().BeFalse();
    }

    [Test]
    public async Task AnyIssueOccursWhileSavingTask_DoesNotThrow()
    {
        var backend = BackendBuilder.Backend()
            .With(AnyBackendTask())
            .AlwaysThrowingAnyException()
            .Launch();
        var sut = new BackendTaskRepository(backend);

        var sutInvocation = async () => await sut.Save(DesktopTask());

        (await sutInvocation.Should().NotThrowAsync())
            .Which.Succeeded.Should().BeFalse();
    }

    [Test]
    public async Task AnyIssueOccursWhileDeletingTask_DoesNotThrow()
    {
        var backendTask = BackendTask(named: "ToBeDeleted");
        var backend = BackendBuilder.Backend()
            .With(backendTask)
            .AlwaysThrowingAnyException()
            .Launch();
        var sut = new BackendTaskRepository(backend);

        var sutInvocation = async () =>
            await sut.Delete(DesktopTask(id: backendTask.Id, named: "ToBeDeleted"));

        (await sutInvocation.Should().NotThrowAsync())
            .Which.Succeeded.Should().BeFalse();
    }
}