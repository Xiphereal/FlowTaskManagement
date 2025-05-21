using Desktop.Domain;

namespace Desktop.Tests.TestAPI;

public static class TaskFactory
{
    public static Task DesktopTask(string named = null, string description = null)
    {
        return new Task(name: named ?? "Any", description: description ?? "Any");
    }
}