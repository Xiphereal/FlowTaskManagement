using System;
using System.Threading.Tasks;
using Desktop.Common;
using Desktop.Project;
using Desktop.Tasks;
using Desktop.Tests.TestAPI;
using FluentAssertions;
using Moq;
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

    [Test]
    public void TaskPopulation_Fails_MessageNotifiesUser()
    {
        var backendRepository =
            new InMemoryTaskRepository([DesktopTask()]);
        backendRepository.FailAlways();
        var messageNotifierMock = new Mock<IMessageNotifier>();
        var sut = TaskListViewModel(backendRepository, messageNotifierMock.Object);

        sut.PopulateTasks();

        messageNotifierMock.Verify(x => x.Notify(
            "Task retrieval has failed due to " +
            "an internal error. Expect some Tasks to be missing. " +
            "Please, try again later."));
    }

    [Test]
    public void TaskDeletion_Fails_MessageNotifiesUserAndTaskRemains()
    {
        // Arrange.
        var existingTask = DesktopTask();
        var backendRepository =
            new InMemoryTaskRepository([existingTask]);

        var messageNotifierMock = new Mock<IMessageNotifier>();

        var sut = TaskListViewModel(backendRepository, messageNotifierMock.Object);
        sut.PopulateTasks();
        backendRepository.FailAlways();

        // Act.
        sut.Delete.Execute(existingTask);

        // Assert.
        messageNotifierMock.Verify(x => x.Notify(
            "Task deletion has failed due to " +
            "an internal error. The Task won't be deleted. " +
            "Please, try again later."));
        sut.Tasks.Should().Contain(existingTask);
    }

    [Test]
    public async Task
        TaskDeletion_FailsForOne_NotifiesOnceAndAllowsDeletingItToTheRestOfRepositories()
    {
        // Arrange.
        var existingTask = DesktopTask();
        var aRepository =
            new InMemoryTaskRepository([existingTask]);
        var anotherRepository =
            new InMemoryTaskRepository([existingTask]);

        var messageNotifierMock = new Mock<IMessageNotifier>();

        var sut = TaskListViewModel(
            [aRepository, anotherRepository],
            messageNotifierMock.Object);
        sut.PopulateTasks();
        aRepository.FailAlways();

        // Act.
        sut.Delete.Execute(existingTask);

        // Assert.
        messageNotifierMock.Verify(
            x => x.Notify(
                "Task deletion has failed due to " +
                "an internal error. The Task won't be deleted. " +
                "Please, try again later."),
            Times.Once);
        sut.Tasks.Should().BeEmpty();

        var result = await anotherRepository.All();
        result.Tasks.Should().BeEmpty();
    }

    private static TaskListViewModel TaskListViewModel(
        params InMemoryTaskRepository[] taskRepositories)
    {
        var messageNotifier = new Mock<IMessageNotifier>().Object;
        return new TaskListViewModel(
            taskRepositories,
            new TaskCreationViewModel(messageNotifier, taskRepositories),
            messageNotifier);
    }

    private static TaskListViewModel TaskListViewModel(
        InMemoryTaskRepository taskRepository,
        IMessageNotifier messageNotifier)
    {
        return new TaskListViewModel(
            [taskRepository],
            new TaskCreationViewModel(messageNotifier, [taskRepository]),
            messageNotifier);
    }

    private static TaskListViewModel TaskListViewModel(
        InMemoryTaskRepository[] taskRepositories,
        IMessageNotifier messageNotifier)
    {
        return new TaskListViewModel(
            taskRepositories,
            new TaskCreationViewModel(messageNotifier, taskRepositories),
            messageNotifier);
    }
}