using System.Net.Http;
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

    public async Task<ResultWithValue> All()
    {
        HttpResponseMessage getResult;

        try
        {
            getResult = await httpClient.GetAsync(TasksUri);
        }
        catch
        {
            return ResultWithValue.Failed();
        }

        var backendTasks =
            await getResult.Content.ReadAsAsync<IEnumerable<BackendTask>>();

        return ResultWithValue.Of(backendTasks.Select(ToDesktopTask).ToArray());
    }

    private static Task ToDesktopTask(BackendTask task)
    {
        return new Task(task.Id, task.Name, task.Description);
    }

    public async Task<ResultWithoutValue> Save(Task task)
    {
        var existingTasks = await All();

        if (existingTasks.Tasks.Contains(task))
        {
            var result = await Modify(task);

            return result;
        }

        try
        {
            var postResult = await httpClient.PostAsJsonAsync(
                TasksUri,
                ToBackendTask(task));

            postResult.EnsureSuccessStatusCode();
        }
        catch
        {
            return ResultWithoutValue.Failed();
        }

        return ResultWithoutValue.Success();
    }

    private async Task<ResultWithoutValue> Modify(Task task)
    {
        try
        {
            var result = await httpClient.PutAsJsonAsync(
                TasksUri,
                ToBackendTask(task));

            result.EnsureSuccessStatusCode();
        }
        catch
        {
            return ResultWithoutValue.Failed();
        }

        return ResultWithoutValue.Success();
    }

    private static BackendTask ToBackendTask(Task task)
    {
        return new BackendTask(task.Id, task.Name, task.Description);
    }

    public async Task<ResultWithoutValue> Delete(Task toBeDeleted)
    {
        try
        {
            var httpResponseMessage = await httpClient.DeleteAsync(
                $"{TasksUri}{toBeDeleted.Id}");

            httpResponseMessage.EnsureSuccessStatusCode();
        }
        catch
        {
            return ResultWithoutValue.Failed();
        }

        return ResultWithoutValue.Success();
    }
}