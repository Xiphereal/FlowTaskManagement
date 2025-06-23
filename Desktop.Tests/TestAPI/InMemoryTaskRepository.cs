using System.Collections.Generic;
using System.Threading.Tasks;
using Desktop.Tasks;

namespace Desktop.Tests.TestAPI;

using SystemTask = Task;

public class InMemoryTaskRepository : ITaskRepository
{
    private readonly List<Domain.Task> tasks = [];

    public InMemoryTaskRepository(IEnumerable<Domain.Task> with)
    {
        tasks.AddRange(with);
    }

    public Task<IReadOnlyList<Domain.Task>> All()
    {
        return SystemTask.FromResult<IReadOnlyList<Domain.Task>>(tasks.ToArray());
    }

    public SystemTask Save(Domain.Task task)
    {
        tasks.Add(task);

        return SystemTask.CompletedTask;
    }

    public SystemTask Delete(Domain.Task toBeDeleted)
    {
        tasks.Remove(toBeDeleted);

        return SystemTask.CompletedTask;
    }
}