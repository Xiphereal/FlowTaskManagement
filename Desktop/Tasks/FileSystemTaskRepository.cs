using System.IO;
using Task = Desktop.Domain.Task;
using SystemTask = System.Threading.Tasks.Task;

namespace Desktop.Tasks;

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
        return new Task(
            name: SplitBySpaces(line).First(),
            description: SplitBySpaces(line).Length > 1 
                ? SplitBySpaces(line).Last()
                : string.Empty);
    }

    private static string[] SplitBySpaces(string line)
    {
        return line.Split(' ');
    }

    public SystemTask Save(Task task)
    {
        File.AppendAllText(
            PersistedTasksFileName,
            $"{task.Name} {task.Description}{Environment.NewLine}");
        
        return SystemTask.CompletedTask;
    }

    public SystemTask Delete(Task toBeDeleted)
    {
        var existingTasks = File.ReadAllLines(PersistedTasksFileName);
        var remainingTasks = existingTasks.Where(x => !Is(x, toBeDeleted));
 
        File.WriteAllLines(PersistedTasksFileName, remainingTasks);
        
        return SystemTask.CompletedTask;
    }

    private static bool Is(string candidateAsLine, Task toBeDeleted)
    {
        return SplitBySpaces(candidateAsLine).First() == toBeDeleted.Name;
    }
}