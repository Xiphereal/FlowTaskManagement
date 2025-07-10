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

    public Task<Result> All()
    {
        return SystemTask.FromResult(
            Result.Of(tasks.ToArray()));
    }

    public Task<bool> Save(Domain.Task task)
    {
        tasks.Add(task);

        return SystemTask.FromResult(true);
    }

    public SystemTask Delete(Domain.Task toBeDeleted)
    {
        tasks.Remove(toBeDeleted);

        return SystemTask.CompletedTask;
    }
}