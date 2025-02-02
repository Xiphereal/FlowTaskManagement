using SystemTask = System.Threading.Tasks.Task;

namespace Desktop.Tasks;

using Task = Domain.Task;

public class FakeTasksRepository : ITaskRepository
{
    public IReadOnlyList<Task> All()
    {
        return
        [
            new Task("A task", "Dummy description"),
            new Task("Another task", "Dummy description"),
            new Task("Yet another task", "Dummy description"),
        ];
    }

    public SystemTask Save(Task task)
    {
        return SystemTask.CompletedTask;
    }

    public SystemTask Delete(Task task)
    {
        return SystemTask.CompletedTask;
    }
}