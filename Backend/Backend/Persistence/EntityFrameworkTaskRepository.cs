using Microsoft.EntityFrameworkCore;
using DomainTask = Backend.Domain.Task;
using Task = System.Threading.Tasks.Task;

namespace Backend.Persistence;

public class EntityFrameworkTaskRepository : ITaskRepository
{
    private readonly BackendContext backendContext;

    public EntityFrameworkTaskRepository(BackendContext backendContext)
    {
        this.backendContext = backendContext;
    }

    public async Task<IEnumerable<DomainTask>> All()
    {
        return await backendContext.Tasks.ToArrayAsync();
    }

    public async Task Save(DomainTask toBeCreated)
    {
        await backendContext.Tasks.AddAsync(toBeCreated);
        await backendContext.SaveChangesAsync();
    }
}