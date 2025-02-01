using System.IO;
using Task = Desktop.Domain.Task;

namespace Desktop;

public class FileSystemTaskRepository : ITaskRepository
{
    private const string PersistedTasksFileName = "Tasks.txt";

    public IReadOnlyList<Task> All()
    {
        if (!File.Exists(PersistedTasksFileName))
            return [];

        return File
            .ReadAllLines(PersistedTasksFileName)
            .Select(line => new Task(name: line, description: string.Empty))
            .ToList();
    }
}