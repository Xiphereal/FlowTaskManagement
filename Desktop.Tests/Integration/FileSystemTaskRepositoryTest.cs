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
        File.WriteAllText(PersistedTasksFileName, "aTaskName aTaskDescription");

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
        File.WriteAllText(PersistedTasksFileName, "aTaskName");

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
        File.WriteAllText(
            PersistedTasksFileName,
            "aTaskName" + Environment.NewLine + "anotherTaskName");

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

        var persistedTask = new Task(name: "anyName", description: "description");
        await sut.Save(persistedTask);

        sut.All().Should().ContainSingle().And.Contain(persistedTask);
    }

    [TearDown]
    public void TearDown()
    {
        if (File.Exists(PersistedTasksFileName))
            File.Delete(PersistedTasksFileName);
    }
}