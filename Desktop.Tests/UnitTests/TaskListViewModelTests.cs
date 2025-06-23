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

        sut.Tasks.Should().BeEquivalentTo(await taskRepository.All());
    }

    private static TaskListViewModel TaskListViewModel(InMemoryTaskRepository taskRepository)
    {
        return new TaskListViewModel(
            taskRepository,
            new TaskCreationViewModel(taskRepository));
    }
}