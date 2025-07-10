using System;
using System.IO;
using Desktop.Tasks;
using FluentAssertions;
using NUnit.Framework;
using static Desktop.Tests.TestAPI.TaskFactory;
using Task = Desktop.Domain.Task;
using SystemTask = System.Threading.Tasks.Task;

namespace Desktop.Tests.Integration;

public class FileSystemTaskRepositoryTest : ITaskRepositoryTests
{
    private const string PersistedTasksFileName = "Tasks.txt";

    [Test]
    public async SystemTask NoTaskArePersisted_ReturnsNothing()
    {
        var sut = new FileSystemTaskRepository();

        var result = await sut.All();

        result.Tasks.Should().BeEmpty();
        result.Succeeded.Should().BeTrue();
    }

    [Test]
    public async SystemTask CanLoadPersistedTasks()
    {
        PersistTask("aTaskName", "aTaskDescription");

        var sut = new FileSystemTaskRepository();

        var result = await sut.All();

        result.Tasks.Should().BeEquivalentTo(
            [
                new Task(
                    name: "aTaskName",
                    description: "aTaskDescription")
            ],
            options => options.ComparingByMembers<Task>().Excluding(x => x.Id));
    }

    [Test]
    public async SystemTask CanLoadPersistedTasksWithoutDescription()
    {
        PersistTask("aTaskName");

        var sut = new FileSystemTaskRepository();

        var result = await sut.All();

        result.Tasks.Should().BeEquivalentTo(
            [
                new Task(
                    name: "aTaskName",
                    description: string.Empty)
            ],
            options => options.ComparingByMembers<Task>().Excluding(x => x.Id));
    }

    [Test]
    public async SystemTask CanLoadSeveralPersistedTasks()
    {
        PersistTask("aTaskName");
        PersistTask("anotherTaskName");

        var sut = new FileSystemTaskRepository();

        var result = await sut.All();

        result.Tasks.Should().BeEquivalentTo(
            [
                new Task(
                    name: "aTaskName",
                    description: string.Empty),
                new Task(
                    name: "anotherTaskName",
                    description: string.Empty)
            ],
            options => options.ComparingByMembers<Task>().Excluding(x => x.Id));
    }

    [Test]
    public async SystemTask CanPersistTasksWhenNoOtherExistedBefore()
    {
        var sut = new FileSystemTaskRepository();

        var taskToPersist = DesktopTask(named: "anyName", description: "anyDesc");
        await sut.Save(taskToPersist);

        var result = await sut.All();
        result.Tasks.Should().ContainSingle().And.Contain(taskToPersist);
    }

    [Test]
    public async SystemTask PersistNewTaskDoesNotOverrideExistingOnes()
    {
        var doc = new FileSystemTaskRepository();
        var existingTask = DesktopTask(named: "existing");
        await doc.Save(existingTask);

        var sut = new FileSystemTaskRepository();

        var taskToPersist = DesktopTask();
        await sut.Save(taskToPersist);

        var result = await sut.All();
        result.Tasks.Should().BeEquivalentTo(
        [
            existingTask,
            taskToPersist,
        ]);
    }

    [Test]
    public async SystemTask TasksCanBeModified()
    {
        var doc = new FileSystemTaskRepository();
        var existingTask = DesktopTask(
            id: Guid.NewGuid(),
            named: "Old name",
            description: "Old description");
        await doc.Save(existingTask);

        var sut = new FileSystemTaskRepository();

        var modifiedTask = DesktopTask(
            existingTask.Id,
            named: "New name",
            description: "New description");
        await sut.Save(modifiedTask);

        var result = await sut.All();
        result.Tasks.Should().BeEquivalentTo([modifiedTask]);
    }

    [Test]
    public async SystemTask CanDeleteExistingTasks()
    {
        var doc = new FileSystemTaskRepository();
        var task = DesktopTask();
        await doc.Save(task);
        var sut = new FileSystemTaskRepository();

        await sut.Delete(task);

        var result = await sut.All();
        result.Tasks.Should().BeEmpty();
    }

    [Test]
    public async SystemTask DeletesJustTheRequestedTask_KeepingTheOtherOnes()
    {
        // Arrange.
        var doc = new FileSystemTaskRepository();

        var oneThatMustRemain = DesktopTask();
        await doc.Save(oneThatMustRemain);

        var toBeDeleted = DesktopTask("toBeDeleted");
        await doc.Save(toBeDeleted);

        var anotherThatMustRemain = DesktopTask("toBeDeletedStartingWithSameName");
        await doc.Save(anotherThatMustRemain);

        var sut = new FileSystemTaskRepository();

        // Act.
        await sut.Delete(toBeDeleted);

        // Assert.
        var result = await sut.All();
        result.Tasks.Should().BeEquivalentTo(
        [
            oneThatMustRemain,
            anotherThatMustRemain,
        ]);
    }

    private static void PersistTask(string name, string description = null)
    {
        var id = Guid.NewGuid();

        File.AppendAllText(
            PersistedTasksFileName,
            $"{id} {name} {description ?? string.Empty}{Environment.NewLine}");
    }

    [TearDown]
    public void TearDown()
    {
        if (File.Exists(PersistedTasksFileName))
            File.Delete(PersistedTasksFileName);
    }
}