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

    public SystemTask Delete(Task toBeDeleted)
    {
        var existingTasks = File.ReadAllLines(PersistedTasksFileName);
        var remainingTasks = existingTasks.Where(x => !x.StartsWith(toBeDeleted.Name));
 
        File.WriteAllLines(PersistedTasksFileName, remainingTasks);
        
        return SystemTask.CompletedTask;
    }
}