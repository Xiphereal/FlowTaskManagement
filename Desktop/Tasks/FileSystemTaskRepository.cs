using System.IO;
using Task = Desktop.Domain.Task;
using SystemTask = System.Threading.Tasks.Task;

namespace Desktop.Tasks;

public class FileSystemTaskRepository : ITaskRepository
{
    private const string PersistedTasksFileName = "Tasks.txt";
    public const string LineElementSeparator = "|";

    public Task<ResultWithValue> All()
    {
        if (!File.Exists(PersistedTasksFileName))
            return SystemTask.FromResult(ResultWithValue.Empty());

        var tasks = File
            .ReadAllLines(PersistedTasksFileName)
            .Select(ToTask)
            .ToList();

        return SystemTask.FromResult(ResultWithValue.Of(tasks));
    }

    private static Task ToTask(string line)
    {
        var lineElements = line.Split(LineElementSeparator);

        return new Task(
            id: Guid.Parse(lineElements.ElementAt(0)),
            name: lineElements.ElementAt(1),
            description: lineElements.Length > 1
                ? lineElements.Last()
                : string.Empty);
    }

    public async Task<ResultWithoutValue> Save(Task task)
    {
        var existingTasks = await All();
        if (existingTasks.Tasks.Contains(task))
            await Delete(task);

        await File.AppendAllTextAsync(
            PersistedTasksFileName,
            $"{task.Id}{LineElementSeparator}{task.Name}{LineElementSeparator}{task.Description}" +
            $"{Environment.NewLine}");

        return ResultWithoutValue.Success();
    }

    public Task<ResultWithoutValue> Delete(Task toBeDeleted)
    {
        var existingTasks = File.ReadAllLines(PersistedTasksFileName);
        var remainingTasks = existingTasks.Where(x => !Is(x, toBeDeleted));

        File.WriteAllLines(PersistedTasksFileName, remainingTasks);

        return SystemTask.FromResult(ResultWithoutValue.Success());
    }

    private static bool Is(string candidateAsLine, Task toBeDeleted)
    {
        return ToTask(candidateAsLine).Id == toBeDeleted.Id;
    }
}