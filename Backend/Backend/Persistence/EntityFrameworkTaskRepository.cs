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
        if (await backendContext.Tasks.AnyAsync(x => x.Id == toBeCreated.Id))
        {
            var toBeModified =
                await backendContext.Tasks.SingleAsync(x => x.Id == toBeCreated.Id);

            await Delete(toBeModified.Name);
        }

        await backendContext.Tasks.AddAsync(toBeCreated);
        await backendContext.SaveChangesAsync();
    }

    public async Task Modify(DomainTask toBeModified)
    {
        if (await backendContext.Tasks.AnyAsync(x => x.Id == toBeModified.Id))
        {
            var oldTask =
                await backendContext.Tasks.SingleAsync(x => x.Id == toBeModified.Id);

            await Delete(oldTask.Name);
        }

        await backendContext.Tasks.AddAsync(toBeModified);
        await backendContext.SaveChangesAsync();
    }

    public async Task Delete(string taskName)
    {
        var toBeDeleted =
            await backendContext.Tasks.SingleAsync(x => x.Name == taskName);
        backendContext.Tasks.Remove(toBeDeleted);

        await backendContext.SaveChangesAsync();
    }

    public Task<bool> Exist(string taskName)
    {
        return backendContext.Tasks.AnyAsync(x => x.Name == taskName);
    }

    public Task<bool> Exist(Guid task)
    {
        return backendContext.Tasks.AnyAsync(x => x.Id == task);
    }
}