using Task = Desktop.Domain.Task;
using SystemTask = System.Threading.Tasks.Task;

namespace Desktop.Tasks;

public interface ITaskRepository
{
    Task<IReadOnlyList<Task>> All();
    Task<bool> Save(Task task);
    SystemTask Delete(Task toBeDeleted);
}