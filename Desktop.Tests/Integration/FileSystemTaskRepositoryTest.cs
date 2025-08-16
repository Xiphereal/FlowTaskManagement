using System;
using System.IO;
using Desktop.Tasks;
using FluentAssertions;
using NUnit.Framework;
using static Desktop.Tests.TestAPI.TaskFactory;
using static Desktop.Tasks.FileSystemTaskRepository;
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
        var taskId = Guid.NewGuid();
        const string taskName = "aTaskName";
        const string taskDescription = "aTaskDescription";
        PersistTask(taskId, taskName, taskDescription);
        var sut = new FileSystemTaskRepository();

        var result = await sut.All();

        result.Tasks.Should().BeEquivalentTo(
            [
                new Task(
                    id: taskId,
                    name: taskName,
                    description: taskDescription)
            ]);
        result.Succeeded.Should().BeTrue();
    }

    [Test]
    public async SystemTask CanLoadPersistedTasksWithoutDescription()
    {
        const string taskName = "aTaskName";
        PersistTask(taskName);

        var sut = new FileSystemTaskRepository();

        var result = await sut.All();

        result.Tasks.Should().BeEquivalentTo(
            [
                new Task(
                    name: taskName,
                    description: string.Empty)
            ],
            options => options.ComparingByMembers<Task>().Excluding(x => x.Id));
        result.Succeeded.Should().BeTrue();
    }

    [Test]
    public async SystemTask CanLoadSeveralPersistedTasks()
    {
        const string aTaskName = "aTaskName";
        PersistTask(aTaskName);
        const string anotherTaskName = "anotherTaskName";
        PersistTask(anotherTaskName);

        var sut = new FileSystemTaskRepository();

        var result = await sut.All();

        result.Tasks.Should().BeEquivalentTo(
            [
                new Task(
                    name: aTaskName,
                    description: string.Empty),
                new Task(
                    name: anotherTaskName,
                    description: string.Empty)
            ],
            options => options.ComparingByMembers<Task>().Excluding(x => x.Id));
        result.Succeeded.Should().BeTrue();
    }

    [Test]
    [Category("Regression")]
    public async SystemTask CanLoadTasksWhoseNameHaveWhitespaces()
    {
        const string taskName = "Task named with whitespaces";
        PersistTask(taskName);
        var sut = new FileSystemTaskRepository();

        var result = await sut.All();

        result.Tasks.Should().BeEquivalentTo(
            [
                new Task(
                    name: taskName,
                    description: string.Empty)
            ],
            options => options.ComparingByMembers<Task>().Excluding(x => x.Id));
        result.Succeeded.Should().BeTrue();
    }

    [Test]
    public async SystemTask CanPersistTasksWhenNoOtherExistedBefore()
    {
        var sut = new FileSystemTaskRepository();

        var taskToPersist = DesktopTask(named: "anyName", description: "anyDesc");
        await sut.Save(taskToPersist);

        var result = await sut.All();
        result.Tasks.Should().ContainSingle().And.Contain(taskToPersist);
        result.Succeeded.Should().BeTrue();
    }

    [Test]
    public async SystemTask PersistNewTaskDoesNotOverrideExistingOnes()
    {
        // Arrange.
        var doc = new FileSystemTaskRepository();
        var existingTask = DesktopTask(named: "existing");
        await doc.Save(existingTask);

        var sut = new FileSystemTaskRepository();

        var taskToPersist = DesktopTask();

        // Act.
        await sut.Save(taskToPersist);

        // Assert.
        var result = await sut.All();
        result.Tasks.Should().BeEquivalentTo(
        [
            existingTask,
            taskToPersist,
        ]);
        result.Succeeded.Should().BeTrue();
    }

    [Test]
    public async SystemTask TasksCanBeModified()
    {
        // Arrange.
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

        // Act.
        await sut.Save(modifiedTask);

        // Assert.
        var result = await sut.All();
        result.Tasks.Should().BeEquivalentTo([modifiedTask]);
        result.Succeeded.Should().BeTrue();
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
        result.Succeeded.Should().BeTrue();
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
        result.Succeeded.Should().BeTrue();
    }

    private static void PersistTask(string name, string description = null)
    {
        var anyId = Guid.NewGuid();

        PersistTask(anyId, name, description);
    }

    private static void PersistTask(Guid id, string name, string description = null)
    {
        File.AppendAllText(
            PersistedTasksFileName,
            $"{id}{LineElementSeparator}{name}{LineElementSeparator}{description ?? string.Empty}" +
            $"{Environment.NewLine}");
    }

    [TearDown]
    public void TearDown()
    {
        if (File.Exists(PersistedTasksFileName))
            File.Delete(PersistedTasksFileName);
    }
}