using Backend.Domain;

namespace Backend.Tests.TestAPI;

public static class TaskFactory
{
    public static Task AnyBackendTask()
    {
        return BackendTask(named: "Any");
    }

    public static Task BackendTask(string named)
    {
        return new Task(named, "Any");
    }
}