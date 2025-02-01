using Desktop;
using FluentAssertions;
using NUnit.Framework;

namespace Desktop.Tests;

[TestFixture]
[TestOf(typeof(FileSystemTaskRepository))]
public class FileSystemTaskRepositoryTest
{
    [Test]
    public void HasNoTasksByDefault()
    {
        var sut = new FileSystemTaskRepository();

        sut.All().Should().BeEmpty();
    }
}