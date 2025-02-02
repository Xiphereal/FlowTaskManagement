using System;
using System.IO;
using Desktop.Tasks;
using FluentAssertions;
using NUnit.Framework;
using Task = Desktop.Domain.Task;
using SystemTask = System.Threading.Tasks.Task;

namespace Desktop.Tests.Integration;

public class FileSystemTaskRepositoryTest
{
    private const string PersistedTasksFileName = "Tasks.txt";

    [Test]
    public void NoTaskArePersisted_ReturnsNothing()
    {
        var sut = new FileSystemTaskRepository();

        sut.All().Should().BeEmpty();
    }

    [Test]
    public void CanLoadPersistedTasks()
    {
        PersistTask("aTaskName", "aTaskDescription");

        var sut = new FileSystemTaskRepository();

        sut.All()
            .Should().BeEquivalentTo(
            [
                new Task(
                    name: "aTaskName",
                    description: "aTaskDescription")
            ]);
    }

    [Test]
    public void CanLoadPersistedTasksWithoutDescription()
    {
        PersistTask("aTaskName");

        var sut = new FileSystemTaskRepository();

        sut.All()
            .Should().BeEquivalentTo(
            [
                new Task(
                    name: "aTaskName",
                    description: string.Empty)
            ]);
    }

    [Test]
    public void CanLoadSeveralPersistedTasks()
    {
        PersistTask("aTaskName");
        PersistTask("anotherTaskName");

        var sut = new FileSystemTaskRepository();

        sut.All()
            .Should().BeEquivalentTo(
            [
                new Task(
                    name: "aTaskName",
                    description: string.Empty),
                new Task(
                    name: "anotherTaskName",
                    description: string.Empty)
            ]);
    }

    [Test]
    public async SystemTask CanPersistTasksWhenNoOtherExistedBefore()
    {
        var sut = new FileSystemTaskRepository();

        var taskToPersist = new Task(name: "anyName", description: "description");
        await sut.Save(taskToPersist);

        sut.All().Should().ContainSingle().And.Contain(taskToPersist);
    }

    [Test]
    public async SystemTask PersistNewTaskDoesNotOverrideExistingOnes()
    {
        PersistTask("existingTask", "existingTaskDesc");

        var sut = new FileSystemTaskRepository();

        var taskToPersist = new Task(name: "anyName", description: "description");
        await sut.Save(taskToPersist);

        sut.All().Should().BeEquivalentTo(
        [
            new Task(name: "existingTask", description: "existingTaskDesc"),
            taskToPersist,
        ]);
    }

    [Test]
    public async SystemTask CanDeleteExistingTasks()
    {
        PersistTask("anyName", string.Empty);
        var sut = new FileSystemTaskRepository();

        await sut.Delete(new Task("anyName", string.Empty));

        sut.All().Should().BeEmpty();
    }

    private static void PersistTask(string name, string description = null)
    {
        File.AppendAllText(
            PersistedTasksFileName,
            $"{name} {description ?? string.Empty}{Environment.NewLine}");
    }

    [TearDown]
    public void TearDown()
    {
        if (File.Exists(PersistedTasksFileName))
            File.Delete(PersistedTasksFileName);
    }
}