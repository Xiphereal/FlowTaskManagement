using System;
using Backend.Domain;

namespace Backend.Tests.TestAPI;

public static class TaskFactory
{
    public static Task AnyBackendTask()
    {
        return BackendTask(named: "Any");
    }

    public static Task BackendTask(string named, string description = "Any")
    {
        return new Task(named, description);
    }

    public static Task BackendTask(Guid id, string named, string description = "Any")
    {
        return new Task(id, named, description);
    }
}