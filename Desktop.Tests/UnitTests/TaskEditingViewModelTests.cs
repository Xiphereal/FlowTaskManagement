using Desktop.Common;
using Desktop.Domain;
using Desktop.Tasks;
using Desktop.Tests.TestAPI;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using static Desktop.Tests.TestAPI.TaskFactory;

namespace Desktop.Tests.UnitTests;

public class TaskEditingViewModelTests
{
    [Test]
    public void TasksModificationsAreCommandedToBePersisted()
    {
        var aRepositoryMock = RepositoryMockThatAlwaysSucceeds();
        var anotherRepositoryMock = RepositoryMockThatAlwaysSucceeds();
        var sut = TaskEditingViewModel(
            taskBeingEdited: AnyDesktopTask(),
            repositories: [aRepositoryMock.Object, anotherRepositoryMock.Object]);

        sut.Save.Execute(("New name", "New description"));

        aRepositoryMock.Verify(m => m.Save(It.IsAny<Task>()));
        anotherRepositoryMock.Verify(m => m.Save(It.IsAny<Task>()));
    }

    private static Mock<ITaskRepository> RepositoryMockThatAlwaysSucceeds()
    {
        var mock = new Mock<ITaskRepository>();
        mock
            .Setup(x => x.Save(It.IsAny<Task>()))
            .ReturnsAsync(ResultWithoutValue.Success());

        return mock;
    }

    [Test]
    public void TaskEditing_Fails_MessageNotifiesUserAndTaskRemainsUnmodified()
    {
        var existingTask = DesktopTask("Old name", "Old description");
        var repository = new InMemoryTaskRepository([existingTask]);
        repository.FailAlways();
        var messageNotifierMock = new Mock<IMessageNotifier>();
        var sut = TaskEditingViewModel(
            taskBeingEdited: existingTask,
            messageNotifierMock.Object,
            repositories: repository);

        sut.Save.Execute(("New name", "New description"));

        messageNotifierMock.Verify(x => x.Notify(
            "Task editing has failed due to " +
            "an internal error. The modifications will be reverted. " +
            "Please, try again later."));
        existingTask.Name.Should().Be("Old name");
        existingTask.Description.Should().Be("Old description");
    }

    private static TaskEditingViewModel TaskEditingViewModel(
        Task taskBeingEdited,
        IMessageNotifier messageNotifier = null,
        params ITaskRepository[] repositories)
    {
        return new TaskEditingViewModel(
            taskBeingEdited,
            messageNotifier ?? new Mock<IMessageNotifier>().Object,
            repositories);
    }
}