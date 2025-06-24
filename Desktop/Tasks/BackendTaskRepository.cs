using System.Net.Http;
using SystemTask = System.Threading.Tasks.Task;
using Task = Desktop.Domain.Task;
using BackendTask = Backend.Domain.Task;

namespace Desktop.Tasks;

public class BackendTaskRepository : ITaskRepository, IRemoteTaskRepository
{
    private readonly HttpClient httpClient;

    public BackendTaskRepository(HttpClient httpClient)
    {
        this.httpClient = httpClient;
    }

    public async Task<bool> IsAvailable()
    {
        try
        {
            var result = await httpClient.GetAsync("/health/");

            result.EnsureSuccessStatusCode();

            return true;
        }
        catch
        {
            return false;
        }
    }

    public async Task<IReadOnlyList<Task>> All()
    {
        HttpResponseMessage getResult;

        try
        {
            getResult = await httpClient.GetAsync("/project/tasks/");
        }
        catch
        {
            return [];
        }

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
        try
        {
            await httpClient.PostAsJsonAsync("/project/tasks/", ToBackendTask(task));
        }
        catch
        {
        }
    }

    private static BackendTask ToBackendTask(Task task)
    {
        return new BackendTask(task.Name, task.Description);
    }

    public async SystemTask Delete(Task toBeDeleted)
    {
        HttpResponseMessage httpResponseMessage;

        try
        {
            httpResponseMessage = await httpClient.DeleteAsync(
                $"/project/tasks/{toBeDeleted.Name}");
        }
        catch
        {
            return;
        }

        httpResponseMessage.EnsureSuccessStatusCode();
    }
}