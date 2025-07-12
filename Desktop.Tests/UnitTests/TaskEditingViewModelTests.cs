using Desktop.Domain;
using Desktop.Tasks;
using Moq;
using NUnit.Framework;
using static Desktop.Tests.TestAPI.TaskFactory;

namespace Desktop.Tests.UnitTests;

public class TaskEditingViewModelTests
{
    [Test]
    public void TasksModificationsAreCommandedToBePersisted()
    {
        var aRepositoryMock = new Mock<ITaskRepository>();
        var anotherRepositoryMock = new Mock<ITaskRepository>();

        var sut = new TaskEditingViewModel(
            AnyDesktopTask(),
            [aRepositoryMock.Object, anotherRepositoryMock.Object]);
        sut.Save.Execute(("New name", "New description"));

        aRepositoryMock.Verify(m => m.Save(It.IsAny<Task>()));
        anotherRepositoryMock.Verify(m => m.Save(It.IsAny<Task>()));
    }
}