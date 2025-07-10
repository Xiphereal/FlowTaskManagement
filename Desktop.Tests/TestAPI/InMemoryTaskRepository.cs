using System.Collections.Generic;
using System.Threading.Tasks;
using Desktop.Tasks;
using Task = Desktop.Domain.Task;

namespace Desktop.Tests.TestAPI;

using SystemTask = System.Threading.Tasks.Task;

public class InMemoryTaskRepository : ITaskRepository
{
    private readonly List<Task> tasks = [];
    private bool failAlways;

    public InMemoryTaskRepository(IEnumerable<Task> with)
    {
        tasks.AddRange(with);
    }

    public Task<ResultWithValue> All()
    {
        if (failAlways)
            return SystemTask.FromResult(
                ResultWithValue.Failed());

        return SystemTask.FromResult(
            ResultWithValue.Of(tasks.ToArray()));
    }

    public Task<ResultWithoutValue> Save(Task task)
    {
        if (failAlways)
            return SystemTask.FromResult(
                ResultWithoutValue.Failed());

        tasks.Add(task);

        return SystemTask.FromResult(ResultWithoutValue.Success());
    }

    public Task<ResultWithoutValue> Delete(Task toBeDeleted)
    {
        if (failAlways)
            return SystemTask.FromResult(
                ResultWithoutValue.Failed());

        tasks.Remove(toBeDeleted);

        return SystemTask.FromResult(ResultWithoutValue.Success());
    }

    public void FailAlways()
    {
        failAlways = true;
    }
}