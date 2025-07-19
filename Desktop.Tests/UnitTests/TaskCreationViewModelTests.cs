using System.Threading.Tasks;
using Desktop.Common;
using Desktop.Tasks;
using Desktop.Tests.TestAPI;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using static Desktop.Tests.TestAPI.TaskFactory;

namespace Desktop.Tests.UnitTests;

public class TaskCreationViewModelTests
{
    private const string TaskCreationFailedMessage =
        "Task creation has failed due to " +
        "an internal error. The Task won't be created. " +
        "Please, try again later.";

    [Test]
    public void TaskCreation_Fails_MessageNotifiesUserAndTasksIsNotAddedToTheList()
    {
        var repository = new InMemoryTaskRepository([]);
        repository.FailAlways();
        var messageNotifierMock = new Mock<IMessageNotifier>();
        var sut = new TaskCreationViewModel(
            messageNotifierMock.Object,
            [repository]);
        var closeable = new Mock<ICloseable>();

        sut.SaveTask.Execute((closeable.Object, "Any name", "Any description"));

        messageNotifierMock.Verify(x => x.Notify(
            TaskCreationFailedMessage));
        sut.CreatedTask.Should().BeNull();
    }

    [Test]
    public async Task
        TaskCreation_FailsForOne_NotifiesOnceAndAllowsSavingItToTheRestOfRepositories()
    {
        // Arrange.
        var aRepository = new InMemoryTaskRepository([]);
        aRepository.FailAlways();
        var anotherRepository = new InMemoryTaskRepository([]);

        var messageNotifierMock = new Mock<IMessageNotifier>();
        var sut = new TaskCreationViewModel(
            messageNotifierMock.Object,
            [aRepository, anotherRepository]);
        var closeable = new Mock<ICloseable>();

        // Act.
        var taskName = "Any name";
        var taskDescription = "Any description";
        sut.SaveTask.Execute((closeable.Object, taskName, taskDescription));

        // Assert.
        messageNotifierMock.Verify(
            x => x.Notify(TaskCreationFailedMessage),
            Times.Once);
        var createdTask = DesktopTask(taskName, taskDescription);
        sut.CreatedTask.Should().BeEquivalentTo(
            createdTask,
            options => options
                .ComparingByMembers<Domain.Task>()
                .Excluding(x => x.Id));

        var result = await anotherRepository.All();
        result.Tasks.Should().ContainEquivalentOf(
            createdTask,
            options => options
                .ComparingByMembers<Domain.Task>()
                .Excluding(x => x.Id));
    }
}