using System.IO;
using Task = Desktop.Domain.Task;

namespace Desktop.Tasks;

using SystemTask = System.Threading.Tasks.Task;

public class FileSystemTaskRepository : ITaskRepository
{
    private const string PersistedTasksFileName = "Tasks.txt";

    public IReadOnlyList<Task> All()
    {
        if (!File.Exists(PersistedTasksFileName))
            return [];

        return File
            .ReadAllLines(PersistedTasksFileName)
            .Select(ToTask)
            .ToList();
    }

    private static Task ToTask(string line)
    {
        var wordsBySpaces = line.Split(' ');
        
        return new Task(
            name: wordsBySpaces.First(),
            description: wordsBySpaces.Length > 1 
                ? wordsBySpaces.Last()
                : string.Empty);
    }

    public SystemTask Save(Task task)
    {
        File.AppendAllText(
            PersistedTasksFileName,
            $"{task.Name} {task.Description}{Environment.NewLine}");
        
        return SystemTask.CompletedTask;
    }
}