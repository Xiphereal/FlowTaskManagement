using System.Threading.Tasks;
using Desktop.Common;
using Desktop.Tasks;
using Desktop.Tests.TestAPI;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using static Desktop.Tests.TestAPI.TaskFactory;

namespace Desktop.Tests.UnitTests;

public class TaskEditingViewModelTests
{
    private const string TaskEditingFailedMessage =
        "Task editing has failed due to " +
        "an internal error. The modifications will be reverted. " +
        "Please, try again later.";

    [Test]
    public void TasksModificationsAreCommandedToBePersisted()
    {
        var aRepositoryMock = RepositoryMockThatAlwaysSucceeds();
        var anotherRepositoryMock = RepositoryMockThatAlwaysSucceeds();
        var sut = TaskEditingViewModel(
            taskBeingEdited: AnyDesktopTask(),
            repositories: [aRepositoryMock.Object, anotherRepositoryMock.Object]);

        sut.Save.Execute(("New name", "New description"));

        aRepositoryMock.Verify(m => m.Save(It.IsAny<Domain.Task>()));
        anotherRepositoryMock.Verify(m => m.Save(It.IsAny<Domain.Task>()));
    }

    private static Mock<ITaskRepository> RepositoryMockThatAlwaysSucceeds()
    {
        var mock = new Mock<ITaskRepository>();
        mock
            .Setup(x => x.Save(It.IsAny<Domain.Task>()))
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
            TaskEditingFailedMessage));
        existingTask.Name.Should().Be("Old name");
        existingTask.Description.Should().Be("Old description");
    }

    [Test]
    public async Task
        TaskEditing_FailsForOne_NotifiesOnceAndAllowsEditingItToTheRestOfRepositories()
    {
        // Arrange
        var existingTask = DesktopTask("Old name", "Old description");
        var aRepository = new InMemoryTaskRepository([existingTask]);
        aRepository.FailAlways();

        var anotherRepository = new InMemoryTaskRepository([existingTask]);

        var messageNotifierMock = new Mock<IMessageNotifier>();
        var sut = TaskEditingViewModel(
            taskBeingEdited: existingTask,
            messageNotifierMock.Object,
            repositories: [aRepository, anotherRepository]);

        // Act.
        var newName = "New name";
        var newDescription = "New description";
        sut.Save.Execute((newName, newDescription));

        // Assert.
        messageNotifierMock.Verify(
            x => x.Notify(TaskEditingFailedMessage),
            Times.Once);
        existingTask.Name.Should().Be(newName);
        existingTask.Description.Should().Be(newDescription);

        var result = await anotherRepository.All();
        result.Tasks.Should().Contain(
            DesktopTask(id: existingTask.Id, newName, newDescription));
    }

    private static TaskEditingViewModel TaskEditingViewModel(
        Domain.Task taskBeingEdited,
        IMessageNotifier messageNotifier = null,
        params ITaskRepository[] repositories)
    {
        return new TaskEditingViewModel(
            taskBeingEdited,
            messageNotifier ?? new Mock<IMessageNotifier>().Object,
            repositories);
    }
}