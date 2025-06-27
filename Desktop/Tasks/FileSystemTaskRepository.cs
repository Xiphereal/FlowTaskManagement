using System.IO;
using Task = Desktop.Domain.Task;
using SystemTask = System.Threading.Tasks.Task;

namespace Desktop.Tasks;

public class FileSystemTaskRepository : ITaskRepository
{
    private const string PersistedTasksFileName = "Tasks.txt";

    public Task<IReadOnlyList<Task>> All()
    {
        if (!File.Exists(PersistedTasksFileName))
            return SystemTask.FromResult<IReadOnlyList<Task>>([]);

        return SystemTask.FromResult<IReadOnlyList<Task>>(
            File
                .ReadAllLines(PersistedTasksFileName)
                .Select(ToTask)
                .ToList());
    }

    private static Task ToTask(string line)
    {
        return new Task(
            id: Guid.Parse(SplitBySpaces(line).ElementAt(0)),
            name: SplitBySpaces(line).ElementAt(1),
            description: SplitBySpaces(line).Length > 1
                ? SplitBySpaces(line).Last()
                : string.Empty);
    }

    private static string[] SplitBySpaces(string line)
    {
        return line.Split(' ');
    }

    public async Task<bool> Save(Task task)
    {
        var existingTasks = await All();
        if (existingTasks.Contains(task))
            await Delete(task);

        await File.AppendAllTextAsync(
            PersistedTasksFileName,
            $"{task.Id} {task.Name} {task.Description}{Environment.NewLine}");

        return true;
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
        return ToTask(candidateAsLine).Id == toBeDeleted.Id;
    }
}