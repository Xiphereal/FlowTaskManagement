using System.Collections.Generic;
using System.Threading.Tasks;
using Desktop.Tasks;
using Task = Desktop.Domain.Task;

namespace Desktop.Tests.TestAPI;

using SystemTask = System.Threading.Tasks.Task;

public class InMemoryTaskRepository : ITaskRepository
{
    private readonly List<Task> tasks = [];

    public InMemoryTaskRepository(IEnumerable<Task> with)
    {
        tasks.AddRange(with);
    }

    public Task<Result> All()
    {
        return SystemTask.FromResult(
            Result.Of(tasks.ToArray()));
    }

    public Task<Result> Save(Task task)
    {
        tasks.Add(task);

        return SystemTask.FromResult(Result.Success());
    }

    public Task<Result> Delete(Task toBeDeleted)
    {
        tasks.Remove(toBeDeleted);

        return SystemTask.FromResult(Result.Success());
    }
}