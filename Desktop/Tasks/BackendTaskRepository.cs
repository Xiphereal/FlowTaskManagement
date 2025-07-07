using System.Net.Http;
using SystemTask = System.Threading.Tasks.Task;
using Task = Desktop.Domain.Task;
using BackendTask = Backend.Domain.Task;

namespace Desktop.Tasks;

public class BackendTaskRepository : ITaskRepository, IRemoteTaskRepository
{
    private const string TasksUri = "/project/tasks/";
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
            getResult = await httpClient.GetAsync(TasksUri);
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
        return new Task(task.Id, task.Name, task.Description);
    }

    public async Task<bool> Save(Task task)
    {
        var existingTasks = await All();

        if (existingTasks.Contains(task))
        {
            var hasBeenSuccessful = await Modify(task);

            return hasBeenSuccessful;
        }

        try
        {
            var postResult = await httpClient.PostAsJsonAsync(
                TasksUri,
                ToBackendTask(task));

            postResult.EnsureSuccessStatusCode();

            return true;
        }
        catch
        {
            return false;
        }
    }

    private async Task<bool> Modify(Task task)
    {
        try
        {
            var result = await httpClient.PutAsJsonAsync(
                TasksUri,
                ToBackendTask(task));

            result.EnsureSuccessStatusCode();

            return true;
        }
        catch
        {
            return false;
        }
    }

    private static BackendTask ToBackendTask(Task task)
    {
        return new BackendTask(task.Id, task.Name, task.Description);
    }

    public async SystemTask Delete(Task toBeDeleted)
    {
        HttpResponseMessage httpResponseMessage;

        try
        {
            httpResponseMessage = await httpClient.DeleteAsync(
                $"{TasksUri}{toBeDeleted.Id}");
        }
        catch
        {
            return;
        }

        httpResponseMessage.EnsureSuccessStatusCode();
    }
}