using System.IO;
using Task = Desktop.Domain.Task;

namespace Desktop;

public class FileSystemTaskRepository : ITaskRepository
{
    public IReadOnlyList<Task> All()
    {
        return [];
    }
}