using System.Net.Http;
using Microsoft.Extensions.Configuration;
using SystemTask = System.Threading.Tasks.Task;
using Task = Desktop.Domain.Task;

namespace Desktop.Tasks;

public class BackendTaskRepository : ITaskRepository
{
    private HttpClient httpClient;

    public BackendTaskRepository(string backendUri)
    {
        httpClient = new HttpClient();
        httpClient.BaseAddress = new Uri(backendUri);
    }

    public IReadOnlyList<Task> All()
    {
        throw new NotImplementedException();
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