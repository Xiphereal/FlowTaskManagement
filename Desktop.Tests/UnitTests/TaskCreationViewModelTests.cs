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
    [Test]
    public void TaskCreation_Fails_MessageNotifiesUserAndTasksIsNotAddedToTheList()
    {
        var repository = new InMemoryTaskRepository([AnyDesktopTask()]);
        repository.FailAlways();
        var messageNotifierMock = new Mock<IMessageNotifier>();
        var sut = new TaskCreationViewModel(
            messageNotifierMock.Object,
            [repository]);
        var closeable = new Mock<ICloseable>();

        sut.SaveTask.Execute((closeable.Object, "Any name", "Any description"));

        messageNotifierMock.Verify(x => x.Notify(
            "Task creation has failed due to " +
            "an internal error. The Task won't be created. " +
            "Please, try again later."));
        sut.CreatedTask.Should().BeNull();
    }
}