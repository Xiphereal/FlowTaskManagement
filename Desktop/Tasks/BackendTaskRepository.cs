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
        var httpResponseMessage = await httpClient.GetAsync("/Project/GetTasks/");

        var content = await httpResponseMessage.Content.ReadAsStringAsync();

        return [];
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