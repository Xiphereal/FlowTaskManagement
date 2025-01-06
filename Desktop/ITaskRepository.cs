using Task = Desktop.Domain.Task;

namespace Desktop;

public interface ITaskRepository
{
    IReadOnlyList<Task> All();
}