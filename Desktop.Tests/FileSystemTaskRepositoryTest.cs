using System.IO;
using FluentAssertions;
using NUnit.Framework;
using Task = Desktop.Domain.Task;

namespace Desktop.Tests;

[TestFixture]
[TestOf(typeof(FileSystemTaskRepository))]
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
        File.WriteAllText(PersistedTasksFileName, "aTask");
        
        var sut = new FileSystemTaskRepository();

        sut.All()
            .Should().BeEquivalentTo([new Task("aTask", description: string.Empty)]);
    }

    [TearDown]
    public void TearDown()
    {
        if (File.Exists(PersistedTasksFileName))
            File.Delete(PersistedTasksFileName);
    }
}