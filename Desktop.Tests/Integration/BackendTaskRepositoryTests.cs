using System.Threading.Tasks;
using Backend.Tests.TestAPI;
using Desktop.Tasks;
using FluentAssertions;
using NUnit.Framework;

namespace Desktop.Tests.Integration;

public class BackendTaskRepositoryTests
{
    [TearDown]
    public void TearDown()
    {
        BackendBuilder.CleanUp();
    }

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

    private static Backend.Domain.Task AnyBackendTask()
    {
        return new Backend.Domain.Task("Any", "Any");
    }

    private static Backend.Domain.Task BackendTask(string named)
    {
        return new Backend.Domain.Task(named, "Any");
    }

    private static Domain.Task AnyDesktopTask()
    {
        return new Domain.Task("Any", "Any");
    }

    private static Domain.Task DesktopTask(string named)
    {
        return new Domain.Task(named, "Any");
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

        await sut.Save(AnyDesktopTask());

        var existingTasks = await sut.All();
        existingTasks.Should().BeEquivalentTo(
        [
            AnyBackendTask(),
        ]);
    }

    [Test]
    public async Task PersistNewTaskDoesNotOverrideExistingOnes()
    {
        var aTask = BackendTask(named: "existing");
        var backend = BackendBuilder.Backend()
            .With(aTask)
            .Launch();
        var sut = new BackendTaskRepository(backend);

        var anotherTask = DesktopTask(named: "Another task");
        await sut.Save(anotherTask);

        var existingTasks = await sut.All();
        existingTasks.Should().BeEquivalentTo(
        [
            DesktopTask(named: aTask.Name),
            anotherTask
        ]);
    }
}