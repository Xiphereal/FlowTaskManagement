using Task = Desktop.Domain.Task;
using SystemTask = System.Threading.Tasks.Task;

namespace Desktop.Tasks;

public interface ITaskRepository
{
    Task<Result> All();
    Task<bool> Save(Task task);
    SystemTask Delete(Task toBeDeleted);
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
        new(false, []);
}