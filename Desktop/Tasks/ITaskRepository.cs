using Task = Desktop.Domain.Task;

namespace Desktop.Tasks;

public interface ITaskRepository
{
    Task<Result> All();
    Task<Result> Save(Task task);
    Task<Result> Delete(Task toBeDeleted);
}

public record Result
{
    private Result(bool succeeded, IReadOnlyList<Task> tasks)
    {
        Succeeded = succeeded;
        Tasks = tasks;
    }

    public bool Succeeded { get; }
    public IReadOnlyList<Task> Tasks { get; }

    public static Result Of(IReadOnlyList<Task> tasks) =>
        new(succeeded: true, tasks);

    public static Result Failed() =>
        new(succeeded: false, []);

    public static Result Success() =>
        new(succeeded: true, []);
}