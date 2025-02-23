using Task = Backend.Domain.Task;
using SystemTask = System.Threading.Tasks.Task;

namespace Backend;

internal class InMemoryTaskRepository : ITaskRepository
{
    private readonly List<Task> tasks =
    [
        new("A task", "Dummy description"),
        new("Another task", "Dummy description"),
        new("Yet another task", "Dummy description"),
    ];

    public Task<IEnumerable<Task>> All()
    {
        return SystemTask.FromResult<IEnumerable<Task>>(tasks);
    }

    public SystemTask Save(Task toBeCreated)
    {
        tasks.Add(toBeCreated);

        return SystemTask.CompletedTask;
    }
}