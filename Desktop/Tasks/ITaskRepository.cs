using Task = Desktop.Domain.Task;

namespace Desktop.Tasks;

public interface ITaskRepository
{
    IReadOnlyList<Task> All();
}