using Task = Desktop.Domain.Task;
using SystemTask = System.Threading.Tasks.Task;

namespace Desktop.Tasks;

public interface ITaskRepository
{
    IReadOnlyList<Task> All();
    SystemTask Save(Task task);
    SystemTask Delete(Task task);
}