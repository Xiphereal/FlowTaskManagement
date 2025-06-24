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

        result.Should().BeEmpty();
    }

    [Test]
    public async Task CanLoadPersistedTasks()
    {
        var backend = BackendBuilder.Backend()
            .With(AnyBackendTask())
            .Launch();
        var sut = new BackendTaskRepository(backend);

        var result = await sut.All();

        result.Should().NotBeEmpty();
    }

    [Test]
    public async Task CanLoadSeveralPersistedTasks()
    {
        var aTask = new Backend.Domain.Task("aTask", "Any");
        var anotherTask = new Backend.Domain.Task("anotherTask", "Any");
        var backend = BackendBuilder.Backend()
            .With(aTask)
            .With(anotherTask)
            .Launch();
        var sut = new BackendTaskRepository(backend);

        var result = await sut.All();

        result.Should().BeEquivalentTo(
        [
            aTask,
            anotherTask
        ]);
    }

    [Test]
    public async Task CanPersistTasksWhenNoOtherExistedBefore()
    {
        var backend = BackendBuilder.Backend()
            .Launch();
        var sut = new BackendTaskRepository(backend);

        await sut.Save(DesktopTask());

        var existingTasks = await sut.All();
        existingTasks.Should().BeEquivalentTo(
        [
            AnyBackendTask(),
        ]);
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

        var existingTasks = await sut.All();
        existingTasks.Should().BeEquivalentTo(
        [
            DesktopTask(named: existingTask.Name),
            taskToPersist
        ]);
    }

    [Test]
    public async Task CanDeleteExistingTasks()
    {
        var backend = BackendBuilder.Backend().Launch();
        var doc = new BackendTaskRepository(backend);
        await doc.Save(DesktopTask());
        var sut = new BackendTaskRepository(backend);

        await sut.Delete(DesktopTask());

        var result = await sut.All();
        result.Should().BeEmpty();
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
        result.Should().BeEquivalentTo(
        [
            oneThatMustRemain,
            anotherThatMustRemain,
        ]);
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

        result.Should().BeEmpty();
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

        await sutInvocation.Should().NotThrowAsync();
    }

    [Test]
    public async Task AnyIssueOccursWhileDeletingTask_DoesNotThrow()
    {
        var backend = BackendBuilder.Backend()
            .With(BackendTask(named: "ToBeDeleted"))
            .AlwaysThrowingAnyException()
            .Launch();
        var sut = new BackendTaskRepository(backend);

        var sutInvocation = async () =>
            await sut.Delete(DesktopTask(named: "ToBeDeleted"));

        await sutInvocation.Should().NotThrowAsync();
    }
}