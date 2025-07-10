using Task = Desktop.Domain.Task;

namespace Desktop.Tasks;

public interface ITaskRepository
{
    Task<ResultWithValue> All();
    Task<ResultWithoutValue> Save(Task task);
    Task<ResultWithoutValue> Delete(Task toBeDeleted);
}

public record ResultWithValue
{
    private ResultWithValue(bool succeeded, IReadOnlyList<Task> tasks)
    {
        Succeeded = succeeded;
        Tasks = tasks;
    }

    public bool Succeeded { get; }
    public IReadOnlyList<Task> Tasks { get; }

    public static ResultWithValue Of(IReadOnlyList<Task> tasks) =>
        new(succeeded: true, tasks);

    public static ResultWithValue Failed() =>
        new(succeeded: false, []);
}

public record ResultWithoutValue
{
    private ResultWithoutValue(bool succeeded)
    {
        Succeeded = succeeded;
    }

    public bool Succeeded { get; }

    public static ResultWithoutValue Failed() =>
        new(succeeded: false);

    public static ResultWithoutValue Success() =>
        new(succeeded: true);
}