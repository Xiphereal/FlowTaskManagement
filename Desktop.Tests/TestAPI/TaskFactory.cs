using System;
using Desktop.Domain;

namespace Desktop.Tests.TestAPI;

public static class TaskFactory
{
    public static Task AnyDesktopTask()
    {
        return DesktopTask(id: null, "Any", "Any");
    }

    public static Task DesktopTask(string named)
    {
        return DesktopTask(id: null, named, description: null);
    }

    public static Task DesktopTask(
        string named,
        string description)
    {
        return DesktopTask(
            id: Guid.NewGuid(),
            named,
            description: description);
    }

    public static Task DesktopTask(
        Guid? id = null,
        string named = null,
        string description = null)
    {
        return new Task(
            id: id ?? Guid.NewGuid(),
            named ?? "Any",
            description: description ?? "Any");
    }
}