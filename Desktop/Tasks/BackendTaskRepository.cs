using System.Net.Http;
using SystemTask = System.Threading.Tasks.Task;
using Task = Desktop.Domain.Task;
using BackendTask = Backend.Domain.Task;

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
            await getResult.Content.ReadAsAsync<IEnumerable<BackendTask>>();

        return backendTasks.Select(ToDesktopTask).ToArray();
    }

    private static Task ToDesktopTask(BackendTask task)
    {
        return new Task(task.Name, task.Description);
    }

    public async SystemTask Save(Task task)
    {
        await httpClient.PostAsJsonAsync("/project/tasks/", ToBackendTask(task));
    }

    private static BackendTask ToBackendTask(Task task)
    {
        return new BackendTask(task.Name, task.Description);
    }

    public SystemTask Delete(Task toBeDeleted)
    {
        throw new NotImplementedException();
    }
}