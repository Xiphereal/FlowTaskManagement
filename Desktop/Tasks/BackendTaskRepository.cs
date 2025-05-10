using System.Net.Http;
using SystemTask = System.Threading.Tasks.Task;
using Task = Desktop.Domain.Task;

namespace Desktop.Tasks;

public class BackendTaskRepository : ITaskRepository
{
    private readonly HttpClient httpClient;

    public BackendTaskRepository(HttpClient httpClient)
    {
        this.httpClient = httpClient;
    }

    public async Task<IReadOnlyList<Task>> All()
    {
        var getResult = await httpClient.GetAsync("/project/tasks/");

        var backendTasks =
            await getResult.Content.ReadAsAsync<IEnumerable<Backend.Domain.Task>>();

        return backendTasks.Select(ToDesktopTask).ToArray();
    }

    private static Task ToDesktopTask(Backend.Domain.Task task)
    {
        return new Task(task.Name, task.Description);
    }

    public SystemTask Save(Task task)
    {
        throw new NotImplementedException();
    }

    public SystemTask Delete(Task toBeDeleted)
    {
        throw new NotImplementedException();
    }
}