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

    public Task<ResultWithValue> All()
    {
        return SystemTask.FromResult(
            ResultWithValue.Of(tasks.ToArray()));
    }

    public Task<ResultWithoutValue> Save(Task task)
    {
        tasks.Add(task);

        return SystemTask.FromResult(ResultWithoutValue.Success());
    }

    public Task<ResultWithoutValue> Delete(Task toBeDeleted)
    {
        tasks.Remove(toBeDeleted);

        return SystemTask.FromResult(ResultWithoutValue.Success());
    }
}