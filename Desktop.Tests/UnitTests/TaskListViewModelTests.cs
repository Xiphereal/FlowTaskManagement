using System;
using System.Threading.Tasks;
using Desktop.Project;
using Desktop.Tasks;
using Desktop.Tests.TestAPI;
using FluentAssertions;
using NUnit.Framework;
using static Desktop.Tests.TestAPI.TaskFactory;
using static Desktop.Tests.TestAPI.Utils;

namespace Desktop.Tests.UnitTests;

public class TaskListViewModelTests
{
    [Test]
    [Category("Regression")]
    public async Task TasksPopulation_IsIdempotent()
    {
        var taskRepository = new InMemoryTaskRepository([DesktopTask()]);
        var sut = TaskListViewModel(taskRepository);

        Repeat(sut.PopulateTasks, SeveralTimes);

        var result = await taskRepository.All();
        sut.Tasks.Should().BeEquivalentTo(result.Tasks);
    }

    [Test]
    public void
        TasksPopulation_DoesNotShowDuplicatedTasks_BetweenBackendAndFileSystemRepositories()
    {
        var taskId = Guid.NewGuid();
        var backendRepository =
            new InMemoryTaskRepository([DesktopTask(taskId, "A name")]);
        var fileSystemRepository =
            new InMemoryTaskRepository([DesktopTask(taskId, "Another name")]);
        var sut = TaskListViewModel(
            backendRepository,
            fileSystemRepository);

        sut.PopulateTasks();

        sut.Tasks.Should().ContainSingle();
    }

    private static TaskListViewModel TaskListViewModel(
        params InMemoryTaskRepository[] taskRepositories)
    {
        return new TaskListViewModel(
            taskRepositories,
            new TaskCreationViewModel(taskRepositories));
    }
}